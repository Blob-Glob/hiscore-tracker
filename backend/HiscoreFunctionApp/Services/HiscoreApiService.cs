using HiscoreFunctionApp.Data;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

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
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Failed to get hiscore");
            }

            string responseBody = await response.Content.ReadAsStringAsync();

            MapToStats(responseBody);

            return new Stats();
        }

        public static Stats MapToStats(string data)
        {
            var values = data.Split(',');

            if (data.Length < 81) // Ensure there are enough values (27 skills * 3 fields = 81)
            {
                throw new ArgumentException("Insufficient data to map to Stats.");
            }

            Stats stats = MapToStats(data);

            return new Stats
            {
                OverallRank = int.Parse(values[0]),
                OverallLevel = int.Parse(values[1]),
                OverallExp = int.Parse(values[2]),

                AttackRank = int.Parse(values[3]),
                AttackLevel = int.Parse(values[4]),
                AttackExp = int.Parse(values[5]),

                DefenceRank = int.Parse(values[6]),
                DefenceLevel = int.Parse(values[7]),
                DefenceExp = int.Parse(values[8]),

                StrengthRank = int.Parse(values[9]),
                StrengthLevel = int.Parse(values[10]),
                StrengthExp = int.Parse(values[11]),

                HitpointsRank = int.Parse(values[12]),
                HitpointsLevel = int.Parse(values[13]),
                HitpointsExp = int.Parse(values[14]),

                RangedRank = int.Parse(values[15]),
                RangedLevel = int.Parse(values[16]),
                RangedExp = int.Parse(values[17]),

                PrayerRank = int.Parse(values[18]),
                PrayerLevel = int.Parse(values[19]),
                PrayerExp = int.Parse(values[20]),

                MagicRank = int.Parse(values[21]),
                MagicLevel = int.Parse(values[22]),
                MagicExp = int.Parse(values[23]),

                CookingRank = int.Parse(values[24]),
                CookingLevel = int.Parse(values[25]),
                CookingExp = int.Parse(values[26]),

                WoodcuttingRank = int.Parse(values[27]),
                WoodcuttingLevel = int.Parse(values[28]),
                WoodcuttingExp = int.Parse(values[29]),

                FletchingRank = int.Parse(values[30]),
                FletchingLevel = int.Parse(values[31]),
                FletchingExp = int.Parse(values[32]),

                FishingRank = int.Parse(values[33]),
                FishingLevel = int.Parse(values[34]),
                FishingExp = int.Parse(values[35]),

                FiremakingRank = int.Parse(values[36]),
                FiremakingLevel = int.Parse(values[37]),
                FiremakingExp = int.Parse(values[38]),

                CraftingRank = int.Parse(values[39]),
                CraftingLevel = int.Parse(values[40]),
                CraftingExp = int.Parse(values[41]),

                SmithingRank = int.Parse(values[42]),
                SmithingLevel = int.Parse(values[43]),
                SmithingExp = int.Parse(values[44]),

                MiningRank = int.Parse(values[45]),
                MiningLevel = int.Parse(values[46]),
                MiningExp = int.Parse(values[47]),

                HerbloreRank = int.Parse(values[48]),
                HerbloreLevel = int.Parse(values[49]),
                HerbloreExp = int.Parse(values[50]),

                AgilityRank = int.Parse(values[51]),
                AgilityLevel = int.Parse(values[52]),
                AgilityExp = int.Parse(values[53]),

                ThievingRank = int.Parse(values[54]),
                ThievingLevel = int.Parse(values[55]),
                ThievingExp = int.Parse(values[56]),

                SlayerRank = int.Parse(values[57]),
                SlayerLevel = int.Parse(values[58]),
                SlayerExp = int.Parse(values[59]),

                FarmingRank = int.Parse(values[60]),
                FarmingLevel = int.Parse(values[61]),
                FarmingExp = int.Parse(values[62]),

                RunecraftingRank = int.Parse(values[63]),
                RunecraftingLevel = int.Parse(values[64]),
                RunecraftingExp = int.Parse(values[65]),

                HunterRank = int.Parse(values[66]),
                HunterLevel = int.Parse(values[67]),
                HunterExp = int.Parse(values[68]),

                ConstructionRank = int.Parse(values[69]),
                ConstructionLevel = int.Parse(values[70]),
                ConstructionExp = int.Parse(values[71])
            };
        }
    }
}
