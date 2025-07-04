namespace Scribbly.Broker.Errors;

/// <summary>
/// A base exception for all Broker related errors.
/// </summary>
public abstract class BrokerException<TNotification> : Exception where TNotification : INotification
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
    public BrokerException(string message): base(message)
    {
        NotificationType = typeof(TNotification);
        Notification = default;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="notice"></param>
    /// <param name="message"></param>
    public BrokerException(TNotification notice, string message): base(message)
    {
        NotificationType = typeof(TNotification);
        Notification = notice;
    }
}