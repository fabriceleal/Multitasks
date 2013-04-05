using Irony.Interpreter;
using Irony.Interpreter.Ast;
using MultiTasks.RT;
using System;

namespace MultiTasks.AST
{
    public class MtFunctionLiteral : MtAstNode
    {
        protected MtArgListForDecl _arguments;
        protected AstNode _body;
        
        public override AstNode ToTS()
        {
            var i = new MtFunctionLiteral();
            i._arguments = _arguments;
            i._body = _body;
            i.ModuleNode = ModuleNode;
            return i;
        }

        public override void Init(Irony.Ast.AstContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            var nodes = treeNode.ChildNodes;
            if (nodes.Count != 2)
            {
                throw new Exception("FunctionLiteral expected 2 children, received {0}".SafeFormat(nodes.Count));
            }

            _arguments = AddChild(string.Empty, nodes[0]) as MtArgListForDecl;
            if (_arguments == null)
            {
                throw new Exception("List of arguments can't be null!");
            }

            _body = AddChild(string.Empty, nodes[1]) as AstNode;
            if (_body == null)
            {
                throw new Exception("Body of function can't be null!");/*v*/
            }
                        
        }

        protected override object DoEvaluate(Irony.Interpreter.ScriptThread thread)
        {
            thread.CurrentNode = this;
            try
            {
                var ret = new MtResult();

                ret.SetValue(new MtObject(new MtFunctionObject(_body, _arguments, thread)));
                
                return ret;
            }
            catch (Exception e)
            {
                throw new Exception("Error on MtFunctionLiteral.DoEvaluate", e);
            }
            finally
            {
                //thread.CurrentNode = Parent;
            }
        }
    }
}
