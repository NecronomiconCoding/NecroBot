using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Start
{
    class Program
    {
        const string fullFileName = @"C:\Users\Fritz\Documents\GitHub\NecroBot\PoGo.NecroBot.CLI\bin\Debug\PoGo.NecroBot.CLI.exe";
        static List<string> Accounts = new List<string> { "rzeiberg", "rzeibergg", "rzeiberggg", "rzeibergggg", "rzeiberg1", "rzeiberg2", "rzeiberg3", "rzeiberg4", "rzeiberg5", };

        static void Main(string[] args)
        {
            foreach (var item in Accounts)
            {
                Process.Start(fullFileName, @"/authFile=" + item + ".json");
            }
        }
    }
}
