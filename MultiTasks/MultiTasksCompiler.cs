using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using Irony.Interpreter;
using System.IO;
using MultiTasks.RT;

namespace MultiTasks
{
    public class MtCompiler
    {
        protected static ScriptApp _CreateScriptApp(Stream output, Stream debugStream)
        {
            var grammar = new MtGrammar();
            var lang = new LanguageData(grammar);
            var parser = new Parser(grammar);
            var runtime = grammar.CreateRuntime(lang) as MultiTasksRuntime;
            if (output != null)
            {
                runtime.OutputStream = output;
            }
            var app = new ScriptApp(runtime);

            app.Globals.Add("TRUE", MtResult.True);
            app.Globals.Add("FALSE", MtResult.False);

            return app;
        }
        
        public static ScriptApp CreateScriptApp()
        {
            return _CreateScriptApp(null, null);
        }

        public static ScriptApp CreateScriptApp(Stream output)
        {
            if (output == null)
                throw new NullReferenceException("I need a stream!");

            if (!output.CanWrite)
                throw new Exception("Stream is not writable!");

            return _CreateScriptApp(output, null);
        }
    }
}
