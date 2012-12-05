using System;
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

        /// <summary>
        /// Exits with exit code -2
        /// </summary>
        /// <param name="e"></param>
        static void PrintExceptionAndExit(Exception e)
        {
            Exception _e = e;

            Console.Error.WriteLine("Exception!");
            while (_e != null)
            {
                Console.Error.WriteLine(e.Message);
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
                // Read file
                var src = File.ReadAllText(args[0]);

                var app = MtCompiler.CreateScriptApp(Console.OpenStandardOutput());

                // Evaluate program
                var wait = app.Evaluate(src) as MtResult;
                
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
