using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiTasks;
using Irony.Parsing;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using System.Threading;
using MultiTasks.RT;

namespace multitasks
{
    class Program
    {
        static void Main(string[] args)
        {
            var src = "\n" +
                "print | identity(_) | _(\"Hello World 1!\");\n" +
                "\"Hello World 2!\" | print(_);\n" +
                "print | _(\"Hello World 3!\");\n" +
                "print(\"Hello World 4!\");\n" +
                "\n";
            //--

            var app = MtCompiler.CreateScriptApp(Console.OpenStandardOutput());

            var wait = app.Evaluate(src) as MtResult;

            wait.GetValueSync((o) => { });
            
            Console.WriteLine("Press any to end...");
            Console.ReadKey();
        }

    }
}
