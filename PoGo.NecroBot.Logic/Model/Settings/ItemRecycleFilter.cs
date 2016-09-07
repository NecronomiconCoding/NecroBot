using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using POGOProtos.Inventory.Item;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Title = "Item Recycle Filter", Description = "", ItemRequired = Required.DisallowNull)]
    public class ItemRecycleFilter
    {
        public ItemRecycleFilter()
        {
        }

        public ItemRecycleFilter(ItemId key, int value)
        {
            Key = key;
            Value = value;
        }

        [DefaultValue("ItemUnknown")]
        [JsonProperty(Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate, Order = 1)]
        public ItemId Key = ItemId.ItemUnknown;

        [DefaultValue(0)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate, Order = 2)]
        public int Value;

        internal static List<ItemRecycleFilter> ItemRecycleFilterDefault()
        {
            return new List<ItemRecycleFilter>
            {
                new ItemRecycleFilter(ItemId.ItemUnknown, 0),
                new ItemRecycleFilter(ItemId.ItemLuckyEgg, 200),
                new ItemRecycleFilter(ItemId.ItemIncenseOrdinary, 100),
                new ItemRecycleFilter(ItemId.ItemIncenseSpicy, 100),
                new ItemRecycleFilter(ItemId.ItemIncenseCool, 100),
                new ItemRecycleFilter(ItemId.ItemIncenseFloral, 100),
                new ItemRecycleFilter(ItemId.ItemTroyDisk, 100),
                new ItemRecycleFilter(ItemId.ItemXAttack, 100),
                new ItemRecycleFilter(ItemId.ItemXDefense, 100),
                new ItemRecycleFilter(ItemId.ItemXMiracle, 100),
                new ItemRecycleFilter(ItemId.ItemSpecialCamera, 100),
                new ItemRecycleFilter(ItemId.ItemIncubatorBasicUnlimited, 100),
                new ItemRecycleFilter(ItemId.ItemIncubatorBasic, 100),
                new ItemRecycleFilter(ItemId.ItemPokemonStorageUpgrade, 100),
                new ItemRecycleFilter(ItemId.ItemItemStorageUpgrade, 100)
            };
        }

    }
}