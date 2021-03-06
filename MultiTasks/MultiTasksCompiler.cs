﻿using System;
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
                var ret = _scriptApp.Evaluate(str);
                if (ret == null)
                {
                    throw new Exception("scriptApp.Evalute returned null.");
                }
                var wrkRet = ret as MtResult;
                if (wrkRet == null)
                {
                    throw new Exception("scriptApp.Evalute returned a non-MtResult.");
                }
                return wrkRet;
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
            MultiTasksRuntime.DebugDisplayInfo();
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
