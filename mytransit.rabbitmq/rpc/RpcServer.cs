using System.Text.Json;
using mytransit.abstractions.Consumer;
using RabbitMQ.Client;

namespace mytransit.rabbitmq.rpc;

public static class RpcServer
{

    private static IChannel _channel;
    public static void Configure(IChannel channel)
    {
        _channel = channel;
    }

    public static async Task RespondAsync<T, V>(this IConsumerContext<V> context, T message)
    {
        var requestType = typeof(T).Name;

        await _channel.QueueDeclareAsync(requestType, exclusive: false);
        var body = JsonSerializer.SerializeToUtf8Bytes(message);
        await _channel.BasicPublishAsync(requestType, null, body: body);
    }


}
