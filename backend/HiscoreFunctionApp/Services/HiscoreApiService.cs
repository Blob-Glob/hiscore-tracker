using HiscoreFunctionApp.Data;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace HiscoreFunctionApp.Services
{
    public interface IHiscoreApiService
    {
        Task<Stats> GetHiscoreAsync(string username);
    }

    public class HiscoreApiService : IHiscoreApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;


        public HiscoreApiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<Stats> GetHiscoreAsync(string username)
        {

            //string apiUrl = _config["HiScoreApiUrl"];
            var response = await _httpClient.GetAsync($"https://secure.runescape.com/m=hiscore_oldschool/index_lite.ws?player={username}");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            //TODO: Convert response body to stats

            return new Stats();

        }
    }
}
