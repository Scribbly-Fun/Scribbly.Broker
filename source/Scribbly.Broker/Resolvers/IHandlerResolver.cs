namespace Scribbly.Broker;

/// <summary>
/// 
/// </summary>
public interface IHandlerResolver
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TNotification"></typeparam>
    /// <typeparam name="TNotificationResult"></typeparam>
    /// <returns></returns>
    IReadOnlyCollection<QueryRequestDelegate<TNotification, TNotificationResult>> ResolveHandlers<TNotification, TNotificationResult>() where TNotification : INotification<TNotificationResult>;
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TNotification"></typeparam>
    /// <returns></returns>
    IReadOnlyCollection<NotificationRequestDelegate<TNotification>> ResolveHandlers<TNotification>() where TNotification : INotification;
}
