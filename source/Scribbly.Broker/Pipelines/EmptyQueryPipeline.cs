namespace Scribbly.Broker.Pipelines;

internal sealed class EmptyQueryPipeline : IQueryPipeline
{
    /// <inheritdoc />
    public async Task<TResult> Handle<TNotification, TResult>(
        TNotification notification, 
        CancellationToken cancellation,
        QueryRequestDelegate<TNotification, TResult> next) 
        where TNotification : INotification<TResult>
    {
        return await next(notification, cancellation);
    }
}