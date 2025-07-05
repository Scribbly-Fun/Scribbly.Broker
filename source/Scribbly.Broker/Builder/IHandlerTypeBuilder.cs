using System.Reflection;
using Scribbly.Broker.Errors;

namespace Scribbly.Broker;

/// <summary>
/// The type handler builder is used to located and aggregate all the handlers used by the <see cref="IBroker"/>
/// </summary>
public interface IHandlerTypeBuilder
{
    /// <summary>
    /// Readonly collection of the handlers discovered during the build processor.
    /// </summary>
    IReadOnlyCollection<Type> NotificationHandlers { get; }

    /// <summary>
    /// Readonly collection of the handlers discovered during the build processor.
    /// </summary>
    IReadOnlyCollection<Type> QueryHandlers { get; }

    /// <summary>
    /// Discovers and adds all the handlers in the provided assembly.
    /// </summary>
    /// <param name="assembly">The assembly containing the handlers.</param>
    /// <exception cref="DuplicateAssemblyException">Throws when the assembly has been added more than once.</exception>
    /// <returns>The builder to discover more handlers.</returns>
    IHandlerTypeBuilder AddHandlersFromAssembly(Assembly assembly);

    /// <summary>
    /// Discovers and adds all the handlers in the provided types assembly.
    /// </summary>
    /// <typeparam name="TAssemblyMarker">A type marking the assembly</typeparam>
    /// <exception cref="DuplicateAssemblyException">Throws when the assembly has been added more than once.</exception>
    /// <returns>The builder to discover more handlers.</returns>
    IHandlerTypeBuilder AddHandlersFromAssembly<TAssemblyMarker>();

    /// <summary>
    /// Discovers and adds all the handlers in the provided types assembly.
    /// </summary>
    /// <param name="assemblyMarker">A type marking the assembly</param>
    /// <exception cref="DuplicateAssemblyException">Throws when the assembly has been added more than once.</exception>
    /// <returns>The builder to discover more handlers.</returns>
    IHandlerTypeBuilder AddHandlersFromAssembly(Type assemblyMarker);

    /// <summary>
    /// Adds a specific handler
    /// </summary>
    /// <typeparam name="THandler">The concrete handler type</typeparam>
    /// <typeparam name="TNotification">The type of notification the handler handles.</typeparam>
    /// <exception cref="DuplicateHandlerException">Throws when the handler has been added more than once.</exception>
    /// <returns></returns>
    IHandlerTypeBuilder AddHandler<THandler, TNotification>() where THandler : INotificationHandler<TNotification> where TNotification : INotification;

    /// <summary>
    /// Adds a specific handler
    /// </summary>
    /// <param name="handlerType">The concrete handler type</param>
    /// <exception cref="InvalidOperationException">When the handler does not implement the INotificationHandler{TNotification} interface</exception>
    /// <exception cref="DuplicateHandlerException">Throws when the handler has been added more than once.</exception>
    /// <returns></returns>
    IHandlerTypeBuilder AddHandler(Type handlerType);
}