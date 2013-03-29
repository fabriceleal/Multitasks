using System;
using System.Diagnostics;
using MultiTasks;
using MultiTasks.RT;
using System.IO;

namespace multitasks
{
    class Program
    {

        /// <summary>
        /// Exists with supplied exit code
        /// </summary>
        /// <param name="exitCode"></param>
        static void PrintUsageAndExit(int exitCode)
        {
            TextWriter destiny = exitCode == 0 ? Console.Out : Console.Error;

            destiny.WriteLine("Usage:");
            destiny.WriteLine("\t mts <filename>");
            destiny.WriteLine("\t mts -h");                

            Environment.Exit(exitCode);
        }

        private static string _SEP = new String('*', 10);
        /// <summary>
        /// Exits with exit code -2
        /// </summary>
        /// <param name="e"></param>
        static void PrintExceptionAndExit(Exception e)
        {
            Exception _e = e;

            Console.Error.WriteLine("Exception:");
            while (_e != null)
            {
                Console.Error.WriteLine(_e.Message);
                Console.Error.WriteLine(_e.Source);
                Console.Error.WriteLine(_e.StackTrace);
                Console.Error.WriteLine(_SEP);
                _e = _e.InnerException;
            }

            Environment.Exit(-2);
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
                PrintUsageAndExit(-1);

            if (args[0] == "-h")
                PrintUsageAndExit(0);

            try
            {
#if DEBUG
                if (!Debugger.IsAttached)
                {
                    Console.Error.WriteLine("This is a debug exe.");
                    
                    Console.Error.WriteLine("Do you want to attach the debugger? (y/N)");                    
                    var answer = Console.ReadLine();
                    // I know this looks horrible
                    if (answer.ToLower().StartsWith("y"))
                    {
                        Debugger.Launch();
                    }

                    Console.Error.WriteLine("Do you want redirect Debug.Print to Console.Error? (y/N)");
                    answer = Console.ReadLine();
                    // I know this also looks horrible
                    if (answer.ToLower().StartsWith("y"))
                    {
                        // Redirect Debug.Print/WriteLines to console.error
                        var consoleListener = new ConsoleTraceListener(true);
                        Debug.Listeners.Add(consoleListener);
                    }                    
                }
#endif

                // Read file
                var src = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, args[0]));

                var app = MtCompiler.CreateScriptApp(Console.OpenStandardOutput());

                // Evaluate program
                var wait = app.Evaluate(src) as MtResult;
                if (wait == null)
                {
                    throw new Exception("Evaluate(src) returned null. WTF??");
                }

                // Wait for the end of the program
                wait.GetValueSync((o) => { });                
            }
            catch (Exception e)
            {
                PrintExceptionAndExit(e);
            }
        }

    }
}
