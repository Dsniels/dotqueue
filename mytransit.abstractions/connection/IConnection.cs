using System;
using mytransit.abstractions.channel;

namespace mytransit.abstractions.connection;

public interface ITransitConnection : IAsyncDisposable
{
    Task<IBus> GetChannel(CancellationToken cancellationToken = default);

}
