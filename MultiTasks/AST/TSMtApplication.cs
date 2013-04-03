using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiTasks.AST
{
    public class TSMtApplication : MtApplication
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            throw new Exception("Don't you dare!");
        }

        public TSMtApplication(AstNode head, MtExpressionList args)
        {
            _head = head;
            _args = args;
        }

    }
}
