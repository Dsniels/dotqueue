using System;
using System.Text.Json;
using mytransit.abstractions.channel;
using mytransit.abstractions.Messages;
using mytransit.rabbitmq.connection;
using RabbitMQ.Client;

namespace mytransit.rabbitmq.bus;

public class Bus : IBus
{

    private readonly IChannel _channel;
    private readonly RabbitMQConnection _connectionManager;

    public Bus(IChannel channel, RabbitMQConnection connectionManager)
    {
        _channel = channel;
        _connectionManager = connectionManager;
    }


    public async ValueTask DisposeAsync()
    {
        await _connectionManager.DisposeAsync();
    }

    public async Task PublishAsync<T>(T message, CancellationToken cancellation = default)
    {
        var name = typeof(T).Name;
        await SetUpExchange(name, cancellation);

        var body = JsonSerializer.SerializeToUtf8Bytes(message);
        var properties = new BasicProperties
        {
            MessageId = Guid.NewGuid().ToString(),
            CorrelationId = Guid.NewGuid().ToString(),
            Timestamp = new AmqpTimestamp(),
        };

        await _channel.BasicPublishAsync(name, routingKey: "", false, properties, body, cancellation);
    }

    private async Task SetUpExchange(string exchangeName, CancellationToken cancellation)
    {
        await _channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Fanout, false, true, null, cancellationToken: cancellation);
    }
}

