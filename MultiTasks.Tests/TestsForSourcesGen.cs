// This file was generated automatically
// Do not change it manually, otherwise you'll lose all your hard work!
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiTasks.RT;
using System;
using System.IO;

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
			
				using(Stream str = new FileStream("01_hello_world.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
				{
					// Compile
					var res = MtCompiler.CreateScriptApp(str).Evaluate(src) as MtResult;

					// Tests ...
					Assert.IsNotNull(res);

					// Wait for end
					res.GetValueSync((o) => { });

					// ... More tests
				}				
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
			
				using(Stream str = new FileStream("02_hello_with_fork.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
				{
					// Compile
					var res = MtCompiler.CreateScriptApp(str).Evaluate(src) as MtResult;

					// Tests ...
					Assert.IsNotNull(res);

					// Wait for end
					res.GetValueSync((o) => { });

					// ... More tests
				}				
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
			
				using(Stream str = new FileStream("03_binds.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
				{
					// Compile
					var res = MtCompiler.CreateScriptApp(str).Evaluate(src) as MtResult;

					// Tests ...
					Assert.IsNotNull(res);

					// Wait for end
					res.GetValueSync((o) => { });

					// ... More tests
				}				
			} 
			catch(Exception e)
			{
				Assert.Fail(e.Message);
			}
		}

		
		[TestMethod]
		public void Eval04PrintAndBindPrint()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("04_print_and_bind_print.mt");
			
				using(Stream str = new FileStream("04_print_and_bind_print.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
				{
					// Compile
					var res = MtCompiler.CreateScriptApp(str).Evaluate(src) as MtResult;

					// Tests ...
					Assert.IsNotNull(res);

					// Wait for end
					res.GetValueSync((o) => { });

					// ... More tests
				}				
			} 
			catch(Exception e)
			{
				Assert.Fail(e.Message);
			}
		}

		
		[TestMethod]
		public void Eval05SleepAndPrint()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("05_sleep_and_print.mt");
			
				using(Stream str = new FileStream("05_sleep_and_print.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
				{
					// Compile
					var res = MtCompiler.CreateScriptApp(str).Evaluate(src) as MtResult;

					// Tests ...
					Assert.IsNotNull(res);

					// Wait for end
					res.GetValueSync((o) => { });

					// ... More tests
				}				
			} 
			catch(Exception e)
			{
				Assert.Fail(e.Message);
			}
		}

		
		[TestMethod]
		public void Eval06SleepAndAdd()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("06_sleep_and_add.mt");
			
				using(Stream str = new FileStream("06_sleep_and_add.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
				{
					// Compile
					var res = MtCompiler.CreateScriptApp(str).Evaluate(src) as MtResult;

					// Tests ...
					Assert.IsNotNull(res);

					// Wait for end
					res.GetValueSync((o) => { });

					// ... More tests
				}				
			} 
			catch(Exception e)
			{
				Assert.Fail(e.Message);
			}
		}

	}

}