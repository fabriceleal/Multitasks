server <= http_server("http://127.0.0.1:7654/") |
{
    start_info <= http_server_start(server) | wait(start_info) |
        server  on context          L(server, request, response) => 
                                        http_set_code(response, 202) |
                                        st <= http_stream(response)  | 
                                        ii <= uri_stream("L:\\TesteFx\\Multitasks\\MultiTasks.Tests\\Examples\\index.html") |
                                        st <- ii |
                                        wait(_) |
                                        http_end(response);

        ;

    sleeping <= sleep(120000) | wait(sleeping) | http_server_stop(server);
};