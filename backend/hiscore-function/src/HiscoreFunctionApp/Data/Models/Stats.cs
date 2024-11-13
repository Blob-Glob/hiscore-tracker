using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiscoreFunctionApp.Data.Models
{
    public class Stats
    {
        public int StatID { get; set; }
        public int UserID { get; set; }
        public int OverallRank { get; set; }
        public int OverallLevel { get; set; }
        public int OverallExp { get; set; }

        public int AttackRank { get; set; }
        public int AttackLevel { get; set; }
        public int AttackExp { get; set; }

        public int DefenceRank { get; set; }
        public int DefenceLevel { get; set; }
        public int DefenceExp { get; set; }

        public int StrengthRank { get; set; }
        public int StrengthLevel { get; set; }
        public int StrengthExp { get; set; }

        public int HitpointsRank { get; set; }
        public int HitpointsLevel { get; set; }
        public int HitpointsExp { get; set; }

        public int RangedRank { get; set; }
        public int RangedLevel { get; set; }
        public int RangedExp { get; set; }

        public int PrayerRank { get; set; }
        public int PrayerLevel { get; set; }
        public int PrayerExp { get; set; }

        public int MagicRank { get; set; }
        public int MagicLevel { get; set; }
        public int MagicExp { get; set; }

        public int CookingRank { get; set; }
        public int CookingLevel { get; set; }
        public int CookingExp { get; set; }

        public int WoodcuttingRank { get; set; }
        public int WoodcuttingLevel { get; set; }
        public int WoodcuttingExp { get; set; }

        public int FletchingRank { get; set; }
        public int FletchingLevel { get; set; }
        public int FletchingExp { get; set; }

        public int FishingRank { get; set; }
        public int FishingLevel { get; set; }
        public int FishingExp { get; set; }

        public int FiremakingRank { get; set; }
        public int FiremakingLevel { get; set; }
        public int FiremakingExp { get; set; }

        public int CraftingRank { get; set; }
        public int CraftingLevel { get; set; }
        public int CraftingExp { get; set; }

        public int SmithingRank { get; set; }
        public int SmithingLevel { get; set; }
        public int SmithingExp { get; set; }

        public int MiningRank { get; set; }
        public int MiningLevel { get; set; }
        public int MiningExp { get; set; }

        public int HerbloreRank { get; set; }
        public int HerbloreLevel { get; set; }
        public int HerbloreExp { get; set; }

        public int AgilityRank { get; set; }
        public int AgilityLevel { get; set; }
        public int AgilityExp { get; set; }

        public int ThievingRank { get; set; }
        public int ThievingLevel { get; set; }
        public int ThievingExp { get; set; }

        public int SlayerRank { get; set; }
        public int SlayerLevel { get; set; }
        public int SlayerExp { get; set; }

        public int FarmingRank { get; set; }
        public int FarmingLevel { get; set; }
        public int FarmingExp { get; set; }

        public int RunecraftingRank { get; set; }
        public int RunecraftingLevel { get; set; }
        public int RunecraftingExp { get; set; }

        public int HunterRank { get; set; }
        public int HunterLevel { get; set; }
        public int HunterExp { get; set; }

        public int ConstructionRank { get; set; }
        public int ConstructionLevel { get; set; }
        public int ConstructionExp { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
