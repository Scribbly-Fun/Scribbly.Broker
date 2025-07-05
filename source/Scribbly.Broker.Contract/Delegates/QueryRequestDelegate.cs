namespace Scribbly.Broker;

/// <summary>
/// A pointer to the queries handler Handle function.
/// The delegate accepts the notification so behaviors can be used for mutations.
/// </summary>
/// <typeparam name="TNotification">The notification type</typeparam>
/// <typeparam name="TResponse">The type returned from the Query</typeparam>
/// <param name="notification">The notification payload</param>
/// <param name="cancellationToken">Optional cancellation token.</param>
/// <returns>an async task representing the invacation of your handler.</returns>
public delegate Task<TResponse> QueryRequestDelegate<in TNotification, TResponse>(
    TNotification notification, 
    CancellationToken cancellationToken = default)
    where TNotification 
    : INotification<TResponse>;