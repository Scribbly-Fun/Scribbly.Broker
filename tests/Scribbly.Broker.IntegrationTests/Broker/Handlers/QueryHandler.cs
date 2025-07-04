using Scribbly.Broker.IntegrationTests.Broker.Notifications;

namespace Scribbly.Broker.IntegrationTests.Broker.Handlers;

public sealed class QueryHandler : INotificationHandler<QueryNumberSingleHandler, int>
{
    /// <inheritdoc />
    public Task<int> Handle(QueryNumberSingleHandler notification, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(notification.Message);
    }
}

public static class QueryMultipleHandlerSharedState
{
    public static int Counter { get; set; }
}

public sealed class QueryMultipleHandler1 : INotificationHandler<QueryNumberMultipleHandlers, int>
{
    /// <inheritdoc />
    public Task<int> Handle(QueryNumberMultipleHandlers notification, CancellationToken cancellationToken = default)
    {
        QueryMultipleHandlerSharedState.Counter += notification.Message;
        
        return Task.FromResult(notification.Message);
    }
}

public sealed class QueryMultipleHandler2 : INotificationHandler<QueryNumberMultipleHandlers, int>
{
    /// <inheritdoc />
    public Task<int> Handle(QueryNumberMultipleHandlers notification, CancellationToken cancellationToken = default)
    {
        QueryMultipleHandlerSharedState.Counter += notification.Message;

        return Task.FromResult(notification.Message);
    }
}
public sealed class QueryMultipleHandler3 : INotificationHandler<QueryNumberMultipleHandlers, int>
{
    /// <inheritdoc />
    public Task<int> Handle(QueryNumberMultipleHandlers notification, CancellationToken cancellationToken = default)
    {
        QueryMultipleHandlerSharedState.Counter += notification.Message;

        return Task.FromResult(notification.Message);
    }
}