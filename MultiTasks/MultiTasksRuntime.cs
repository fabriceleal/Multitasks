using System;
using System.Text;
using Irony.Interpreter;
using Irony.Parsing;
using MultiTasks.RT;
using System.IO;
using System.Threading;
using System.Diagnostics;
using MultiTasks.Functional;
using System.Net;

namespace MultiTasks
{
    public partial class MultiTasksRuntime : LanguageRuntime
    {
        public MultiTasksRuntime(LanguageData lang) : base(lang) { }
                
        public Stream OutputStream { get; set; }

        public override void Init()
        {
            base.Init();
                                                
            BuiltIns.AddMethod(MtPrint, "print", 1);
            BuiltIns.AddMethod(MtFalse, "false");
            BuiltIns.AddMethod(MtTrue, "true");
            BuiltIns.AddMethod(MtIdentity, "identity", 1);
            BuiltIns.AddMethod(MtSleep, "sleep", 1, 2);
            BuiltIns.AddMethod(MtAdd, "add", 1);
            BuiltIns.AddMethod(MtMult, "mult", 1);
            BuiltIns.AddMethod(MtSubt, "subt", 1);
            BuiltIns.AddMethod(MtDiv, "div", 1);
            BuiltIns.AddMethod(MtZero, "zero", 1);
            BuiltIns.AddMethod(MtCar, "car", 1, 1);
            BuiltIns.AddMethod(MtCdr, "cdr", 1, 1);
            BuiltIns.AddMethod(MtMap, "map", 2, 2);
            BuiltIns.AddMethod(MtStringStreamCreate, "str_stream", 1, 1);
            BuiltIns.AddMethod(MtUriStreamCreate, "uri_stream", 1, 1);
            BuiltIns.AddMethod(MtStreamsClose, "close_stream", 1);
            BuiltIns.AddMethod(MtWait, "wait", 1);

            // HTTP Server
            BuiltIns.AddMethod(MtHttpServerCreate, "http_server", 1);
            BuiltIns.AddMethod(MtHttpServerStart, "http_server_start", 1);
            BuiltIns.AddMethod(MtHttpServerStop, "http_server_stop", 1);
            BuiltIns.AddMethod(MtHttpSetCode, "http_set_code", 2, 2);
            BuiltIns.AddMethod(MtHttpSetContentType, "http_set_content_type", 2, 2);
            BuiltIns.AddMethod(MtHttpStream, "http_stream", 1, 1);
            BuiltIns.AddMethod(MtHttpEnd, "http_end", 1, 1);

            // TODO
            // Get/Set request/response header value
            // Get request/response stream             


            // MORE ...
            // TODO Add insert, append, concat
            // TODO Add object literals
            // TODO Add streams literals?
            // TODO Add map, filter, reduce           
            // TODO Add sorting examples
            // TODO Add quote (for lazy evaluation of MtResults)
            // TODO Add flow control instructions (continuations, exceptions, ...)
            // TODO Add signaling, waiting
            // TODO Add compose(f, f)

            DoNETBindings();
        }


        #region HttpServer

        public object MtHttpEnd(ScriptThread thread, object[] arguments)
        {
            var result = new MtResult();
            
            var arg0 = arguments[0] as MtResult;
            arg0.GetValue(response =>
            {
                var wrkResponse = response.Value as HttpListenerResponse;
                if (wrkResponse == null)
                {
                    throw new Exception("No response!");
                }
                wrkResponse.Close();
            });
            
            return result;
        }

        public object MtHttpStream(ScriptThread thread, object[] arguments)
        {
            var result = new MtResult();

            var arg0 = arguments[0] as MtResult;
            arg0.GetValue(httpstuff =>
            {
                if (httpstuff.Value is HttpListenerRequest)
                {
                    var wrkValue = httpstuff.Value as HttpListenerRequest;
                    result.SetValue(new MtObject(new MtStreamWrapper(wrkValue.InputStream)));
                }
                else if (httpstuff.Value is HttpListenerResponse)
                {
                    var wrkValue = httpstuff.Value as HttpListenerResponse;
                    result.SetValue(new MtObject(new MtStreamWrapper(wrkValue.OutputStream)));                    
                }
                else
                {
                    throw new Exception("Neither a request nor a response!");
                }
            });

            return result;
        }

