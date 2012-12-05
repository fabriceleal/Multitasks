using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiTasks.RT
{
    public class MtObjectEmpty : MtObjectBase
    {
        private static MtObjectEmpty _empty = new MtObjectEmpty();

        public MtObjectEmpty Instance
        {
            get
            {
                return _empty;
            }
        }

        private MtObjectEmpty() { }
    }
}
