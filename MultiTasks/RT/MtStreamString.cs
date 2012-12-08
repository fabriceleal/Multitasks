using System;
using System.Text;
using System.IO;

namespace MultiTasks.RT
{
    public class MtStreamString : MtStream
    {
        public MtStreamString(string st) : base(new MemoryStream(Encoding.UTF8.GetBytes(st)))
        {

        }

        public void ReadAsString(Action<string> callback)
        {
            var wrkStream = _stream as MemoryStream;

            callback(Encoding.UTF8.GetString(wrkStream.GetBuffer()));
        }
    }
}
