namespace PoGo.NecroBot.Logic.Filters {
    public class PokemonTransferFilter {
        public PokemonTransferFilter() {
        }

        public PokemonTransferFilter(int keepMinCp, float keepMinIvPercentage, int keepMinDuplicatePokemon) {
            KeepMinCp = keepMinCp;
            KeepMinIvPercentage = keepMinIvPercentage;
            KeepMinDuplicatePokemon = keepMinDuplicatePokemon;
        }

        public int KeepMinCp { get; set; }
        public float KeepMinIvPercentage { get; set; }
        public int KeepMinDuplicatePokemon { get; set; }
    }
}
