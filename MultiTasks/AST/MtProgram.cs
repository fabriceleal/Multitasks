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
    public class MtProgram : MtAstNode
    {
        public List<MtAstNode> _chains = new List<MtAstNode>();

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            var nodes = treeNode.GetMappedChildNodes();

            foreach (var child in nodes) {
                // Only allow non-null, mtchains!
                if (child.AstNode != null)
                    _chains.Add(AddChild(string.Empty, child) as MtAstNode);
            }

            AsString = "Program";
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            try
            {
                var programResult = new MtResult();
                
                for (int i = 0; i < _chains.Count; ++i)
                {
                    var ch = _chains[i];
                    var subthread = ch.NewScriptThread(thread);
                    var chResult = ch.Evaluate(subthread) as MtResult;

                    //_chains[i]
                    if (i == _chains.Count - 1)
                    {
                        chResult.GetValue(delegate(MtObject x)
                        {
                            // Lookout:
                            // do not capture anything inside here

                            programResult.SetValue(x);
                        });
                    }
                }

                // Execute all chains!
                //_chains.ForEach(parseChain);

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
