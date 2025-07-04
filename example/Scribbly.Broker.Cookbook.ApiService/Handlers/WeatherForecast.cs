namespace Scribbly.Broker.Cookbook.ApiService.Handlers;

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary) : TraceableRecord
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}