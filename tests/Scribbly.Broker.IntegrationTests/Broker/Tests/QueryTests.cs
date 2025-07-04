using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Scribbly.Broker.Errors;
using Scribbly.Broker.IntegrationTests.Broker.Behaviors;
using Scribbly.Broker.IntegrationTests.Broker.Handlers;
using Scribbly.Broker.IntegrationTests.Broker.Notifications;

namespace Scribbly.Broker.IntegrationTests.Broker.Tests;

public class QueryTests(DefaultMediatorTestingFixture testingFixture) : IClassFixture<DefaultMediatorTestingFixture>
{
    private readonly IBroker _broker = testingFixture.ServiceProvider.GetRequiredService<IBroker>();

    [Theory]
    [InlineData(20)]
    [InlineData(1234)]
    [InlineData(998)]
    [InlineData(1223)]
    [InlineData(1112)]
    public async Task Query_Notification_When_TypeIsLocated_ReturnsResultFromHandler(int value)
    {
        var response = await _broker.Query<QueryNumberSingleHandler, int>(new QueryNumberSingleHandler(value));
        response.First().Should().Be(value);

        QueryMultipleHandlerSharedState.Counter = 0;
    }
    
    [Theory]
    [InlineData(20)]
    [InlineData(1234)]
    [InlineData(998)]
    [InlineData(1223)]
    [InlineData(1112)]
    public async Task Query_Notification_WithMultipleHandler_ShouldReturnAllResults(int value)
    {
        var response = await _broker.Query<QueryNumberMultipleHandlers, int>(new QueryNumberMultipleHandlers(value));
        response.Count().Should().Be(3);

        QueryMultipleHandlerSharedState.Counter = 0;
    }
    
    [Theory]
    [InlineData(20)]
    [InlineData(1234)]
    [InlineData(998)]
    [InlineData(1223)]
    [InlineData(1112)]
    public async Task Query_Notification_WithMultipleHandler_ShouldYieldAllResults(int value)
    {
        var response = await _broker.Query<QueryNumberMultipleHandlers, int>(new QueryNumberMultipleHandlers(value));
        QueryMultipleHandlerSharedState.Counter.Should().Be(value * 3);
        QueryMultipleHandlerSharedState.Counter = 0;
    }

    [Fact]
    public async Task Query_Notification_When_TypeIsNotLocated_ThrowMediatorException()
    {
        Func<Task> task = () => _broker.Query<QueryWithOutHandler, string>(new QueryWithOutHandler("BAD"));

        await task.Should().ThrowAsync<BrokerHandlersNotFound<QueryWithOutHandler>>();

        QueryMultipleHandlerSharedState.Counter = 0;
    }

    [Fact]
    public async Task Query_Notification_WithBehaviors_Should_InvokeBehaviors()
    {
        var response = await _broker.Query<QueryNumberSingleHandler, int>(new QueryNumberSingleHandler(12));

        TestPipelineBehavior1.Counter.Should().BeGreaterThan(0);
        TestPipelineBehavior2.Counter.Should().BeGreaterThan(0);

        TestPipelineBehavior1.Counter = 0;
        TestPipelineBehavior2.Counter = 0;

        QueryMultipleHandlerSharedState.Counter = 0;
    }

}