using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using HiscoreFunctionApp.Services;

namespace HiscoreFunctionApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices((context, services) =>
                {
                    // Register HttpClient
                    services.AddHttpClient();
                    // Register your services
                    services.AddSingleton<IHiscoreApiService, HiscoreApiService>();
                })
                .Build();

            host.Run();
        }
    }
}
