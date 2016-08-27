#region using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Inventory.Item;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class UseIncubatorsTask
    {
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Refresh inventory so that the player stats are fresh
            await session.Inventory.RefreshCachedInventory();

            var playerStats = (await session.Inventory.GetPlayerStats()).FirstOrDefault();
            if (playerStats == null)
                return;

            var kmWalked = playerStats.KmWalked;

            var incubators = (await session.Inventory.GetEggIncubators())
                .Where(x => x.UsesRemaining > 0 || x.ItemId == ItemId.ItemIncubatorBasicUnlimited)
                .OrderByDescending(x => x.ItemId == ItemId.ItemIncubatorBasicUnlimited)
                .ToList();

            var unusedEggs = (await session.Inventory.GetEggs())
                .Where(x => string.IsNullOrEmpty(x.EggIncubatorId))
                .OrderBy(x => x.EggKmWalkedTarget - x.EggKmWalkedStart)
                .ToList();

            var rememberedIncubatorsFilePath = Path.Combine(session.LogicSettings.ProfilePath, "temp", "incubators.json");
            var rememberedIncubators = GetRememberedIncubators(rememberedIncubatorsFilePath);
            var pokemons = (await session.Inventory.GetPokemons()).ToList();

            // Check if eggs in remembered incubator usages have since hatched
            // (instead of calling session.Client.Inventory.GetHatchedEgg(), which doesn't seem to work properly)
            foreach (var incubator in rememberedIncubators)
            {
                var hatched = pokemons.FirstOrDefault(x => !x.IsEgg && x.Id == incubator.PokemonId);
                if (hatched == null) continue;

                session.EventDispatcher.Send(new EggHatchedEvent
                {
                    Id = hatched.Id,
                    PokemonId = hatched.PokemonId,
                    Level = PokemonInfo.GetLevel(hatched),
                    Cp = hatched.Cp,
                    MaxCp = PokemonInfo.CalculateMaxCp(hatched),
                    Perfection = Math.Round(PokemonInfo.CalculatePokemonPerfection(hatched), 2)
                });
            }

            var newRememberedIncubators = new List<IncubatorUsage>();

            foreach (var incubator in incubators)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (incubator.PokemonId == 0)
                {
                    // Unlimited incubators prefer short eggs, limited incubators prefer long eggs
                    // Special case: If only one incubator is available at all, it will prefer long eggs
                    var egg = (incubator.ItemId == ItemId.ItemIncubatorBasicUnlimited && incubators.Count > 1)
                        ? unusedEggs.FirstOrDefault()
                        : unusedEggs.LastOrDefault();

                    if (egg == null)
                        continue;

                    // Skip (save) limited incubators depending on user choice in config
                    if (!session.LogicSettings.UseLimitedEggIncubators 
                        && incubator.ItemId != ItemId.ItemIncubatorBasicUnlimited)
                        continue;

                    var response = await session.Client.Inventory.UseItemEggIncubator(incubator.Id, egg.Id);
                    unusedEggs.Remove(egg);

                    newRememberedIncubators.Add(new IncubatorUsage { IncubatorId = incubator.Id, PokemonId = egg.Id });

                    session.EventDispatcher.Send(new EggIncubatorStatusEvent
                    {
                        IncubatorId = incubator.Id,
                        WasAddedNow = true,
                        PokemonId = egg.Id,
                        KmToWalk = egg.EggKmWalkedTarget,
                        KmRemaining = response.EggIncubator.TargetKmWalked - kmWalked
                    });
                }
                else
                {
                    newRememberedIncubators.Add(new IncubatorUsage
                    {
                        IncubatorId = incubator.Id,
                        PokemonId = incubator.PokemonId
                    });

                    session.EventDispatcher.Send(new EggIncubatorStatusEvent
                    {
                        IncubatorId = incubator.Id,
                        PokemonId = incubator.PokemonId,
                        KmToWalk = incubator.TargetKmWalked - incubator.StartKmWalked,
                        KmRemaining = incubator.TargetKmWalked - kmWalked
                    });
                }
            }

            if (!newRememberedIncubators.SequenceEqual(rememberedIncubators))
                SaveRememberedIncubators(newRememberedIncubators, rememberedIncubatorsFilePath);
        }

        private static List<IncubatorUsage> GetRememberedIncubators(string filePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            if (File.Exists(filePath))
                return JsonConvert.DeserializeObject<List<IncubatorUsage>>(File.ReadAllText(filePath, Encoding.UTF8));

            return new List<IncubatorUsage>(0);
        }

        private static void SaveRememberedIncubators(List<IncubatorUsage> incubators, string filePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            File.WriteAllText(filePath, JsonConvert.SerializeObject(incubators), Encoding.UTF8);
        }

        private class IncubatorUsage : IEquatable<IncubatorUsage>
        {
            public string IncubatorId;
            public ulong PokemonId;

            public bool Equals(IncubatorUsage other)
            {
                return other != null && other.IncubatorId == IncubatorId && other.PokemonId == PokemonId;
            }
        }
    }
}