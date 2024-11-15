using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using HiscoreFunctionApp.Data.Models;
using Polly;
using Polly.Retry;

namespace HiscoreFunctionApp.Services
{
    public interface IHiscoreApiService
    {
        Task<Stats?> GetHiscoreAsync(string username);
    }

    public class HiscoreApiService : IHiscoreApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly ILogger<HiscoreApiService> _logger;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

        public HiscoreApiService(HttpClient httpClient, IConfiguration config, ILogger<HiscoreApiService> logger)
        {
            _httpClient = httpClient;
            _config = config;
            _logger = logger;

            _retryPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (response, timespan, retryCount, context) =>
                    {
                        _logger.LogWarning($"Request failed with {response.Result.StatusCode}. Waiting {timespan} before next retry. Retry attempt {retryCount}");
                    });
        }

        public async Task<Stats?> GetHiscoreAsync(string username)
        {
            var response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync($"https://secure.runescape.com/m=hiscore_oldschool/index_lite.ws?player={username}"));
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogError($"User not found: {username}");
                    return null;
                }

                _logger.LogError($"Error: {response.StatusCode} {response.ReasonPhrase}");

                return null;
            }

            string responseBody = await response.Content.ReadAsStringAsync();

            return MapToStats(responseBody);
        }

        public static Stats MapToStats(string data)
        {
            // Split data by line breaks to handle each skill separately
            var lines = data.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 27)
            {
                throw new ArgumentException("Insufficient data to map to Stats.");
            }

            // Parse and map the data to the Stats model
            return new Stats
            {
                OverallRank = ParseOrDefault(lines[0].Split(','), 0),
                OverallLevel = ParseOrDefault(lines[0].Split(','), 1),
                OverallExp = ParseOrDefault(lines[0].Split(','), 2),

                AttackRank = ParseOrDefault(lines[1].Split(','), 0),
                AttackLevel = ParseOrDefault(lines[1].Split(','), 1),
                AttackExp = ParseOrDefault(lines[1].Split(','), 2),

                DefenceRank = ParseOrDefault(lines[2].Split(','), 0),
                DefenceLevel = ParseOrDefault(lines[2].Split(','), 1),
                DefenceExp = ParseOrDefault(lines[2].Split(','), 2),

                StrengthRank = ParseOrDefault(lines[3].Split(','), 0),
                StrengthLevel = ParseOrDefault(lines[3].Split(','), 1),
                StrengthExp = ParseOrDefault(lines[3].Split(','), 2),

                HitpointsRank = ParseOrDefault(lines[4].Split(','), 0),
                HitpointsLevel = ParseOrDefault(lines[4].Split(','), 1),
                HitpointsExp = ParseOrDefault(lines[4].Split(','), 2),

                RangedRank = ParseOrDefault(lines[5].Split(','), 0),
                RangedLevel = ParseOrDefault(lines[5].Split(','), 1),
                RangedExp = ParseOrDefault(lines[5].Split(','), 2),

                PrayerRank = ParseOrDefault(lines[6].Split(','), 0),
                PrayerLevel = ParseOrDefault(lines[6].Split(','), 1),
                PrayerExp = ParseOrDefault(lines[6].Split(','), 2),

                MagicRank = ParseOrDefault(lines[7].Split(','), 0),
                MagicLevel = ParseOrDefault(lines[7].Split(','), 1),
                MagicExp = ParseOrDefault(lines[7].Split(','), 2),

                CookingRank = ParseOrDefault(lines[8].Split(','), 0),
                CookingLevel = ParseOrDefault(lines[8].Split(','), 1),
                CookingExp = ParseOrDefault(lines[8].Split(','), 2),

                WoodcuttingRank = ParseOrDefault(lines[9].Split(','), 0),
                WoodcuttingLevel = ParseOrDefault(lines[9].Split(','), 1),
                WoodcuttingExp = ParseOrDefault(lines[9].Split(','), 2),

                FletchingRank = ParseOrDefault(lines[10].Split(','), 0),
                FletchingLevel = ParseOrDefault(lines[10].Split(','), 1),
                FletchingExp = ParseOrDefault(lines[10].Split(','), 2),

                FishingRank = ParseOrDefault(lines[11].Split(','), 0),
                FishingLevel = ParseOrDefault(lines[11].Split(','), 1),
                FishingExp = ParseOrDefault(lines[11].Split(','), 2),

                FiremakingRank = ParseOrDefault(lines[12].Split(','), 0),
                FiremakingLevel = ParseOrDefault(lines[12].Split(','), 1),
                FiremakingExp = ParseOrDefault(lines[12].Split(','), 2),

                CraftingRank = ParseOrDefault(lines[13].Split(','), 0),
                CraftingLevel = ParseOrDefault(lines[13].Split(','), 1),
                CraftingExp = ParseOrDefault(lines[13].Split(','), 2),

                SmithingRank = ParseOrDefault(lines[14].Split(','), 0),
                SmithingLevel = ParseOrDefault(lines[14].Split(','), 1),
                SmithingExp = ParseOrDefault(lines[14].Split(','), 2),

                MiningRank = ParseOrDefault(lines[15].Split(','), 0),
                MiningLevel = ParseOrDefault(lines[15].Split(','), 1),
                MiningExp = ParseOrDefault(lines[15].Split(','), 2),

                HerbloreRank = ParseOrDefault(lines[16].Split(','), 0),
                HerbloreLevel = ParseOrDefault(lines[16].Split(','), 1),
                HerbloreExp = ParseOrDefault(lines[16].Split(','), 2),

                AgilityRank = ParseOrDefault(lines[17].Split(','), 0),
                AgilityLevel = ParseOrDefault(lines[17].Split(','), 1),
                AgilityExp = ParseOrDefault(lines[17].Split(','), 2),

                ThievingRank = ParseOrDefault(lines[18].Split(','), 0),
                ThievingLevel = ParseOrDefault(lines[18].Split(','), 1),
                ThievingExp = ParseOrDefault(lines[18].Split(','), 2),

                SlayerRank = ParseOrDefault(lines[19].Split(','), 0),
                SlayerLevel = ParseOrDefault(lines[19].Split(','), 1),
                SlayerExp = ParseOrDefault(lines[19].Split(','), 2),

                FarmingRank = ParseOrDefault(lines[20].Split(','), 0),
                FarmingLevel = ParseOrDefault(lines[20].Split(','), 1),
                FarmingExp = ParseOrDefault(lines[20].Split(','), 2),

                RunecraftingRank = ParseOrDefault(lines[21].Split(','), 0),
                RunecraftingLevel = ParseOrDefault(lines[21].Split(','), 1),
                RunecraftingExp = ParseOrDefault(lines[21].Split(','), 2),

                HunterRank = ParseOrDefault(lines[22].Split(','), 0),
                HunterLevel = ParseOrDefault(lines[22].Split(','), 1),
                HunterExp = ParseOrDefault(lines[22].Split(','), 2),

                ConstructionRank = ParseOrDefault(lines[23].Split(','), 0),
                ConstructionLevel = ParseOrDefault(lines[23].Split(','), 1),
                ConstructionExp = ParseOrDefault(lines[23].Split(','), 2)
            };
        }

        // Helper function to parse values or return a default (e.g., -1) if parsing fails
        private static int ParseOrDefault(string[] values, int index)
        {
            if (values.Length > index && int.TryParse(values[index], out int result))
            {
                return result;
            }
            return -1; // default value if parsing fails
        }
    }
}