        /// <summary>
        /// Create http server. Pass prefixes as arguments
        /// </summary>
        /// <param name="thread"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public object MtHttpServerCreate(ScriptThread thread, object[] arguments)
        {
            var result = new MtResult();
            var prefixes = new string[arguments.Length];
            var count = arguments.Length;

            for (var i = 0; i < arguments.Length; ++i)
            {
                var copy_i = i;
                var wrkArg = arguments[copy_i] as MtResult;

                wrkArg.GetValue(o =>
                {
                    prefixes[copy_i] = o.Value as string;

                    if (Interlocked.Decrement(ref count) == 0)
                    {
                        result.SetValue(new MtObject(new MtServerHttp(prefixes)));
                    }
                });
            }

            return result;
        }

        /// <summary>
        /// Start one or more http servers
        /// </summary>
        /// <param name="thread"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public object MtHttpServerStart(ScriptThread thread, object[] arguments)
        {
            var result = new MtResult();

            if (arguments.Length == 1)
            {
                // Wrap the single server in the result
                var arg = arguments[0] as MtResult;
                arg.GetValue(o =>
                {
                    var server = o.Value as MtServerHttp;
                    server.Start(() =>
                    {
                        result.SetValue(MtObject.True);
                    }, e =>
                    {
                        result.SetValue(MtObject.False);
                    });
                });
            }
            else if (arguments.Length > 1)
            {
                // Wrap all servers in the result
                var results = new MtResult[arguments.Length];
                var count = arguments.Length;
                for (int i = 0; i < arguments.Length; ++i)
                {
                    int copy_i = i;
                    var wrkArg = arguments[copy_i] as MtResult;

                    // Call this after server start/exception
                    Action done = () =>
                    {
                        if (Interlocked.Decrement(ref count) == 0)
                        {
                            result.SetValue(new MtObject(results));
                        }
                    };

                    // Extract server, try to start
                    wrkArg.GetValue(o =>
                    {
                        var server = o.Value as MtServerHttp;
                        server.Start(() => {
                            results[copy_i] = MtResult.True;

                            done();

                        }, e => {
                            results[copy_i] = MtResult.False;

                            done();

                        });
                    });
                }
            }
            else
            {
                throw new Exception("I need args!");
            }

            return result;
        }

        /// <summary>
        /// Stop one or more http servers
        /// (copy pasted from mthttpserverstartp. Arrrrrgh...)
        /// </summary>
        /// <param name="thread"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public object MtHttpServerStop(ScriptThread thread, object[] arguments)
        {
            var result = new MtResult();

            if (arguments.Length == 1)
            {
                // Wrap the single server in the result
                var arg = arguments[0] as MtResult;
                arg.GetValue(o =>
                {
                    var server = o.Value as MtServerHttp;
                    server.Stop(() =>
                    {
                        result.SetValue(MtObject.True);
                    }, e =>
                    {
                        result.SetValue(MtObject.False);
                    });
                });
            }
            else if (arguments.Length > 1)
            {
                // Wrap all servers in the result
                var results = new MtResult[arguments.Length];
                var count = arguments.Length;
                for (int i = 0; i < arguments.Length; ++i)
                {
                    int copy_i = i;
                    var wrkArg = arguments[copy_i] as MtResult;

                    // Call this after server start/exception
                    Action done = () =>
                    {
                        if (Interlocked.Decrement(ref count) == 0)
                        {
                            result.SetValue(new MtObject(results));
                        }
                    };

                    // Extract server, try to start
                    wrkArg.GetValue(o =>
                    {
                        var server = o.Value as MtServerHttp;
                        server.Stop(() =>
                        {
                            results[copy_i] = MtResult.True;

                            done();

                        }, e =>
                        {
                            results[copy_i] = MtResult.False;

                            done();

                        });
                    });
                }
            }
            else
            {
                throw new Exception("I need args!");
            }

            return result;
        }

        public object MtHttpSetContentType(ScriptThread thread, object[] arguments)
        {
            var result = new MtResult();
            var arg0 = arguments[0] as MtResult;
            var arg1 = arguments[1] as MtResult;

            arg0.GetValue(o =>
            {
                arg1.GetValue(code =>
                {
                    var response = o.Value as HttpListenerResponse;
                    response.ContentType = code.Value as string;

                    result.SetValue(MtObject.True);
                });
            });

            return result;
        }

