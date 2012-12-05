using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using MultiTasks.RT;

namespace MultiTasks.Tests
{
    [TestClass]
    public class TestsEvaluation
    {
        [TestMethod]
        public void EvalHelloWorldPrintPipedToIdentityPipedToApp()
        {
            var stream = new MemoryStream();
            var app = MtCompiler.CreateScriptApp(stream);
            
            var res = app.Evaluate("print | identity(_) | _(\"Hello World!\");\n") as MtResult;
            Assert.IsNotNull(res);

            res.GetValueSync((o) => { /* Do nothing */ });
            
            String expected = string.Format("Hello World!{0}", Environment.NewLine);
            String outputed = Encoding.UTF8.GetString(stream.ToArray());

            Assert.AreEqual<string>(expected, outputed, string.Format("Actualy is '{0}'", outputed));
        }

        [TestMethod]
        public void EvalHelloWorldPipedToPrint()
        {
            var stream = new MemoryStream();
            var app = MtCompiler.CreateScriptApp(stream);
            var res = app.Evaluate("\"Hello World!\" | print(_);\n") as MtResult;
            Assert.IsNotNull(res);
            res.GetValueSync((o) => { /* Do nothing */ });

            String expected = string.Format("Hello World!{0}", Environment.NewLine);
            String outputed = Encoding.UTF8.GetString(stream.ToArray());

            Assert.AreEqual<string>(expected, outputed, string.Format("Actualy is '{0}'", outputed));
        }

        [TestMethod]
        public void EvalHelloWorldPrintPipedToApp()
        {
            var stream = new MemoryStream();
            var app = MtCompiler.CreateScriptApp(stream);
            var res = app.Evaluate("print | _(\"Hello World!\");\n") as MtResult;
            Assert.IsNotNull(res);
            res.GetValueSync((o) => { /* Do nothing */ });

            String expected = string.Format("Hello World!{0}", Environment.NewLine);
            String outputed = Encoding.UTF8.GetString(stream.ToArray());

            Assert.AreEqual<string>(expected, outputed, string.Format("Actualy is '{0}'", outputed));
        }

        [TestMethod]
        public void EvalHelloWorldApp()
        {
            var stream = new MemoryStream();
            var app = MtCompiler.CreateScriptApp(stream);
            var res = app.Evaluate("print(\"Hello World!\");\n") as MtResult;
            Assert.IsNotNull(res);
            res.GetValueSync((o) => { /* Do nothing */ });

            String expected = string.Format("Hello World!{0}", Environment.NewLine);
            String outputed = Encoding.UTF8.GetString(stream.ToArray());

            Assert.AreEqual<string>(expected, outputed, string.Format("Actualy is '{0}'", outputed));
        }
    }
}
