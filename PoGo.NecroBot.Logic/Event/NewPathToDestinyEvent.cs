using PoGo.NecroBot.Logic.Model.Google;

namespace PoGo.NecroBot.Logic.Event
{
    public class NewPathToDestinyEvent : IEvent
    {
        public GoogleResult GoogleData;
    }
}