using Scribbly.Broker;
using Scribbly.Broker.Behaviors;
using Scribbly.Broker.Cookbook.ApiService.Handlers;
using Scribbly.Broker.Cookbook.ApiService.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScribblyBroker(options =>
{
    options.AsScoped = true;

    options.Assembly = typeof(Program).Assembly;

    options
        .AddBehavior<TracingBehavior>()
        .AddBehavior<ExceptionBehavior>();
});


builder.AddServiceDefaults();

builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

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

app.MapDefaultEndpoints();

app.Run();

