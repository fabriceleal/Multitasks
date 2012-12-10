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
			
				using(FileStream str = new FileStream("01_hello_world.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
			
				using(FileStream str = new FileStream("02_hello_with_fork.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
			
				using(FileStream str = new FileStream("03_binds.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
			
				using(FileStream str = new FileStream("04_print_and_bind_print.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
			
				using(FileStream str = new FileStream("05_sleep_and_print.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
			
				using(FileStream str = new FileStream("06_sleep_and_add.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval07IfAndPrints()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("07_if_and_prints.mt");
			
				using(FileStream str = new FileStream("07_if_and_prints.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval08BoolConstantsPrint()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("08_bool_constants_print.mt");
			
				using(FileStream str = new FileStream("08_bool_constants_print.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval10AnonymousFactorial()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("10_anonymous_factorial.mt");
			
				using(FileStream str = new FileStream("10_anonymous_factorial.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval11Fork1()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("11_fork_1.mt");
			
				using(FileStream str = new FileStream("11_fork_1.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval12Fork2()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("12_fork_2.mt");
			
				using(FileStream str = new FileStream("12_fork_2.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval13Fork3()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("13_fork_3.mt");
			
				using(FileStream str = new FileStream("13_fork_3.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval14Fork4()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("14_fork_4.mt");
			
				using(FileStream str = new FileStream("14_fork_4.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval15Fork5()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("15_fork_5.mt");
			
				using(FileStream str = new FileStream("15_fork_5.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval16Fork6()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("16_fork_6.mt");
			
				using(FileStream str = new FileStream("16_fork_6.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval17Fork7()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("17_fork_7.mt");
			
				using(FileStream str = new FileStream("17_fork_7.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval18Fork8()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("18_fork_8.mt");
			
				using(FileStream str = new FileStream("18_fork_8.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval19CallAnonymous()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("19_call_anonymous.mt");
			
				using(FileStream str = new FileStream("19_call_anonymous.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval20Arrays()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("20_arrays.mt");
			
				using(FileStream str = new FileStream("20_arrays.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval21ArraysCar()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("21_arrays_car.mt");
			
				using(FileStream str = new FileStream("21_arrays_car.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval22ArraysCdr()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("22_arrays_cdr.mt");
			
				using(FileStream str = new FileStream("22_arrays_cdr.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval23ArrayMappedPrint()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("23_array_mapped_print.mt");
			
				using(FileStream str = new FileStream("23_array_mapped_print.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval24NestedArrayMapMap()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("24_nested_array_map_map.mt");
			
				using(FileStream str = new FileStream("24_nested_array_map_map.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval25StreamLiteral()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("25_stream_literal.mt");
			
				using(FileStream str = new FileStream("25_stream_literal.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval26StreamReadWrite()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("26_stream_read_write.mt");
			
				using(FileStream str = new FileStream("26_stream_read_write.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval27HttpServer()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("27_http_server.mt");
			
				using(FileStream str = new FileStream("27_http_server.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval28HttpServerEvents()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("28_http_server_events.mt");
			
				using(FileStream str = new FileStream("28_http_server_events.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval29HttpServerComplete()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("29_http_server_complete.mt");
			
				using(FileStream str = new FileStream("29_http_server_complete.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval30BindsAndFork()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("30_binds_and_fork.mt");
			
				using(FileStream str = new FileStream("30_binds_and_fork.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval31Closure1()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("31_closure_1.mt");
			
				using(FileStream str = new FileStream("31_closure_1.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval32Closure2()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("32_closure_2.mt");
			
				using(FileStream str = new FileStream("32_closure_2.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval33HttpServerClosuresEvents()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("33_http_server_closures_events.mt");
			
				using(FileStream str = new FileStream("33_http_server_closures_events.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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
		public void Eval34HttpServerSimple()
		{
			try 
			{				
				// Read file content (as embedded resource)
				var src = Utils.ReadSourceFileContent("34_http_server_simple.mt");
			
				using(FileStream str = new FileStream("34_http_server_simple.mt.log", FileMode.OpenOrCreate, FileAccess.Write))
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