using System;
using mytransit.abstractions.Messages;

namespace mytransit.abstractions.channel;

public interface IBus: IAsyncDisposable
{
    Task PublishAsync<T>(MessageEnvelope<T> message, CancellationToken cancellation = default);
}
