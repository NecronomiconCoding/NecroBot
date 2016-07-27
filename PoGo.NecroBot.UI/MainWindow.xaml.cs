using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

using PoGo.NecroBot.UI.Models;

namespace PoGo.NecroBot.UI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow {
        private StateModel _model;

        public StateModel Model
        {
            get { return _model; }
            set { _model = value; }
        }

        public MainWindow() {
            // Initialize View Model
            Model = new StateModel();

            InitializeComponent();

            
            this.DataContext = Model;
            //this.ToggleSwitch.DataContext = Model;
        }


        #region "Window Command Button Event Handlers"
        private void BtnSettings_Click(object sender, RoutedEventArgs e) {
            this.SettingsFlyout.IsOpen = !this.SettingsFlyout.IsOpen;
        }

        private void BtnToggleState_Click(object sender, RoutedEventArgs e) {

        }
        #endregion
    }
}
