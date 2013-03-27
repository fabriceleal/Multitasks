// This file was generated automatically
// Do not change it manually, otherwise you'll lose all your hard work!
using System;
using System.IO;

namespace SilverlightMultitasksDemo.Examples
{

	public class Example
	{
		public string Source { get; private set; }
		public string Title { get; private set; }

		public Example(string title, string source)
		{
			Title = title;
			Source = source;
		}
	}

    public class ExampleList
    {		

        static Example[] _examples = new Example[]{
			 new Example("Hello World", "\"Hello World!\" | print(_);")
			,  new Example("Hello With Fork", "{\r\n    \"h\" | print(_);\r\n    \"e\" | print(_);\r\n    \"l\" | print(_);\r\n    \"l\" | print(_);\r\n    \"o\" | print(_);\r\n};")
			,  new Example("Binds", "a <= 1 | b <= 2 | print(a);")
			,  new Example("Print And Bind Print", "print(\"Hello World!\");\r\na <= 1 | {\r\n    print(a);\r\n    b <= 2 | {\r\n        print(b);\r\n    };\r\n};\r\n")
			,  new Example("Sleep And Print", "{\r\n	sleep(5000) | wait(_) | print(\"Hello\");\r\n	sleep(1000) | wait(_) | print(\"World\");\r\n};")
			,  new Example("Sleep And Add", "a <= sleep(5000, 2) | b <= sleep(5000, 1) | c <= sleep(5000, 3) | print(add(a, b, c));\r\n")
			,  new Example("If And Prints", "if TRUE\r\n	print(\"this will be printed\");\r\n	print(\"this will never be reached\");\r\n;")
			,  new Example("Bool Constants Print", "TRUE  | print(_);\r\nFALSE | print(_);")
			,  new Example("Anonymous Factorial", "\r\na <= \r\n    L (b) => if zero(b)\r\n                1;\r\n                mult(b, a(subt(b, 1)));\r\n             ;\r\n     | print(a(5));\r\n")
			,  new Example("Fork 1", "{\r\n	print(\"1\");\r\n};")
			,  new Example("Fork 2", "{\r\n	print(\"1\");\r\n	print(\"2\");\r\n};")
			,  new Example("Fork 3", "{\r\n	print(\"1\");\r\n	print(\"2\");\r\n	print(\"3\");\r\n};")
			,  new Example("Fork 4", "{\r\n	print(\"1\");\r\n	print(\"2\");\r\n	print(\"3\");\r\n	print(\"4\");\r\n};")
			,  new Example("Fork 5", "{\r\n	print(\"1\");\r\n	print(\"2\");\r\n	print(\"3\");\r\n	print(\"4\");\r\n	print(\"5\");\r\n};")
			,  new Example("Fork 6", "{\r\n	print(\"1\");\r\n	print(\"2\");\r\n	print(\"3\");\r\n	print(\"4\");\r\n	print(\"5\");\r\n	print(\"6\");\r\n};")
			,  new Example("Fork 7", "{\r\n	print(\"1\");\r\n	print(\"2\");\r\n	print(\"3\");\r\n	print(\"4\");\r\n	print(\"5\");\r\n	print(\"6\");\r\n	print(\"7\");\r\n};")
			,  new Example("Fork 8", "{\r\n	print(\"1\");\r\n	print(\"2\");\r\n	print(\"3\");\r\n	print(\"4\");\r\n	print(\"5\");\r\n	print(\"6\");\r\n	print(\"7\");\r\n	print(\"8\");\r\n};")
			,  new Example("Call Anonymous", "\r\n\r\na <= \r\n	L (b) => b; | print(a(5));")
			,  new Example("Arrays", "[1, 2, 3, 4];")
			,  new Example("Arrays Car", "[1, 2, 3, 4] | car(_) | print(_);")
			,  new Example("Arrays Cdr", "\r\n[1, 2, 3, 4] | cdr(_) | car(_) | print(_);\r\n")
			,  new Example("Array Mapped Print", "\r\n[1,2,3,4,5] | map(_, print);\r\n")
			,  new Example("Nested Array Map Map", "\r\na <= [[1,2], [3, 4], [5, 6]] | \r\n    b <= L (b) => map(b, print); |\r\n        map(a, b);\r\n")
			,  new Example("String Stream Literal", "\r\nstr_stream(\"Hello World\");\r\n")
			,  new Example("Binds And Fork", "a <= 1 |\r\nb <= 2 |\r\nc <= 3 |\r\nd <= 4 |\r\n{\r\n	print(a);\r\n	print(b);\r\n	print(c);\r\n	print(d);\r\n};")
			,  new Example("Closure 1", "\r\na <= 1 |\r\n	b <= L (b) => add(b, a); |\r\n		a <= 3 |\r\n			print(b(4));\r\n")
			,  new Example("Closure 2", "\r\na <= 1 | c <= 2 | \r\n{\r\n	b <= L (b) => add(b, c, a); |\r\n		a <= 3 |\r\n			print(b(4));\r\n\r\n	b <= L (b) => add(b, 10, c, a); |\r\n			print(b(4));\r\n};\r\n\r\n")
		};

		public static Example[] All
		{
			get {				
				return _examples;
			}
		}


	}

}
