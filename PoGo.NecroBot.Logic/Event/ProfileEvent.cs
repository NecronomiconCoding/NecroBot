using POGOProtos.Networking.Responses;

namespace PoGo.NecroBot.Logic.Event
{
    public class ProfileEvent : IEvent
    {
        public GetPlayerResponse Profile;
    }
}
