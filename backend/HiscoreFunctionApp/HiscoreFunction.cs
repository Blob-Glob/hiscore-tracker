using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HiscoreFunctionApp.Data;
using HiscoreFunctionApp.Services;

namespace HiscoreFunctionApp
{
    public class HiscoreFunction
    {
        private readonly ILogger<HiscoreFunction> _logger;
        private readonly IHiscoreApiService _hiscoreApiService;

        public HiscoreFunction(ILogger<HiscoreFunction> logger, IHiscoreApiService hiscoreApiService, HttpClient httpClient)
        {
            _logger = logger;
            _hiscoreApiService = hiscoreApiService;
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

            if(user == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            //TODO: Send a request to osrs api

            var p = await _hiscoreApiService.GetHiscoreAsync(user.Name);



            //TODO: Save the user and intial hiscore to the database




            return req.CreateResponse(HttpStatusCode.Created);








        }
    }
}
