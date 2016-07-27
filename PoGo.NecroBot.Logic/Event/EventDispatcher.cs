using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Event
{
    public interface IEventDispatcher
    {
        void Send(IEvent evt);
    }

    public delegate void EventDelegate(IEvent evt);
    public class EventDispatcher : IEventDispatcher
    {
        public event EventDelegate EventReceived;

        public void Send(IEvent evt)
        {
            EventReceived?.Invoke(evt);
        }
    }
}
