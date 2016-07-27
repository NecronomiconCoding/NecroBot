using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;

namespace PoGo.NecroBot.Logic.Tasks
{
    public class RenamePokemonTask
    {
        public static void Execute(Context ctx, StateMachine machine)
        {
            var pokemons = ctx.Inventory.GetPokemons().Result;

            foreach (var pokemon in pokemons)
            {
                var perfection = Math.Round(PokemonInfo.CalculatePokemonPerfection(pokemon));
                var pokemonName = pokemon.PokemonId.ToString();
                if (pokemonName.Length > 10 - perfection.ToString().Length)
                {
                    pokemonName = pokemonName.Substring(0, 10 - perfection.ToString().Length);
                }
                var newNickname = $"{pokemonName}_{perfection}";

                if (perfection > ctx.LogicSettings.KeepMinIvPercentage && newNickname != pokemon.Nickname && ctx.LogicSettings.RenameAboveIv)
                {
                    var result = ctx.Client.Inventory.NicknamePokemon(pokemon.Id, newNickname).Result;

                    machine.Fire(new NoticeEvent
                    {
                        Message = $"Pokemon {pokemon.PokemonId} ({pokemon.Id}) renamed from {pokemon.Nickname} to {newNickname}."
                    });

                    DelayingUtils.Delay(ctx.LogicSettings.DelayBetweenPlayerActions, 0);
                }
                else if (newNickname == pokemon.Nickname && !ctx.LogicSettings.RenameAboveIv)
                {
                    var result = ctx.Client.Inventory.NicknamePokemon(pokemon.Id, pokemon.PokemonId.ToString());

                    machine.Fire(new NoticeEvent
                    {
                        Message = $"Pokemon {pokemon.PokemonId} ({pokemon.Id}) renamed from {pokemon.Nickname} to {pokemon.PokemonId}."
                    });

                    DelayingUtils.Delay(ctx.LogicSettings.DelayBetweenPlayerActions, 0);
                }
            }

        }
    }
}
