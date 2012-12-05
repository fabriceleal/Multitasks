using System;
using System.Collections.Generic;
using MultiTasks.RT;
using Irony.Interpreter.Ast;

namespace MultiTasks.AST
{

    /// <summary>
    /// This is the only AST Node which does not return a MtResult. 
    /// </summary>
    public class MtArguments : MtAstNode
    {

        List<AstNode> _args = new List<AstNode>();

        public override void Init(Irony.Ast.AstContext context, Irony.Parsing.ParseTreeNode treeNode)
        {            
            base.Init(context, treeNode);

            foreach (var node in treeNode.ChildNodes)
            {
                var child = AddChild(string.Empty, node);
                if (child == null)
                    throw new Exception("Argument without an AST node!");

                _args.Add(child);
            }            
        }

        protected override object DoEvaluate(Irony.Interpreter.ScriptThread thread)
        {
            thread.CurrentNode = this;
            try
            {
                MtResult[] result = new MtResult[_args.Count];

                for (var i = 0; i < _args.Count; ++i)
                {
                    var subthread = _args[i].NewScriptThread(thread);

                    result[i] = _args[i].Evaluate(subthread) as MtResult;
                    if (result[i] == null)
                        throw new Exception("Argument evaluated to null!");
                }

                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error on MtArguments.DoEvaluate", e);
            }
            finally
            {
                thread.CurrentNode = Parent;
            }
        }
    }
}
