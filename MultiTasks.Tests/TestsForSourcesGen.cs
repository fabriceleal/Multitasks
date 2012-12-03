// This file was generated automatically
// Do not change it manually, otherwise you'll lose all your hard work!
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiTasks.RT;
using System;

namespace MultiTasks.Tests
{

    [TestClass]
    public class TestsForSources
    {
		
		[TestMethod]
		public void Eval01HelloWorld()
		{
			try 
			{
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("01_hello_world.mt");
			
				// Compile
				var res = MtCompiler.CreateScriptApp().Evaluate(src) as MtResult;

				// Tests ...
				Assert.IsNotNull(res);

				// Wait for end
				res.GetValueSync((o) => { });

				// ... More tests
			} 
			catch(Exception e)
			{
				Assert.Fail(e.Message);
			}
		}

		
		[TestMethod]
		public void Eval02HelloWithFork()
		{
			try 
			{
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("02_hello_with_fork.mt");
			
				// Compile
				var res = MtCompiler.CreateScriptApp().Evaluate(src) as MtResult;

				// Tests ...
				Assert.IsNotNull(res);

				// Wait for end
				res.GetValueSync((o) => { });

				// ... More tests
			} 
			catch(Exception e)
			{
				Assert.Fail(e.Message);
			}
		}

		
		[TestMethod]
		public void Eval03Binds()
		{
			try 
			{
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("03_binds.mt");
			
				// Compile
				var res = MtCompiler.CreateScriptApp().Evaluate(src) as MtResult;

				// Tests ...
				Assert.IsNotNull(res);

				// Wait for end
				res.GetValueSync((o) => { });

				// ... More tests
			} 
			catch(Exception e)
			{
				Assert.Fail(e.Message);
			}
		}

	}

}