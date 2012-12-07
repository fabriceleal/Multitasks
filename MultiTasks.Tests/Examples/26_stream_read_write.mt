
a <= str_stream("Hello World")
| b <= uri_stream("26_output_test.txt")
| b <- a
| close_stream(b, a);