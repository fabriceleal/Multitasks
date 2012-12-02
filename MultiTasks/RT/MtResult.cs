using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultiTasks.RT
{
    public class MtResult
    {
        private MtObject _o;
        private bool _hasValue = false;
        private ManualResetEvent _receivedValue = new ManualResetEvent(false);

        private object sync = new object();

        public MtResult SetValue(MtObject o)
        {
            lock (sync) {
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

        // Async, of course!
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

                waitAndFire.BeginInvoke((IAsyncResult r) => {
                    waitAndFire.EndInvoke(r);
                }, null);

                return this;
            }
            catch (Exception e)
            {
                throw new Exception("Exception on MtResult.GetValue.", e);
            }            
        }

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
