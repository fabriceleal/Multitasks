using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiTasks.Functional
{
    internal abstract class Reducer<Y, T>
    {
        private T _tot;

        public Reducer(T initial)
        {
            _tot = initial;
        }

        public void Reduce(Y[] list)
        {
            Reduce(list, 0);
        }

        private void Reduce(Y[] list, int i)
        {
            if (i == list.Length)
            {
                FinalAction(_tot);
            }
            else
            {
                _tot = OnReduce(list[i], _tot);
                Reduce(list, i + 1);
            }
        }

        protected abstract T OnReduce(Y value, T currentTot);
        protected abstract void FinalAction(T tot);

    }
}
