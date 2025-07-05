using System.Reflection;
using Scribbly.Broker.Errors;

namespace Scribbly.Broker;

/// <summary>
/// An implementation of the handler builder storing the discovered types in Hashsets that is later used to created instances.
/// </summary>
public sealed class HandlerTypeBuilder : IHandlerTypeBuilder
{
    private readonly HashSet<Assembly> _assemblies = [];
    private readonly HashSet<Type> _notification = [];
    private readonly HashSet<Type> _queries = [];

    /// <inheritdoc />
    public IReadOnlyCollection<Type> NotificationHandlers => _notification.ToList().AsReadOnly();

    /// <inheritdoc />
    public IReadOnlyCollection<Type> QueryHandlers => _queries.ToList().AsReadOnly();

    /// <inheritdoc />
    public IHandlerTypeBuilder AddHandlersFromAssembly(Assembly assembly)
    {
        if (!_assemblies.Add(assembly))
        {
            throw new DuplicateAssemblyException(assembly);
        }

        foreach (var type in GetNotificationHandlersFromAssembly(assembly))
        {
            if (!_notification.Add(type))
            {
                throw new DuplicateHandlerException(type);
            }
        }
        
        foreach (var type in GetQueryHandlersFromAssembly(assembly))
        {
            if (!_queries.Add(type))
            {
                throw new DuplicateHandlerException(type);
            }
        }

        return this;
    }

    /// <inheritdoc />
    public IHandlerTypeBuilder AddHandlersFromAssembly<TAssemblyMarker>()
    {
        var assembly = typeof(TAssemblyMarker).Assembly;

        if (!_assemblies.Add(assembly))
        {
            throw new DuplicateAssemblyException(assembly);
        }

        foreach (var type in GetNotificationHandlersFromAssembly(assembly))
        {
            if (!_notification.Add(type))
            {
                throw new DuplicateHandlerException(type);
            }
        }

        foreach (var type in GetQueryHandlersFromAssembly(assembly))
        {
            if (!_queries.Add(type))
            {
                throw new DuplicateHandlerException(type);
            }
        }

        return this;
    }

    /// <inheritdoc />
    public IHandlerTypeBuilder AddHandlersFromAssembly(Type assemblyMarker)
    {
        var assembly = assemblyMarker.Assembly;

        if (!_assemblies.Add(assembly))
        {
            throw new DuplicateAssemblyException(assembly);
        }

        foreach (var type in GetNotificationHandlersFromAssembly(assembly))
        {
            if (!_notification.Add(type))
            {
                throw new DuplicateHandlerException(type);
            }
        }

        foreach (var type in GetQueryHandlersFromAssembly(assembly))
        {
            if (!_queries.Add(type))
            {
                throw new DuplicateHandlerException(type);
            }
        }

        return this;
    }

    /// <inheritdoc />
    public IHandlerTypeBuilder AddHandler<THandler, TNotification>() where THandler : INotificationHandler<TNotification> where TNotification : INotification
    {
        var type = typeof(THandler);
        if (!_notification.Add(type))
        {
            throw new DuplicateHandlerException(type);
        }

        return this;
    }

    /// <inheritdoc />
    public IHandlerTypeBuilder AddHandler(Type handlerType)
    {
        if (!handlerType.IsAssignableFrom(typeof(INotificationHandler<>)))
        {
            throw new InvalidHandlerException(handlerType);
        }

        if (!_notification.Add(handlerType))
        {
            throw new DuplicateHandlerException(handlerType);
        }

        return this;
    }

    private IEnumerable<Type> GetNotificationHandlersFromAssembly(Assembly assembly)
    {
        return assembly.GetTypes().Where(t =>
            t is { IsInterface: false, IsAbstract: false } &&
            t.GetInterfaces().Any(x => 
                x.IsGenericType && 
                x.GetGenericTypeDefinition() == typeof(INotificationHandler<>)));
    }
    
    private IEnumerable<Type> GetQueryHandlersFromAssembly(Assembly assembly)
    {
        return assembly.GetTypes().Where(t =>
            t is { IsInterface: false, IsAbstract: false } &&
            t.GetInterfaces().Any(x => 
                x.IsGenericType && 
                x.GetGenericTypeDefinition() == typeof(INotificationHandler<,>)));
    }
}