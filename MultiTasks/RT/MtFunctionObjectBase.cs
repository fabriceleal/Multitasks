using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Interpreter;
using Irony.Interpreter.Ast;

namespace MultiTasks.RT
{
    public abstract class MtFunctionObjectBase : MtObjectBase, ICallTarget
    {
        public static void ExtractAsFunction(object obj, Action<ICallTarget> finalAction)
        {
            Action<object> parseAndCallHead = null;
            parseAndCallHead = (probableFun) =>
            {
                if (probableFun == null)
                    throw new Exception("probableFun is null!");

                if (probableFun as ICallTarget != null)
                {
                    var wrkFun = probableFun as ICallTarget;
                    finalAction(wrkFun);
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

            parseAndCallHead(obj);
        }

        object ICallTarget.Call(Irony.Interpreter.ScriptThread thread, object[] parameters)
        {
            throw new NotImplementedException("Any child of MtFunctionObjectBase should implement Call().");
        }
    }
}
