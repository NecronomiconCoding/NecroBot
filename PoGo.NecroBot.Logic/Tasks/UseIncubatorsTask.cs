using System.Linq;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Inventory.Item;

namespace PoGo.NecroBot.Logic.Tasks
{
    class UseIncubatorsTask
    {
        public static async Task Execute(Context ctx, StateMachine machine)
        {
            // Refresh inventory so that the player stats are fresh
            await ctx.Inventory.RefreshCachedInventory();

            var playerStats = (await ctx.Inventory.GetPlayerStats()).FirstOrDefault();
            if (playerStats == null)
                return;

            var kmWalked = playerStats.KmWalked;

            var incubators = (await ctx.Inventory.GetEggIncubators())
                .Where(x => x.UsesRemaining > 0 || x.ItemId == ItemId.ItemIncubatorBasicUnlimited)
                .OrderByDescending(x => x.ItemId == ItemId.ItemIncubatorBasicUnlimited)
                .ToList();

            var unusedEggs = (await ctx.Inventory.GetEggs())
                .Where(x => string.IsNullOrEmpty(x.EggIncubatorId))
                .OrderBy(x => x.EggKmWalkedTarget - x.EggKmWalkedStart)
                .ToList();

            foreach (var incubator in incubators)
            {
                if (incubator.PokemonId == 0)
                {
                    // Unlimited incubators prefer short eggs, limited incubators prefer long eggs
                    var egg = incubator.ItemId == ItemId.ItemIncubatorBasicUnlimited
                        ? unusedEggs.FirstOrDefault()
                        : unusedEggs.LastOrDefault();

                    if (egg == null)
                        continue;

                    var response = await ctx.Client.Inventory.UseItemEggIncubator(incubator.Id, egg.Id);
                    unusedEggs.Remove(egg);

                    machine.Fire(new EggIncubatorStatusEvent
                    {
                        IncubatorId = incubator.Id,
                        WasAddedNow = true,
                        PokemonId = egg.Id,
                        KmToWalk = egg.EggKmWalkedTarget,
                        KmRemaining = response.EggIncubator.TargetKmWalked - kmWalked
                    });

                    await Utils.Statistics.RandomDelay(500);
                }
                else
                {
                    machine.Fire(new EggIncubatorStatusEvent
                    {
                        IncubatorId = incubator.Id,
                        PokemonId = incubator.PokemonId,
                        KmToWalk = incubator.TargetKmWalked - incubator.StartKmWalked,
                        KmRemaining = incubator.TargetKmWalked - kmWalked
                    });
                }
            }
        }
    }
}