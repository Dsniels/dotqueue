using System;

namespace mytransit.abstractions.Consumer;

public interface ISetUpConsumer
{
    Task StartConsumer<T>(IConsumer<T> consumer, CancellationToken cancellation = default);

}
