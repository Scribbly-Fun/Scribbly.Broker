namespace Scribbly.Broker.IntegrationTests.Broker.Notifications;

public record Notice(string Message) : INotification;