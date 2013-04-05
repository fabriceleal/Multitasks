using Irony.Interpreter.Ast;
using System;
using System.Collections.Generic;

namespace MultiTasks.AST
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
        private List<AstNode> _argList = new List<AstNode>();

        public override void Init(Irony.Ast.AstContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            foreach(var node in treeNode.ChildNodes)
            {
                var id = AddChild(string.Empty, node) as AstNode;
                if (id == null)
                {
                    throw new Exception("Argument can't be null!");
                }
                _argList.Add(id);
            }
        }

        protected override object DoEvaluate(Irony.Interpreter.ScriptThread thread)
        {
            throw new Exception("MtArgListForDecl can't be evaluated!");
        }
    }
}
