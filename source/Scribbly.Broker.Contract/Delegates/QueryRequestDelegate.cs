namespace Scribbly.Broker;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TNotification"></typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <param name="notification"></param>
/// <param name="cancellationToken"></param>
/// <returns></returns>
public delegate Task<TResponse> QueryRequestDelegate<in TNotification, TResponse>(
    TNotification notification, 
    CancellationToken cancellationToken = default)
    where TNotification 
    : INotification<TResponse>;