using System;

namespace mytransit.abstractions.rpc;

public interface IRpcClient<T> : IAsyncDisposable
{

    Task GetResponse<V>(T body, CancellationToken cancellationToken = default);

}
