server <= http_server("http://127.0.0.1:7654/") |
ii <= uri_stream("L:\\TesteFx\\Multitasks\\MultiTasks.Tests\\Examples\\index.html") |
{
    start_info <= http_server_start(server) | wait(start_info) |
        server  on context          L(server, request, response) => 
                                        http_set_code(response, 202) |
										http_set_content_type(response, "text/html") |
                                        st <= http_stream(response) |
                                        st <- ii |
                                        wait(_) |
                                        http_end(response);

        ;

    sleeping <= sleep(60000) | wait(sleeping) | http_server_stop(server);
};