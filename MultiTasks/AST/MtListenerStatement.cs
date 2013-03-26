using Irony.Ast;
using Irony.Parsing;
using Irony.Interpreter;
using System;
using MultiTasks.RT;
using Irony.Interpreter.Ast;
using System.Diagnostics;
using System.Threading;

namespace MultiTasks.AST
{
    public class MtListenerStatement : MtAstNode
    {
        private AstNode _eventEmitter;
        private string _eventName;
        private AstNode _listener;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            _eventEmitter = AddChild(string.Empty, treeNode.ChildNodes[0]) as AstNode;
            _eventName = (treeNode.ChildNodes[1].AstNode as AstNode).AsString;
            _listener = AddChild(string.Empty, treeNode.ChildNodes[2]) as AstNode;
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            try
            {
                var ret = new MtResult();

                var emitterThread = _eventEmitter.NewScriptThread(thread);
                var emitter = _eventEmitter.Evaluate(emitterThread) as MtResult;

                var listThread = _listener.NewScriptThread(thread);
                var list = _listener.Evaluate(listThread) as MtResult;

                emitter.GetValue(o =>
                {
                    list.GetValue(l =>
                    {
                        // set listener
                        var wrkEmitter = o.Value as IEventEmitter;

                        if (wrkEmitter == null)
                        {
#if DEBUG && !SILVERLIGHT
                            Debug.Print("I dont throw events");
#endif
                        }
                        else
                        {
                            ICallTarget callable = null;
                            var haveCallable = new ManualResetEvent(false);

                            // Extract Callable (Compute it once)
                            MtFunctionObjectBase.ExtractAsFunction(
                                    l, fun => { 
                                        callable = fun;
                                        haveCallable.Set();
                                    });
                            

                            wrkEmitter.On(_eventName, args => { 
#if DEBUG && !SILVERLIGHT
                                Debug.Print("EventEmitter Called {0}", _eventName);
#endif
                                var resArgs = new MtResult[args.Length];
                                for (var i = 0; i < args.Length; ++i)
                                {
                                    resArgs[i] = MtResult.CreateAndWrap(args[i]);
                                }
                                
                                haveCallable.WaitOne();


                                // Call it 
                                var result = callable.Call(thread, resArgs) as MtResult;
                                
                            });
                        }
                        
                        ret.SetValue(o);
                    });
                });

                return ret.WaitForValue();
                //return ret;
            }
            catch (Exception e)
            {
                throw new Exception("Exception on listener statement evaluate", e);
            }
            finally
            {
                thread.CurrentNode = Parent;
            }
        }
    }
}
