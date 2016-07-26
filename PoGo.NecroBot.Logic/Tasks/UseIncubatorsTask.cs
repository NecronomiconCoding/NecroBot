using System.Linq;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Logging;
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
                    Logger.Write($"Putting egg in incubator ({response.EggIncubator.TargetKmWalked - kmWalked:0.00}km left)");

                    unusedEggs.Remove(egg);
                    await Task.Delay(500);
                }
                else
                {
                    Logger.Write($"Incubator status update: {incubator.TargetKmWalked - kmWalked:0.00}km left");
                }
            }
        }
    }
}