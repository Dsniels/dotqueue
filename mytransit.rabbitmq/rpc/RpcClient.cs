using System.Text.Json;
using mytransit.abstractions.rpc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace mytransit.rabbitmq.rpc;

public class RpcClient<T> : IRpcClient<T>
{

    private readonly IChannel _channel;

    public RpcClient(IChannel channel)
    {
        _channel = channel;
    }

    public static async Task StartClient() { }


    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }

    public async Task GetResponse<V>(T body, CancellationToken cancellationToken = default)
    {
        var responseType = typeof(V).Name;
        var requestType = typeof(T).Name;

        var replayQueue = await _channel.QueueDeclareAsync(queue: responseType, exclusive: true);
        var requestQueue = await _channel.QueueDeclareAsync(queue: requestType, exclusive: false);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, args) =>
        {
            var body = JsonSerializer.Deserialize<V>(args.Body.Span);
        };
        await _channel.BasicConsumeAsync(queue: replayQueue.QueueName, autoAck: true, consumer: consumer);

        var payload = JsonSerializer.SerializeToUtf8Bytes(body);
        var props = new BasicProperties
        {
            ReplyTo = replayQueue,
            CorrelationId = Guid.NewGuid().ToString()
        };

        await _channel.BasicPublishAsync("", requestType, false, props, payload);

    }
}
