using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Interpreter.Ast;
using Irony.Interpreter;
using MultiTasks.RT;

namespace MultiTasks.AST
{
    public class MtBind : MtAstNode
    {
        private AstNode _targetName;
        private MtAstNode _tail;

        public override void Init(Irony.Ast.AstContext context, Irony.Parsing.ParseTreeNode treeNode)
        {            
            base.Init(context, treeNode);

            var nodes = treeNode.ChildNodes;
            if (nodes.Count != 2)
                throw new Exception("MtBind expects 2 children (received {0}).".SafeFormat(nodes.Count));

            // AstNodeInterpreter, from Irony
            _targetName = AddChild(string.Empty, nodes[0]) as AstNode;
            if (_targetName == null)
                throw new Exception("No identifier for Bind!");

            // An arbitrary Mt node
            _tail = AddChild(string.Empty, nodes[1]) as MtAstNode;
            if (_tail == null)
                throw new Exception("No tail for Bind!");
            
        }

        protected override object DoEvaluate(Irony.Interpreter.ScriptThread thread)
        {
            try
            {
                //var bindResult = new MtResult();
                var bindResult = MtResult.CreateAndWrap(123);
                
                var accessor = thread.Bind(_targetName.Term.Name, BindingRequestFlags.Write | BindingRequestFlags.ExistingOrNew);
                accessor.SetValueRef(thread, bindResult);   

                return bindResult;
            }
            catch (Exception e)
            {
                throw new Exception("Exception on MtBind.DoEvaluate", e);
            }
            finally
            {

            }
        }
    }
}
