#region

using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.GeneratedCode;
using System;
using System.Collections.Generic;

#endregion

namespace PokemonGo.RocketAPI.Console
{
    public class Settings : ISettings
    {
        public AuthType AuthType => (AuthType)Enum.Parse(typeof(AuthType), UserSettings.Default.AuthType, true);
        public string PtcUsername => UserSettings.Default.PtcUsername;
        public string PtcPassword => UserSettings.Default.PtcPassword;
        public double DefaultLatitude => UserSettings.Default.DefaultLatitude;
        public double DefaultLongitude => UserSettings.Default.DefaultLongitude;
        public double DefaultAltitude => UserSettings.Default.DefaultAltitude;
        public float KeepMinIVPercentage => UserSettings.Default.KeepMinIVPercentage;
        public int KeepMinCP => UserSettings.Default.KeepMinCP;
        public double WalkingSpeedInKilometerPerHour => UserSettings.Default.WalkingSpeedInKilometerPerHour;
        public bool EvolveAllPokemonWithEnoughCandy => UserSettings.Default.EvolveAllPokemonWithEnoughCandy;
        public bool TransferDuplicatePokemon => UserSettings.Default.TransferDuplicatePokemon;
        public int DelayBetweenMove => UserSettings.Default.DelayBetweenMove;
        public bool UsePokemonToNotCatchFilter => UserSettings.Default.UsePokemonToNotCatchFilter;

        public string GoogleRefreshToken
        {
            get { return UserSettings.Default.GoogleRefreshToken; }
            set
            {
                UserSettings.Default.GoogleRefreshToken = value;
                UserSettings.Default.Save();
            }
        }

        public ICollection<KeyValuePair<ItemId, int>> ItemRecycleFilter => new[]
        {
            // Keep these items at set amount.
            new KeyValuePair<ItemId, int>(ItemId.ItemUnknown, 0),
            new KeyValuePair<ItemId, int>(ItemId.ItemPokeBall, 20),
            new KeyValuePair<ItemId, int>(ItemId.ItemGreatBall, 20),
            new KeyValuePair<ItemId, int>(ItemId.ItemUltraBall, 50),
            new KeyValuePair<ItemId, int>(ItemId.ItemMasterBall, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemPotion, 0),
            new KeyValuePair<ItemId, int>(ItemId.ItemSuperPotion, 0),
            new KeyValuePair<ItemId, int>(ItemId.ItemHyperPotion, 20),
            new KeyValuePair<ItemId, int>(ItemId.ItemMaxPotion, 50),
            new KeyValuePair<ItemId, int>(ItemId.ItemRevive, 10),
            new KeyValuePair<ItemId, int>(ItemId.ItemMaxRevive, 50),
            new KeyValuePair<ItemId, int>(ItemId.ItemLuckyEgg, 200),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncenseOrdinary, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncenseSpicy, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncenseCool, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncenseFloral, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemTroyDisk, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemXAttack, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemXDefense, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemXMiracle, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemRazzBerry, 200),
            new KeyValuePair<ItemId, int>(ItemId.ItemBlukBerry, 10),
            new KeyValuePair<ItemId, int>(ItemId.ItemNanabBerry, 10),
            new KeyValuePair<ItemId, int>(ItemId.ItemWeparBerry, 30),
            new KeyValuePair<ItemId, int>(ItemId.ItemPinapBerry, 30),
            new KeyValuePair<ItemId, int>(ItemId.ItemSpecialCamera, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncubatorBasicUnlimited, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncubatorBasic, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemPokemonStorageUpgrade, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemItemStorageUpgrade, 100)
        };

        public ICollection<PokemonId> PokemonsToEvolve => new[]
        {
            // Only evolve these Pokémon.
            PokemonId.Rattata,
            PokemonId.Spearow,
            PokemonId.Ekans,
            PokemonId.Pikachu,
            PokemonId.Sandshrew,
            PokemonId.Clefable,
            PokemonId.Vulpix,
            PokemonId.Jigglypuff,
            PokemonId.Zubat,
            PokemonId.Paras,
            PokemonId.Venonat,
            PokemonId.Diglett,
            PokemonId.Meowth,
            PokemonId.Psyduck,
            PokemonId.Mankey,
            PokemonId.Tentacool,
            PokemonId.Ponyta,
            PokemonId.Slowpoke,
            PokemonId.Magnemite,
            PokemonId.Doduo,
            PokemonId.Seel,
            PokemonId.Grimer,
            PokemonId.Shellder,
            PokemonId.Drowzee,
            PokemonId.Krabby,
            PokemonId.Voltorb,
            PokemonId.Exeggcute,
            PokemonId.Cubone,
            PokemonId.Koffing,
            PokemonId.Rhyhorn,
            PokemonId.Horsea,
            PokemonId.Goldeen,
            PokemonId.Staryu,
            PokemonId.Omanyte,
            PokemonId.Kabuto,
            PokemonId.Pidgey,
            PokemonId.Weedle,
            PokemonId.Caterpie
        };

        public ICollection<PokemonId> PokemonsNotToTransfer => new[]
        {
            // Do not transfer these Pokémon.
            PokemonId.Dragonite,
            PokemonId.Charizard,
            PokemonId.Zapdos,
            PokemonId.Snorlax,
            PokemonId.Alakhazam,
            PokemonId.Mew,
            PokemonId.Mewtwo
        };

        public ICollection<PokemonId> PokemonsNotToCatch => new[]
        {
            // Do not catch these Pokémon.
            PokemonId.Pidgey,
            PokemonId.Rattata
        };
    }
}
