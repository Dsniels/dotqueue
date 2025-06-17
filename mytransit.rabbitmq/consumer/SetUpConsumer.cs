using System.Text.Json;
using mytransit.abstractions.Consumer;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace mytransit.rabbitmq.consumer;

public class SetUpConsumer : ISetUpConsumer
{

    private readonly IChannel _channel;

    public SetUpConsumer(IChannel channel)
    {
        _channel = channel;

    }


    public async Task StartConsumer<T>(IConsumer<T> consumer, CancellationToken cancellation = default)
    {
        var exchangeName = typeof(T).Name;
        var queueName = await SetUpQueues(exchangeName, cancellation);
        await _channel.BasicQosAsync(0, 1, false, cancellation);
        var consume = new AsyncEventingBasicConsumer(_channel);
        consume.ReceivedAsync += async (_, args) =>
        {
            var message = JsonSerializer.Deserialize<T>(args.Body.Span);
            if (message is null) return;
            var context = new ConsumerContext<T>
            {
                CorrelationId = args.BasicProperties.CorrelationId,
                MessageId = args.BasicProperties.MessageId,
                Message = message
            };

            await consumer.Consume(context);

        };
        await _channel.BasicConsumeAsync(queueName, true, consume, cancellation);
    }


    private async Task<string> SetUpQueues(string exchangeName, CancellationToken cancellation = default)
    {
        await _channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Fanout, false, true, null, cancellationToken: cancellation);
        var queue = await _channel.QueueDeclareAsync("", false, true, true, cancellationToken: cancellation);
        await _channel.QueueBindAsync(queue.QueueName, exchangeName, "", cancellationToken: cancellation);
        return queue.QueueName;
    }


}
