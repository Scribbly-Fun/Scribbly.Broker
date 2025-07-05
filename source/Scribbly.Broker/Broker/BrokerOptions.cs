using Scribbly.Broker.Pipelines;
using System.Reflection;

namespace Scribbly.Broker;

/// <summary>
/// Configuration options for the broker services
/// <seealso cref="IBroker"/>
/// </summary>
public sealed class BrokerOptions : IPipelineBuilder, IHandlerTypeBuilder
{
    private readonly IHandlerTypeBuilder _handlerBuilder = new HandlerTypeBuilder();
    
    /// <summary>
    /// When true the services will be registered as a scoped service and can support scoped services in the handlers.
    /// </summary>
    public bool AsScoped { get; set; } = true;

    /// <summary>
    /// When true the broker will throw exceptions indicating invalid configuration or missing handlers.
    /// <remarks>Defaults to true, when false these errors could be hard to detect.</remarks>
    /// </summary>
    public bool ThrowInvalidHandlers { get; set; } = true;

    /// <summary>
    /// When true all mediated exceptions will include the notification object data.
    /// <remarks>this defaults to false and could waste memory.  As such it should only be turned on for debugging.</remarks>
    /// </summary>
    public bool CaptureNotificationsInErrors { get; set; } = false;

    /// <summary>
    /// A collection of behaviors to add to the pipeline
    /// </summary>
    internal List<Type> Behaviors { get; set; } = [];

    /// <summary>
    /// An optional pipeline to override the default behavior execution pipeline.
    /// </summary>
    internal Type? NotificationPipeline { get; set; }
    
    /// <summary>
    /// An optional pipeline to override the default behavior execution pipeline.
    /// </summary>
    internal Type? QueryPipeline { get; set; }

    /// <inheritdoc />
    public IPipelineBuilder AddBehavior<TBehavior>() where TBehavior : IBrokerBehavior
    {
        Behaviors.Add(typeof(TBehavior));
        return this;
    }

    /// <inheritdoc />
    public IPipelineBuilder AddBehavior(Type type)
    {
#if NET8_0_OR_GREATER
        if (!type.IsAssignableTo(typeof(IBrokerBehavior)))
        {
            throw new InvalidOperationException("Type must be a Mediator NotificationPipeline");
        }
#endif

        Behaviors.Add(type);
        return this;
    }

    /// <inheritdoc />
    public IPipelineBuilder AddNotificationPipeline<TPipeline>() where TPipeline : INotificationPipeline
    {
        if (NotificationPipeline is not null)
        {
            throw new ArgumentException($"The {nameof(AddNotificationPipeline)} method can only be called once");
        }

        NotificationPipeline = typeof(TPipeline);
        return this;
    }

    /// <inheritdoc />
    public IPipelineBuilder AddQueryPipeline<TPipeline>() where TPipeline : IQueryPipeline
    {
        if (NotificationPipeline is not null)
        {
            throw new ArgumentException($"The {nameof(AddQueryPipeline)} method can only be called once");
        }

        QueryPipeline = typeof(TPipeline);
        return this;
    }

    /// <inheritdoc />
    public IReadOnlyCollection<Type> NotificationHandlers => _handlerBuilder.NotificationHandlers;

    /// <inheritdoc />
    public IReadOnlyCollection<Type> QueryHandlers => _handlerBuilder.QueryHandlers;

    /// <inheritdoc />
    public IHandlerTypeBuilder AddHandlersFromAssembly(Assembly assembly) => 
        _handlerBuilder.AddHandlersFromAssembly(assembly);

    /// <inheritdoc />
    public IHandlerTypeBuilder AddHandlersFromAssembly<TAssemblyMarker>() => 
        _handlerBuilder.AddHandlersFromAssembly<TAssemblyMarker>();

    /// <inheritdoc />
    public IHandlerTypeBuilder AddHandlersFromAssembly(Type assemblyMarker) => 
        _handlerBuilder.AddHandlersFromAssembly(assemblyMarker);

    /// <inheritdoc />
    public IHandlerTypeBuilder AddHandler<THandler, TNotification>() where THandler : INotificationHandler<TNotification> where TNotification : INotification =>
        _handlerBuilder.AddHandler<THandler, TNotification>();

    /// <inheritdoc />
    public IHandlerTypeBuilder AddHandler(Type handlerType) => 
        _handlerBuilder.AddHandler(handlerType);
}