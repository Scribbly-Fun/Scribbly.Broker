using Scribbly.Broker;
using Scribbly.Broker.Behaviors;
using Scribbly.Broker.Cookbook.ApiService.Handlers;
using Scribbly.Broker.Cookbook.ApiService.Queries;
using Scribbly.Stencil;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScribblyBroker(options =>
{
    options.AsScoped = true;

    options.AddHandlersFromAssembly<Program>();

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

app.MapStencilApp();

app.MapDefaultEndpoints();

app.Run();