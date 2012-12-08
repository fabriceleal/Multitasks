using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiTasks.RT
{
    public abstract class MtServer: IEventEmitter
    {

        protected Dictionary<string, Action<object[]>> _events = new Dictionary<string, Action<object[]>>();

        void IEventEmitter.Raise(string eventName, object[] args)
        {
            if (_events.ContainsKey(eventName))
            {
                _events[eventName](args);
            }
        }

        void IEventEmitter.On(string eventName, Action<object[]> action)
        {
            if (!_events.ContainsKey(eventName))
            {
                _events.Add(eventName, action);
            }
            // else, ignore
        }

        public abstract void Start(
                string endpoint, 
                Action OnStarted, 
                Action<Exception> OnException);

        public abstract void Stop(
                Action OnStopped, 
                Action<Exception> OnException);

        public abstract void GetContext(
                Action<object> OnContext,
                Action<Exception> OnContextException,
                Action<Exception> OnException);

    }
}
