namespace Scribbly.Broker.IntegrationTests.Broker.Notifications;

internal record NoticeWithOutHandler(string Message) : INotification;

internal record QueryWithOutHandler(string Message) : INotification<string>;