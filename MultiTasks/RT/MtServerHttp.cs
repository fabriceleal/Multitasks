using System;
using System.Net;
using System.Diagnostics;
using System.Threading;

namespace MultiTasks.RT
{
    // Do not include this class for the silverlight demo!
    public class MtServerHttp : MtServer
    {

        private HttpListener _httpListener;
        private object _sync = new object();
        private bool _running = false;
        
       
        public MtServerHttp(string[] endpoints)
        {            
            _httpListener = new HttpListener();

            foreach (var prefix in endpoints)
            {
                _httpListener.Prefixes.Add(prefix);
            }

        }

        #region Methods       

        public override void GetContext(
                Action<HttpListenerContext> OnContext, 
                Action<Exception> OnContextException,
                Action<Exception> OnException)
        {
            try
            {
#if DEBUG
                Debug.Print("MtServerHttp Context!");
#endif

                _httpListener.BeginGetContext(r =>
                {

                    HttpListenerContext context = null;

                    try
                    {
                        context = _httpListener.EndGetContext(r);
                                                
                        OnContext(context);
                    }
                    catch (Exception e)
                    {
                        OnContextException(e);
                    }
                                       
                    lock (_sync)
                    {
                        if (_running)
                        {
                            GetContext(OnContext, OnContextException, OnException);
                        }
                    }

                }, null);
                
            }
            catch (Exception e)
            {
                OnException(e);
            }
        }

        public override void Start(Action OnStarted, Action<Exception> OnException)
        {
            try
            {
#if DEBUG
                Debug.Print("MtServerHttp Started");
#endif

                lock (_sync)
                {
                    _httpListener.Start();
                    _running = true;
                }

                (this as IEventEmitter).Raise("start", new object[] { this });

                OnStarted();

                // Start listening for contexts
                GetContext(ctx => {

#if DEBUG
                    Debug.Print("Has context");
#endif

                    ThreadPool.QueueUserWorkItem(state => 
                    {
                        (this as IEventEmitter).Raise("context", new object[] { this, ctx.Request, ctx.Response });
                    });
                                        
                }, e => {

#if DEBUG
                    Debug.Print("Exception getting context {0}", e.Message);
#endif

                }, e => {

#if DEBUG
                    Debug.Print("Exception! {0}", e.Message);
#endif

                });
            }
            catch (Exception e)
            {
                OnException(e);
            }
        }

        public override void Stop(Action OnStopped, Action<Exception> OnException)
        {
            try
            {
#if DEBUG
                Debug.Print("MtServerHttp Stop");
#endif
                lock (_sync)
                {
                    _httpListener.Stop();
                    _running = false;
                }                

                (this as IEventEmitter).Raise("stop", new object[] { this });

                OnStopped();
            }
            catch (Exception e)
            {
                OnException(e);
            }
        }
        
        #endregion

    }
}
