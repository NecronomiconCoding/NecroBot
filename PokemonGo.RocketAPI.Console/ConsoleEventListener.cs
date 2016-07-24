using PokemonGo.RocketAPI.GeneratedCode;
using PokemonGo.RocketAPI.Logic.Event;
using PokemonGo.RocketAPI.Logic.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Console
{
    public class ConsoleEventListener
    {
        public void Listen(IEvent evt)
        {
            dynamic eve = evt;

            HandleEvent(eve);
        }

        public void HandleEvent(ErrorEvent evt)
        {
            Logger.Write(evt.ToString(), LogLevel.Error);
        }

        public void HandleEvent(NoticeEvent evt)
        {
            Logger.Write(evt.ToString());
        }

        public void HandleEvent(WarnEvent evt)
        {
            Logger.Write(evt.ToString(), LogLevel.Warning);
        }

        public void HandleEvent(UseLuckyEggEvent evt)
        {
            Logger.Write($"Used Lucky Egg, remaining: {evt.Count}", LogLevel.Egg);
        }

        public void HandleEvent(PokemonEvolveEvent evt)
        {
            Logger.Write(evt.Result == EvolvePokemonOut.Types.EvolvePokemonStatus.PokemonEvolvedSuccess
                        ? $"{evt.Id} successfully for {evt.Exp}xp"
                        : $"Failed {evt.Id}. EvolvePokemonOutProto.Result was {evt.Result}, stopping evolving {evt.Id}",
                    LogLevel.Evolve);
        }

        public void HandleEvent(TransferPokemonEvent evt)
        {
            Logger.Write($"{evt.Id} with {evt.Cp} ({evt.Perfection.ToString("0.00")} % perfect) CP (Best: {evt.BestCp} | ({evt.BestPerfection.ToString("0.00")} % perfect))", LogLevel.Transfer);
        }

        public void HandleEvent(ItemRecycledEvent evt)
        {
            Logger.Write($"{evt.Count}x {(ItemId)evt.Id}", LogLevel.Recycling);
        }
    }
}
