namespace Scribbly.Broker.Errors;

/// <summary>
/// 
/// </summary>
public sealed class DuplicateHandlerException : BrokerException
{
    /// <inheritdoc />
    public DuplicateHandlerException(Type type) : base($"The {type.Name} has already been added")
    {
    }
}

/// <summary>
/// 
/// </summary>
public sealed class InvalidHandlerException : BrokerException
{
    /// <inheritdoc />
    public InvalidHandlerException(Type type) : base($"The {type.Name} does not implement INotificationHandler")
    {
    }
}