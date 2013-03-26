using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Text;

namespace SilverlightMultitasksDemo
{
    static class _
    {
        public static string ExtFormat(this string fmt, params object[] args)
        {
            return string.Format(fmt, args);
        }

        public static void WriteUTF8(this MemoryStream self, string str)
        {
            var bytes_str = Encoding.UTF8.GetBytes(str);
            self.Write(bytes_str, 0, bytes_str.Length);
        }
    }
}
