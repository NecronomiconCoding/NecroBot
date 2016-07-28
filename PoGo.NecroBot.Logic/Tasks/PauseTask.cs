#region using directives

using System;
using System.Linq;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PokemonGo.RocketAPI;
using POGOProtos.Inventory.Item;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class PauseTask
    {
        public static async Task Execute(ISession session)
        {
            ConsoleKeyInfo str;
            do
            {
                str = Console.ReadKey(true);
            } while (str == null);
            Console.WriteLine("Got key");
        }
    }
}
