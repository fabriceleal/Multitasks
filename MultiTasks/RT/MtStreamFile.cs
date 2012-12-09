using System.IO;

namespace MultiTasks.RT
{
    public class MtStreamFile : MtStream
    {
        private string _filename;

        public MtStreamFile(string filename) : base(new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            _filename = filename;
        }
    }
}
