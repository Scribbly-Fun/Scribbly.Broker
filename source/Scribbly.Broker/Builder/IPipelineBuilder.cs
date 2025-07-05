using Scribbly.Broker.Pipelines;

namespace Scribbly.Broker;

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