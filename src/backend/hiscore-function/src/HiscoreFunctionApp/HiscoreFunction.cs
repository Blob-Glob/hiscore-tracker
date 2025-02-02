using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HiscoreFunctionApp.Services;
using HiscoreFunctionApp.Data.Models;
using HiscoreFunctionApp.Data;
using Microsoft.EntityFrameworkCore;

namespace HiscoreFunctionApp
{
    public class HiscoreFunction
    {
        private readonly ILogger<HiscoreFunction> _logger;
        private readonly IHiscoreApiService _hiscoreApiService;
        private readonly HiscoreContext _hiscoreContext;

        public HiscoreFunction(ILogger<HiscoreFunction> logger, IHiscoreApiService hiscoreApiService, HiscoreContext hiscoreContext)
        {
            _logger = logger;
            _hiscoreApiService = hiscoreApiService;
            _hiscoreContext = hiscoreContext;
        }

        [Function("RetrieveHiscore")]
        public async Task<HttpResponseData> RunRetrieveHiscore([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var requestBody = await req.ReadAsStringAsync();
            if (string.IsNullOrEmpty(requestBody))
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var user = JsonConvert.DeserializeObject<Users>(requestBody);
            if (user == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var userStats = await _hiscoreApiService.GetHiscoreAsync(user.Name);
            if (userStats == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            var existingUser = await _hiscoreContext.Users.FirstOrDefaultAsync(u => u.Name == user.Name);
            if (existingUser == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            userStats.UserID = existingUser.UserID;
            _hiscoreContext.Stats.Add(userStats);
            await _hiscoreContext.SaveChangesAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync("Stats retrieved and saved successfully.");
            return response;
        }

        [Function("DailyHiscoreRetrieval")]
        public async Task RunDailyHiscoreRetrieval([TimerTrigger("0 0 0 * * *")] TimerInfo timerInfo)
        {
            _logger.LogInformation("C# Timer trigger function executed at: {time}", DateTime.Now);

            var users = await _hiscoreContext.Users.ToListAsync();
            foreach (var user in users)
            {
                var userStats = await _hiscoreApiService.GetHiscoreAsync(user.Name);
                if (userStats != null)
                {
                    userStats.UserID = user.UserID;
                    _hiscoreContext.Stats.Add(userStats);
                }
            }

            await _hiscoreContext.SaveChangesAsync();
            _logger.LogInformation("Daily hiscore retrieval completed.");
        }

        [Function("AddUser")]
        public async Task<HttpResponseData> RunAddUser([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var requestBody = await req.ReadAsStringAsync();

            if (requestBody == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var user = JsonConvert.DeserializeObject<Users>(requestBody);

            if (user == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var existingUser = await _hiscoreContext.Users.FirstOrDefaultAsync(u => u.Name == user.Name);
            if (existingUser != null)
            {
                var conflictResponse = req.CreateResponse(HttpStatusCode.Conflict);
                await conflictResponse.WriteStringAsync("User already exists.");
                return conflictResponse;
            }

            var userStats = await _hiscoreApiService.GetHiscoreAsync(user.Name);

            if (userStats == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            _hiscoreContext.Users.Add(user);
            var newUser = await _hiscoreContext.SaveChangesAsync();

            userStats.UserID = user.UserID;
            _hiscoreContext.Stats.Add(userStats);
            await _hiscoreContext.SaveChangesAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync("User added successfully.");
            return response;
        }

        [Function("GetUserStats")]
        public async Task<HttpResponseData> RunGetUserStats([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var requestBody = await req.ReadAsStringAsync();

            if (requestBody == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var user = JsonConvert.DeserializeObject<Users>(requestBody);

            if (user == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var userInfo = await _hiscoreContext.Users.FirstOrDefaultAsync(u => u.Name == user.Name);

            if (userInfo == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            var userStats = await _hiscoreContext.Stats
                .Where(s => s.UserID == userInfo.UserID)
                .OrderByDescending(s => s.CreatedAt)
                .FirstOrDefaultAsync();

            if (userStats == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync(JsonConvert.SerializeObject(userStats));
            return response;
        }
    }
}
