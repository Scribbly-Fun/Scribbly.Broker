using Microsoft.Extensions.DependencyInjection;

namespace Scribbly.Broker;

/// <summary>
/// Resolves handlers using the microsoft DI container.
/// </summary>
public sealed class MicrosoftHostingHandlerResolver : IHandlerResolver
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Creates a new handler resolver.
    /// </summary>
    /// <param name="serviceProvider">The DI containers provider</param>
    public MicrosoftHostingHandlerResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<NotificationRequestDelegate<TNotification>> ResolveHandlers<TNotification>() where TNotification : INotification
    {
        return _serviceProvider
            .GetServices<INotificationHandler<TNotification>>()
            .Select(h => new NotificationRequestDelegate<TNotification>(h.Handle))
            .ToList();
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<QueryRequestDelegate<TNotification, TNotificationResult>> ResolveHandlers<TNotification, TNotificationResult>() where TNotification : INotification<TNotificationResult>
    {
        return _serviceProvider
            .GetServices<INotificationHandler<TNotification, TNotificationResult>>()
            .Select(h => new QueryRequestDelegate<TNotification, TNotificationResult>(h.Handle))
            .ToList();
    }
}