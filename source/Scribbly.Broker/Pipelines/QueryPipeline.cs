namespace Scribbly.Broker.Pipelines;

internal sealed class QueryPipeline : IQueryPipeline
{
    private readonly List<IBrokerBehavior> _pipelines;

    public QueryPipeline(List<IBrokerBehavior> pipelines)
    {
        _pipelines = pipelines.ToList();
    }

    public Task<TResult> Handle<TNotification, TResult>(
        TNotification notification,
        CancellationToken cancellation,
        QueryRequestDelegate<TNotification, TResult> finalHandler)
        where TNotification : INotification<TResult>
    {
        QueryRequestDelegate<TNotification, TResult> current = finalHandler;

        for (int i = _pipelines.Count - 1; i >= 0; i--)
        {
            var pipeline = _pipelines[i];
            var next = current;

            current = async (n, c) =>
            {
                TResult? result = default;

                await pipeline.Handle(n, c, async (innerN, innerC) =>
                {
                    result = await next(innerN, innerC);
                });

                return result!;
            };
        }

        return current(notification, cancellation);
    }
}