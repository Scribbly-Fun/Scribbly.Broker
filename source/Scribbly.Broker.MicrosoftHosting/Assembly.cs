using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Scribbly.Broker.UnitTests")]
[assembly: InternalsVisibleTo("Scribbly.Broker.IntegrationTests")]
[assembly: InternalsVisibleTo("Scribbly.Broker.Cookbook.Tests")]

namespace Scribbly.Broker;

/// <summary>
/// Marker to locate this assembly using reflection.
/// </summary>
public interface IAssemblyMarker
{
}