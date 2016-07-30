#region using directives

using System;
using System.Globalization;
using System.Threading;
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
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var pokemons = await session.Inventory.GetPokemons();

            foreach (var pokemon in pokemons)
            {
<<<<<<< HEAD
                cancellationToken.ThrowIfCancellationRequested();
=======
                if (pokemon.Nickname.Length != 0) continue;
>>>>>>> 37bdcde70412e85354a0f23458f69c9617c5ac99

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

                if (newNickname != oldNickname &&
                    (
                        (session.LogicSettings.RenameAboveIv && perfection >= session.LogicSettings.KeepMinIvPercentage) ||
                        session.LogicSettings.RenameAllIv
                    )
                )
                {
                    await session.Client.Inventory.NicknamePokemon(pokemon.Id, newNickname);

                    session.EventDispatcher.Send(new NoticeEvent
                    {
                        Message = session.Translation.GetTranslation(Common.TranslationString.PokemonRename, pokemon.PokemonId, pokemon.Id, oldNickname, newNickname)
                    });
                }
            }
        }
    }
}