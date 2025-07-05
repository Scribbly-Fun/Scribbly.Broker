using Microsoft.Extensions.DependencyInjection;
using Scribbly.Broker.Pipelines;

namespace Scribbly.Broker;

/// <summary>
/// This is the glue that enables all the broker functionality.  Without the services registered and handlers discovered this <see cref="IBroker"/> cannot be used.
/// </summary>
public static class BrokerRegistration
{
    /// <summary>
    /// Registers and discovers all the handlers and pipelines.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static IServiceCollection AddScribblyBroker(this IServiceCollection services, Action<BrokerOptions>? optionsAction = null)
    {
        var options = new BrokerOptions();
        optionsAction?.Invoke(options);

        services.AddSingleton(options);

        services.AddSingleton(options.Behaviors);

        if (options.Behaviors.Any())
        {
            services.AddTransient<INotificationPipeline, NotificationPipeline>();
            services.AddTransient<IQueryPipeline, QueryPipeline>();

            foreach (var type in options.Behaviors)
            {
                services.AddTransient(typeof(IBrokerBehavior), type);
            }

            services.AddTransient(sp =>
            {
                var pipelines = sp.GetServices<IBrokerBehavior>();
                return pipelines.ToList();
            });
        }
        else
        {
            services.AddSingleton<INotificationPipeline, EmptyNotificationPipeline>();
            services.AddSingleton<IQueryPipeline, EmptyQueryPipeline>();
        }

        if (options.AsScoped)
        {
            services.AddScoped<IBroker, Broker>();
            services.AddScoped<IBrokerPublisher, Broker>();
            services.AddScoped<IBrokerQuery, Broker>();
            services.AddScoped<IHandlerResolver, MicrosoftHostingHandlerResolver>();
#if NET8_0_OR_GREATER
            services.AddScoped<IBrokerStream, Broker>();
#endif
        }
        else
        {
            services.AddSingleton<IBroker, Broker>();
            services.AddSingleton<IBrokerPublisher, Broker>();
            services.AddSingleton<IBrokerQuery, Broker>();
            services.AddSingleton<IHandlerResolver, MicrosoftHostingHandlerResolver>();
#if NET8_0_OR_GREATER
            services.AddSingleton<IBrokerStream, Broker>();
#endif
        }

        var notificationHandlers = options.Assembly
            .GetTypes()
            .Where(t =>
                t is { IsInterface: false, IsAbstract: false } &&
                t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(INotificationHandler<>)));

        foreach (var implementationType in notificationHandlers)
        {
            services.AddTransient(
                implementationType
                    .GetInterfaces()
                    .First(x => x.GetGenericTypeDefinition() == typeof(INotificationHandler<>)),
                implementationType);
        }

        var notificationHandlersWithResponse = options.Assembly
            .GetTypes()
            .Where(t =>
                t is { IsInterface: false, IsAbstract: false } &&
                t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(INotificationHandler<,>)));

        foreach (var implementationType in notificationHandlersWithResponse)
        {
            services.AddTransient(
                implementationType
                    .GetInterfaces()
                    .First(x => x.GetGenericTypeDefinition() == typeof(INotificationHandler<,>)),
                implementationType);
        }

        return services;
    }
}