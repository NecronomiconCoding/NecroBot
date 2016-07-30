#region using directives

using System;
using System.Collections.Generic;
using POGOProtos.Data;

#endregion

namespace PoGo.NecroBot.Logic.Event
{
    public class SnipeModeEvent : IEvent
    {
        public bool Active;
    }
}