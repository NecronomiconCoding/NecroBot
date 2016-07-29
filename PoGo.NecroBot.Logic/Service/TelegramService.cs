using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public TelegramService(string apiKey, ISession session)
        {
            this.bot = new TelegramBotClient(apiKey);
            this.session = session;

            var me = bot.GetMeAsync().Result;

            bot.OnMessage += OnTelegramMessageReceived;
            bot.StartReceiving();

            Logger.Write("Using TelegramAPI with " + me.Username);
        }

        private async void OnTelegramMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.TextMessage)
                return;

            var answerTextmessage = "";

            switch (message.Text.ToString().ToLower())
            {
                case "/top":
                    var topPokemons = await session.Inventory.GetHighestsCp(10);

                    foreach (var pokemon in topPokemons)
                    {
                        answerTextmessage += "\nCP: " + pokemon.Cp + " | NAME: " + pokemon.PokemonId;
                    }
                    SendMessage(message.Chat.Id, answerTextmessage);
                    break;
                case "/all":
                    var myPokemons = await session.Inventory.GetPokemons();
                    var allMyPokemons = myPokemons.ToList();

                    var allPokemons = allMyPokemons.OrderByDescending(x => x.Cp).ThenBy(n => n.StaminaMax);

                    foreach (var pokemon in allPokemons)
                    {
                        answerTextmessage += "\nCP: " + pokemon.Cp + " | NAME: " + pokemon.PokemonId;
                    }
                    SendMessage(message.Chat.Id, answerTextmessage);
                    break;
                case "/profile":
                    var stats = session.Inventory.GetPlayerStats().Result;
                    var stat = stats.FirstOrDefault();

                    answerTextmessage += "----- LVL" + stat.Level + " | " + session.Profile.PlayerData.Username + "-----";
                    answerTextmessage += "\nExperience: " + stat.Experience + "/" + stat.NextLevelXp;
                    answerTextmessage += "\nPokemons Captured: " + stat.PokemonsCaptured;
                    answerTextmessage += "\nPokemon Deployed: " + stat.PokemonDeployed;
                    answerTextmessage += "\nPokeStop Visits: " + stat.PokeStopVisits;
                    answerTextmessage += "\nEggs Hatched: " + stat.EggsHatched;
                    answerTextmessage += "\nEvolutions: " + stat.Evolutions;
                    answerTextmessage += "\nUnique Pokedex Entries: " + stat.UniquePokedexEntries;
                    answerTextmessage += "\nKM Walked: " + stat.KmWalked;
                    SendMessage(message.Chat.Id, answerTextmessage);
                    break;
                case "/loc":
                    SendLocation(message.Chat.Id, session.Client.CurrentLatitude, session.Client.CurrentLongitude);
                    break;
                default:
                    answerTextmessage += "Commands:\n";
                    answerTextmessage += "\n";
                    answerTextmessage += "/top - shows top 10 pokemons\n";
                    answerTextmessage += "/all - shows all pokemons\n";
                    answerTextmessage += "/profile - shows your profile\n";
                    answerTextmessage += "/loc - shows bot location\n";
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
