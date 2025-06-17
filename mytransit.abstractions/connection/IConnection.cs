using System;
using mytransit.abstractions.channel;
using mytransit.abstractions.Consumer;

namespace mytransit.abstractions.connection;

public interface ITransitConnection : IAsyncDisposable
{
    Task<IBus> GetBus(CancellationToken cancellationToken = default);

    Task<ISetUpConsumer> GetUpConsumer(CancellationToken cancellationToken = default);
}
