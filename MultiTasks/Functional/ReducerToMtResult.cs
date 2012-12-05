using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiTasks.RT;

namespace MultiTasks.Functional
{
    internal abstract class ReducerToMtResult<T>
    {
        private T _tot;
        private MtResult _resultToWrap;

        public ReducerToMtResult(T initial, MtResult resultToWrap)
        {
            _tot = initial;
            _resultToWrap = resultToWrap;
        }

        public void Reduce(object[] list)
        {
            Reduce(list, 0);
        }

        private void Reduce(object[] list, int i)
        {
            if (i == list.Length)
            {
                FinalAction();
            }
            else
            {
                var item = list[i] as MtResult;
                if (item == null)
                    throw new Exception("Argument should be a MtResult!");

                item.GetValue((a) =>
                {
                    _tot = OnReduce(a, _tot);
                    Reduce(list, i + 1);
                });
            }
        }

        protected abstract T OnReduce(MtObject o, T currentValue);
        protected void FinalAction()
        {
            _resultToWrap.SetValue(new MtObject(_tot));
        }
    }
}
