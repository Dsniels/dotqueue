RabbitMQ Library for .net

Download the source files, and add the reference to your project, or download the nuget packages via github

Add the service
```C#
    services.AddMyTransit();

```


Setup consumers
```C#
    services.AddMyTransit(opts =>
    {
        opts.AddConsumer<ConsumerEvent>();
    });


```




Publisher

```C#
        var bus = scope.ServiceProvider.GetRequiredService<IBus>();
        await bus.PublishAsync(new Message<EventType>
        {
            MessageId = Guid.NewGuid().ToString(),
            CorrelationalId = Guid.NewGuid().ToString(),
            Payload = new EventType{ message = message }
        });

```


Consumer
```C#
using System;
using mytransit.core.Consumer;

namespace Eventsss;

public class EventType
{
    public string message { get; set; }
}
public class ConsumerEvent : IConsumer<EventHola>
{
    public async Task ConsumeAsync(IConsumeContext<EventType> context)
    {
        Console.WriteLine($"Message: {context.Message.message}");
        await Task.CompletedTask;
        
    }

}

```
