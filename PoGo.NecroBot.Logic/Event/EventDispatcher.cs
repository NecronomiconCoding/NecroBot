using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Event
{
    public delegate void EventDelegate(IEvent evt);

    public interface IEventDispatcher
    {
        event EventDelegate EventReceived;
        void Send(IEvent evt);
    }

    public class EventDispatcher : IEventDispatcher
    {
        public event EventDelegate EventReceived;

        public void Send(IEvent evt)
        {
            EventReceived?.Invoke(evt);
        }
    }
}
