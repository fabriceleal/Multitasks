using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiTasks;
using Irony.Interpreter.Ast;
using Irony.Interpreter;

namespace MultiTasks.RT
{
    public class MtFunctionObject : MtObjectBase, ICallTarget
    {

        private AstNode _body;
        private MtArgListForDecl _args;

        public MtFunctionObject(AstNode body, MtArgListForDecl arguments)
        {
            _body = body;
            _args = arguments;
        }

        object ICallTarget.Call(Irony.Interpreter.ScriptThread thread, object[] parameters)
        {
            // 2. Function object is responsable for:
            // 2.1 Create a new context
            var subthread = _body.NewScriptThread(thread);

            // 2.2 For each parameter, bind its correspondent argument  
            // (ignore extra parameters, set missing to Empty)
            for (var i = 0; i < _args.ArgList.Length; ++i)
            {
                var argDecl = _args.ArgList[i];
                var accessor = subthread.Bind(argDecl.AsString, BindingRequestFlags.Write | BindingRequestFlags.ExistingOrNew);
                accessor.SetValueRef(subthread, parameters[i]);
            }

            // Add var for recursive definitions
            {
                var accessor = subthread.Bind("$", BindingRequestFlags.Write | BindingRequestFlags.ExistingOrNew);
                accessor.SetValueRef(subthread, this);
            }

            // 2.3 Call body in this new context
            var res = _body.Evaluate(subthread) as MtResult;
            if (res == null)
                throw new Exception("Function can't evaluate to null!");
            return res;

            //return MtResult.CreateAndWrap(12345);
        }
    }
}
