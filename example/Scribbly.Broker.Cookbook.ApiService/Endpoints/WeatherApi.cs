using Scribbly.Broker.Cookbook.ApiService.Handlers;
using Scribbly.Broker.Cookbook.ApiService.Queries;
using Scribbly.Stencil;

namespace Scribbly.Broker.Cookbook.ApiService;

[EndpointGroup("/weather")]
public partial class WeatherApi
{
    private static readonly string[] Summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

    [GetEndpoint("/", "StreamWeather", "Gets the weather stream and publishes each message")]
    private static async Task<List<WeatherForecast>> StreamWeather(IBrokerStream streamer, IBrokerPublisher publisher)
    {
        var forecasts = new List<WeatherForecast>();

        foreach (var summary in Summaries)
        {
            await foreach (var forecast in streamer.QueryStream<WeatherQuery, WeatherForecast>(new WeatherQuery(summary)))
            {
                forecasts.Add(forecast);

                await publisher.Publish(forecast);
            }
        }
    
        return forecasts;
    }
}