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
        public static async Task Execute(Session session, StateMachine machine)
        {
            var pokemons = await session.Inventory.GetPokemons();

            foreach (var pokemon in pokemons)
            {
                var perfection = Math.Round(PokemonInfo.CalculatePokemonPerfection(pokemon));
                var pokemonName = pokemon.PokemonId.ToString();
                if (pokemonName.Length > 10 - perfection.ToString(CultureInfo.InvariantCulture).Length)
                {
                    pokemonName = pokemonName.Substring(0, 10 - perfection.ToString(CultureInfo.InvariantCulture).Length);
                }
                var newNickname = $"{pokemonName}_{perfection}";

                if (perfection > session.LogicSettings.KeepMinIvPercentage && newNickname != pokemon.Nickname &&
                    session.LogicSettings.RenameAboveIv)
                {
                    await session.Client.Inventory.NicknamePokemon(pokemon.Id, newNickname);

                    session.EventDispatcher.Send(new NoticeEvent
                    {
                        Message = session.Translations.GetTranslation(Common.TranslationString.PokemonRename, pokemon.PokemonId, pokemon.Id, pokemon.Nickname, newNickname)
                    });
                }
                else if (newNickname == pokemon.Nickname && !session.LogicSettings.RenameAboveIv)
                {
                    await session.Client.Inventory.NicknamePokemon(pokemon.Id, pokemon.PokemonId.ToString());

                    session.EventDispatcher.Send(new NoticeEvent
                    {
                        Message = session.Translations.GetTranslation(Common.TranslationString.PokemonRename, pokemon.PokemonId, pokemon.Id, pokemon.Nickname, pokemon.PokemonId)
                    });
                }
            }
        }
    }
}