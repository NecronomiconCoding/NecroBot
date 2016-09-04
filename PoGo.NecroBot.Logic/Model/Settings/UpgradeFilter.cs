using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POGOProtos.Enums;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    public class UpgradeFilter
    {
        

        public UpgradeFilter(string LevelUpByCPorIV, double UpgradePokemonCpMinimum, double UpgradePokemonIvMinimum, string UpgradePokemonMinimumStatsOperator, bool OnlyUpgradeFavorites)
        {
            this.LevelUpByCPorIV = LevelUpByCPorIV;
            this.UpgradePokemonCpMinimum = UpgradePokemonCpMinimum;
            this.UpgradePokemonIvMinimum = UpgradePokemonIvMinimum;
            this.UpgradePokemonMinimumStatsOperator = UpgradePokemonMinimumStatsOperator;
            this.OnlyUpgradeFavorites = OnlyUpgradeFavorites;
        }

        public string LevelUpByCPorIV { get; set; }
        public double UpgradePokemonCpMinimum { get; set; }
        public double UpgradePokemonIvMinimum {get;set;}
    public string UpgradePokemonMinimumStatsOperator { get; set; }
        public bool OnlyUpgradeFavorites { get; set; }
        internal static Dictionary<PokemonId, UpgradeFilter> Default()
        {
            return new Dictionary<PokemonId, UpgradeFilter>()
            {
                { PokemonId.Dratini, new UpgradeFilter("iv", 600, 99, "or", false) }
            };
        }
    }
}
