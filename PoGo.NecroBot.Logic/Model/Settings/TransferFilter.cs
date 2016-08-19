using System.Collections.Generic;
using POGOProtos.Enums;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    public class TransferFilter
    {
        public TransferFilter()
        {
        }

        public TransferFilter(int keepMinCp, int keepMinLvl, bool useKeepMinLvl, float keepMinIvPercentage, string keepMinOperator, int keepMinDuplicatePokemon,
            List<List<PokemonMove>> moves = null, List<PokemonMove> deprecatedMoves = null, string movesOperator = "or")
        {
            KeepMinCp = keepMinCp;
            KeepMinLvl = keepMinLvl;
            UseKeepMinLvl = useKeepMinLvl;
            KeepMinIvPercentage = keepMinIvPercentage;
            KeepMinDuplicatePokemon = keepMinDuplicatePokemon;
            KeepMinOperator = keepMinOperator;
            Moves = (moves == null && deprecatedMoves != null)
                ? new List<List<PokemonMove>> { deprecatedMoves }
                : moves ?? new List<List<PokemonMove>>();
            MovesOperator = movesOperator;
        }

        public int KeepMinCp { get; set; }
        public int KeepMinLvl { get; set; }
        public bool UseKeepMinLvl { get; set; }
        public float KeepMinIvPercentage { get; set; }
        public int KeepMinDuplicatePokemon { get; set; }
        public List<List<PokemonMove>> Moves { get; set; }
        public List<PokemonMove> DeprecatedMoves { get; set; }
        public string KeepMinOperator { get; set; }
        public string MovesOperator { get; set; }
    }
}