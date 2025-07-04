namespace Scribbly.Broker.Cookbook.ApiService.Handlers;

public sealed class WeatherForecastHandler(ILogger<WeatherForecastHandler> logger) : INotificationHandler<WeatherForecast> 
{
    /// <inheritdoc />
    public Task Handle(WeatherForecast notification, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Handled Weather: {Weather}", notification);
        return Task.CompletedTask;
    }
}