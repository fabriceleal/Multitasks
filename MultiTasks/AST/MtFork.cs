using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using System.Threading;
using Irony.Interpreter;
using MultiTasks.RT;

namespace MultiTasks.AST
{
    public class MtFork : MtAstNode
    {
        public List<MtAstNode> _chains = new List<MtAstNode>();

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            var nodes = treeNode.GetMappedChildNodes();
                        
            if (nodes.Count == 1 && nodes[0].AstNode as MtFork != null)
            {                
                nodes = nodes[0].GetMappedChildNodes();
            }
            
            foreach (var child in nodes)
            {
                // Only allow non-null, mtchains!
                if (child.AstNode != null)
                    _chains.Add(AddChild(string.Empty, child) as MtAstNode);
            }
            

            AsString = "Fork";
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            try
            {
                var programResult = new MtResult();

                // do NOT use this for anything else other than counting the number of to-finish-threads
                var count = _chains.Count; 
                var waitForAllToEnd = new ManualResetEvent(false);

                // TODO THIS NEEDS TO BE FIXED!
                // The rest of the program only cares about the result of the last expression
                // which doesn't depend of the result of the previous expressions;
                // however, the program shouldn't end. 
                // SOLUTION: Keep a global counter of all awaiting MtResults?

                for (int i = 0; i < _chains.Count; ++i)
                {
                    // This is here because of issues with closures using the for variable
                    var safe_i = i + 0;
                    var ch = _chains[i];
                    var subthread = ch.NewScriptThread(thread);
                    var chResult = ch.Evaluate(subthread) as MtResult;

                    // Lookout:
                    // do not capture anything loop related inside here
                    chResult.GetValue(delegate(MtObject x)
                    {
                        if (Interlocked.Decrement(ref count) == 0)
                        {
                            waitForAllToEnd.Set();
                        }

                        // Wait for the last chain to end. 
                        // Last refers in the order of the chain in the code, 
                        // not the order of execution!!!
                        if (safe_i == _chains.Count - 1)
                        {
                            // Wait for the end of *all* the chains before returning ...
                            waitForAllToEnd.WaitOne();
                            programResult.SetValue(x);
                        }
                    });
                }


                // Return value does not matter. This is asynchronous, remember?
                return programResult;
            }
            catch (Exception e)
            {
                throw new Exception("Exception on MtProgram.", e);
            }
            finally
            {
                thread.CurrentNode = Parent;
            }
        }
    }
}
