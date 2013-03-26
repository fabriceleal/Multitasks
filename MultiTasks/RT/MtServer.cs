using System;
using System.Net;

namespace MultiTasks.RT
{
    // Do not include this class for the Silverlight demo!
    public abstract class MtServer: IEventEmitter
    {

        private EventEmitter _eventEmitter = new EventEmitter();

        void IEventEmitter.Raise(string eventName, object[] args)
        {
            (_eventEmitter as IEventEmitter).Raise(eventName, args);
        }

        void IEventEmitter.On(string eventName, Action<object[]> action)
        {
            (_eventEmitter as IEventEmitter).On(eventName, action);
        }


        public abstract void Start(
                Action OnStarted, 
                Action<Exception> OnException);

        public abstract void Stop(
                Action OnStopped, 
                Action<Exception> OnException);

        public abstract void GetContext(
                Action<HttpListenerContext> OnContext,
                Action<Exception> OnContextException,
                Action<Exception> OnException);

    }
}
