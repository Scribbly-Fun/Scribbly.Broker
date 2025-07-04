![scribbly_banner.png](./docs/scribbly_banner.png)

![GitHub](https://img.shields.io/github/license/Scribbly-Fun/Scribbly.Broker) 
![GitHub all releases](https://img.shields.io/github/downloads/Scribbly-Fun/Scribbly.Broker/total) 
![Nuget](https://img.shields.io/nuget/dt/Scribbly.Broker)
[![GitHub issues](https://img.shields.io/github/issues/Scribbly-Fun/Scribbly.Broker)](https://github.com/Scribbly-Fun/Scribbly.Broker/issues)
![GitHub Repo stars](https://img.shields.io/github/stars/Scribbly-Fun/Scribbly.Broker?style=social)
![GitHub forks](https://img.shields.io/github/forks/Scribbly-Fun/Scribbly.Broker?style=social)
[![DOTNET TEST](https://github.com/Scribbly-Fun/Scribbly.Broker/actions/workflows/dotnet-test.yml/badge.svg)](https://github.com/Scribbly-Fun/Scribbly.Broker/actions/workflows/dotnet-test.yml)
[![PUBLISH RELEASE](https://github.com/Scribbly-Fun/Scribbly.Broker/actions/workflows/dotnet-release.yml/badge.svg)](https://github.com/Scribbly-Fun/Scribbly.Broker/actions/workflows/dotnet-release.yml)
![GitHub last commit (branch)](https://img.shields.io/github/last-commit/Scribbly-Fun/Scribbly.Broker/main)

# Scribbly Broker

A publisher used for commands and queries based on the Mediator Design Pattern.

![Static Badge](https://img.shields.io/badge/COMMAND-blue)  
![Static Badge](https://img.shields.io/badge/QUERY-green)
![Static Badge](https://img.shields.io/badge/NOTIFY-blue)


## Table of Contents
1. [Packages](#Packages)
1. [Notifcations](#Notifcations)
2. [Handlers](#Handlers)
3. [Behaviors](#Behaviors)
5. [Pipelines](#Pipelines)

## Example

Below is a brief snip of code to get you started before reading more.

1. Add a reference to the `Scribbly.Broker.MicrosoftHosting` package

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScribblyBroker(options =>
{
    options.AsScoped = true;

    options.Assembly = typeof(Program).Assembly;

    options
        .AddBehavior<TracingBehavior>()
        .AddBehavior<ExceptionBehavior>();
});

```

2. Create some notifications `INotification`

``` csharp
public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary) : INotification
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

```
3. Create some queries `INotification<TResult>` (these accept data and return a result)

```csharp

public record WeatherQuery(string Summary) : INotification<WeatherForecast>;

```

4. Create handlers `INotificationHandler` for the notifications

```csharp
public sealed class WeatherForecastHandler(ILogger<WeatherForecastHandler> logger) : INotificationHandler<WeatherForecast> 
{
    /// <inheritdoc />
    public Task Handle(WeatherForecast notification, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Handled Weather: {Weather}", notification);
        return Task.CompletedTask;
    }
}

```

5. Create query handlers `INotificationHandler` for the messages that return results

```csharp
public sealed class RandomQueryHandler: INotificationHandler<WeatherQuery, WeatherForecast>
{
    private static Random Random = new Random();
    
    /// <inheritdoc />
    public Task<WeatherForecast> Handle(WeatherQuery notification, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(Random.Next(1, 20))),
            Random.Next(12, 55),
            notification.Summary
        ));
    }
}

```
5. Publish and query

```csharp

app.MapGet("/weather", async (IBrokerStream streamer, IBrokerPublisher publisher) =>
{
    var forecasts = new List<WeatherForecast>();

    foreach (var summary in summaries)
    {
        await foreach (var forecast in streamer.QueryStream<WeatherQuery, WeatherForecast>(new WeatherQuery(summary)))
        {
            forecasts.Add(forecast);

            await publisher.Publish(forecast);
        }
    }
    
    return forecasts;
});
```

# 🎁 Packages

![Scribbly.Broker.Contract](https://img.shields.io/badge/Scribbly.Broker.Contract-blue)  
![Scribbly.Broker](https://img.shields.io/badge/Scribbly.Broker-blue)  
![Scribbly.Broker.MicrosoftHosting](https://img.shields.io/badge/Scribbly.Broker.MicrosoftHosting-blue)  
![Scribbly.Broker.OpenTelemetry](https://img.shields.io/badge/Scribbly.Broker.OpenTelemetry-blue)  

# 💪 Notifcations


# 🛒 Handlers


# 🛁 Behaviors


# 🛁 Pipelines


# 🎉 Hosting
