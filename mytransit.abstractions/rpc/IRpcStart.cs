using System;
using mytransit.abstractions.Consumer;

namespace mytransit.abstractions.rpc;

public interface IRpcStart
{
    Task StartRPCServer<T>(IConsumer<T> consumer, CancellationToken cancellationToken = default);

}
