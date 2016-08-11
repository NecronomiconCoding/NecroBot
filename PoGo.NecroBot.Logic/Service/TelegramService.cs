using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Data;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace PoGo.NecroBot.Logic.Service
{
    public class TelegramService
    {
        private TelegramBotClient bot;
        private ISession session;
        private bool loggedIn;
        private DateTime _lastLoginTime;
        public TelegramService(string apiKey, ISession session)
        {
            this.bot = new TelegramBotClient(apiKey);
            this.session = session;
            var me = new Telegram.Bot.Types.User();

            me = bot.GetMeAsync().Result;

            bot.OnMessage += OnTelegramMessageReceived;
            bot.StartReceiving();

            this.session.EventDispatcher.Send(new NoticeEvent { Message = "Using TelegramAPI with " + me.Username });
        }

        private async void OnTelegramMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            if (message == null || message.Type != MessageType.TextMessage)
                return;

            var answerTextmessage = "";

            if (session.Profile == null || session.Inventory == null)
            {
                return;
            }

            var messagetext = message.Text.ToLower().Split(' ');

            if (!loggedIn)
            {
                if (messagetext[0].ToLower().Contains("/login"))
                {
                    if (messagetext[0].ToLower().Contains(session.LogicSettings.TelegramPassword))
                    {
                        loggedIn = true;
                        _lastLoginTime = DateTime.Now;
                        answerTextmessage += session.Translation.GetTranslation(TranslationString.LoggedInTelegram);
                        SendMessage(message.Chat.Id, answerTextmessage);
                    }
                    else
                    {
                        answerTextmessage += session.Translation.GetTranslation(TranslationString.LoginFailedTelegram);
                        SendMessage(message.Chat.Id, answerTextmessage);
                    }
                    return;
                }
                answerTextmessage += session.Translation.GetTranslation(TranslationString.NotLoggedInTelegram);
                SendMessage(message.Chat.Id, answerTextmessage);
                return;
            }
            if (loggedIn && _lastLoginTime.AddMinutes(5).Ticks > DateTime.Now.Ticks)
            {
                loggedIn = false;
                answerTextmessage += session.Translation.GetTranslation(TranslationString.NotLoggedInTelegram);
                SendMessage(message.Chat.Id, answerTextmessage);
                return;
            }

            switch (messagetext[0].ToLower())
            {
                case "/top":
                    var times = 10;
                    var sortby = "cp";

                    if (messagetext.Length >= 2)
                    {
                        sortby = messagetext[1];
                    }
                    if (messagetext.Length == 3)
                    {
                        try
                        {
                            times = Convert.ToInt32(messagetext[2]);
                        }
                        catch (FormatException)
                        {
                            SendMessage(message.Chat.Id, session.Translation.GetTranslation(TranslationString.UsageHelp, "/top [cp/iv] [number of pokemon to show]"));
                            break;
                        }
                    }
                    else if (messagetext.Length > 3)
                    {
                        SendMessage(message.Chat.Id, session.Translation.GetTranslation(TranslationString.UsageHelp, "/top [cp/iv] [number of pokemon to show]"));
                        break;
                    }

                    IEnumerable<PokemonData> topPokemons;
                    if (sortby.Equals("iv"))
                    {
                        topPokemons = await session.Inventory.GetHighestsPerfect(times);
                    }
                    else if (sortby.Equals("cp"))
                    {
                        topPokemons = await session.Inventory.GetHighestsCp(times);
                    }
                    else
                    {
                        SendMessage(message.Chat.Id, session.Translation.GetTranslation(TranslationString.UsageHelp, "/top [cp/iv] [number of pokemon to show]"));
                        break;
                    }

                    foreach (var pokemon in topPokemons)
                    {
                        answerTextmessage += session.Translation.GetTranslation(TranslationString.ShowPokeTemplate, new object[] { pokemon.Cp, PokemonInfo.CalculatePokemonPerfection(pokemon).ToString("0.00"), session.Translation.GetPokemonTranslation(pokemon.PokemonId) });

                        if (answerTextmessage.Length > 3800)
                        {
                            SendMessage(message.Chat.Id, answerTextmessage);
                            answerTextmessage = "";
                        }
                    }
                    SendMessage(message.Chat.Id, answerTextmessage);
                    break;

                case "/all":
                    var myPokemons = await session.Inventory.GetPokemons();
                    var allMyPokemons = myPokemons.ToList();
                    IEnumerable<PokemonData> allPokemons = null;

                    if (messagetext.Length == 1)
                    {
                        allPokemons = await session.Inventory.GetHighestsCp(allMyPokemons.Count);
                    }
                    else if (messagetext.Length == 2)
                    {
                        if (messagetext[1] == "iv")
                        {
                            allPokemons = await session.Inventory.GetHighestsPerfect(allMyPokemons.Count);
                        }
                        else if (messagetext[1] != "cp")
                        {
                            SendMessage(message.Chat.Id, session.Translation.GetTranslation(TranslationString.UsageHelp, "/all [cp/iv]"));
                            break;
                        }
                    }
                    else
                    {
                        SendMessage(message.Chat.Id, session.Translation.GetTranslation(TranslationString.UsageHelp, "/all [cp/iv]"));
                        break;
                    }

                    foreach (var pokemon in allPokemons)
                    {
                        answerTextmessage += session.Translation.GetTranslation(TranslationString.ShowPokeTemplate, new object[] { pokemon.Cp, PokemonInfo.CalculatePokemonPerfection(pokemon).ToString("0.00"), session.Translation.GetPokemonTranslation(pokemon.PokemonId) });

                        if (answerTextmessage.Length > 3800)
                        {
                            SendMessage(message.Chat.Id, answerTextmessage);
                            answerTextmessage = "";
                        }
                    }
                    SendMessage(message.Chat.Id, answerTextmessage);
                    break;

                case "/profile":
                    var stats = session.Inventory.GetPlayerStats().Result;
                    var stat = stats.FirstOrDefault();

                    var myPokemons2 = await session.Inventory.GetPokemons();
                    answerTextmessage += session.Translation.GetTranslation(TranslationString.ProfileStatsTemplateString,
                        new object[]
                         {
                             stat.Level,
                             session.Profile.PlayerData.Username,
                             stat.Experience, stat.NextLevelXp,
                             stat.PokemonsCaptured,
                             stat.PokemonDeployed,
                             stat.PokeStopVisits,
                             stat.EggsHatched,
                             stat.Evolutions,
                             stat.UniquePokedexEntries,
                             stat.KmWalked,
                             myPokemons2.ToList().Count,
                             session.Profile.PlayerData.MaxPokemonStorage
                         });
                    SendMessage(message.Chat.Id, answerTextmessage);
                    break;

                case "/pokedex":
                    var pokedex = session.Inventory.GetPokeDexItems().Result;
                    var pokedexSort = pokedex.OrderBy(x => x.InventoryItemData.PokedexEntry.PokemonId);

                    answerTextmessage += session.Translation.GetTranslation(TranslationString.PokedexCatchedTelegram);
                    foreach (var pokedexItem in pokedexSort)
                    {
                        answerTextmessage += session.Translation.GetTranslation(TranslationString.PokedexPokemonCatchedTelegram, Convert.ToInt32(pokedexItem.InventoryItemData.PokedexEntry.PokemonId), session.Translation.GetPokemonTranslation(pokedexItem.InventoryItemData.PokedexEntry.PokemonId), pokedexItem.InventoryItemData.PokedexEntry.TimesCaptured, pokedexItem.InventoryItemData.PokedexEntry.TimesEncountered);

                        if (answerTextmessage.Length > 3800)
                        {
                            SendMessage(message.Chat.Id, answerTextmessage);
                            answerTextmessage = "";
                        }
                    }

                    var pokemonsToCapture = Enum.GetValues(typeof(PokemonId)).Cast<PokemonId>().Except(pokedex.Select(x => x.InventoryItemData.PokedexEntry.PokemonId));

                    SendMessage(message.Chat.Id, answerTextmessage);
                    answerTextmessage = "";

                    answerTextmessage += session.Translation.GetTranslation(TranslationString.PokedexNeededTelegram);

                    foreach (var pokedexItem in pokemonsToCapture)
                    {
                        if (Convert.ToInt32(pokedexItem) > 0)
                        {
                            answerTextmessage += session.Translation.GetTranslation(TranslationString.PokedexPokemonNeededTelegram, Convert.ToInt32(pokedexItem), session.Translation.GetPokemonTranslation(pokedexItem));

                            if (answerTextmessage.Length > 3800)
                            {
                                SendMessage(message.Chat.Id, answerTextmessage);
                                answerTextmessage = "";
                            }
                        }
                    }
                    SendMessage(message.Chat.Id, answerTextmessage);

                    break;

                case "/loc":
                    SendLocation(message.Chat.Id, session.Client.CurrentLatitude, session.Client.CurrentLongitude);
                    break;

                case "/items":
                    var inventory = session.Inventory;
                    answerTextmessage += session.Translation.GetTranslation(TranslationString.CurrentPokeballInv,
                        new object[]
                        {
                            await inventory.GetItemAmountByType(ItemId.ItemPokeBall),
                            await inventory.GetItemAmountByType(ItemId.ItemGreatBall),
                            await inventory.GetItemAmountByType(ItemId.ItemUltraBall),
                            await inventory.GetItemAmountByType(ItemId.ItemMasterBall)
                        });
                    answerTextmessage += "\n";
                    answerTextmessage += session.Translation.GetTranslation(TranslationString.CurrentPotionInv,
                       new object[]
                       {
                            await inventory.GetItemAmountByType(ItemId.ItemPotion),
                            await inventory.GetItemAmountByType(ItemId.ItemSuperPotion),
                            await inventory.GetItemAmountByType(ItemId.ItemHyperPotion),
                            await inventory.GetItemAmountByType(ItemId.ItemMaxPotion)
                       });
                    answerTextmessage += "\n";
                    answerTextmessage += session.Translation.GetTranslation(TranslationString.CurrentReviveInv,
                        new object[]
                        {
                            await inventory.GetItemAmountByType(ItemId.ItemRevive),
                            await inventory.GetItemAmountByType(ItemId.ItemMaxRevive),
                        });
                    answerTextmessage += "\n";
                    answerTextmessage += session.Translation.GetTranslation(TranslationString.CurrentMiscItemInv,
                        new object[]
                        {
                            await session.Inventory.GetItemAmountByType(ItemId.ItemRazzBerry) +
                            await session.Inventory.GetItemAmountByType(ItemId.ItemBlukBerry) +
                            await session.Inventory.GetItemAmountByType(ItemId.ItemNanabBerry) +
                            await session.Inventory.GetItemAmountByType(ItemId.ItemWeparBerry) +
                            await session.Inventory.GetItemAmountByType(ItemId.ItemPinapBerry),
                            await session.Inventory.GetItemAmountByType(ItemId.ItemIncenseOrdinary) +
                            await session.Inventory.GetItemAmountByType(ItemId.ItemIncenseSpicy) +
                            await session.Inventory.GetItemAmountByType(ItemId.ItemIncenseCool) +
                            await session.Inventory.GetItemAmountByType(ItemId.ItemIncenseFloral),
                            await session.Inventory.GetItemAmountByType(ItemId.ItemLuckyEgg),
                            await session.Inventory.GetItemAmountByType(ItemId.ItemTroyDisk)
                        });
                    SendMessage(message.Chat.Id, answerTextmessage);
                    break;

                case "/status":
                    SendMessage(message.Chat.Id, Console.Title);
                    break;

                case "/restart":
                    Process.Start(Assembly.GetEntryAssembly().Location);
                    SendMessage(message.Chat.Id, "Restarted Bot. Closing old Instance... BYE!");
                    Environment.Exit(-1);
                    break;

                default:
                    answerTextmessage += session.Translation.GetTranslation(TranslationString.HelpTemplate);
                    SendMessage(message.Chat.Id, answerTextmessage);
                    break;
            }
        }

        private async void SendLocation(long chatID, double currentLatitude, double currentLongitude)
        {
            await bot.SendLocationAsync(chatID, (float)currentLatitude, (float)currentLongitude);
        }

        private async void SendMessage(long chatID, string message)
        {
            await bot.SendTextMessageAsync(chatID, message, replyMarkup: new ReplyKeyboardHide());
        }
    }
}