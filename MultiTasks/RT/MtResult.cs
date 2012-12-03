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
                };

                //var mainThId = Thread.CurrentThread.ManagedThreadId;
                waitAndFire.BeginInvoke((IAsyncResult r) => {
                    //Debug.Print("Main = " + mainThId);
                    //Debug.Print("Sub = " + Thread.CurrentThread.ManagedThreadId);
                    waitAndFire.EndInvoke(r);
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

        public static MtResult CreateAndWrap(object o)
        {
            return (new MtResult().SetValue(new MtObject(o)));
        }

        private static MtResult _true;

        private static MtResult _false;

        public static MtResult True
        {
            get
            {
                if (_true == null)
                    _true = (new MtResult()).SetValue(MtObject.True);
                return _true;
            }
        }
        
        public static MtResult False
        {
            get
            {
                if (_false == null)
                    _false = (new MtResult()).SetValue(MtObject.False);
                return _false;
            }
        }

        #endregion
    }
}
