var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Scribbly_Broker_Cookbook_ApiService>("apiservice");

builder.AddProject<Projects.Scribbly_Broker_Cookbook_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
