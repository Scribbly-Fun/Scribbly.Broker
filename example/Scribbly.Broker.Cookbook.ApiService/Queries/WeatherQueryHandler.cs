using Scribbly.Broker.Cookbook.ApiService.Handlers;
using System;

namespace Scribbly.Broker.Cookbook.ApiService.Queries;

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

public sealed class StaticQueryHandler : INotificationHandler<WeatherQuery, WeatherForecast>
{
    /// <inheritdoc />
    public Task<WeatherForecast> Handle(WeatherQuery notification, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            22,
            notification.Summary
        ));
    }
}

public sealed class RealQueryHandler: INotificationHandler<WeatherQuery, WeatherForecast>
{
    /// <inheritdoc />
    public Task<WeatherForecast> Handle(WeatherQuery notification, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            27,
            notification.Summary
        ));
    }
}