using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiTasks.AST
{
    public class TSMtBind : MtBind
    {

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            throw new Exception("Don't you dare!!!");
        }

        public TSMtBind(MtBind v)
        {
            _targetName = v._targetName;
            _expression = v._expression;
        }
        
    }
}
