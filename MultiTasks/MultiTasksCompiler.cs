using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using Irony.Interpreter;
using System.IO;

namespace MultiTasks
{
    public class MtCompiler
    {
        public static ScriptApp CreateScriptApp()
        {
            var grammar = new MtGrammar();
            var lang = new LanguageData(grammar);
            var parser = new Parser(grammar);
            var runtime = grammar.CreateRuntime(lang);
            var app = new ScriptApp(runtime);
            return app;
        }

        public static ScriptApp CreateScriptApp(Stream output)
        {
            if (output == null)
                throw new NullReferenceException("I need a stream!");

            if (!output.CanWrite)
                throw new Exception("Stream is not writable!");

            var grammar = new MtGrammar();
            var lang = new LanguageData(grammar);
            var parser = new Parser(grammar);
            var runtime = grammar.CreateRuntime(lang) as MultiTasksRuntime;
            runtime.OutputStream = output;
            var app = new ScriptApp(runtime);
            return app;
        }
    }
}
