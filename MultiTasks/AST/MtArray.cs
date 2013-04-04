
using System;
using MultiTasks.RT;
using Irony.Interpreter;

namespace MultiTasks.AST
{
    public class MtArray : MtAstNode
    {
        private MtExpressionList _expressionList;

        public override void Init(Irony.Ast.AstContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            if (treeNode.ChildNodes.Count != 1)
            {
                throw new Exception("Expected 1 child, received {0}".SafeFormat(treeNode.ChildNodes.Count));
            }

            _expressionList = AddChild(string.Empty, treeNode.ChildNodes[0]) as MtExpressionList;
            if (_expressionList == null)
            {
                throw new Exception("Child should be a MtExpressionList!");
            }
        }

        protected override object DoEvaluate(Irony.Interpreter.ScriptThread thread)
        {
            thread.CurrentNode = this;
            try
            {
                // TODO Create a new context with $ as the current array? RECURSIVE ARRAYS? \ :D /

                var ret = new MtResult();

                var expressions = _expressionList.Evaluate(thread) as MtResult[];

                ret.SetValue(new MtObject(expressions));

                return ret; 
            }
            catch (Exception e)
            {
                throw new Exception("Exception on MtArray.DoEvaluate.", e);
            }
            finally
            {
                //thread.CurrentNode = Parent;
            }
        }
    }
}
