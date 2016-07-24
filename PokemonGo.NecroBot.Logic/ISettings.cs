using System.Collections.Generic;
using AllEnum;

namespace PokemonGo.NecroBot.Logic
{
    public interface ISettings
        : RocketAPI.ISettings
    {
        float KeepMinIVPercentage { get; }

        int KeepMinCP { get; }

        double WalkingSpeedInKilometerPerHour { get; }

        bool EvolveAllPokemonWithEnoughCandy { get; }

        bool TransferDuplicatePokemon { get; }

        int DelayBetweenPokemonCatch { get; }

        bool UsePokemonToNotCatchFilter { get; }

        int KeepMinDuplicatePokemon { get; }

        bool PrioritizeIVOverCP { get; }

        int MaxTravelDistanceInMeters { get; }

        bool EnableHighestPokemonInfo { get; }

        double LastPositionLatitude { get; set; }

        double LastPositionLongitude { get; set; }

        double LastPositionAltitude { get; set; }

        ICollection<KeyValuePair<ItemId, int>> ItemRecycleFilter { get; }

        ICollection<PokemonId> PokemonsToEvolve { get; }

        ICollection<PokemonId> PokemonsNotToTransfer { get; }

        ICollection<PokemonId> PokemonsNotToCatch { get; }
    }
}
