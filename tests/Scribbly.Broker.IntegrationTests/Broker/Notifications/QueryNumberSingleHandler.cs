namespace Scribbly.Broker.IntegrationTests.Broker.Notifications;

public record QueryNumberSingleHandler(int Message) : INotification<int>;
public record QueryNumberMultipleHandlers(int Message) : INotification<int>;
