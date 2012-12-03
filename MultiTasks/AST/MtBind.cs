using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiTasks.AST
{
    public class MtBind : MtAstNode
    {
        public override void Init(Irony.Ast.AstContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            // TODO
            base.Init(context, treeNode);
        }

        protected override object DoEvaluate(Irony.Interpreter.ScriptThread thread)
        {
            // TODO
            return base.DoEvaluate(thread);
        }
    }
}
