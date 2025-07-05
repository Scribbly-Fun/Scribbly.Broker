using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Scribbly.Broker.Errors;
using Scribbly.Broker.IntegrationTests.Broker.Handlers;

namespace Scribbly.Broker.IntegrationTests.Builder.Tests;

public class HandlerTypeBuilderAssemblyTests
{
    [Fact]
    public void AddHandlersFromAssembly_Generic_Should_AddAllNotificationHandlers()
    {
        var services = new ServiceCollection();

        services.AddScribblyBroker(ops =>
        {
            ops.AsScoped = false;

            ops.AddHandlersFromAssembly<HandlerTypeBuilderAssemblyTests>();
        });

        var sp = services.BuildServiceProvider();

        sp.GetRequiredService<BrokerOptions>().NotificationHandlers.Count.Should().Be(TestingConstants.NumberOfNotificationHandlers);
    }
    
    [Fact]
    public void AddHandlersFromAssembly_Generic_Should_AddAllQueryHandlers()
    {
        var services = new ServiceCollection();

        services.AddScribblyBroker(ops =>
        {
            ops.AsScoped = false;

            ops.AddHandlersFromAssembly<HandlerTypeBuilderAssemblyTests>();
        });

        var sp = services.BuildServiceProvider();

        sp.GetRequiredService<BrokerOptions>().QueryHandlers.Count.Should().Be(TestingConstants.NumberOfQueryHandlers);
    }

    [Fact]
    public void AddHandlersFromAssembly_Generic_Should_Throw_DuplicateAssemblyException_WhenAddedMoreThanOnce()
    {
        Action action = () =>
        {
            var services = new ServiceCollection();

            services.AddScribblyBroker(ops =>
            {
                ops.AsScoped = false;

                ops.AddHandlersFromAssembly<HandlerTypeBuilderAssemblyTests>();
                ops.AddHandlersFromAssembly<HandlerTypeBuilderAssemblyTests>();
            });
        };

        action.Should().Throw<DuplicateAssemblyException>();
    }

    [Fact]
    public void AddHandlersFromAssembly_Assembly_Should_AddAllNotificationHandlers()
    {
        var services = new ServiceCollection();

        services.AddScribblyBroker(ops =>
        {
            ops.AsScoped = false;

            ops.AddHandlersFromAssembly(typeof(HandlerTypeBuilderAssemblyTests).Assembly);
        });

        var sp = services.BuildServiceProvider();

        sp.GetRequiredService<BrokerOptions>().NotificationHandlers.Count.Should().Be(TestingConstants.NumberOfNotificationHandlers);
    }

    [Fact]
    public void AddHandlersFromAssembly_Assembly_Should_AddAllQueryHandlers()
    {
        var services = new ServiceCollection();

        services.AddScribblyBroker(ops =>
        {
            ops.AsScoped = false;

            ops.AddHandlersFromAssembly(typeof(HandlerTypeBuilderAssemblyTests).Assembly);
        });

        var sp = services.BuildServiceProvider();

        sp.GetRequiredService<BrokerOptions>().QueryHandlers.Count.Should().Be(TestingConstants.NumberOfQueryHandlers);
    }

    [Fact]
    public void AddHandlersFromAssembly_Assembly_Should_Throw_DuplicateAssemblyException_WhenAddedMoreThanOnce()
    {
        Action action = () =>
        {
            var services = new ServiceCollection();

            services.AddScribblyBroker(ops =>
            {
                ops.AsScoped = false;

                ops.AddHandlersFromAssembly(typeof(HandlerTypeBuilderAssemblyTests).Assembly);
                ops.AddHandlersFromAssembly(typeof(HandlerTypeBuilderAssemblyTests).Assembly);
            });
        };

        action.Should().Throw<DuplicateAssemblyException>();
    }

    [Fact]
    public void AddHandlersFromAssembly_Type_Should_AddAllNotificationHandlers()
    {
        var services = new ServiceCollection();

        services.AddScribblyBroker(ops =>
        {
            ops.AsScoped = false;

            ops.AddHandlersFromAssembly(typeof(HandlerTypeBuilderAssemblyTests));
        });

        var sp = services.BuildServiceProvider();

        sp.GetRequiredService<BrokerOptions>().NotificationHandlers.Count.Should().Be(TestingConstants.NumberOfNotificationHandlers);
    }

    [Fact]
    public void AddHandlersFromAssembly_Type_Should_AddAllQueryHandlers()
    {
        var services = new ServiceCollection();

        services.AddScribblyBroker(ops =>
        {
            ops.AsScoped = false;

            ops.AddHandlersFromAssembly(typeof(HandlerTypeBuilderAssemblyTests));
        });

        var sp = services.BuildServiceProvider();

        sp.GetRequiredService<BrokerOptions>().QueryHandlers.Count.Should().Be(TestingConstants.NumberOfQueryHandlers);
    }
    
    [Theory]
    [InlineData(typeof(NoticeHandler))]
    public void AddHandlersFromAssembly_Type_Should_IncludeSpecificNotification(Type handlerToAssert)
    {
        var services = new ServiceCollection();

        services.AddScribblyBroker(ops =>
        {
            ops.AsScoped = false;

            ops.AddHandlersFromAssembly(typeof(HandlerTypeBuilderAssemblyTests));
        });

        var sp = services.BuildServiceProvider();

        sp.GetRequiredService<BrokerOptions>().NotificationHandlers.Should().Contain(t => t == handlerToAssert);
    }
    
    [Theory]
    [InlineData(typeof(QueryHandler))]
    [InlineData(typeof(QueryMultipleHandler1))]
    [InlineData(typeof(QueryMultipleHandler2))]
    [InlineData(typeof(QueryMultipleHandler3))]
    public void AddHandlersFromAssembly_Type_Should_IncludeSpecificQuery(Type handlerToAssert)
    {
        var services = new ServiceCollection();

        services.AddScribblyBroker(ops =>
        {
            ops.AsScoped = false;

            ops.AddHandlersFromAssembly(typeof(HandlerTypeBuilderAssemblyTests));
        });

        var sp = services.BuildServiceProvider();

        sp.GetRequiredService<BrokerOptions>().QueryHandlers.Should().Contain(t => t == handlerToAssert);
    }

    [Fact]
    public void AddHandlersFromAssembly_Type_Should_Throw_DuplicateAssemblyException_WhenAddedMoreThanOnce()
    {
        Action action = () =>
        {
            var services = new ServiceCollection();

            services.AddScribblyBroker(ops =>
            {
                ops.AsScoped = false;

                ops.AddHandlersFromAssembly(typeof(HandlerTypeBuilderAssemblyTests));
                ops.AddHandlersFromAssembly(typeof(HandlerTypeBuilderAssemblyTests));
            });
        };

        action.Should().Throw<DuplicateAssemblyException>();
    }
    
}