        public object MtHttpSetCode(ScriptThread thread, object[] arguments)
        {
            var result = new MtResult();
            var arg0 = arguments[0] as MtResult;
            var arg1 = arguments[1] as MtResult;

            arg0.GetValue(o =>
            {
                arg1.GetValue(code => 
                {
                    var response = o.Value as HttpListenerResponse;
                    response.StatusCode = (int)code.Value;
                     
                    result.SetValue(MtObject.True);
                });                
            });

            return result;
        }

        #endregion

        public object MtWait(ScriptThread thread, object[] arguments)
        {
            // Wait for all arguments. Blocking.
            for (var i = 0; i < arguments.Length; ++i)
            {
                var arg = arguments[i] as MtResult;
                if (arg == null)
                {
                    throw new Exception("Unexpected argument!");
                }
                arg.WaitForValue();
            }

            // Returns all the arguments, in an array
            return MtResult.CreateAndWrap(arguments);
        }

        public object MtStringStreamCreate(ScriptThread thread, object[] arguments)
        {
#if DEBUG
            Debug.Print("Create stream from string");
#endif            

            try
            {
                var result = new MtResult();
                var rStr = arguments[0] as MtResult;
                rStr.GetValue((str) =>
                {
                    result.SetValue(new MtObject(new MtStreamString(str.Value as string)));
                });
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error on string stream create.", e);
            }
        }

        public object MtUriStreamCreate(ScriptThread thread, object[] arguments)
        {
#if DEBUG
            Debug.Print("Create stream from uri");
#endif            

            try
            {

                var result = new MtResult();
                var rUri = arguments[0] as MtResult;
                rUri.GetValue((uri) =>
                {
                    string strUri = uri.Value as string;

                    try
                    {
                        var dummy = new Uri(strUri);
                    }
                    catch
                    {
                        // Assume its a filename
                        // Try to combine with Env.CurrentDir
                        strUri = Path.Combine(Environment.CurrentDirectory, strUri);
                    }

                    var wrkUri = new Uri(strUri);
                    if (wrkUri.Scheme == Uri.UriSchemeFile)
                    {
                        result.SetValue(new MtObject(new MtStreamFile(strUri as string)));
                    }
                    else
                    {
                        throw new Exception("Unsupported uri scheme: " + wrkUri.Scheme);
                    }
                });

                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Exception on uri stream create.", e);
            }
        }

        public object MtStreamsClose(ScriptThread thread, object[] arguments)
        {
#if DEBUG
            Debug.Print("Close streams");
#endif            

            try
            {

                var result = new MtResult();
                var count = arguments.Length;

                foreach (var arg in arguments)
                {
                    var wrkArg = arg as MtResult;
                    if (wrkArg == null)
                    {
                        throw new Exception("Argument is not a MtResult!");
                    }

                    wrkArg.GetValue(stream =>
                    {
                        var wrkStream = stream.Value as MtStream;
                        if (wrkStream == null)
                        {
                            throw new Exception("Argument is not a stream!");
                        }

                        wrkStream.Close();

                        if (Interlocked.Decrement(ref count) == 0)
                        {
                            result.SetValue(MtObject.True);
                        }
                    });
                }
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Exception on close(streams, ...)", e);
            }
        }

        #region Aux

        /// <summary>
        /// Don't call this one directly. Use Reduce(object[], Action_MtObject_, Action)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="i"></param>
        /// <param name="reducer"></param>
        /// <param name="finalAction"></param>
        private void Reduce(object[] list, int i, Action<MtObject> reducer, Action finalAction)
        {
            if (i == list.Length)
            {
                finalAction();
            }
            else
            {
                var item = list[i] as MtResult;
                if (item == null)
                    throw new Exception("Argument should be a MtResult!");

                item.GetValue((a) =>
                {
                    reducer(a);
                    Reduce(list, i + 1, reducer, finalAction);
                });
            }
        }

        private void Reduce(object[] list, Action<MtObject> reducer, Action finalAction)
        {
            Reduce(list, 0, reducer, finalAction);
        }

        #endregion

        #region Runtime builtin functions

        #region Lists

        // TODO
        private MtResult MtCurry(ScriptThread thread, object[] args)
        {
            return MtResult.CreateAndWrap(123);
        }
                
