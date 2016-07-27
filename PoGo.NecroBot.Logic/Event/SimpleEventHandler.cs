using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Event
{
    public delegate void EventDeletate(IEvent evt);
    public class SimpleEventHandler : IEventHandler
    {
        public event EventDeletate EventFired;
        public void OnEvent(IEvent evt)
        {
            EventFired?.Invoke(evt);
        }
    }
}
