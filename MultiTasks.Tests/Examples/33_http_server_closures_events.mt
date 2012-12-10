a <= 1
| b <= 2
| c <= 3
| server <= http_server("http://127.0.0.1:7654/")
| server  on start    L(server) => print(a);
| server  on stop     L(server) => print(c);
| server  on context  L(server, req, resp) => print(b);
| r <= http_server_start(server)
| wait(r)
| r <= sleep(15000)
| wait(r)
| r <= http_server_stop(server)
| wait(r)
;