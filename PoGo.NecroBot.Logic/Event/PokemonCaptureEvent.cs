#region using directives

using POGOProtos.Enums;
using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Responses;

#endregion

namespace PoGo.NecroBot.Logic.Event
{
    public class PokemonCaptureEvent : IEvent
    {
        public int Attempt;
        public int BallAmount;
        public string CatchType;
        public int Cp;
        public double Distance;
        public int Exp;
        public int FamilyCandies;
        public PokemonId Id;
        public ulong UniqueId;
        public double Level;
        public int MaxCp;
        public double Perfection;
        public ItemId Pokeball;
        public double Probability;
        public int Stardust;
        public CatchPokemonResponse.Types.CatchStatus Status;
        public double Latitude;
        public double Longitude;
        public string SpawnPointId;
        public ulong EncounterId;
        public PokemonMove Move1;
        public PokemonMove Move2;
        public long Expires;
        public string CatchTypeText;
        public string Rarity;
    }
}
