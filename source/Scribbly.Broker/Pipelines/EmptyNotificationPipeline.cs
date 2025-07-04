namespace Scribbly.Broker.Pipelines;

internal sealed class EmptyNotificationPipeline : INotificationPipeline
{
    /// <inheritdoc />
    public async Task Handle<TNotification>(TNotification notification, CancellationToken cancellation, NotificationRequestDelegate<TNotification> next) where TNotification : INotification
    {
        await next(notification, cancellation);
    }
}