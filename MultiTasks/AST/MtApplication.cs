using System;
using Irony.Interpreter.Ast;
using Irony.Interpreter;
using MultiTasks.RT;

namespace MultiTasks.AST
{
    public class MtApplication : MtAstNode
    {

        private AstNode _head;
        private MtExpressionList _args;

        public override void Init(Irony.Ast.AstContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            _head = AddChild(string.Empty, treeNode.ChildNodes[0]);

            _args = AddChild(string.Empty, treeNode.ChildNodes[1]) as MtExpressionList;

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
                if (args == null)
                {
                    throw new Exception("Args evaluated to null!");
                }

                var subthreadF = _head.NewScriptThread(thread);                
                var headResult = _head.Evaluate(subthreadF);
                if (headResult == null)
                {
                    throw new Exception("Head can't evaluate to null!");
                }
                /*Action<object> parseAndCallHead = null; 
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

                parseAndCallHead(headResult);*/
                MtFunctionObjectBase.ExtractAsFunction(headResult, (wrkFun) =>
                {
                    var resultFun = wrkFun.Call(thread, args) as MtResult;
                    resultFun.GetValue((r3) =>
                    {
                        appResult.SetValue(r3);
                    });
                });

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
