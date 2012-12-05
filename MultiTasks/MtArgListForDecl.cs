using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiTasks.AST;
using MultiTasks.RT;
using Irony.Interpreter.Ast;

namespace MultiTasks
{
    public class MtArgListForDecl : MtAstNode
    {
        public AstNode[] ArgList
        {
            get
            {
                return _argList.ToArray();
            }
        }
        private List<AstNode> _argList;

        public override void Init(Irony.Ast.AstContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            foreach(var node in treeNode.ChildNodes)
            {
                var id = AddChild(string.Empty, node) as AstNode;
                if (id == null)
                    throw new Exception("Argument can't be null!");
                _argList.Add(id);
            }
        }

        protected override object DoEvaluate(Irony.Interpreter.ScriptThread thread)
        {
            throw new Exception("MtArgListForDecl can't be evaluated!");
        }
    }
}
