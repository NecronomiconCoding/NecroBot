﻿namespace PoGo.NecroBot.Logic.Event
{
    public class WarnEvent : IEvent
    {
        public string Message = "";

        public override string ToString() => Message;
    }
}