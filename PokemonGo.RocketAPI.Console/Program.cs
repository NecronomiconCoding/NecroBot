#region

using System;
using System.Threading;
using System.Threading.Tasks;
using PokemonGo.RocketAPI.Exceptions;
using PokemonGo.RocketAPI.Console.Forms;
using System.Windows.Forms;

#endregion

namespace PokemonGo.RocketAPI.Console
{
    internal class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}