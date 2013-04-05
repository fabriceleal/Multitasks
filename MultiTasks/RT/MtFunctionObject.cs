using System;
using Irony.Interpreter.Ast;
using Irony.Interpreter;
using MultiTasks.AST;
using MultiTasks;

namespace MultiTasks.RT
{
    /// <summary>
    /// 
    /// </summary>
    public class MtFunctionObject : MtFunctionObjectBase, ICallTarget
    {

        private AstNode _body;
        private MtArgListForDecl _args;
        private ScriptThread _thread;

        public MtFunctionObject(AstNode body, MtArgListForDecl arguments) : this(body, arguments, null)
        { }

        public MtFunctionObject(AstNode body, MtArgListForDecl arguments, ScriptThread thread)
        {
            _body = body;
            _args = arguments;
            _thread = thread;
        }

        object ICallTarget.Call(Irony.Interpreter.ScriptThread thread, object[] parameters)
        {
            try
            {
                if (_thread != null)
                {
                    thread = _thread;
                }

                var body_to_eval = _body.ConvertToTS(); // Cloners.Convert(_body);

                // 2. Function object is responsable for:
                // 2.1 Create a new context
                var subthread = body_to_eval.NewScriptThread(thread);

                // 2.2 For each parameter, bind its correspondent argument  
                // (ignore extra parameters, set missing to Empty)
                for (var i = 0; i < _args.ArgList.Length; ++i)
                {
                    var argDecl = _args.ArgList[i];
                    var accessor = subthread.Bind(argDecl.AsString, BindingRequestFlags.Write | BindingRequestFlags.NewOnly);
                    accessor.SetValueRef(subthread, parameters[i]);
                }

                // Add var for recursive definitions
                {
                    var accessor = subthread.Bind("$", BindingRequestFlags.Write | BindingRequestFlags.ExistingOrNew);
                    accessor.SetValueRef(subthread, this);
                }

                // 2.3 Call body in this new context
                var res = body_to_eval.Evaluate(subthread) as MtResult;
                if (res == null)
                    throw new Exception("Function can't evaluate to null!");
                return res;
            }
            catch (Exception e)
            {
                throw new Exception("Exception on Function Object call.", e);
            }            
        }
    }
}
