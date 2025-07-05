namespace Scribbly.Broker.Errors;

/// <summary>
/// A base exception for all Broker related errors.
/// </summary>
public abstract class BrokerNotificationException<TNotification> : BrokerException where TNotification : INotification
{
    /// <summary>
    /// 
    /// </summary>
    public Type NotificationType { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public TNotification? Notification { get; init; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    protected BrokerNotificationException(string message): base(message)
    {
        NotificationType = typeof(TNotification);
        Notification = default;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="notice"></param>
    /// <param name="message"></param>
    protected BrokerNotificationException(TNotification notice, string message): base(message)
    {
        NotificationType = typeof(TNotification);
        Notification = notice;
    }
}