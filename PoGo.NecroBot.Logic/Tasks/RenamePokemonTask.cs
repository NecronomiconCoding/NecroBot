#region using directives

using System;
using System.Globalization;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;

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
                string oldNickname = (pokemon.Nickname.Length != 0) ? pokemon.Nickname : pokemon.PokemonId.ToString();

                if (perfection >= session.LogicSettings.KeepMinIvPercentage && newNickname != oldNickname &&
                    session.LogicSettings.RenameAboveIv)
                {
                    await session.Client.Inventory.NicknamePokemon(pokemon.Id, newNickname);
                    await Statistics.LogInventory(session);
                    session.EventDispatcher.Send(new NoticeEvent
                    {
                        Message = session.Translation.GetTranslation(Common.TranslationString.PokemonRename, pokemon.PokemonId, pokemon.Id, oldNickname, newNickname)
                    });
                }
            }
        }
    }
}