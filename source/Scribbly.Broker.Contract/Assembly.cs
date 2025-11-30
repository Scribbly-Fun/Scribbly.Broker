using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Scribbly.Broker.UnitTests")]
[assembly: InternalsVisibleTo("Scribbly.Broker.IntegrationTests")]
[assembly: InternalsVisibleTo("Scribbly.Broker.Cookbook.Tests")]

// ReSharper disable once CheckNamespace
namespace Scribbly.Broker.Contract;

/// <summary>
/// Marker to locate this assembly using reflection.
/// </summary>
public interface IAssemblyMarker
{
}