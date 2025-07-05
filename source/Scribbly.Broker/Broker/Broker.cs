#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif
using Scribbly.Broker.Errors;
using Scribbly.Broker.Pipelines;

namespace Scribbly.Broker;

/// <summary>
/// The broker is responsible for accepting messages and routing them through behavior pipelines and eventually the handlers.
/// The broker depends on an <seealso cref="IHandlerResolver"/> in order to resolve the handlers.  This service can be overridden to leverage a custom resolver.
/// The broker routes handlers through pipelines using a <seealso cref="INotificationPipeline"/> and <seealso cref="IQueryPipeline"/>
/// </summary>
internal sealed class Broker : IBroker
{
    private readonly BrokerOptions _options;
    private readonly IHandlerResolver _resolver;
    private readonly INotificationPipeline _notificationPipeline;
    private readonly IQueryPipeline _queryPipeline;

    /// <summary>
    /// Creates a new broker.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="resolver"></param>
    /// <param name="notificationPipeline"></param>
    /// <param name="queryPipeline"></param>
    public Broker(BrokerOptions options, IHandlerResolver resolver, INotificationPipeline notificationPipeline, IQueryPipeline queryPipeline)
    {
        _options = options;
        _resolver = resolver;
        _notificationPipeline = notificationPipeline;
        _queryPipeline = queryPipeline;
    }

    /// <inheritdoc />
    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellation = default) where TNotification : INotification
    {
        var handlers = _resolver.ResolveHandlers<TNotification>();

        GuardInvocationOfHandlers(notification, handlers);
        
        foreach (var notificationHandler in handlers)
        {
            await _notificationPipeline.Handle(notification, cancellation, notificationHandler);
        }
    }

    /// <inheritdoc />
    public Task PublishConcurrent<TNotification>(TNotification notification, CancellationToken cancellation = default) where TNotification : INotification
    {
        var handlers = _resolver.ResolveHandlers<TNotification>();

        GuardInvocationOfHandlers(notification, handlers);

        return Task.WhenAll(handlers.Select(h => _notificationPipeline.Handle(notification, cancellation, h)));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TNotificationResult>> Query<TNotification, TNotificationResult>(
        TNotification notification,
        CancellationToken cancellation = default) 
        where TNotification : INotification<TNotificationResult>
    {
        var handlers = _resolver.ResolveHandlers<TNotification, TNotificationResult>();

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
        var handlers = _resolver.ResolveHandlers<TNotification, TNotificationResult>();

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
        var handlers = _resolver.ResolveHandlers<TNotification, TNotificationResult>();

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
