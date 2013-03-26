using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MultiTasks.RT
{
    public class EventEmitter : IEventEmitter
    {
        protected Dictionary<string, Action<object[]>> _events = new Dictionary<string, Action<object[]>>();
        protected Dictionary<string, Queue<PendingEvent>> _pending = new Dictionary<string, Queue<PendingEvent>>();
        private object _sync = new object();

        protected class PendingEvent
        {
            //public string Name;
            public object[] Args;

            public PendingEvent(/*string name,*/ object[] args) 
            {
                //Name = name;
                Args = args;
            }
        }

        void IEventEmitter.Raise(string eventName, object[] args)
        {
            lock (_sync)
            {
                if (_events.ContainsKey(eventName))
                {
                    // Execute handler!
                    _events[eventName](args);
                }
#if DEBUG && !SILVERLIGHT
                else
                {
                    Debug.Print("Raised event {0}, but no one is listening.", eventName);
                }
#endif                
            }
        }

        void IEventEmitter.On(string eventName, Action<object[]> action)
        {
            lock (_sync)
            {
                if (!_events.ContainsKey(eventName))
                {
                    _events.Add(eventName, action);
                }
                // else, ignore
#if DEBUG && !SILVERLIGHT
                else
                {
                    Debug.Print("Already had listener for {0}, ignore", eventName);
                }
#endif

            }
        }
        
    }
}
