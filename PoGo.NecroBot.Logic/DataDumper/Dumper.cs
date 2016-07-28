#region using directives

using System;
using System.IO;
using PoGo.NecroBot.Logic;
using PoGo.NecroBot.Logic.State;

#endregion

namespace PoGo.NecroBot.Logic.DataDumper
{
    public static class Dumper
    {
        private static IDumper _dumper;
        private static string _subPath;

        /// <summary>
        ///     This is used for dumping contents to a file stored in the Logs folder.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="data">Dumps the string data to the file</param>
        /// <param name="filename">Filename to be used for naming the file.</param>
        private static void DumpToFile(ISession session, string data, string filename)
        {
            var path = Path.Combine(session.LogicSettings.ProfilePath,"Dumps",$"NecroBot-{filename}-{DateTime.Today.ToString("yyyy-MM-dd")}-{DateTime.Now.ToString("HH")}.txt");

            using (
                var dumpFile =
                    File.AppendText(path)
                )
            {
                dumpFile.WriteLine(data);
                dumpFile.Flush();
            }
        }

        /// <summary>
        ///     Set the dumper.
        /// </summary>
        /// <param name="dumper"></param>
        /// <param name="subPath"></param>
        public static void SetDumper(IDumper dumper, string subPath = "")
        {
            _dumper = dumper;
            _subPath = subPath;
        }

        /// <summary>
        ///     Clears the specified dumpfile.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="filename"/>File to clear/param>
        public static void ClearDumpFile(ISession session, string filename)
        {
            var path = Path.Combine(session.LogicSettings.ProfilePath,"Dumps",$"NecroBot-{filename}-{DateTime.Today.ToString("yyyy-MM-dd")}-{DateTime.Now.ToString("HH")}.txt");
            // Clears all contents of a file first if overwrite is true
            File.WriteAllText(path, string.Empty);
        }

        /// <summary>
        ///     Dumps data to a file
        /// </summary>
        /// <param name="session"></param>
        /// <param name="data">Dumps the string data to the file</param>
        /// <param name="filename">Filename to be used for naming the file.</param>
        public static void Dump(ISession session, string data, string filename)
        {
            string uniqueFileName = $"{filename}";

            DumpToFile(session, data, uniqueFileName);
        }
    }
}