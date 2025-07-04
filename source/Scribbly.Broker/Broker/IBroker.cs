
namespace Scribbly.Broker;

/// <summary>
/// 
/// </summary>
public interface IBrokerPublisher
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TNotification"></typeparam>
    /// <param name="notification"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    Task Publish<TNotification>(TNotification notification, CancellationToken cancellation = default) where TNotification : INotification;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TNotification"></typeparam>
    /// <param name="notification"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    Task PublishConcurrent<TNotification>(TNotification notification, CancellationToken cancellation = default) where TNotification : INotification;

}

/// <summary>
/// 
/// </summary>
public interface IBrokerQuery
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TNotification"></typeparam>
    /// <typeparam name="TNotificationResult"></typeparam>
    /// <param name="notification"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    Task<IEnumerable<TNotificationResult>> Query<TNotification, TNotificationResult>(
        TNotification notification,
        CancellationToken cancellation = default)
        where TNotification : INotification<TNotificationResult>;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TNotification"></typeparam>
    /// <typeparam name="TNotificationResult"></typeparam>
    /// <param name="notification"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    Task<IEnumerable<TNotificationResult>> QueryConcurrent<TNotification, TNotificationResult>(
        TNotification notification,
        CancellationToken cancellation = default)
        where TNotification : INotification<TNotificationResult>;

}

#if NET8_0_OR_GREATER
/// <summary>
/// 
/// </summary>
public interface IBrokerStream
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TNotification"></typeparam>
    /// <typeparam name="TNotificationResult"></typeparam>
    /// <param name="notification"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    IAsyncEnumerable<TNotificationResult> QueryStream<TNotification, TNotificationResult>(
        TNotification notification,
        CancellationToken cancellation = default) 
        where TNotification : INotification<TNotificationResult>;
}
#endif


/// <summary>
/// 
/// </summary>
#if NET8_0_OR_GREATER
public interface IBroker : IBrokerPublisher, IBrokerQuery, IBrokerStream { }
#else
public interface IBroker : IBrokerPublisher, IBrokerQuery { }
#endif