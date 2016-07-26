#region using directives

using System;
using System.IO;

#endregion

namespace PoGo.NecroBot.Logic.Dumper
{
    public static class Dumper
    {
        private static IDumper _dumper;
        private static string _subPath;

        /// <summary>
        ///     This is used for dumping contents to a file stored in the Logs folder.
        /// </summary>
        /// <param name="data">Dumps the string data to the file</param>
        /// <param name="filename">Filename to be used for naming the file.</param>
        private static void DumpToFile(string data, string filename)
        {
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + _subPath + "\\Dumps");

            string path = Directory.GetCurrentDirectory() + _subPath + $"\\Dumps\\NecroBot-{filename}.txt";

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
        ///     Set the Dumper.
        /// </summary>
        /// <param name="logger"></param>
        public static void SetDumper(IDumper dumper, string subPath = "")
        {
            _dumper = dumper;
            _subPath = subPath;
        }

        /// <summary>
        ///     Clears the specified dumpfile.
        /// </summary>
        /// <param name="filename">File to clear/param>
        public static void ClearDumpFile(string filename)
        {
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + _subPath + "\\Dumps");

            string path = Directory.GetCurrentDirectory() + _subPath + $"\\Dumps\\NecroBot-{filename}-{DateTime.Today.ToString("yyyy-MM-dd")}-{DateTime.Now.ToString("HH")}.txt";

            // Clears all contents of a file first if overwrite is true
            File.WriteAllText(path, string.Empty);
        }

        /// <summary>
        ///     Dumps data to a file
        /// </summary>
        /// <param name="data">Dumps the string data to the file</param>
        /// <param name="filename">Filename to be used for naming the file.</param>
        public static void Dump(string data, string filename)
        {
            string uniqueFileName = $"{filename}-{DateTime.Today.ToString("yyyy-MM-dd")}-{DateTime.Now.ToString("HH")}";

            DumpToFile(data, uniqueFileName);
        }
    }
}