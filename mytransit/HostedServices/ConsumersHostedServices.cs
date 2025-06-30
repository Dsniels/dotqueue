using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using mytransit.abstractions.Consumer;
using mytransit.extensions.Options;

namespace mytransit.extensions.HostedServices;

public class ConsumersHostedServices : BackgroundService
{
    private readonly Configurator _configurator;
    private readonly IServiceProvider _serviceProvider;

    public ConsumersHostedServices(Configurator configurator, IServiceProvider serviceProvider)
    {
        _configurator = configurator;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var SetUpConsumer = _serviceProvider.GetRequiredService<ISetUpConsumer>();
        foreach (var consumerType in _configurator.Consumers)
        {
            var consumer = _serviceProvider.GetRequiredService(consumerType);
            var eventType = consumerType.GetInterfaces().FirstOrDefault(I => I.IsGenericType && I.GetGenericTypeDefinition() == typeof(IConsumer<>))!.GetGenericArguments()[0];
            var method = typeof(ISetUpConsumer).GetMethod(nameof(ISetUpConsumer.StartConsumer))!.MakeGenericMethod(eventType);
            await (Task)method.Invoke(SetUpConsumer, new object[] {consumer, stoppingToken })!;
        }
    }
}
