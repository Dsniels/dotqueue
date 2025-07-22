using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using mytransit.abstractions.channel;
using mytransit.abstractions.connection;
using mytransit.abstractions.Consumer;
using mytransit.extensions.HostedServices;
using mytransit.extensions.Options;
using mytransit.rabbitmq.connection;

namespace mytransit;

public static class MyTransit
{
    public static IServiceCollection AddMyTransit(this IServiceCollection services, string connectionString)
    {
        services.Configure<ConnectionOptions>(opts => opts.ConnectionString = connectionString);
        services.TryAddSingleton<ITransitConnection>(services =>
        {
            var opts = services.GetRequiredService<IOptions<ConnectionOptions>>();
            return RabbitMQConnection.StartAsync(opts).GetAwaiter().GetResult();
        });
        services.AddSingleton<IBus>(services =>
        {
            var connection = services.GetRequiredService<ITransitConnection>();
            return connection.GetBus().GetAwaiter().GetResult();
        });
        return services;
    }

    public static IServiceCollection AddMyTransit(this IServiceCollection services, string connectionString, Action<Configurator> opts)
    {
        services.Configure<ConnectionOptions>(opts => opts.ConnectionString = connectionString);
        services.AddSingleton<RabbitMQConnection>(services =>
        {
            var opts = services.GetRequiredService<IOptions<ConnectionOptions>>();
            return RabbitMQConnection.StartAsync(opts).GetAwaiter().GetResult();
        });
        services.AddSingleton<IBus>(services =>
        {
            var connection = services.GetRequiredService<RabbitMQConnection>();
            return connection.GetBus().GetAwaiter().GetResult();
        });
        services.AddSingleton<ISetUpConsumer>(services =>
        {
            var connection = services.GetRequiredService<RabbitMQConnection>();
            return connection.GetUpConsumer().GetAwaiter().GetResult();
        });

        var config = new Configurator(services);
        opts(config);
        services.AddSingleton(config);
        services.AddHostedService<ConsumersHostedServices>();

        return services;
    }
}
