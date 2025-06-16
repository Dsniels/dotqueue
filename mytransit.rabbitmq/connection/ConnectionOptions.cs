using System;

namespace mytransit.rabbitmq.connection;

public class ConnectionOptions
{

    public string ConnectionString { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
}
