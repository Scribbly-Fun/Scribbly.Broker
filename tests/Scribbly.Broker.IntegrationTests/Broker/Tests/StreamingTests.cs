using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Scribbly.Broker.Errors;
using Scribbly.Broker.IntegrationTests.Broker.Behaviors;
using Scribbly.Broker.IntegrationTests.Broker.Handlers;
using Scribbly.Broker.IntegrationTests.Broker.Notifications;

namespace Scribbly.Broker.IntegrationTests.Broker.Tests;

public class StreamingTests(DefaultMediatorTestingFixture testingFixture) : IClassFixture<DefaultMediatorTestingFixture>
{
    private readonly IBroker _broker = testingFixture.ServiceProvider.GetRequiredService<IBroker>();

    [Theory]
    [InlineData(12143)]
    [InlineData(11212)]
    [InlineData(4354)]
    [InlineData(9988)]
    public async Task Query_Stream_When_TypeIsLocated_ReturnsResultFromHandler(int value)
    {
        var total = 0;
        await foreach (var item in _broker.QueryStream<QueryNumberSingleHandler, int>(new QueryNumberSingleHandler(value)))
        {
            total += item;
        }

        total.Should().Be(value);
    }

    [Theory]
    [InlineData(12143)]
    [InlineData(11212)]
    [InlineData(4354)]
    [InlineData(9988)]
    public async Task Query_Stream_WithMultipleHandler_ShouldHandleAllNotifications(int value)
    {
        var counter = 0;
        await foreach (var item in _broker.QueryStream<QueryNumberMultipleHandlers, int>(new QueryNumberMultipleHandlers(value)))
        {
            counter++;
        }

        counter.Should().Be(3);

        QueryMultipleHandlerSharedState.Counter = 0;
    }
    
    [Theory]
    [InlineData(12143)]
    [InlineData(11212)]
    [InlineData(4354)]
    [InlineData(9988)]
    public async Task Query_Stream_WithMultipleHandler_ShouldHandleAggregateResults(int value)
    {
        var total = 0;
        await foreach (var item in _broker.QueryStream<QueryNumberMultipleHandlers, int>(new QueryNumberMultipleHandlers(value)))
        {
            total+= item;
        }

        total.Should().Be(value * 3);

        QueryMultipleHandlerSharedState.Counter = 0;
    }

    [Fact]
    public async Task Query_Stream_When_TypeIsNotLocated_ThrowMediatorException()
    {
        try
        {
            await foreach (var item in _broker.QueryStream<QueryWithOutHandler, string>(new QueryWithOutHandler("BAD")))
            {
            }

        }
        catch (BrokerHandlersNotFound<QueryWithOutHandler> e)
        {
            e.NotificationType.Should().Be<QueryWithOutHandler>();
            return;
        }
        
        Assert.Fail("Code did not hit excepted catch");
    }

    [Fact]
    public async Task Query_Stream_WithBehaviors_Should_InvokeBehaviors()
    {
        await foreach (var _ in _broker.QueryStream<QueryNumberSingleHandler, int>(new QueryNumberSingleHandler(12)))
        {
        }

        TestPipelineBehavior1.Counter.Should().BeGreaterThan(0);
        TestPipelineBehavior2.Counter.Should().BeGreaterThan(0);

        TestPipelineBehavior1.Counter = 0;
        TestPipelineBehavior2.Counter = 0;
    }
}