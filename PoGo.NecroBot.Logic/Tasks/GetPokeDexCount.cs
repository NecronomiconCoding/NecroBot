using PoGo.NecroBot.Logic.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.Common;

namespace PoGo.NecroBot.Logic.Tasks
{
    class GetPokeDexCount
    {
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            int timesCaptured = 0;
            var PokeDex = await session.Inventory.GetPokeDexItems();
            for (int i = 0; i < PokeDex.Count; i++)
            {
                var CaughtPokemon = PokeDex[i].ToString().Split(new[] { "timesCaptured" }, StringSplitOptions.None);
                var split = CaughtPokemon[1].Split(' ');
                int Times = int.Parse(split[1]);
                if (Times > 0)
                    timesCaptured++;
            }
            Logger.Write(session.Translation.GetTranslation(TranslationString.AmountPkmSeenCaught, PokeDex.Count, timesCaptured));
        }
    }
}