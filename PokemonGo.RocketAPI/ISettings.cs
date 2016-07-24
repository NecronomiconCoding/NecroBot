#region

using System.Collections.Generic;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.GeneratedCode;

#endregion

namespace PokemonGo.RocketAPI
{
    public interface ISettings
    {
        /// <summary>
        /// The authentication type, either Google or PTC
        /// </summary>
        AuthType AuthType { get; }
        double DefaultLatitude { get; }
        double DefaultLongitude { get; }
        double DefaultAltitude { get; }
        string PtcPassword { get; }
        string PtcUsername { get; }

        /// <summary>
        /// The minimum IV % of a pokemon ot keep
        /// </summary>
        float KeepMinIVPercentage { get; }

        /// <summary>
        /// The minimum CP of a pokemon to keep
        /// </summary>
        int KeepMinCP { get; }

        /// <summary>
        /// The speed at which the bot 'walks', in KM/H
        /// </summary>
        double WalkingSpeedInKilometerPerHour { get; }

        /// <summary>
        /// If true, evolves all pokemon who have enough candy
        /// </summary>
        bool EvolveAllPokemonWithEnoughCandy { get; }

        /// <summary>
        /// If true, transfers duplicate pokemon
        /// </summary>
        bool TransferDuplicatePokemon { get; }

        /// <summary>
        /// The delay between attempts to catch pokemon in milliseconds
        /// </summary>
        int DelayBetweenPokemonCatch { get; }

        /// <summary>
        /// If true, pokemon to catch will be filtered
        /// </summary>
        bool UsePokemonToNotCatchFilter { get; }

        /// <summary>
        /// The minimum amount of duplicate pokemon to keep
        /// </summary>
        int KeepMinDuplicatePokemon { get; }

        /// <summary>
        /// If this is true, pokemon with low IVs will be transferred EVEN IF they have a high CP
        /// </summary>
        bool PrioritizeIVOverCP { get; }

        /// <summary>
        /// The maximum distance to travel in any direction
        /// </summary>
        int MaxTravelDistanceInMeters { get; }

        /// <summary>
        /// Items & amounts to keep, the rest are recycled
        /// </summary>
        ICollection<KeyValuePair<ItemId, int>> ItemsToKeep { get; }

        /// <summary>
        /// Pokemon which will be evolved if there is enough candy
        /// </summary>
        ICollection<PokemonId> PokemonToEvolve { get; }

        /// <summary>
        /// Pokemon to keep (will not transfer)
        /// </summary>
        ICollection<PokemonId> PokemonToKeep { get; }

        /// <summary>
        /// Pokemon to not catch
        /// </summary>
        ICollection<PokemonId> PokemonToIgnore { get; }
    }
}