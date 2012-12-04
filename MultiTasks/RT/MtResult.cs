using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace MultiTasks.RT
{
    public class MtResult
    {
        public delegate MtResult SetValueDelegate(MtObject o);

        private static long _counter = 0;
        private long _id;
        public MtResult() { _id = Interlocked.Increment(ref _counter); }


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
        public MtResult SetValue(MtObject o)
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
                Action waitAndFire = () =>
                {
                    Debug.Print("MtResult {0} #2 Thread {1} wait signal.", _id, Thread.CurrentThread.ManagedThreadId);

                    // Wait event ...
                    _receivedValue.WaitOne();

                    Debug.Print("MtResult {0} #3 Thread {1} received signal.", _id, Thread.CurrentThread.ManagedThreadId);

                    // Raise callback
                    try
                    {
                        Debug.Print("MtResult {0} #4 Thread {1} Callback with value({2})", _id, Thread.CurrentThread.ManagedThreadId, _o.Value);
                        callback(_o);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Exception on MtResult.GetValue callback.", e);
                    }
                };

                Debug.Print("MtResult {0} #1 Thread {1} starts waiting for value ...", _id, Thread.CurrentThread.ManagedThreadId);

                waitAndFire.BeginInvoke((IAsyncResult r) => {
                    waitAndFire.EndInvoke(r);
                    
                    Debug.Print("MtResult {0} #5 Thread {1} has value({2}).", _id, Thread.CurrentThread.ManagedThreadId, _o.Value);

                }, null);

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

            return (new MtResult().SetValue(new MtObject(o)));
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
