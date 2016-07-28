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
        public static async Task Execute(ISession session)
        {
            var pokemons = await session.Inventory.GetPokemons();

            foreach (var pokemon in pokemons)
            {
                double perfection = Math.Round(PokemonInfo.CalculatePokemonPerfection(pokemon));
                string pokemonName = pokemon.PokemonId.ToString();
                // iv number + templating part + pokemonName <= 12
                int nameLength = 12 - (perfection.ToString(CultureInfo.InvariantCulture).Length + session.LogicSettings.RenameTemplate.Length - 6);
                if (pokemonName.Length > nameLength)
                {
                    pokemonName = pokemonName.Substring(0, nameLength);
                }
                string newNickname = String.Format(session.LogicSettings.RenameTemplate, pokemonName, perfection);

                if (perfection >= session.LogicSettings.KeepMinIvPercentage && newNickname != pokemon.Nickname &&
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
