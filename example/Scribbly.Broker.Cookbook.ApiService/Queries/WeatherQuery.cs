using Scribbly.Broker.Cookbook.ApiService.Handlers;

namespace Scribbly.Broker.Cookbook.ApiService.Queries;

public record WeatherQuery(string Summary) : TraceableRecord, INotification<WeatherForecast>;