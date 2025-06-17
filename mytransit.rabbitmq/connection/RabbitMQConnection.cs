using System;
using Microsoft.Extensions.Options;
using mytransit.abstractions.channel;
using mytransit.abstractions.connection;
using mytransit.abstractions.Consumer;
using mytransit.rabbitmq.bus;
using mytransit.rabbitmq.consumer;
using RabbitMQ.Client;

namespace mytransit.rabbitmq.connection;

public class RabbitMQConnection : ITransitConnection
{
    private IChannel _channel;
    private IConnection _connection;

    public static async Task<RabbitMQConnection> StartAsync(IOptions<ConnectionOptions> options)
    {
        var config = options.Value;
        var factory = new ConnectionFactory{Uri = new Uri(config.ConnectionString)};
        var connection =await  factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();
        return new RabbitMQConnection(connection, channel);
    }

    private RabbitMQConnection(IConnection connection, IChannel channel)
    {
        _connection = connection;
        _channel = channel;
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }

    public Task<IBus> GetBus(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IBus>(new Bus(_channel,this));
    }

    public Task<ISetUpConsumer> GetUpConsumer(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<ISetUpConsumer>(new SetUpConsumer(_channel));
    }
}
