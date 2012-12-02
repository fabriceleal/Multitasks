using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiTasks;
using Irony.Parsing;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using System.Threading;

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

            var app = MtCompiler.CreateScriptApp();

            app.ConsoleWrite += new EventHandler<ConsoleWriteEventArgs>(app_ConsoleWrite);
            
            var wait = app.Evaluate(src);

            //Console.WriteLine("Wait some seconds...");
            //Thread.Sleep(5000);
            Console.WriteLine("Press any to end...");
            Console.ReadKey();
        }

        static void app_ConsoleWrite(object sender, ConsoleWriteEventArgs e)
        {
            Console.WriteLine(e.Text);
        }
    }
}
