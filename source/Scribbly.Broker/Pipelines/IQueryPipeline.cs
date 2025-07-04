namespace Scribbly.Broker.Pipelines;

#pragma warning disable CS1591
public interface IQueryPipeline
{
    Task<TResult> Handle<TNotification, TResult>(
        TNotification notification,
        CancellationToken cancellation,
        QueryRequestDelegate<TNotification, TResult> next)
        where TNotification : INotification<TResult>;
}