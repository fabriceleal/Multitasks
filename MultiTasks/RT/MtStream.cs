using System;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace MultiTasks.RT
{
    public abstract class MtStream
    {

        public const int ReadBufferSize = 256;

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
            if (!_stream.CanRead)
            {
                throw new Exception("You can't read from this stream!");
            }
            var r = new MtStreamReadingContext(0);
            return r;
        }


        public static void ReadFromWriteTo(MtStream src, MtStream dest, Action doneCallback)
        {
#if DEBUG && !SILVERLIGHT
            Debug.Print("ReadFrom x to y");
#endif

            try
            {
                var readCtx = src.CreateReadingContext();
                var writCtx = dest.CreateWritingContext();

                Action<byte[], bool> readCallback = null;
                readCallback = (bytes, atEnd) =>
                {
                    dest.Write(writCtx, bytes);

                    // Continue only if there are more bytes to read ...
                    if (!atEnd)
                    {
                        src.Read(readCtx, readCallback, doneCallback);
                    }
                };

                src.Read(readCtx, readCallback, doneCallback);
            }
            catch (Exception e)
            {
                throw new Exception("Exception on Stream.readFromTo", e);
            }
        }

        public MtStreamWritingContext CreateWritingContext()
        {
            if (!_stream.CanWrite)
            {
                throw new Exception("You can't write in this stream!");
            }
            var w = new MtStreamWritingContext(0);
            return w;
        }

        #region Write

        public void Write(MtStreamWritingContext ctx, byte[] stuff)
        {
#if DEBUG && !SILVERLIGHT
            Debug.Print("Write");
#endif

            try
            {
                AsyncCallback wroteCallback = null;
                wroteCallback = r =>
                {
                    _stream.EndWrite(r);

                    ctx._position += stuff.Length;
                };

                // Put cursor in the right place
                //_stream.Position = ctx._position;
                // TODO Fix this, for now ignore (assume cursor on the right place)

#if DEBUG && !SILVERLIGHT                
                MultiTasksRuntime.DebugDisplayInfo();
#endif
                _stream.BeginWrite(stuff, 0, stuff.Length, wroteCallback, null);
            }
            catch (Exception e)
            {
                throw new Exception("Exception on Stream.write", e);
            }
        }

        public void Write(MtStreamWritingContext ctx, byte[] stuff, Action doneCallback)
        {
#if DEBUG && !SILVERLIGHT
            Debug.Print("Write");
#endif
            
            try
            {
                AsyncCallback wroteCallback = null;
                wroteCallback = r =>
                {
                    _stream.EndWrite(r);

                    ctx._position += stuff.Length;

                    doneCallback();
                };

                // Put cursor in the right place
                _stream.Position = ctx._position;

#if DEBUG && !SILVERLIGHT
                MultiTasksRuntime.DebugDisplayInfo();
#endif
                _stream.BeginWrite(stuff, 0, stuff.Length, wroteCallback, null);
            }
            catch (Exception e)
            {
                throw new Exception("Exception on Stream.write", e);
            }
        }

        #endregion

        public void Read(MtStreamReadingContext ctx, Action<byte[], bool> readStuffCallback, Action doneCallback)
        {
#if DEBUG && !SILVERLIGHT
            Debug.Print("Read");
#endif
            
            try
            {

                var buffer = new byte[ReadBufferSize];

                AsyncCallback readCallback = null;

                readCallback = r =>
                {
                    var count = _stream.EndRead(r);

                    // Create a new array, copy read bytes to there
                    var read = new byte[count];

                    // Update position
                    ctx._position += count;

                    if (count > 0)
                    {
                        // Copy buffer to final count array
                        Array.Copy(buffer, read, count);

                        readStuffCallback(read, ctx._position >= _stream.Length);
                    }

                    if (ctx._position >= _stream.Length)
                    {
                        doneCallback();
                    }
                };

                if (ctx._position >= _stream.Length)
                {
                    throw new Exception("Can't read further, end of stream!");
                }

                // Put cursor in the right place
                _stream.Position = ctx._position;

#if DEBUG && !SILVERLIGHT
                MultiTasksRuntime.DebugDisplayInfo();
#endif
                _stream.BeginRead(buffer, 0, ReadBufferSize, readCallback, null);    
            }
            catch (Exception e)
            {
                throw new Exception("Exception on Stream.read.", e);
            }
        }

        public void Close()
        {
            _stream.Flush();
            _stream.Close();
            _stream.Dispose();
        }

    }
}
