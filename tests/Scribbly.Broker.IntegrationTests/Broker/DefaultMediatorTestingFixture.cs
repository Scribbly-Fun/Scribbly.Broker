using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Scribbly.Broker.IntegrationTests.Broker.Behaviors;

namespace Scribbly.Broker.IntegrationTests.Broker;

public class DefaultMediatorTestingFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; private set; } 

    public DefaultMediatorTestingFixture()
    {
        var services = new ServiceCollection();

        services.AddScribblyBroker(ops =>
        {
            ops.AsScoped = false;

            ops.AddHandlersFromAssembly<DefaultMediatorTestingFixture>();

            ops
                .AddBehavior<TestPipelineBehavior1>()
                .AddBehavior<TestPipelineBehavior2>();
        });

        ServiceProvider = services.BuildServiceProvider();
    }

    public TService GetService<TService>() where TService : notnull => ServiceProvider.GetRequiredService<TService>();

    public void Dispose()
    {
        if (ServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
