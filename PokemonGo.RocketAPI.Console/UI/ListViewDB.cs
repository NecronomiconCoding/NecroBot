using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokemonGo.RocketAPI.Console.UI
{
    public partial class ListViewDB : ListView
    {

        protected override void InitLayout()
        {
            base.InitLayout();
            this.DoubleBuffered = true;

        }


    }

}