        private MtResult MtMap(ScriptThread thread, object[] args)
        {
            var result = new MtResult();
            var arrExpression = args[0] as MtResult;
            var fun = args[1];

            arrExpression.GetValue((arr) =>
            {
                // arr
                MtResult[] wrkArr = arr.Value as MtResult[];

                if (wrkArr == null)
                {
                    throw new Exception("Array expression is null!");
                }

                MtFunctionObjectBase.ExtractAsFunction(fun, (wrkFun) =>
                {
                    // Launch and wait for all to end
                    var res = new MtResult[wrkArr.Length];                    
                    var count = wrkArr.Length;
                    var waitForEndOfAll = new ManualResetEvent(false); 

                    for (var i = 0; i < wrkArr.Length; ++i)
                    {
                        int copy_i = i;

                        var ret = wrkFun.Call(thread, new object[] { wrkArr[i] });
                        if(ret == null)
                        {
                            throw new Exception("Return of application in map is null!");
                        }

                        var wrkRet = ret as MtResult;
                        wrkRet.WaitForValue((r) =>
                        {
                            res[copy_i] = r;

                            if (Interlocked.Decrement(ref count) == 0)
                            {
                                waitForEndOfAll.Set();
                            }
                        });
                    }

                    waitForEndOfAll.WaitOne();                    
                    result.SetValue(new MtObject(res));
                });

            });

            return result;
        }

        // TODO
        private MtResult MtCompose(ScriptThread thread, object[] args)
        {
            var result = new MtResult();

            // compose(f, g) := L (x) => f(g(x))
                        
            return result;
        }

        private MtResult MtCar(ScriptThread thread, object[] args)
        {
            var ret = new MtResult();
            var arr = args[0] as MtResult;
            arr.GetValue((o) =>
            {
                var wrkArr = o.Value as MtResult[];
                if (wrkArr == null)
                {
                    throw new Exception("Argument is not an array!");
                }

                wrkArr[0].GetValue((head) =>
                {
                    ret.SetValue(head);
                });                
            });
            return ret;
        }

        private MtResult MtCdr(ScriptThread thread, object[] args)
        {
            var ret = new MtResult();
            var arr = args[0] as MtResult;
            arr.GetValue((o) =>
            {
                var wrkArr = o.Value as MtResult[];
                if (wrkArr == null)
                {
                    throw new Exception("Argument is not an array!");
                }

                if (wrkArr.Length == 0 || wrkArr.Length == 1)
                {
                    ret.SetValue(new MtObject(new MtResult[] { }));
                }
                else
                {
                    ret.SetValue((state) =>
                    {
                        var tail = new MtResult[wrkArr.Length - 1];

                        Array.Copy(wrkArr, 1, tail, 0, tail.Length);

                        return new MtObject(tail);
                    });
                }
                
            });
            return ret;
        }

        #endregion

        public MtResult MtSleep(ScriptThread thread, object[] args)
        {
            var result = new MtResult();
            MtObject sleepResult = null;

            var sleepMs = args[0] as MtResult;
            var sleepEval = args.Length > 1 ? args[1] as MtResult : MtResult.True;

            var waitValue = new ManualResetEvent(false);
            
            // eval number of milliseconds to wait
            sleepMs.GetValue((waitPeriod) =>
            {
                // eval value to return
                sleepEval.GetValue((retValue) =>
                {
                    sleepResult = retValue;

                    // Signal: We have a value!
                    waitValue.Set();
                });

                // set value, async
                result.SetValue((state) =>
                {
#if DEBUG
                    Debug.Print("MtSleep #1 Thread {0} {1:mm:ss.ffff} start sleep", Thread.CurrentThread.ManagedThreadId, DateTime.Now);
#endif
                    
                    // Wait period and, if need be, wait for sleepResult
                    Thread.Sleep((int)waitPeriod.Value);

#if DEBUG
                    Debug.Print("MtSleep #2 Thread {0} {1:mm:ss.ffff} end sleep, wait value", Thread.CurrentThread.ManagedThreadId, DateTime.Now);
#endif

                    // Wait for signal...
                    waitValue.WaitOne();

#if DEBUG
                    Debug.Print("MtSleep #3 Thread {0} {1:mm:ss.ffff} has value", Thread.CurrentThread.ManagedThreadId, DateTime.Now);
#endif

                    if (sleepResult == null)
                        throw new Exception("Sleep can't return null!");

                    return sleepResult;
                });
                
            });
            
            return result;
        }

        #region Arithmetic

        private class AddReducer : ReducerToMtResult<int>
        {
            public AddReducer(MtResult result) : base(0, result) { }

