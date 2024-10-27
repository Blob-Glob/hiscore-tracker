using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HiscoreFunctionApp.Data;

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
        public async Task<HttpResponseData> RunRetrieveHiscore([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync("Hello, World!");
            return response;
        }

        [Function("DailyHiscoreRetrieval")]
        public async Task RunDailyHiscoreRetrieval([TimerTrigger("0 0 0 * * *")] TimerInfo timerInfo)
        {
            _logger.LogInformation("C# Timer trigger function executed at: {time}", DateTime.Now);



        }

        //function to add a user to the tracker
        [Function("AddUser")]
        public async Task<HttpResponseData> RunAddUser([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var requestBody = await req.ReadAsStringAsync();

            if(requestBody == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var user = JsonConvert.DeserializeObject<User>(requestBody);

            //TODO: Send a request to osrs api







            //TODO: Save the user and intial hiscore to the database




            return req.CreateResponse(HttpStatusCode.Created);








        }
    }
}
