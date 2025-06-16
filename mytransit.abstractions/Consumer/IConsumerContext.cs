using System;

namespace mytransit.abstractions.Consumer;

public interface IConsumerContext<out T>
{
    T Message { get; }
    string MessageId { get; }
    string CorrelationId { get; }
    
}

