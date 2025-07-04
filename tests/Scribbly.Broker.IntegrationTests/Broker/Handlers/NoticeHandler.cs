using Scribbly.Broker.IntegrationTests.Broker.Notifications;

namespace Scribbly.Broker.IntegrationTests.Broker.Handlers;

public sealed class NoticeHandler : INotificationHandler<Notice>
{
    public static string? Notice;

    /// <inheritdoc />
    public Task Handle(Notice notification, CancellationToken cancellationToken = default)
    {
        Notice = notification.Message;
        return Task.CompletedTask;
    }
}