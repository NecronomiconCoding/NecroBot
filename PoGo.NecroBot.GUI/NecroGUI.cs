using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using PoGo.NecroBot.Logic.Logging;


namespace PoGo.NecroBot.GUI
{
    public partial class NecroGUI : Form
    {
        delegate void SetTextCallback(string message, ConsoleColor color = ConsoleColor.Black);
        delegate void SetLightTextCallback(string message);
        public NecroGUI()
        {
            InitializeComponent();
        }
    }
}
