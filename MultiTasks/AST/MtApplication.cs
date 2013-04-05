using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using MultiTasks.RT;
using System;
using System.Diagnostics;

namespace MultiTasks.AST
{
    public class MtApplication : MtAstNode
    {

        protected AstNode _head;
        protected MtExpressionList _args;

        public override AstNode ToTS()
        {
            var x = new MtApplication();            
            x._head = _head.ConvertToTS();            
            x._args = (MtExpressionList) (_args.ConvertToTS());
            x.ModuleNode = ModuleNode;
            return x;
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
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
                
                MtFunctionObjectBase.ExtractAsFunction(headResult, (wrkFun) =>
                {

#if DEBUG && !SILVERLIGHT
                    if (wrkFun is BuiltInCallTarget)
                    {
                        var builtin = wrkFun as BuiltInCallTarget;
                        Debug.Print("Calling builtin: {0}", builtin.Name);
                    }
                    else
                    {
                        Debug.Print("Calling user function");
                    }
#endif

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
                //thread.CurrentNode = Parent;
            }
        }

    }
}
