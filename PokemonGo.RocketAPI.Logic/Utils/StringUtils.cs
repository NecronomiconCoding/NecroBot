#region

using System.Collections.Generic;
using System.Linq;
using PokemonGo.RocketAPI.GeneratedCode;

#endregion

namespace PokemonGo.RocketAPI.Logic.Utils
{
    public static class StringUtils
    {
        public static string GetSummedFriendlyNameOfItemAwardList(IEnumerable<FortSearchResponse.Types.ItemAward> items)
        {
            var enumerable = items as IList<FortSearchResponse.Types.ItemAward> ?? items.ToList();

            if (!enumerable.Any())
                return string.Empty;
        
            return
                enumerable.GroupBy(i => i.ItemId)
                    .Select(kvp => new {ItemName = kvp.Key.ToString(), Amount = kvp.Sum(x => x.ItemCount)})
                    .Select(y => $"{(y.ItemName.Length > 5 ? y.ItemName.Substring(4, y.ItemName.Length - 4) : y.ItemName)} (x{y.Amount})")
                    .Aggregate((a, b) => $"{a}, {b}");
        }
    }
}