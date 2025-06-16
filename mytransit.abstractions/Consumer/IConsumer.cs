
namespace mytransit.abstractions.Consumer;

public interface IConsumer<in T>
{
    Task Consume(IConsumerContext<T> context, CancellationToken cancellation = default);
}
