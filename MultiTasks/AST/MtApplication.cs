using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Interpreter.Ast;
using Irony.Interpreter;
using MultiTasks.RT;

namespace MultiTasks.AST
{
    public class MtApplication : MtAstNode
    {
        private AstNode _head;
        //private List<AstNode> _args = new List<AstNode>();
        private MtArguments _args;

        public override void Init(Irony.Ast.AstContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            // Function
            _head = AddChild(string.Empty, treeNode.ChildNodes[0]);

            // Arguments
            //for (int i = 1; i < treeNode.ChildNodes.Count; ++i)
            // {
            //    _args.Add(AddChild(string.Empty, treeNode.ChildNodes[i]));
            //}

            _args = AddChild(string.Empty, treeNode.ChildNodes[1]) as MtArguments;

            AsString = "Application";
        }

        protected override object DoEvaluate(Irony.Interpreter.ScriptThread thread)
        {
            // PROLOG
            thread.CurrentNode = this;
            try
            {
                var appResult = new MtResult();

                MtResult[] args = _args.Evaluate(thread) as MtResult[];

                var subthreadF = _head.NewScriptThread(thread);
                var headResult = _head.Evaluate(subthreadF);

                Action<object> parseAndCallHead = null; 
                parseAndCallHead = (probableFun) =>
                {
                    if (probableFun == null)
                        throw new Exception("probableFun is null!");

                    if (probableFun as ICallTarget != null)
                    {
                        var wrkFun = probableFun as ICallTarget;
                        var resultFun = wrkFun.Call(thread, args) as MtResult;
                        resultFun.GetValue((r3) => {
                            appResult.SetValue(r3);
                        });
                    }
                    else if (probableFun as MtObject != null)
                    {
                        parseAndCallHead((probableFun as MtObject).Value);
                    }
                    else if (probableFun as MtResult != null)
                    {
                        var wrkFun = probableFun as MtResult;
                        wrkFun.GetValue((o) =>
                        {
                            parseAndCallHead(o);
                        });
                    }
                    else
                    {
                        throw new Exception("probableFun is neither MtResult nor ICallTarget, is {0}.".SafeFormat(probableFun.GetType().FullName));
                    }
                };

                parseAndCallHead(headResult);

                return appResult;
            }
            catch (Exception e)
            {
                throw new Exception("Exception on MtApplication.", e);
            }
            finally
            {
                // EPILOG
                thread.CurrentNode = Parent;
            }
        }

    }
}
