using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.State;

namespace PoGo.NecroBot.CLI.Models
{
    internal class LoggingStrings
    {
        internal static string Attention;

        internal static string Berry;

        internal static string Debug;

        internal static string Egg;

        internal static string Error;

        internal static string Evolved;

        internal static string Farming;

        internal static string Info;

        internal static string Pkmn;

        internal static string Pokestop;

        internal static string Recycling;

        internal static string Sniper;

        internal static string Transferred;

        internal static string Update;

        internal static string New;

        internal static void SetStrings(ISession session)
        {
            Attention =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryAttention) ?? "ATTENTION";

            Berry =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryBerry) ?? "BERRY";

            Debug =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryDebug) ?? "DEBUG";

            Egg =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryEgg) ?? "EGG";

            Error =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryError) ?? "ERROR";

            Evolved =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryEvolved) ?? "EVOLVED";

            Farming =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryFarming) ?? "FARMING";

            Info =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryInfo) ?? "INFO";

            Pkmn =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryPkmn) ?? "PKMN";

            Pokestop =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryPokestop) ?? "POKESTOP";

            Recycling =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryRecycling) ?? "RECYCLING";

            Sniper =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntrySniper) ?? "SNIPER";

            Transferred =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryTransfered) ?? "TRANSFERED";

            Update =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryUpdate ) ?? "UPDATE";

            New =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryNew ) ?? "NEW";
        }
    }
}
