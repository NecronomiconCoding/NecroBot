using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using PokemonGo.RocketAPI.Extensions;
using POGOProtos.Map.Fort;
using POGOProtos.Inventory.Item;
using PoGo.NecroBot.Logic.Logging;

namespace PoGo.NecroBot.Logic.Tasks
{
    public class UseAllIncubators
    {
        public static void Execute(Context ctx, StateMachine machine)
        {
            var playerStats = (ctx.Inventory.GetPlayerStats().Result).FirstOrDefault();
            if (playerStats == null)
                return;

            var kmWalked = playerStats.KmWalked;
            //get rewards of Egg something like this
            //var hatchedEgg =    ctx.Client.Inventory.GetHatchedEgg().Result;
            //int exp = Convert.ToInt32(hatchedEgg.ExperienceAwarded);

            var items = ctx.Inventory.GetIncubators().Result;
            var incubators = items
                .Where(x => x.UsesRemaining > 0 || x.ItemId == ItemId.ItemIncubatorBasicUnlimited)
                .OrderByDescending(x => x.ItemId == ItemId.ItemIncubatorBasicUnlimited)
                .ToList();

            var eggs = ctx.Inventory.GetEggs().Result;
            var unusedEggs = eggs
                .Where(x => string.IsNullOrEmpty(x.EggIncubatorId))
                .OrderBy(x => x.EggKmWalkedTarget - x.EggKmWalkedStart)
                .ToList();

            foreach (var incubator in incubators)
            {
                if (incubator.PokemonId == 0)
                {
                    var egg = incubator.ItemId == ItemId.ItemIncubatorBasicUnlimited
                        ? unusedEggs.FirstOrDefault()
                        : unusedEggs.LastOrDefault();

                    if (egg == null)
                        continue;

                    var response = ctx.Client.Inventory.UseItemEggIncubator(incubator.Id, egg.Id).Result;
                    Logger.Write($"Putting egg in incubator ({response.EggIncubator.TargetKmWalked - kmWalked:0.00}km left)");

                    unusedEggs.Remove(egg);
                }
                else
                {
                    Logger.Write($"Incubator status update: {incubator.TargetKmWalked - kmWalked:0.00}km left");
                }
            }
        }

    }
}