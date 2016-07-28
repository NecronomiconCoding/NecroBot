using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using POGOProtos.Enums;
using POGOProtos.Inventory.Item;
using PoGo.NecroBot.Logic.Filters;

namespace PoGo.NecroBot.Logic.Settings {
    public interface IConfigurationSettings {
        bool UseGpxPathing { get; set; }
        bool UsePokemonToNotCatchFilter { get; set; }
        bool PrioritizeIvOverCp { get; set; }
        bool KeepPokemonsThatCanEvolve { get; set; }
        bool TransferDuplicatePokemon { get; set; }
        bool EvolveAllPokemonAboveIv { get; set; }
        bool EvolveAllPokemonWithEnoughCandy { get; set; }
        bool UseEggIncubators { get; set; }
        bool UseLuckyEggsWhileEvolving { get; set; }
        bool StartupWelcomeDelay { get; set; }
        bool AutoUpdate { get; set; }
        bool RenameAboveIv { get; set; }
        bool DumpPokemonStats { get; set; }

        int KeepMinCp { get; set; }
        int DelayBetweenPokemonCatch { get; set; }
        int KeepMinDuplicatePokemon { get; set; }
        int UseLuckyEggsMinPokemonAmount { get; set; }
        int MaxTravelDistanceInMeters { get; set; }
        int AmountOfPokemonToDisplayOnStart { get; set; }

        float KeepMinIvPercentage { get; set; }
        float EvolveAboveIvValue { get; set; }
        double DefaultAltitude { get; set; }
        double DefaultLatitude { get; set; }
        double DefaultLongitude { get; set; }
        double WalkingSpeedInKilometerPerHour { get; set; }

        string GpxFile { get; set; }
        string TranslationLanguageCode { get; set; }
        

        List<KeyValuePair<ItemId, int>> ItemRecycleFilter { get; set; }
        List<PokemonId> PokemonsToEvolve { get; set; }
        List<PokemonId> PokemonsNotToTransfer { get; set; }
        List<PokemonId> PokemonsNotToCatch { get; set; }
        Dictionary<PokemonId, PokemonTransferFilter> PokemonsTransferFilter { get; set; }
    }
}
