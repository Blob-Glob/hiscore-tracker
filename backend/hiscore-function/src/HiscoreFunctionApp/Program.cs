using HiscoreFunctionApp.Data;
using HiscoreFunctionApp.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureAppConfiguration(config =>
    {
        // Load settings from appsettings.json and environment variables
        config.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables();
    })
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        services.AddHttpClient();
        services.AddSingleton<IHiscoreApiService, HiscoreApiService>();

        // Access the configuration from context.Configuration
        var connectionString = context.Configuration.GetConnectionString("HiscoreContext");

        services.AddDbContextFactory<HiscoreContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
                sqlOptions.EnableRetryOnFailure()));
    })
    .Build();

host.Run();
