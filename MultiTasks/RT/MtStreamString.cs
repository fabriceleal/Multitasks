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
            var bytes = wrkStream.GetBuffer();            
            var as_string = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            // In silverlight, calling Encoding.GetString(byte[]) 
            // says that is inaccessible due to its protection level. Go figure.

            callback(as_string);
        }
    }
}
