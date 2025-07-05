namespace Scribbly.Broker;

/// <summary>
/// A pointer to the notification handlers Handle function.
/// The delegate accepts the notification so behaviors can be used for mutations.
/// </summary>
/// <typeparam name="TNotification">The notification type</typeparam>
/// <param name="notification">The notification payload</param>
/// <param name="cancellationToken">Optional cancellation token.</param>
/// <returns>an async task representing the invacation of your handler.</returns>
public delegate Task NotificationRequestDelegate<in TNotification>(
    TNotification notification, 
    CancellationToken cancellationToken = default) 
    where TNotification 
    : INotification;