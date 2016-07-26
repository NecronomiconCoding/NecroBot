using PokemonGo.RocketAPI.GeneratedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Callbacks
{
    public interface IPokeCallBack
    {
        void OnNoPokeBalls(MapPokemon pkmnId, PokemonData data);
        void OnCaught(int counter, MapPokemon pkmnId, PokemonData data, string attempts);
        void OnCatchFailed(int counter, MapPokemon pkmnId, PokemonData data, string attempts);
        void OnEvolved(PokemonData data, EvolvePokemonOut evolvedPokemon);
        void OnEvolvedFailed(PokemonData data, EvolvePokemonOut evolvedPokemon);
        void OnEggFound();
        void OnRecycled(Item item);
        void OnTransfer(PokemonData pokemonData);
        void OnBerryUsed(Item item);
    }
}
