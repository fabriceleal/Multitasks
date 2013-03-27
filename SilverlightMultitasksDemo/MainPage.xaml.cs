using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Text;
using MultiTasks;
using System.Threading;

using SilverlightMultitasksDemo.Examples;

namespace SilverlightMultitasksDemo
{    
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            
            // Add examples
            foreach (var example in ExampleList.All)
            {
                AddExample(example.Title, example.Source);
            }                
        }

        private Thread t;

#region Examples

        private void AddExample(string title, string src)
        {
            var bt = new HyperlinkButton();
            bt.Content = title;
            bt.Click += GetHandlerForExample(src);
            ExamplesStack.Children.Add(bt);
        }

        RoutedEventHandler GetHandlerForExample(string srcExample)
        {
            return (object sender, RoutedEventArgs e) =>
            {
                if (t != null && t.IsAlive)
                    return;

                txCode.Text = srcExample;
            };
        }

#endregion

        private void WriteStuff(string text)
        {   
            Dispatcher.BeginInvoke(() => { txOutput.Text += text; });
        }

        private class MemoryStreamWithNotifications : MemoryStream
        {

            public delegate void WritedEvent(byte[] buffer, int offset, int length);

            public event WritedEvent Writed;

            private object _writeLock = new object();

            public override void Write(byte[] buffer, int offset, int length)
            {
                lock (_writeLock)
                {
                    base.Write(buffer, offset, length);

                    if (Writed != null)
                    {
                        Writed(buffer, offset, length);
                    }
                }
            }
        }

        private void Exec(object param)
        {
            var out_stream = new MemoryStreamWithNotifications();
            out_stream.Writed += out_stream_Writed;
            
            out_stream.WriteUTF8("[INFO] Starting...\r\n");

            var start = DateTime.Now;

            try
            {
                // This is fast
                var compiler = MtCompiler.CreateScriptApp(out_stream);

                // This may take a while
                var res = compiler.Evaluate(param as string);
                if (res == null)
                {
                    throw new Exception("Evaluation returned null. WTF!?");
                }

                // This may take a LONG time
                res.WaitForValue();

                var dur = DateTime.Now - start;

                out_stream.WriteUTF8("[INFO] Completed in {0} seconds.\r\n".ExtFormat(dur.TotalSeconds));
            }
            catch (ThreadAbortException)
            {
                var dur = DateTime.Now - start;
                out_stream.WriteUTF8("[ABORTED] After {0} seconds.\r\n".ExtFormat(dur.TotalSeconds));
            }
            catch (Exception ex)
            {
                out_stream.WriteUTF8("[ERROR] Exception executing script {0}.\r\n".ExtFormat(ex.Message));
            }
            finally
            {
                Done();
            }
        }

        void Done()
        {
            Dispatcher.BeginInvoke(() =>
            {
                // All is OK
                t = null;
                txCode.IsEnabled = true;
            });
        }

        void out_stream_Writed(byte[] buffer, int offset, int length)
        {
            WriteStuff(Encoding.UTF8.GetString(buffer, offset, length));
        }

        private void btExecute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Disable everything, to avoid click-heroes
                txCode.IsEnabled = false;
                
                txOutput.Text = "";

                if (t != null && t.IsAlive)
                    throw new Exception("There's something running, wait for a while ...");

                t = new Thread(Exec);                
                t.Start((object) txCode.Text);               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().FullName, MessageBoxButton.OK);
                if (t != null)
                {
                    if (t.IsAlive)
                    {
                        // Do something to cancel the thread. WHAT???
                    }
                }
            }            
        }

        //private void btCancel_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (t != null) 
        //        {
        //            if (t.IsAlive)
        //            {
        //                // Do something to cancel the thread. WHAT???
                        
        //            }
        //            t = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, ex.GetType().FullName, MessageBoxButton.OK);
        //    }
        //}
    
    }
}
