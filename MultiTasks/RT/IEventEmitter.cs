using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiTasks.RT
{
    interface IEventEmitter
    {
        void On(string eventName, Action<object[]> action);

        void Raise(string eventName, object[] args);
    }
}
