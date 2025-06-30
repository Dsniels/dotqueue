using System;

namespace mytransit.abstractions.Consumer;



public class ConsumerContext<T> : IConsumerContext<T>
{
    public T Message { get; set; }

    public string MessageId { get; set; }

    public string CorrelationId { get; set; }




}

