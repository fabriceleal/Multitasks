using System;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using MultiTasks.RT;

namespace MultiTasks.AST
{
    public class MtIf : MtAstNode
    {
        protected AstNode _expression;
        protected AstNode _trueBranch;
        protected AstNode _falseBranch;

        public override AstNode ToTS()
        {
            var x = new MtIf();
            x._expression = _expression.ConvertToTS();
            x._trueBranch = _trueBranch.ConvertToTS();
            x._falseBranch = _falseBranch.ConvertToTS();
            return x;
        }

        public override void Init(Irony.Ast.AstContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            var nodes = treeNode.ChildNodes;

            if (nodes.Count != 3)
            {
                throw new Exception("If node extended 3 children, received {0}".SafeFormat(nodes.Count));
            }

            _expression = AddChild(string.Empty, nodes[0]) as AstNode;
            if (_expression == null)
            {
                throw new Exception("No expression for if!");
            }

            _trueBranch = AddChild(string.Empty, nodes[1]) as AstNode;
            if (_expression == null)
            {
                throw new Exception("No true branch for if!");
            }

            _falseBranch = AddChild(string.Empty, nodes[2]) as AstNode;
            if (_expression == null)
            {
                throw new Exception("No false branch for if!");
            }
        }

        protected override object DoEvaluate(Irony.Interpreter.ScriptThread thread)
        {
            thread.CurrentNode = this;
            try
            {
                var myRes = new MtResult();

                // Eval expression
                var subthread = _expression.NewScriptThread(thread);
                var res = _expression.Evaluate(subthread) as MtResult;
                res.GetValue((resExpr) => 
                {
                    AstNode toEval = null;
                    if (resExpr.Value == MtObject.False.Value)
                    {
                        // Evaluate false branch
                        toEval = _falseBranch;
                    }
                    else
                    {
                        // Evaluate true branch
                        toEval = _trueBranch;
                    }

                    // Evaluate!
                    var subsubthread = toEval.NewScriptThread(thread);
                    var resBranch = toEval.Evaluate(subsubthread) as MtResult;
                    resBranch.GetValue((_resBranch) =>
                    {
                        myRes.SetValue(_resBranch);
                    });
                });

                return myRes;
            }
            catch (Exception e)
            {
                throw new Exception("Exception on If.DoEvaluate.", e);
            }
            finally
            {
                //thread.CurrentNode = Parent;
            }
        }

    }
}
