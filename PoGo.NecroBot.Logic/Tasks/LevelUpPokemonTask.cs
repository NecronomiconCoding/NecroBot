#region using directives

using System;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    internal class LevelUpPokemonTask
    {
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            var pokemonIdsToEvolve = new List<ulong>();

            if (session.LogicSettings.LevelUpByCPorIv.ToLower().Contains("iv"))
            {
                pokemonIdsToEvolve =
                    session.Inventory.GetPokemons()
                        .Result.Where(
                            pokemonData =>
                                PokemonInfo.CalculatePokemonPerfection(pokemonData) >=
                                session.LogicSettings.KeepMinIvPercentage)
                        .Select(p => p.Id)
                        .ToList();
            }
            else if (session.LogicSettings.LevelUpByCPorIv.ToLower().Contains("cp"))
            {
                pokemonIdsToEvolve =
                    session.Inventory.GetPokemons()
                        .Result.Where(pokemonData => pokemonData.Cp > session.LogicSettings.KeepMinCp)
                        .Select(p => p.Id).ToList();
            }

            foreach (var pokeId in pokemonIdsToEvolve)
            {
                while (true)
                {
                    var upgradeResult = await session.Inventory.UpgradePokemon(pokeId);
                    if (upgradeResult.Result.ToString().ToLower().Contains("success"))
                    {
                        Logger.Write("Pokemon Upgraded:" + upgradeResult.UpgradedPokemon.PokemonId + ":" +
                                     upgradeResult.UpgradedPokemon.Cp);
                    }
                    else if (upgradeResult.Result.ToString().ToLower().Contains("insufficient"))
                    {
                        Logger.Write("Pokemon Upgrade Failed Not Enough Resources");
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
