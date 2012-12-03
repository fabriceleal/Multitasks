// This file was generated automatically
// Do not change it manually, otherwise you'll lose all your hard work!
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiTasks.RT;

namespace MultiTasks.Tests
{

    [TestClass]
    public class TestsForSources
    {
		
		[TestMethod]
		public void Eval01HelloWorld()
		{
			// Read file content (as embedded resource)
			var src = Utils.ReadSourceFileContent("01_hello_world");
			
			// Compile
			var res = MtCompiler.CreateScriptApp().Evaluate(src) as MtResult;

			// Tests ...
			Assert.IsNotNull(res);

			// Wait for end
			res.GetValueSync((o) => { });

			// ... More tests
		}

		
		[TestMethod]
		public void Eval02HelloWithFork()
		{
			// Read file content (as embedded resource)
			var src = Utils.ReadSourceFileContent("02_hello_with_fork");
			
			// Compile
			var res = MtCompiler.CreateScriptApp().Evaluate(src) as MtResult;

			// Tests ...
			Assert.IsNotNull(res);

			// Wait for end
			res.GetValueSync((o) => { });

			// ... More tests
		}

		
		[TestMethod]
		public void Eval03Binds()
		{
			// Read file content (as embedded resource)
			var src = Utils.ReadSourceFileContent("03_binds");
			
			// Compile
			var res = MtCompiler.CreateScriptApp().Evaluate(src) as MtResult;

			// Tests ...
			Assert.IsNotNull(res);

			// Wait for end
			res.GetValueSync((o) => { });

			// ... More tests
		}

	}

}