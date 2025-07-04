namespace Scribbly.Broker;

/// <summary>
/// Adds a behavior to the handlers.
/// </summary>
public interface IBrokerBehavior
{
    /// <summary>
    /// Middleware that wraps the handlers.
    /// </summary>
    /// <typeparam name="TNotification">The notification to send to the handler</typeparam>
    /// <param name="notification">The notification to send to the handler</param>
    /// <param name="cancellation">cancellation token</param>
    /// <param name="next">The next method to invoke in the pipeline</param>
    /// <returns>An async task</returns>
    Task Handle<TNotification>(TNotification notification, CancellationToken cancellation, NotificationRequestDelegate<TNotification> next) where TNotification : INotification;
}