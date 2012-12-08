using System;
using Irony.Interpreter.Ast;
using Irony.Interpreter;
using MultiTasks.RT;

namespace MultiTasks.AST
{
    public class MtBind : MtAstNode
    {
        private AstNode _targetName;
        private MtAstNode _expression;

        public override void Init(Irony.Ast.AstContext context, Irony.Parsing.ParseTreeNode treeNode)
        {            
            base.Init(context, treeNode);

            var nodes = treeNode.ChildNodes;
            if (nodes.Count != 2)
            {
                throw new Exception("MtBind expects 2 children (received {0}).".SafeFormat(nodes.Count));
            }

            // AstNodeInterpreter, from Irony
            _targetName = AddChild(string.Empty, nodes[0]) as AstNode;
            if (_targetName == null)
            {
                throw new Exception("No identifier for Bind!");
            }

            // An arbitrary Mt node
            _expression = AddChild(string.Empty, nodes[1]) as MtAstNode;
            if (_expression == null)
            {
                throw new Exception("No expression for Bind!");
            }
        }

        protected override object DoEvaluate(Irony.Interpreter.ScriptThread thread)
        {
            thread.CurrentNode = this;
            try
            {
                var res = new MtResult();
                
                var accessor = thread.Bind(_targetName.AsString, BindingRequestFlags.Write | BindingRequestFlags.ExistingOrNew);
                accessor.SetValueRef(thread, res);
                
                // To allow recursive definitions, we have 
                // to evaluate _expression inside the new context.
                var exprResult = _expression.Evaluate(thread) as MtResult;
                exprResult.GetValue((o) =>
                {
                    res.SetValue((state) =>
                    {
                        return o;
                    });
                });

                return res;                
            }
            catch (Exception e)
            {
                throw new Exception("Exception on MtBind.DoEvaluate", e);
            }
            finally
            {
                thread.CurrentNode = Parent;
            }
        }
    }
}
