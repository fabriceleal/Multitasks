using System.IO;

namespace MultiTasks.RT
{
    public class MtStreamWrapper : MtStream
    {
        public MtStreamWrapper(Stream stream) : base(stream) { }
    }
}
