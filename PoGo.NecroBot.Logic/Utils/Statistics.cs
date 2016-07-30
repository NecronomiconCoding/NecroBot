#region using directives

#region using directives

using System;
using System.Globalization;
using System.Linq;
using POGOProtos.Networking.Responses;
using System.IO;

#endregion

// ReSharper disable CyclomaticComplexity

#endregion

namespace PoGo.NecroBot.Logic.Utils
{
    public delegate void StatisticsDirtyDelegate();

    public class Statistics
    {
        private readonly DateTime _initSessionDateTime = DateTime.Now;

        private StatsExport _exportStats;
        private static string _playerName;
        private static DateTime _lastRefresh;
        public int TotalExperience;
        public int TotalItemsRemoved;
        public int TotalPokemons;
        public int TotalPokemonsTransfered;
        public int TotalStardust;

        public void Dirty(Inventory inventory)
        {
            _exportStats = GetCurrentInfo(inventory);
            DirtyEvent?.Invoke();
        }

        public event StatisticsDirtyDelegate DirtyEvent;

        private string FormatRuntime()
        {
            return (DateTime.Now - _initSessionDateTime).ToString(@"dd\.hh\:mm\:ss");
        }

        public StatsExport GetCurrentInfo(Inventory inventory)
        {
            var stats = inventory.GetPlayerStats().Result;
            StatsExport output = null;
            var stat = stats.FirstOrDefault();
            if (stat != null)
            {
                var ep = stat.NextLevelXp - stat.PrevLevelXp - (stat.Experience - stat.PrevLevelXp);
                var time = Math.Round(ep/(TotalExperience/GetRuntime()), 2);
                var hours = 0.00;
                var minutes = 0.00;
                if (double.IsInfinity(time) == false && time > 0)
                {
                    time = Convert.ToDouble(TimeSpan.FromHours(time).ToString("h\\.mm"), CultureInfo.InvariantCulture);
                    hours = Math.Truncate(time);
                    minutes = Math.Round((time - hours)*100);
                }

                output = new StatsExport
                {
                    Level = stat.Level,
                    HoursUntilLvl = hours,
                    MinutesUntilLevel = minutes,
                    CurrentXp = stat.Experience - stat.PrevLevelXp - GetXpDiff(stat.Level),
                    LevelupXp = stat.NextLevelXp - stat.PrevLevelXp - GetXpDiff(stat.Level),
                };
            }
            return output;
        }

        public string GetTemplatedStats(string template, string xpTemplate)
        {
            var xpStats = string.Format(xpTemplate, _exportStats.Level, _exportStats.HoursUntilLvl, _exportStats.MinutesUntilLevel, _exportStats.CurrentXp, _exportStats.LevelupXp);
            return string.Format(template, _playerName, FormatRuntime(), xpStats, TotalExperience / GetRuntime(), TotalPokemons / GetRuntime(), 
                TotalStardust, TotalPokemonsTransfered, TotalItemsRemoved);
        }

        public double GetRuntime()
        {
            return (DateTime.Now - _initSessionDateTime).TotalSeconds/3600;
        }

        public static int GetXpDiff(int level)
        {
            if (level > 0 && level <= 40)
            {
                int[] xpTable =
                {
                    0, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000,
                    10000, 10000, 10000, 10000, 15000, 20000, 20000, 20000, 25000, 25000,
                    50000, 75000, 100000, 125000, 150000, 190000, 200000, 250000, 300000, 350000,
                    500000, 500000, 750000, 1000000, 1250000, 1500000, 2000000, 2500000, 1000000, 1000000
                };
                return xpTable[level - 1];
            }
            return 0;
        }

        public void SetUsername(GetPlayerResponse profile)
        {
            _playerName = profile.PlayerData.Username ?? "";
        }

        public static async System.Threading.Tasks.Task LogInventory(PoGo.NecroBot.Logic.State.ISession session)
        {
            if (session.LogicSettings.LogInventory)
            {
                GetInventoryResponse inventory = await session.Inventory.GetCachedInventory();
                var now = DateTime.UtcNow;

                if (now.Ticks > _lastRefresh.AddSeconds(session.LogicSettings.LogInventoryDelaySeconds).Ticks)
                {
                    string tmpDir = Path.Combine(session.LogicSettings.ProfilePath, "temp");
                    Directory.CreateDirectory(tmpDir);
                    string strPath = Path.Combine(session.LogicSettings.ProfilePath, "temp", _playerName + ".inventory.json");

                    System.IO.File.WriteAllText(strPath, inventory.InventoryDelta.InventoryItems.ToString());
                    _lastRefresh = now;
                }
            }
        }
    }

    public class StatsExport
    {
        public long CurrentXp;
        public double HoursUntilLvl;
        public double MinutesUntilLevel;
        public int Level;
        public long LevelupXp;
    }
}