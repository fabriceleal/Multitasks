
using MultiTasks.RT;
using System;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using System.Collections.Generic;
namespace MultiTasks.AST
{
    public class MtFunctionLiteral : MtAstNode
    {
        private MtArgListForDecl _arguments;
        private AstNode _body;       

        public override void Init(Irony.Ast.AstContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            var nodes = treeNode.ChildNodes;
            if (nodes.Count != 2)
                throw new Exception("FunctionLiteral expected 2 children, received {0}".SafeFormat(nodes.Count));

            _arguments = AddChild(string.Empty, nodes[0]) as MtArgListForDecl;
            if (_arguments == null)
                throw new Exception("List of arguments can't be null!");

            _body = AddChild(string.Empty, nodes[1]) as AstNode;
            if (_body == null)
                throw new Exception("Body of function can't be null!");/*v*/

                        
        }

        private MtResult FunctionObject(ScriptThread thread, object[] args)
        {
            return MtResult.CreateAndWrap(123);
        }

        protected override object DoEvaluate(Irony.Interpreter.ScriptThread thread)
        {
            thread.CurrentNode = this;
            try
            {
                var ret = new MtResult();

                //ret.SetValue(new MtObject(null));
                                
                // 1. Create function object, return it

                // 2. Function object is responsable for:
                // 2.1 Create a new context
                // 2.2 For each parameter, bind its correspondent argument
                // 2.3 Call body in this new context

                return ret;
            }
            catch (Exception e)
            {
                throw new Exception("Error on MtFunctionLiteral.DoEvaluate", e);
            }
            finally
            {
                thread.CurrentNode = Parent;
            }
        }
    }
}
