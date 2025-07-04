namespace Scribbly.Broker.Errors;

/// <summary>
/// An meaningful exception used when the broker can't locate a handler for the message published.
/// </summary>
/// <typeparam name="TNotification"></typeparam>
public sealed class BrokerHandlersNotFound<TNotification> : BrokerException<TNotification> where TNotification : INotification
{
    /// <inheritdoc />
    public BrokerHandlersNotFound() 
        : base($"Failed to find handler for {typeof(TNotification)}, please ensure the handlers exist in the discoverable assembly")
    {
    }

    /// <inheritdoc />
    public BrokerHandlersNotFound(TNotification notice) 
        : base(notice, $"Failed to find handler for {typeof(TNotification)}, please ensure the handlers exist in the discoverable assembly")
    {
    }
}