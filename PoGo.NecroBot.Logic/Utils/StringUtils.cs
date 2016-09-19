#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using POGOProtos.Inventory.Item;

#endregion

namespace PoGo.NecroBot.Logic.Utils
{
    public static class StringUtils
    {
        public static string GetSummedFriendlyNameOfItemAwardList(IEnumerable<ItemAward> items)
        {
            var enumerable = items as IList<ItemAward> ?? items.ToList();

            if (!enumerable.Any())
                return string.Empty;

            return
                enumerable.GroupBy(i => i.ItemId)
                    .Select(kvp => new {ItemName = kvp.Key.ToString(), Amount = kvp.Sum(x => x.ItemCount)})
                    .Select(y => $"{y.Amount} x {y.ItemName}")
                    .Aggregate((a, b) => $"{a}, {b}");
        }


        private static readonly Func<bool, bool, bool> AndFunc = (x, y) => x && y;
        private static readonly Func<bool, bool, bool> OrFunc = (x, y) => x || y;
        private static readonly Func<string, Func<bool, bool, bool>> GetBoolOperator =
            myOperator => myOperator.ToLower().Equals("and") ? AndFunc : OrFunc;

        public static bool BoolFunc(this bool expr, bool expr2, string operatorStr)
        {
            return GetBoolOperator(operatorStr)(expr, expr2);
        }

        public static bool BoolFunc(this string operatorStr, params bool[] expr)
        {
            return operatorStr.ToLower().Equals("and") ? expr.All(b => b) : expr.Any(b => b);
        }

        public static bool ReverseBoolFunc(this string operatorStr, params bool[] expr)
        {
            return operatorStr.ToLower().Equals("and") ? expr.Any(b => b) : expr.All(b => b);
        }

        public static bool InverseBool(this string operatorStr, bool expr)
        {
            return operatorStr.ToLower().Equals("and") ? !expr : expr;
        }

    }
}