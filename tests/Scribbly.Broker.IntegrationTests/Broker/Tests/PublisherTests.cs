using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Scribbly.Broker.Errors;
using Scribbly.Broker.IntegrationTests.Broker.Behaviors;
using Scribbly.Broker.IntegrationTests.Broker.Handlers;
using Scribbly.Broker.IntegrationTests.Broker.Notifications;

namespace Scribbly.Broker.IntegrationTests.Broker.Tests;

public class PublisherTests(DefaultMediatorTestingFixture testingFixture) : IClassFixture<DefaultMediatorTestingFixture>
{
    private readonly IBroker _broker = testingFixture.ServiceProvider.GetRequiredService<IBroker>();

    [Theory]
    [InlineData("hdsbfjkhdsabgjhbsakg")]
    [InlineData("sdfgsdgr656745865485ehrytjgd")]
    [InlineData("456msdlksldfsugh85486_-dsjknkdj")]
    [InlineData("nvcmslfgkrnne98456798456")]
    public async Task Publish_Notification_When_TypeIsLocated_PublishesNotification(string message)
    {
        await _broker.Publish(new Notice(message));

        NoticeHandler.Notice.Should().Be(message);
    }
    
    [Fact]
    public async Task Publish_Notification_When_TypeIsNotLocated_ThrowsException()
    {
        Func<Task> taskFunc = () => _broker.Publish(new NoticeWithOutHandler("BAD"));
        await taskFunc.Should().ThrowAsync<BrokerHandlersNotFound<NoticeWithOutHandler>>();
    }

    [Fact]
    public async Task Publish_Notification_WithBehaviors_Should_InvokeBehaviors()
    {
        await _broker.Publish(new Notice("hello"));

        TestPipelineBehavior1.Counter.Should().BeGreaterThan(0);
        TestPipelineBehavior2.Counter.Should().BeGreaterThan(0);

        TestPipelineBehavior1.Counter = 0;
        TestPipelineBehavior2.Counter = 0;
    }

}
