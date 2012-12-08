using System;
using System.Net;
using System.Threading;

namespace MultiTasks.RT
{
    public class MtServerHttp : MtServer
    {
        private HttpListener _httpListener;
                
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
                Action<object> OnContext, 
                Action<Exception> OnContextException,
                Action<Exception> OnException)
        {
            try
            {                

                _httpListener.BeginGetContext(r =>
                {

                    HttpListenerContext context = null;

                    try
                    {
                        context = _httpListener.EndGetContext(r);

                        ThreadPool.QueueUserWorkItem(state =>
                        {

                        });

                    }
                    catch (Exception e)
                    {
                        OnContextException(e);
                    }

                    OnContext(context);
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
                _httpListener.Start();
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
                _httpListener.Stop();
            }
            catch (Exception e)
            {
                OnException(e);
            }
        }

        #endregion

    }
}
