namespace Scribbly.Broker;

/// <summary>
/// Handles notifications published via the Scribbly.Broker service
/// </summary>
/// <typeparam name="TNotification">The notification message to send</typeparam>
public interface INotificationHandler<in TNotification> where TNotification : INotification
{
    /// <summary>
    /// Handles the notification
    /// </summary>
    /// <param name="notification">The message</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns></returns>
    Task Handle(TNotification notification, CancellationToken cancellationToken = default);
}

/// <summary>
/// Handles notifications published via the Scribbly.Broker service
/// </summary>
/// <typeparam name="TNotification">The notification message to send</typeparam>
/// <typeparam name="TResponse">A response object returned from the handler</typeparam>
public interface INotificationHandler<in TNotification, TResponse> where TNotification : INotification<TResponse>
{
    /// <summary>
    /// Handles the notification and returns a response to the caller.
    /// </summary>
    /// <param name="notification">The message</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns></returns>
    Task<TResponse> Handle(TNotification notification, CancellationToken cancellationToken = default);
}