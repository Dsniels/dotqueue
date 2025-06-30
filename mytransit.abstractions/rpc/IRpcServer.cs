using System;
using mytransit.abstractions.Consumer;

namespace mytransit.abstractions.rpc;

public static class IRpcServer
{
    public static Task RespondAsync<T, V>(this IConsumerContext<V> context, T message)
    {
        return Task.CompletedTask;
    }
}
