#region using directives

#region using directives

using System;
using System.Globalization;
using System.Linq;
using POGOProtos.Networking.Responses;

#endregion

// ReSharper disable CyclomaticComplexity

#endregion

namespace PoGo.NecroBot.Logic.Utils
{
    public delegate void StatisticsDirtyDelegate();

    public class Statistics
    {
        private readonly DateTime _initSessionDateTime = DateTime.Now;

        private string _currentLevelInfos;
        private string _playerName;
        public int TotalExperience;
        public int TotalItemsRemoved;
        public int TotalPokemons;
        public int TotalPokemonsTransfered;
        public int TotalStardust;

        public void Dirty(Inventory inventory)
        {
            _currentLevelInfos = GetCurrentInfo(inventory);
            DirtyEvent?.Invoke();
        }

        public event StatisticsDirtyDelegate DirtyEvent;

        private string FormatRuntime()
        {
            return (DateTime.Now - _initSessionDateTime).ToString(@"dd\.hh\:mm\:ss");
        }

        public string GetCurrentInfo(Inventory inventory)
        {
            var stats = inventory.GetPlayerStats().Result;
            var output = string.Empty;
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

                output =
                    $"{stat.Level} (next level in {hours}h {minutes}m | {stat.Experience - stat.PrevLevelXp - GetXpDiff(stat.Level)}/{stat.NextLevelXp - stat.PrevLevelXp - GetXpDiff(stat.Level)} XP)";
            }
            return output;
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

        public override string ToString()
        {
            return
                $"{_playerName} - Runtime {FormatRuntime()} - Lvl: {_currentLevelInfos} | EXP/H: {TotalExperience/GetRuntime():0} | P/H: {TotalPokemons/GetRuntime():0} | Stardust: {TotalStardust:0} | Transfered: {TotalPokemonsTransfered:0} | Items Recycled: {TotalItemsRemoved:0}";
        }
    }
}