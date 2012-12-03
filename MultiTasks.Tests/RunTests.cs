using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;
using MultiTasks.RT;

namespace MultiTasks.Tests
{
    /// <summary>
    /// Summary description for RunTests
    /// </summary>
    [TestClass]
    public class RunTests
    {
        public KeyValuePair<string, string>[] Sources;

        [TestInitialize]
        public void Initialize()
        {
            var ls = new List<KeyValuePair<string, string>>();
            foreach (var resourceName in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if(resourceName.EndsWith(".mt")) {
                    using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                    {
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, (int)stream.Length);

                        ls.Add(new KeyValuePair<string, string>(resourceName, Encoding.UTF8.GetString(buffer)));
                    }
                }                
            }
            Sources = ls.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Freaking weird - save files as ANSI, UTF8 headers result in syntax errors (bytes 239, 187, 191)
        /// </remarks>
        [TestMethod]
        public void TestAllExamples()
        {
            if (Sources == null)
                throw new Exception("Exception initializing sources.");

            foreach (var src in Sources) 
            {
                using (MemoryStream st = new MemoryStream())
                {
                    var compiler = MtCompiler.CreateScriptApp(st);
                    MtResult res = null;
                    try
                    {
                        res = compiler.Evaluate("\"Hello World!\" | print(_);") as MtResult;
                        Assert.IsNotNull(res, "Error evaluating example " + src.Key + ".");
                        res.GetValueSync((o) => { });
                    }
                    catch (Exception e)
                    {
                        Assert.Fail( src.Key + " : " + e.Message);
                    }                    
                }
            }
        }
    }
}
