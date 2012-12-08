index       <= uri_stream("index.html") |
notfound    <= uri_stream("404.html") |
server      <= http_server("http://127.0.0.1/") |
{
    start_info <= http_server_start(server) | wait(start_info) |
        server  on context          L(request, response) => 
                                        if true()
                                            response    <- index;
                                            response    <- notfound; | close_stream(request, response);

                on context_error    L(e) => print(e);
        ;

    sleep(15000) | http_server_close(server);
};