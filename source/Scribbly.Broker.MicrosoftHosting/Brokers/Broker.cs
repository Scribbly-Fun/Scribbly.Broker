using Microsoft.Extensions.DependencyInjection;
using Scribbly.Broker.Errors;
using Scribbly.Broker.Pipelines;
using System.Runtime.CompilerServices;

namespace Scribbly.Broker;

internal sealed class Broker : IBroker
{
    private readonly IServiceProvider _serviceProvider;
    private readonly BrokerOptions _options;
    private readonly INotificationPipeline _notificationPipeline;
    private readonly IQueryPipeline _queryPipeline;

    public Broker(IServiceProvider serviceProvider, BrokerOptions options, INotificationPipeline notificationPipeline, IQueryPipeline queryPipeline)
    {
        _serviceProvider = serviceProvider;
        _options = options;
        _notificationPipeline = notificationPipeline;
        _queryPipeline = queryPipeline;
    }

    /// <inheritdoc />
    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellation = default) where TNotification : INotification
    {
        var handlers = _serviceProvider
            .GetServices<INotificationHandler<TNotification>>()
            .Select(h => new NotificationRequestDelegate<TNotification>(h.Handle))
            .ToList();

        GuardInvocationOfHandlers(notification, handlers);
        
        foreach (var notificationHandler in handlers)
        {
            await _notificationPipeline.Handle(notification, cancellation, notificationHandler);
        }
    }

    /// <inheritdoc />
    public Task PublishConcurrent<TNotification>(TNotification notification, CancellationToken cancellation = default) where TNotification : INotification
    {
        var handlers = _serviceProvider
            .GetServices<INotificationHandler<TNotification>>()
            .Select(h => new NotificationRequestDelegate<TNotification>(h.Handle))
            .ToList();

        GuardInvocationOfHandlers(notification, handlers);

        return Task.WhenAll(handlers.Select(h => _notificationPipeline.Handle(notification, cancellation, h)));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TNotificationResult>> Query<TNotification, TNotificationResult>(
        TNotification notification,
        CancellationToken cancellation = default) 
        where TNotification : INotification<TNotificationResult>
    {
        var handlers = _serviceProvider
            .GetServices<INotificationHandler<TNotification, TNotificationResult>>()
            .Select(h => new QueryRequestDelegate<TNotification, TNotificationResult>(h.Handle))
            .ToList();

        GuardInvocationOfHandlers(notification, handlers);

        var results = new List<TNotificationResult>();

        foreach (var notificationHandler in handlers)
        {
            var result = await _queryPipeline.Handle(notification, cancellation, notificationHandler);
            results.Add(result);
        }

        return results;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TNotificationResult>> QueryConcurrent<TNotification, TNotificationResult>(
        TNotification notification, 
        CancellationToken cancellation = default) 
        where TNotification : INotification<TNotificationResult>
    {
        var handlers = _serviceProvider
            .GetServices<INotificationHandler<TNotification, TNotificationResult>>()
            .Select(h => new QueryRequestDelegate<TNotification, TNotificationResult>(h.Handle))
            .ToList();

        GuardInvocationOfHandlers(notification, handlers);

        return await Task.WhenAll(handlers.Select(h => _queryPipeline.Handle(notification, cancellation, h)));
    }

#if NET8_0_OR_GREATER

    /// <inheritdoc />
    public async IAsyncEnumerable<TNotificationResult> QueryStream<TNotification, TNotificationResult>(
        TNotification notification,
        [EnumeratorCancellation] CancellationToken cancellation = default) 
        where TNotification : INotification<TNotificationResult>
    {
        var handlers = _serviceProvider
            .GetServices<INotificationHandler<TNotification, TNotificationResult>>()
            .Select(h => new QueryRequestDelegate<TNotification, TNotificationResult>(h.Handle))
            .ToList();

        GuardInvocationOfHandlers(notification, handlers);

        foreach (var handler in handlers)
        {
            yield return await _queryPipeline.Handle(notification, cancellation, handler);
        }
    }

#endif

    private void GuardInvocationOfHandlers<TNotification>(TNotification notification, IEnumerable<NotificationRequestDelegate<TNotification>> handlers) where TNotification : INotification
    {
        if (handlers.Any())
        {
            return;
        }

        if (!_options.ThrowInvalidHandlers)
        {
            return;
        }

        if (_options.CaptureNotificationsInErrors)
        {
            throw new BrokerHandlersNotFound<TNotification>(notification);
        }

        throw new BrokerHandlersNotFound<TNotification>();
    }
    
    private void GuardInvocationOfHandlers<TNotification, TResult>(TNotification notification, IEnumerable<QueryRequestDelegate<TNotification, TResult>> handlers) where TNotification : INotification<TResult>
    {
        if (handlers.Any())
        {
            return;
        }

        if (!_options.ThrowInvalidHandlers)
        {
            return;
        }

        if (_options.CaptureNotificationsInErrors)
        {
            throw new BrokerHandlersNotFound<TNotification>(notification);
        }

        throw new BrokerHandlersNotFound<TNotification>();
    }
}
