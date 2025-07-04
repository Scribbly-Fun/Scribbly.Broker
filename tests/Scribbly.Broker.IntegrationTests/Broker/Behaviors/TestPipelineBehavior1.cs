namespace Scribbly.Broker.IntegrationTests.Broker.Behaviors;

public sealed class TestPipelineBehavior1 : IBrokerBehavior
{
    public static int Counter = 0;

    /// <inheritdoc />
    public async Task Handle<TNotification>(TNotification notification, CancellationToken cancellation, NotificationRequestDelegate<TNotification> next) where TNotification : INotification
    {
        try
        {
            Counter++;
            await next(notification, cancellation);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}