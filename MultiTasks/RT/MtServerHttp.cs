using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using Irony.Interpreter;

namespace MultiTasks.RT
{
    public class MtServerHttp : MtServer
    {
        private HttpListener _httpListener;
                
        public MtServerHttp()
        {
            _httpListener = new HttpListener();
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

        public override void Start(string endpoint, Action OnStarted, Action<Exception> OnException)
        {
            try
            {
                _httpListener.Prefixes.Add(endpoint);
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
