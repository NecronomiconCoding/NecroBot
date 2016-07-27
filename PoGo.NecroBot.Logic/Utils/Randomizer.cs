using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Utils
{
    public static class Randomizer
    {
        private static Random _random = new Random();
        private static Dictionary<string, NamedRandomEntry> _namedEntries = new Dictionary<string, NamedRandomEntry>();

        public static int GetNext(int min, int max)
        {
            return _random.Next(min, max + 1);
        }

        public static Task Sleep(int milliseconds, double variance = 0.1)
        {
            return Task.Delay(GetNext(Convert.ToInt32(milliseconds * (1 - variance)), Convert.ToInt32(milliseconds * (1 + variance))));
        }

        public static int GetNamed(string name, int min, int max, int changeAfterSeconds)
        {
            NamedRandomEntry entry;
            if (!_namedEntries.TryGetValue(name, out entry))
            {
                entry = new NamedRandomEntry()
                {
                    Name = name,
                    LastValue = GetNext(min, max),
                    LastGenerated = DateTime.Now
                };
                _namedEntries.Add(name, entry);
                //Console.WriteLine("named random created: " + name + "  = " + entry.LastValue);
                return entry.LastValue;
            }
            if (entry.LastGenerated.AddSeconds(changeAfterSeconds) < DateTime.Now)
            {
                entry.LastValue = GetNext(min, max);
                entry.LastGenerated = DateTime.Now;
                //Console.WriteLine("named random updated: " + name + "  = " + entry.LastValue);
            }
            //Console.WriteLine("named random read: " + name + "  = " + entry.LastValue);
            return entry.LastValue;
        }

        private class NamedRandomEntry
        {
            public string Name;
            public int LastValue;
            public DateTime LastGenerated;
        }

    }

}
