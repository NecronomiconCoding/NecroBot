#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Networking.Responses;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    internal class LevelUpPokemonTask
    {
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            var pokemonIdsToLevelUp = new List<ulong>();

            if (session.LogicSettings.LevelUpByCPorIv.ToLower().Contains("iv"))
            {
                pokemonIdsToLevelUp.AddRange(
                    session.Inventory.GetPokemons().Result
                        .Where(pokemonData => PokemonInfo.CalculatePokemonPerfection(pokemonData) >= session.LogicSettings.KeepMinIvPercentage && session.LogicSettings.PokemonsToLevelUp.Contains(pokemonData.PokemonId))
                        .Select(p => p.Id)
                        .ToList());
            }

            if (session.LogicSettings.LevelUpByCPorIv.ToLower().Contains("cp"))
            {
                pokemonIdsToLevelUp.AddRange(
                    session.Inventory.GetPokemons().Result
                        .Where(pokemonData => pokemonData.Cp > session.LogicSettings.KeepMinCp && session.LogicSettings.PokemonsToLevelUp.Contains(pokemonData.PokemonId))
                        .Select(p => p.Id)
                        .ToList());
            }

            foreach (var pokeId in pokemonIdsToLevelUp)
            {
                while (true)
                {
                    var upgradeResult = await session.Inventory.UpgradePokemon(pokeId);
                    if (upgradeResult.Result == UpgradePokemonResponse.Types.Result.Success)
                    {
                        Logger.Write("Pokemon Upgraded:" + upgradeResult.UpgradedPokemon.PokemonId + ":" + upgradeResult.UpgradedPokemon.Cp);
                    }
                    else 
                    {
                        if (upgradeResult.Result == UpgradePokemonResponse.Types.Result.ErrorInsufficientResources)
                            Logger.Write("Pokemon Upgrade Failed: Not enough candies");
                        if (upgradeResult.Result == UpgradePokemonResponse.Types.Result.ErrorUpgradeNotAvailable)
                            Logger.Write("Pokemon Upgrade Failed: Upgrade not available");
                        if (upgradeResult.Result == UpgradePokemonResponse.Types.Result.ErrorPokemonIsDeployed)
                            Logger.Write("Pokemon Upgrade Failed: Pokemon is defending gym");
                        if (upgradeResult.Result == UpgradePokemonResponse.Types.Result.ErrorPokemonNotFound)
                            Logger.Write("Pokemon Upgrade Failed: Something bad happened to your pokemon");
                        break;
                    }
                }
            }
        }
    }
}
