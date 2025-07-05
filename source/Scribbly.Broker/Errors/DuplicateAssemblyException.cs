using System.Reflection;

namespace Scribbly.Broker.Errors;

/// <summary>
/// 
/// </summary>
public sealed class DuplicateAssemblyException : BrokerException
{
    /// <inheritdoc />
    public DuplicateAssemblyException(Assembly assembly) : base($"The {assembly.FullName} has already been added")
    {
    }
}