            protected override int OnReduce(MtObject o, int currentValue)
            {
                return (int)(o.Value) + currentValue;
            }
        }

        private class SubtReducer : ReducerToMtResult<int>
        {
            public SubtReducer(MtResult result) : base(0, result) { }

            protected override int OnReduce(MtObject o, int currentValue)
            {
                return (int)(o.Value) - currentValue;
            }
        }

        private class MultReducer : ReducerToMtResult<int>
        {
            public MultReducer(MtResult result) : base(1, result) { }

            protected override int OnReduce(MtObject o, int currentValue)
            {
                return (int)(o.Value) * currentValue;
            }
        }

        private class DivReducer : ReducerToMtResult<int>
        {
            public DivReducer(MtResult result) : base(1, result) { }

            protected override int OnReduce(MtObject o, int currentValue)
            {
                return (int)(o.Value) / currentValue;
            }
        }

        public MtResult MtAdd(ScriptThread thread, object[] args)
        {
            var result = new MtResult();
            var reducer = new AddReducer(result);
            reducer.Reduce(args);
            return result;
        }

        public MtResult MtSubt(ScriptThread thread, object[] args)
        {
            var result = new MtResult();
            var reducer = new SubtReducer(result);
            reducer.Reduce(args);
            return result;
        }

        public MtResult MtMult(ScriptThread thread, object[] args)
        {
            var result = new MtResult();
            var reducer = new MultReducer(result);
            reducer.Reduce(args);
            return result;
        }

        public MtResult MtDiv(ScriptThread thread, object[] args)
        {
            var result = new MtResult();
            var reducer = new DivReducer(result);
            reducer.Reduce(args);
            return result;
        }

        #endregion

        public MtResult MtFalse(ScriptThread thread, object[] args)
        {
            // does nothing, just returns false            
            return MtResult.False;
        }

        public MtResult MtTrue(ScriptThread thread, object[] args)
        {
            // does nothing, just returns true
            return MtResult.True;
        }

        public MtResult MtIdentity(ScriptThread thread, object[] args)
        {
            try
            {
                var arg = args[0] as MtResult;
                if (arg == null)
                    throw new Exception("Argument cant be null!");

                return arg;
            }
            catch (Exception e)
            {
                throw new Exception("Exception on Runtime function: identity", e);
            }            
        }

        private MtResult MtZero(ScriptThread thread, object[] args)
        {
            try
            {
                var res = new MtResult();

                var arg = args[0] as MtResult;
                arg.GetValue((o) =>
                {
                    int value = (int)o.Value;
                    res.SetValue(value == 0 ? MtObject.True : MtObject.False);
                });

                return res;
            }
            catch (Exception e)
            {
                throw new Exception("Exception on Runtime function: zero", e);
            }
        }

        static byte[] _newLineRaw = Encoding.UTF8.GetBytes(Environment.NewLine);

        #region MtPrint

        /// <summary>
        /// This is used to lock streams when printing, so text from several prints 
        /// doesn't get interleaved
        /// </summary>
        private object _mtPrintLock = new object();

        public MtResult MtPrint(ScriptThread thread, object[] args)
        {
            try
            {
                var r = new MtResult();

               // lock (_mtPrintLock) /* Lock all streams */
               // {
                Monitor.Enter(_mtPrintLock);
                try
                {
                    Reduce(args, (o) =>
                    {

                        String toWrite = o == null ? "<null>" : o.ToString();

                        if (OutputStream != null)
                        {
                            byte[] raw = Encoding.UTF8.GetBytes(toWrite);
                            OutputStream.Write(raw, 0, raw.Length);
                        }

                        thread.App.Write(toWrite);
                    }, () =>
                    {

                        // Write new lines
                        if (OutputStream != null)
                        {
                            OutputStream.Write(_newLineRaw, 0, _newLineRaw.Length);
                            OutputStream.Flush();
                        }

                        thread.App.WriteLine("");

                        r.SetValue(MtObject.True);
                    });
                }
                catch (Exception e)
                {
                    throw new Exception("Error printing stuff.", e);
                }
                finally
                {
                    Monitor.Exit(_mtPrintLock);
                }

                
               // }

                return r;
            }
            catch (Exception e)
            {
                throw new Exception("Exception on Runtime function: print", e);
            }
        }

        #endregion

        #endregion
    }
}
