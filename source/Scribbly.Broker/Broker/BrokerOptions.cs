using System.Reflection;
using Scribbly.Broker.Pipelines;

namespace Scribbly.Broker;

/// <summary>
/// Configuration options for the broker services
/// <seealso cref="IBroker"/>
/// </summary>
public sealed class BrokerOptions : IPipelineBuilder
{
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
    /// Defines the assembly to search for handlers.
    /// </summary>
    public Assembly Assembly { get; set; } = Assembly.GetExecutingAssembly();

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

}

/// <summary>
/// Builds up a collection of behaviors known as a pipeline
/// </summary>
public interface IPipelineBuilder
{
    /// <summary>
    /// Adds a behavior to the collection of the behaviors.
    /// <remarks>Behaviors will be registered in the DI container and support injection.</remarks>
    /// </summary>
    /// <typeparam name="TBehavior">The type of behavior to add</typeparam>
    /// <returns>The builder to add more types.</returns>
    IPipelineBuilder AddBehavior<TBehavior>() where TBehavior : IBrokerBehavior;

    /// <summary>
    /// Adds a behavior to the collection of the behaviors.
    /// <remarks>Behaviors will be registered in the DI container and support injection.</remarks>
    /// </summary>
    /// <param name="type">The type</param>
    /// <exception cref="InvalidOperationException">When the type is not a behavior</exception>
    /// <returns>The builder to add more types.</returns>
    IPipelineBuilder AddBehavior(Type type);

    /// <summary>
    /// Overrides the pipelines used internally by the <see cref="IBroker"/>
    /// and allows the consumer to inject code into the <see cref="IBrokerBehavior"/> execution pipeline
    /// </summary>
    /// <typeparam name="TPipeline"></typeparam>
    /// <returns></returns>
    IPipelineBuilder AddNotificationPipeline<TPipeline>() where TPipeline : INotificationPipeline;
    
    /// <summary>
    /// Overrides the pipelines used internally by the <see cref="IBroker"/>
    /// and allows the consumer to inject code into the <see cref="IBrokerBehavior"/> execution pipeline
    /// </summary>
    /// <typeparam name="TPipeline"></typeparam>
    /// <returns></returns>
    IPipelineBuilder AddQueryPipeline<TPipeline>() where TPipeline : IQueryPipeline;
}
