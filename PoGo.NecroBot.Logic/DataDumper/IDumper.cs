namespace PoGo.NecroBot.Logic.DataDumper
{
    public interface IDumper
    {
        /// <summary>
        ///     Dump specific data.
        /// </summary>
        /// <param name="data">The data to dump.</param>
        /// <param name="filename">File to dump to</param>
        void Dump(string data, string filename);
    }
}