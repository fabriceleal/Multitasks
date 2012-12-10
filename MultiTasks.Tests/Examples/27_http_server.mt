
server <= http_server("http://127.0.0.1/") |
    start_info <= http_server_start(server) |
        wait(start_info) |
            http_server_stop(server);
