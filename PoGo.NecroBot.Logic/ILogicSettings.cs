#region using directives

using System.Collections.Generic;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;

#endregion

namespace PoGo.NecroBot.Logic
{
    public interface ILogicSettings
    {
        bool AutoUpdate { get; }
        float KeepMinIvPercentage { get; }
        int KeepMinCp { get; }
        double WalkingSpeedInKilometerPerHour { get; }
        bool EvolveAllPokemonWithEnoughCandy { get; }
        bool KeepPokemonsThatCanEvolve { get; }
        bool TransferDuplicatePokemon { get; }
        int DelayBetweenPokemonCatch { get; }
        bool UsePokemonToNotCatchFilter { get; }
        int KeepMinDuplicatePokemon { get; }
        bool PrioritizeIvOverCp { get; }
        int MaxTravelDistanceInMeters { get; }
        bool UseGpxPathing { get; }
        string GpxFile { get; }
        bool UseLuckyEggsWhileEvolving { get; }
        bool EvolveAllPokemonAboveIv { get; }
        float EvolveAboveIvValue { get; }
        bool RenameAboveIv { get; }
        int AmountOfPokemonToDisplayOnStart { get; }
        string TranslationLanguageCode { get; }

        ICollection<KeyValuePair<ItemId, int>> ItemRecycleFilter { get; }

        ICollection<PokemonId> PokemonsToEvolve { get; }

        ICollection<PokemonId> PokemonsNotToTransfer { get; }

        ICollection<PokemonId> PokemonsNotToCatch { get; }
    }
}