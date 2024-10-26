using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace HiscoreFunctionApp
{
    public class HiscoreFunction
    {
        private readonly ILogger<HiscoreFunction> _logger;

        public HiscoreFunction(ILogger<HiscoreFunction> logger)
        {
            _logger = logger;
        }

        [Function("RetriveHiscore")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync("Hello, World!");
            return response;
        }
    }
}
