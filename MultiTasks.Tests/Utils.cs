using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace MultiTasks.Tests
{
    public class Utils
    {

        static public string ReadSourceFileContent(string resourceName)
        {
            resourceName = "MultiTasks.Tests.Examples." + resourceName;
            try
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, (int)stream.Length);
                    return Encoding.UTF8.GetString(buffer);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Exception reading embedded file " + resourceName, e);
            }
        }
    }
}
