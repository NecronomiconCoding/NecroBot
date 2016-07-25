#region using directives

using System;
using System.Globalization;
using System.Linq;
using POGOProtos.Networking.Responses;

// ReSharper disable CyclomaticComplexity

#endregion

namespace PoGo.NecroBot.Logic.Utils
{
    public delegate void StatisticsDirtyDelegate();
    public class Statistics
    {
        public int TotalExperience;
        public int TotalPokemons;
        public int TotalItemsRemoved;
        public int TotalPokemonsTransfered;
        public int TotalStardust;
  
        public event StatisticsDirtyDelegate DirtyEvent;

        private string CurrentLevelInfos;
        private string PlayerName;
        private DateTime InitSessionDateTime = DateTime.Now;

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

                output = $"{stat.Level} (next level in {hours}h {minutes}m | {stat.Experience - stat.PrevLevelXp - GetXpDiff(stat.Level)}/{stat.NextLevelXp - stat.PrevLevelXp - GetXpDiff(stat.Level)} XP)";
            }
            return output;
        }

        public double GetRuntime()
        {
            return (DateTime.Now - InitSessionDateTime).TotalSeconds/3600;
        }

        private string FormatRuntime()
        {
            return (DateTime.Now - InitSessionDateTime).ToString(@"dd\.hh\:mm\:ss");
        }

        public void Dirty(Inventory inventory)
        {
            CurrentLevelInfos = GetCurrentInfo(inventory);
            DirtyEvent?.Invoke();
        }

        public static int GetXpDiff(int level)
        {
            switch (level)
            {
                case 1:
                    return 0;
                case 2:
                    return 1000;
                case 3:
                    return 2000;
                case 4:
                    return 3000;
                case 5:
                    return 4000;
                case 6:
                    return 5000;
                case 7:
                    return 6000;
                case 8:
                    return 7000;
                case 9:
                    return 8000;
                case 10:
                    return 9000;
                case 11:
                    return 10000;
                case 12:
                    return 10000;
                case 13:
                    return 10000;
                case 14:
                    return 10000;
                case 15:
                    return 15000;
                case 16:
                    return 20000;
                case 17:
                    return 20000;
                case 18:
                    return 20000;
                case 19:
                    return 25000;
                case 20:
                    return 25000;
                case 21:
                    return 50000;
                case 22:
                    return 75000;
                case 23:
                    return 100000;
                case 24:
                    return 125000;
                case 25:
                    return 150000;
                case 26:
                    return 190000;
                case 27:
                    return 200000;
                case 28:
                    return 250000;
                case 29:
                    return 300000;
                case 30:
                    return 350000;
                case 31:
                    return 500000;
                case 32:
                    return 500000;
                case 33:
                    return 750000;
                case 34:
                    return 1000000;
                case 35:
                    return 1250000;
                case 36:
                    return 1500000;
                case 37:
                    return 2000000;
                case 38:
                    return 2500000;
                case 39:
                    return 1000000;
                case 40:
                    return 1000000;
            }
            return 0;
        }

        public void SetUsername(GetPlayerResponse profile)
        {
            PlayerName = profile.PlayerData.Username ?? "";
        }

        public override string ToString()
        {
            return $"{PlayerName} - Runtime {FormatRuntime()} - Lvl: {CurrentLevelInfos} | EXP/H: {TotalExperience/GetRuntime():0} | P/H: {TotalPokemons/GetRuntime():0} | Stardust: {TotalStardust:0} | Transfered: {TotalPokemonsTransfered:0} | Items Recycled: {TotalItemsRemoved:0}";
        }
    }
}
