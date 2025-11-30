using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Scribbly.Broker.Behaviors;

/// <summary>
/// Wraps your pipeline inside a try/catch that catches any exception.
/// </summary>
/// <param name="logger">A logger to log out the exception.</param>
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

/// <summary>
/// Wraps your pipeline inside a try/catch that catches a specific exception.
/// </summary>
/// <typeparam name="T1">The type of exception the behavior should catch</typeparam>
/// <param name="logger">A logger to log out the exception.</param>
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

/// <summary>
/// Wraps your pipeline inside a try/catch that catches specific exceptions.
/// </summary>
/// <typeparam name="T1">The type of exception the behavior should catch</typeparam>
/// <typeparam name="T2">The type of exception the behavior should catch</typeparam>
/// <param name="logger">A logger to log out the exception.</param>
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

/// <summary>
/// Wraps your pipeline inside a try/catch that catches specific exceptions.
/// </summary>
/// <typeparam name="T1">The type of exception the behavior should catch</typeparam>
/// <typeparam name="T2">The type of exception the behavior should catch</typeparam>
/// <typeparam name="T3">The type of exception the behavior should catch</typeparam>
/// <param name="logger">A logger to log out the exception.</param>
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

/// <summary>
/// Wraps your pipeline inside a try/catch that catches specific exceptions.
/// </summary>
/// <typeparam name="T1">The type of exception the behavior should catch</typeparam>
/// <typeparam name="T2">The type of exception the behavior should catch</typeparam>
/// <typeparam name="T3">The type of exception the behavior should catch</typeparam>
/// <typeparam name="T4">The type of exception the behavior should catch</typeparam>
/// <param name="logger">A logger to log out the exception.</param>
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