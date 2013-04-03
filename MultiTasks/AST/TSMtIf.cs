using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using System;

namespace MultiTasks.AST
{
    public class TSMtIf : MtIf
    {

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            throw new Exception("Don't you dare!!!");
        }

        public TSMtIf(AstNode expression, AstNode trueBranch, AstNode falseBranch)
        {
            _expression = expression;
            _trueBranch = trueBranch;
            _falseBranch = falseBranch;
        }
    }
}
