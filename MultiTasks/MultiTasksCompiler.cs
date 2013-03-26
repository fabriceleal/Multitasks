using System;
using Irony.Parsing;
using Irony.Interpreter;
using System.IO;
using MultiTasks.RT;
using System.Threading;
using System.Diagnostics;

namespace MultiTasks
{
    public class MtCompiler
    {
        private ScriptApp _scriptApp;

        protected MtCompiler(ScriptApp scriptApp)
        {
            _scriptApp = scriptApp;
        }

        public MtResult Evaluate(string str)
        {
            try
            {
                return _scriptApp.Evaluate(str) as MtResult;
            }
            catch (Exception e)
            {
                throw new Exception("Exception evaluating script.", e);
            }
        }

        protected static MtCompiler InternCreateScriptApp(Stream output, Stream debugStream)
        {
            var grammar = new MtGrammar();
            var lang = new LanguageData(grammar);
            var parser = new Parser(grammar);
            var runtime = grammar.CreateRuntime(lang) as MultiTasksRuntime;

            if (output != null)
            {
                runtime.OutputStream = output;
            }
#if DEBUG && !SILVERLIGHT

            if (debugStream != null)
            {
                // Add as a listener to debug
                var listener = new TextWriterTraceListener(debugStream);
                Debug.Listeners.Add(listener);
            }

#endif
            var app = new ScriptApp(runtime);

            // Add constants
            app.Globals.Add("TRUE", MtResult.True);
            app.Globals.Add("FALSE", MtResult.False);

#if DEBUG && !SILVERLIGHT

            // Get thread pool info
            {               
                int workerThreads = -1, completionPortThreads = -1;

                ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
                Debug.Print("ThreadPool Info - Max threads - Worker: {0} Completion Port: {1}", workerThreads, completionPortThreads);

                workerThreads = -1;
                completionPortThreads = -1;

                ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);
                Debug.Print("ThreadPool Info - Min threads - Worker: {0} Completion Port: {1}", workerThreads, completionPortThreads);
            }
#endif

            return new MtCompiler(app);
        }

        public static MtCompiler CreateScriptApp()
        {
            return InternCreateScriptApp(null, null);
        }

        public static MtCompiler CreateScriptApp(Stream output)
        {
            if (output == null)
            {
                throw new NullReferenceException("I need a stream!");
            }

            if (!output.CanWrite)
            {
                throw new Exception("Stream is not writable!");
            }

            return InternCreateScriptApp(output, null);
        }
    }
}
