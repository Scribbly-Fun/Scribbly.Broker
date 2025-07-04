namespace Scribbly.Broker;

/// <summary>
/// Marks a message as a type the Scribbly.Broker can publish
/// <remarks>All messages published must be marked with the <see cref="INotification"/></remarks>
/// </summary>
public interface INotification
{

}

/// <summary>
/// Marks a message as a type the Scribbly.Broker can query
/// <remarks>All messages published must be marked with the <see cref="INotification"/></remarks>
/// </summary>
/// <typeparam name="TResponse">The type to return</typeparam>
public interface INotification<TResponse> : INotification
{

}