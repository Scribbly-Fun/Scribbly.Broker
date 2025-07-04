var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Scribbly_Broker_Cookbook_ApiService>("scrb-broker");

builder.Build().Run();
