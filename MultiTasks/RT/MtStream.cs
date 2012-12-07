using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MultiTasks.RT
{
    public abstract class MtStream
    {

        public const int ReadBufferSize = 1024;

        public class MtStreamWritingContext
        {
            internal int _position;

            internal MtStreamWritingContext(int position)
            {
                _position = position;
            }
        }

        public class MtStreamReadingContext
        {
            internal int _position;

            internal MtStreamReadingContext(int position)
            {
                _position = position;
            }
        }
                
        protected Stream _stream;

        public MtStream(Stream stream)
        {
            _stream = stream;
        }

        public MtStreamReadingContext CreateReadingContext()
        {
            var r = new MtStreamReadingContext(0);
            return r;
        }


        public void ReadFromWriteTo(MtStream src, MtStream dest, Action doneCallback)
        {
            var readCtx = src.CreateReadingContext();
            var writCtx = dest.CreateWritingContext();

            Action<byte[]> readCallback = null; 
            readCallback = bytes =>
            {
                dest.Write(writCtx, bytes);
                src.Read(readCtx, readCallback, doneCallback);
            };
            
            src.Read(readCtx, readCallback, doneCallback);
        }

        private MtStreamWritingContext CreateWritingContext()
        {
            var w = new MtStreamWritingContext(0);
            return w;
        }

        #region Write

        public void Write(MtStreamWritingContext ctx, byte[] stuff)
        {
            AsyncCallback wroteCallback = null;
            wroteCallback = r =>
            {
                _stream.EndWrite(r);

                ctx._position += stuff.Length;
            };
            _stream.BeginWrite(stuff, ctx._position, stuff.Length, wroteCallback, null);
        }

        public void Write(MtStreamWritingContext ctx, byte[] stuff, Action doneCallback)
        {
            AsyncCallback wroteCallback = null;
            wroteCallback = r =>
            {
                _stream.EndWrite(r);

                ctx._position += stuff.Length;

                doneCallback();
            };
            _stream.BeginWrite(stuff, ctx._position, stuff.Length, wroteCallback, null);
        }

        #endregion

        public void Read(MtStreamReadingContext ctx, Action<byte[]> readStuffCallback, Action doneCallback)
        {
            var buffer = new byte[1024];

            AsyncCallback readCallback = null;

            readCallback = r => 
                {
                    var count = _stream.EndRead(r);

                    // Create a new array, copy read bytes to there
                    var read = new byte[count];
                    if (count > 0)
                    {
                        // Update position
                        ctx._position += count;

                        // Copy buffer to final count array
                        Array.Copy(buffer, read, count);

                        readStuffCallback(read);

                        // Continue ...
                        _stream.BeginRead(buffer, ctx._position, ReadBufferSize, readCallback, null);
                    }
                    else if (count == 0)
                    {
                        doneCallback();
                    }
                    else
                    {
                        throw new Exception("Ups! Error reading stream...");
                    }
                };
            //--

            _stream.BeginRead(buffer, ctx._position, ReadBufferSize, readCallback, null);
        }



    }
}
