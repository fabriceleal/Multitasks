using System;
using System.Threading;
using System.Diagnostics;

namespace MultiTasks.RT
{
    public class MtResult
    {
        private static long _counter = 0;
        private long _id;
                        
        internal MtResult() 
        { 
            _id = Interlocked.Increment(ref _counter); 
        }

        private MtObject _o;
        private bool _hasValue = false;
        private ManualResetEvent _receivedValue = new ManualResetEvent(false);

        private object _sync = new object();

        /// <summary>
        /// Set value (synchronous). This should be called only once, 
        /// by the method / class who created it, when one wishes to signal
        /// the end of a chain / function / ...
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        internal MtResult SetValue(MtObject o)
        {
            lock (_sync) {
                if (o == null)
                    throw new Exception("Do not call MtResult.SetValue with null!");

                if (_hasValue)
                    throw new Exception("Already has value!");

                try
                {
                    _o = o;
                    _hasValue = true;

                    // Raise event!
                    _receivedValue.Set();

                    return this;
                }
                catch (Exception e)
                {
                    throw new Exception("Exception on MtResult.SetValue.", e);
                }
            }            
        }

        internal MtResult SetValue(Func<object, MtObject> generateO)
        {
            // TODO Review this
            lock(_sync) 
            {
                if (generateO == null)
                    throw new Exception("Do not call MtResult.SetValue(async) with null!");

                if (_hasValue)
                    throw new Exception("Already has value!");

                try
                {
#if DEBUG && !SILVERLIGHT
                    MultiTasksRuntime.DebugDisplayInfo();
#endif

                    ThreadPool.QueueUserWorkItem(state =>
                    {
                        var o = generateO(state);
                        
                        _o = o;
                        _hasValue = true;

                        // Raise event!
                        _receivedValue.Set();
                    });

                    return this;
                }
                catch (Exception e)
                {
                    throw new Exception("Exception on MtResult.SetValue(async).", e);
                }
            }
        }


        public MtResult WaitForValue()
        {
#if DEBUG && !SILVERLIGHT
            Debug.Print("MtResult.WaitForValue(no callback) {0} #2 Thread {1} wait signal.", _id, Thread.CurrentThread.ManagedThreadId);
#endif
            // Wait event ...
            _receivedValue.WaitOne();
#if DEBUG && !SILVERLIGHT
            Debug.Print("MtResult.WaitForValue(no callback) {0} #2 Thread {1} received signal.", _id, Thread.CurrentThread.ManagedThreadId);
#endif
            return this;
        }

        public MtResult WaitForValue(Action<MtResult> callback)
        {
            if (callback == null)
                throw new Exception("I need a callback to wait for a value! Nothing is sync in this lang.");

            try
            {
#if DEBUG && !SILVERLIGHT
                MultiTasksRuntime.DebugDisplayInfo();
#endif
                ThreadPool.QueueUserWorkItem(state =>
                {

#if DEBUG && !SILVERLIGHT
                    Debug.Print("MtResult.WaitForValue {0} #2 Thread {1} wait signal.", _id, Thread.CurrentThread.ManagedThreadId);
#endif

                    // Wait event ...
                    _receivedValue.WaitOne();

#if DEBUG && !SILVERLIGHT
                    Debug.Print("MtResult.WaitForValue {0} #3 Thread {1} received signal.", _id, Thread.CurrentThread.ManagedThreadId);
#endif

                    // Raise callback
                    try
                    {
#if DEBUG && !SILVERLIGHT
                        Debug.Print("MtResult.WaitForValue {0} #4 Thread {1} Callback with value({2})", _id, Thread.CurrentThread.ManagedThreadId, _o.Value);
#endif

                        callback(this);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Exception on MtResult.WaitForValue callback.", e);
                    }
                });

                return this;
            }
            catch (Exception e)
            {
                throw new Exception("Exception on MtResult.WaitForValue.", e);
            }
        }

        /// <summary>
        /// Get Value (asynchronous)
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public MtResult GetValue(Action<MtObject> callback)
        {
            if (callback == null)
                throw new Exception("I need a callback to give you the value! Nothing is sync in this lang.");

            try
            {
#if DEBUG && !SILVERLIGHT
                MultiTasksRuntime.DebugDisplayInfo();
#endif
                ThreadPool.QueueUserWorkItem(state =>
                {

#if DEBUG && !SILVERLIGHT
                    Debug.Print("MtResult.GetValue {0} #2 Thread {1} wait signal.", _id, Thread.CurrentThread.ManagedThreadId);
#endif

                    // Wait event ...
                    _receivedValue.WaitOne();

#if DEBUG && !SILVERLIGHT
                    Debug.Print("MtResult.GetValue {0} #3 Thread {1} received signal.", _id, Thread.CurrentThread.ManagedThreadId);
#endif

                    // Raise callback
                    try
                    {
#if DEBUG && !SILVERLIGHT
                        Debug.Print("MtResult.GetValue {0} #4 Thread {1} Callback with value({2})", _id, Thread.CurrentThread.ManagedThreadId, _o.Value);
#endif                       
 
                        callback(_o);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Exception on MtResult.GetValue callback.", e);
                    }
                });

                return this;
            }
            catch (Exception e)
            {
                throw new Exception("Exception on MtResult.GetValue.", e);
            }
        }
        
        /// <summary>
        /// Get Value (synchronous)
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public MtResult GetValueSync(Action<MtObject> callback)
        {
            if (callback == null)
                throw new Exception("I need a callback to give you the value! Nothing is sync in this lang.");

            try
            {
   
                // Wait event ...
                _receivedValue.WaitOne();

                // Raise callback
                try
                {
                    callback(_o);
                }
                catch (Exception e)
                {
                    throw new Exception("Exception on MtResult.GetValue callback.", e);
                }
              
                return this;
            }
            catch (Exception e)
            {
                throw new Exception("Exception on MtResult.GetValue.", e);
            }       
        }
        
        public override string ToString()
        {
            _receivedValue.WaitOne();

            return "<MtResult with value>";
        }
                
        #region "Static"
        
        public static MtResult CreateAndWrap(MtObject o)
        {
            return (new MtResult().SetValue(o));
        }

        public static MtResult CreateAndWrap(object o)
        {
            if (o as MtObject != null)
                throw new Exception("Wrong method called! Call CreateAndWrap(MtObject)");

            return MtResult.CreateAndWrap(new MtObject(o));
        }

        public static MtResult True
        {
            get
            {
                // Always create new instances of MtResult!
                return CreateAndWrap(MtObject.True);
            }
        }
        
        public static MtResult False
        {
            get
            {
                // Always create new instances of MtResult!
                return CreateAndWrap(MtObject.False);
            }
        }

        #endregion

    }
}
