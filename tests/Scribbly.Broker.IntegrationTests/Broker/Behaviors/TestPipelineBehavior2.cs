namespace Scribbly.Broker.IntegrationTests.Broker.Behaviors;

public sealed class TestPipelineBehavior2 : IBrokerBehavior
{
    public static int Counter = 0;

    /// <inheritdoc />
    public async Task Handle<TNotification>(TNotification notification, CancellationToken cancellation, NotificationRequestDelegate<TNotification> next) where TNotification : INotification
    {
        Counter++;
        await next(notification, cancellation);
    }
}
