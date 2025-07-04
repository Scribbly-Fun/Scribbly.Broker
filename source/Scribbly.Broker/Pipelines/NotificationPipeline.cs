namespace Scribbly.Broker.Pipelines;

internal sealed class NotificationPipeline : INotificationPipeline
{
    private readonly List<IBrokerBehavior> _pipelines;

    public NotificationPipeline(List<IBrokerBehavior> pipelines)
    {
        _pipelines = pipelines;
    }

    public async Task Handle<TNotification>(
        TNotification notification,
        CancellationToken cancellation,
        NotificationRequestDelegate<TNotification> finalHandler)
        where TNotification : INotification
    {
        var current = finalHandler;

        for (var i = _pipelines.Count - 1; i >= 0; i--)
        {
            var pipeline = _pipelines[i];

            var next = current;
            current = (n, c) => pipeline.Handle(n, c, next);
        }

        await current(notification, cancellation);
    }
}