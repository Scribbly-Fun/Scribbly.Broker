namespace Scribbly.Broker;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TNotification"></typeparam>
/// <param name="notification"></param>
/// <param name="cancellationToken"></param>
/// <returns></returns>
public delegate Task NotificationRequestDelegate<in TNotification>(
    TNotification notification, 
    CancellationToken cancellationToken = default) 
    where TNotification 
    : INotification;