using Microsoft.Extensions.DependencyInjection;
using mytransit.abstractions.Consumer;

namespace mytransit.extensions.Options;

public class Configurator : IConfigurator
{
    public List<Type> Consumers { get; set; } = new ();
    private readonly IServiceCollection _services;
    public Configurator(IServiceCollection services)
    {
        _services = services;
    }


    public IConfigurator AddConsumer<T>() where T : class
    {
        var consumer = typeof(T);
        var interfaceType = consumer.GetInterfaces().FirstOrDefault(I => I.IsGenericType && I.GetGenericTypeDefinition() == typeof(IConsumer<>));
        _services.AddScoped(consumer);
        Consumers.Add(consumer);
        return this;
    }
}
