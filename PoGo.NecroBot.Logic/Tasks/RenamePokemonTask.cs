#region using directives

using System;
using System.Globalization;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class RenamePokemonTask
    {
        public static async Task Execute(Context ctx, StateMachine machine)
        {
            var pokemons = await ctx.Inventory.GetPokemons();

            foreach (var pokemon in pokemons)
            {
                var perfection = Math.Round(PokemonInfo.CalculatePokemonPerfection(pokemon));
                var pokemonName = pokemon.PokemonId.ToString();
                if (pokemonName.Length > 10 - perfection.ToString(CultureInfo.InvariantCulture).Length)
                {
                    pokemonName = pokemonName.Substring(0, 10 - perfection.ToString(CultureInfo.InvariantCulture).Length);
                }
                var newNickname = $"{pokemonName}_{perfection}";

                if (perfection > ctx.LogicSettings.KeepMinIvPercentage && newNickname != pokemon.Nickname &&
                    ctx.LogicSettings.RenameAboveIv)
                {
                    await ctx.Client.Inventory.NicknamePokemon(pokemon.Id, newNickname);

                    machine.Fire(new NoticeEvent
                    {
                        Message =
                            $"Pokemon {pokemon.PokemonId} ({pokemon.Id}) renamed from {pokemon.Nickname} to {newNickname}."
                    });
                }
                else if (newNickname == pokemon.Nickname && !ctx.LogicSettings.RenameAboveIv)
                {
                    await ctx.Client.Inventory.NicknamePokemon(pokemon.Id, pokemon.PokemonId.ToString());

                    machine.Fire(new NoticeEvent
                    {
                        Message =
                            $"Pokemon {pokemon.PokemonId} ({pokemon.Id}) renamed from {pokemon.Nickname} to {pokemon.PokemonId}."
                    });
                }
            }
        }
    }
}