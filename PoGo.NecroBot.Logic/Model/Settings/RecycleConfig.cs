using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using POGOProtos.Inventory.Item;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Title = "Recycle Config", Description = "Set your recycle settings.", ItemRequired = Required.DisallowNull)]
    public class RecycleConfig
    {
        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 1)]
        public bool VerboseRecycling = true;

        [DefaultValue(90)]
        [Range(0, 100)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 2)]
        public double RecycleInventoryAtUsagePercentage = 90.0;

        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 3)]
        public bool RandomizeRecycle;

        [DefaultValue(5)]
        [Range(0, 100)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 4)]
        public int RandomRecycleValue = 5;

        /*Amounts*/
        [DefaultValue(120)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 6)]
        public int TotalAmountOfPokeballsToKeep = 120;

        [DefaultValue(80)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 7)]
        public int TotalAmountOfPotionsToKeep = 80;

        [DefaultValue(60)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 8)]
        public int TotalAmountOfRevivesToKeep = 60;

        [DefaultValue(50)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 9)]
        public int TotalAmountOfBerriesToKeep = 50;

        internal static List<KeyValuePair<ItemId, int>> ItemRecycleFilterDefault()
        {
            return new List<KeyValuePair<ItemId, int>>
            {
                new KeyValuePair<ItemId, int>(ItemId.ItemUnknown, 0),
                new KeyValuePair<ItemId, int>(ItemId.ItemLuckyEgg, 200),
                new KeyValuePair<ItemId, int>(ItemId.ItemIncenseOrdinary, 100),
                new KeyValuePair<ItemId, int>(ItemId.ItemIncenseSpicy, 100),
                new KeyValuePair<ItemId, int>(ItemId.ItemIncenseCool, 100),
                new KeyValuePair<ItemId, int>(ItemId.ItemIncenseFloral, 100),
                new KeyValuePair<ItemId, int>(ItemId.ItemTroyDisk, 100),
                new KeyValuePair<ItemId, int>(ItemId.ItemXAttack, 100),
                new KeyValuePair<ItemId, int>(ItemId.ItemXDefense, 100),
                new KeyValuePair<ItemId, int>(ItemId.ItemXMiracle, 100),
                new KeyValuePair<ItemId, int>(ItemId.ItemSpecialCamera, 100),
                new KeyValuePair<ItemId, int>(ItemId.ItemIncubatorBasicUnlimited, 100),
                new KeyValuePair<ItemId, int>(ItemId.ItemIncubatorBasic, 100),
                new KeyValuePair<ItemId, int>(ItemId.ItemPokemonStorageUpgrade, 100),
                new KeyValuePair<ItemId, int>(ItemId.ItemItemStorageUpgrade, 100)
            };
        }

    }
}