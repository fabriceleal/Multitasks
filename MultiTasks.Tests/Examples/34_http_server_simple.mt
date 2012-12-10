a <= 1
| b <= 2
| c <= 3
| server <= http_server("http://127.0.0.1:7654/")
| server  on start    L(server) => print("started");
| server  on stop     L(server) => print("stopped");
| server  on context  L(server, request, response) => 
        http_set_code(response, 202) 
        | st <= http_stream(response) 
        | t <= str_stream("hello world") 
        | st <- t 
        | wait(_) 
        | http_end(response);
| r <= http_server_start(server)
| wait(r)
| r <= sleep(15000)
| wait(r)
| r <= http_server_stop(server)
| wait(r)
;