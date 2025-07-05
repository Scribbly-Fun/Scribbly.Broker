namespace Scribbly.Broker.Errors;

/// <summary>
/// The base of all Scribbly Broker exceptions.
/// </summary>
public abstract class BrokerException : Exception
{
    /// <summary>
    /// Creates a new broker exception
    /// </summary>
    /// <param name="message">A message to store in the exception</param>
    protected BrokerException(string message) : base(message)
    {
        
    }
    
    /// <summary>
    /// Wraps an exception with the broker exception
    /// </summary>
    /// <param name="message">A message to store in the exception</param>
    /// <param name="innerException">The exception wrapped by the broker exception</param>
    protected BrokerException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}