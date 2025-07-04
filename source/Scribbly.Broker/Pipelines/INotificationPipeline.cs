namespace Scribbly.Broker.Pipelines;

#pragma warning disable CS1591
public interface INotificationPipeline
{
    Task Handle<TNotification>(
        TNotification notification,
        CancellationToken cancellation,
        NotificationRequestDelegate<TNotification> next)
        where TNotification : INotification;
}