var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.HiscoreTrackerApp_ApiService>("apiservice");

builder.AddProject<Projects.HiscoreTrackerApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddAzureFunctionsProject<Projects.HiscoreTrackerApp_Function>("functionservice");

builder.Build().Run();
