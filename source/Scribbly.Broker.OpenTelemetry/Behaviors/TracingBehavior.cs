using System.Diagnostics;

namespace Scribbly.Broker.Behaviors;

public sealed class TracingBehavior(ActivitySource source) : IBrokerBehavior
{
    /// <inheritdoc />
    public async Task Handle<TNotification>(TNotification notification, CancellationToken cancellation, NotificationRequestDelegate<TNotification> next) where TNotification : INotification
    {
        using var span = source.StartActivity($"handler.{typeof(TNotification).Name}", ActivityKind.Server);

        await next(notification, cancellation);
    }
}