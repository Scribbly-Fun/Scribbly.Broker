using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Scribbly.Broker.Behaviors;

public sealed class ExceptionBehavior(ILogger<ExceptionBehavior> logger) : IBrokerBehavior
{
    /// <inheritdoc />
    public async Task Handle<TNotification>(TNotification notification, CancellationToken cancellation, NotificationRequestDelegate<TNotification> next) where TNotification : INotification
    {
        try
        {
            await next(notification, cancellation);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed Processing {Notification}", notification);

            Activity.Current?.SetStatus(ActivityStatusCode.Error, ex.Message);
        }
    }
}

public sealed class ExceptionBehavior<T1>(ILogger<ExceptionBehavior> logger) : IBrokerBehavior where T1 : Exception
{
    /// <inheritdoc />
    public async Task Handle<TNotification>(TNotification notification, CancellationToken cancellation, NotificationRequestDelegate<TNotification> next) where TNotification : INotification
    {
        try
        {
            await next(notification, cancellation);
        }
        catch (Exception ex) when (ex is T1)
        {
            logger.LogError(ex, "Failed Processing {Notification}", notification);

            Activity.Current?.SetStatus(ActivityStatusCode.Error, ex.Message);
        }
    }
}

public sealed class ExceptionBehavior<T1, T2>(ILogger<ExceptionBehavior> logger) : IBrokerBehavior where T1 : Exception where T2 : Exception
{
    /// <inheritdoc />
    public async Task Handle<TNotification>(TNotification notification, CancellationToken cancellation, NotificationRequestDelegate<TNotification> next) where TNotification : INotification
    {
        try
        {
            await next(notification, cancellation);
        }
        catch (Exception ex) when (ex is T1 or T2)
        {
            logger.LogError(ex, "Failed Processing {Notification}", notification);

            Activity.Current?.SetStatus(ActivityStatusCode.Error, ex.Message);
        }
    }
}

public sealed class ExceptionBehavior<T1, T2, T3>(ILogger<ExceptionBehavior> logger) : IBrokerBehavior where T1 : Exception where T2 : Exception where T3 : Exception
{
    /// <inheritdoc />
    public async Task Handle<TNotification>(TNotification notification, CancellationToken cancellation, NotificationRequestDelegate<TNotification> next) where TNotification : INotification
    {
        try
        {
            await next(notification, cancellation);
        }
        catch (Exception ex) when (ex is T1 or T2 or T3)
        {
            logger.LogError(ex, "Failed Processing {Notification}", notification);

            Activity.Current?.SetStatus(ActivityStatusCode.Error, ex.Message);
        }
    }
}

public sealed class ExceptionBehavior<T1, T2, T3, T4>(ILogger<ExceptionBehavior> logger) : IBrokerBehavior where T1 : Exception where T2 : Exception where T3 : Exception where T4 : Exception
{
    /// <inheritdoc />
    public async Task Handle<TNotification>(TNotification notification, CancellationToken cancellation, NotificationRequestDelegate<TNotification> next) where TNotification : INotification
    {
        try
        {
            await next(notification, cancellation);
        }
        catch (Exception ex) when (ex is T1 or T2 or T3 or T4)
        {
            logger.LogError(ex, "Failed Processing {Notification}", notification);

            Activity.Current?.SetStatus(ActivityStatusCode.Error, ex.Message);
        }
    }
}