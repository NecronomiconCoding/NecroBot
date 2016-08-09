using System;
using System.Windows;
using PoGo.NecroBot.Logic;
using PoGo.NecroBot.ConfigUI.Models;
using System.IO;

namespace PoGo.NecroBot.ConfigUI
{

    /// <summary> Interaction logic for MainWindow.xaml </summary>
    public partial class MainWindow : Window
    {
        private GlobalSettings _set = new GlobalSettings();


        public ObservableSettings Settings
        {
            get { return (ObservableSettings)GetValue(SettingsProperty); }
            set { SetValue(SettingsProperty, value); }
        }
        public static readonly DependencyProperty SettingsProperty =
            DependencyProperty.Register("Settings", typeof(ObservableSettings), typeof(MainWindow), new PropertyMetadata(null));



        public MainWindow()
        {
            this.DataContext = this;
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadButton_Click(this, null);
        }

        private void DefaultsButton_Click(object sender, RoutedEventArgs e)
        {
            _set = new GlobalSettings();
            Settings = ObservableSettings.CreateFromGlobalSettings(_set);
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            _set = GlobalSettings.Load("");
            if (null == _set) _set = GlobalSettings.Load("");
            if (null == _set) throw new Exception("There was an error attempting to build default config files - may be a file permissions issue! Cannot proceed.");
            Settings = ObservableSettings.CreateFromGlobalSettings(_set);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var profilePath = Path.Combine(Directory.GetCurrentDirectory(), "");
            var profileConfigPath = Path.Combine(profilePath, "config");
            var configFile = Path.Combine(profileConfigPath, "config.json");
            var authFile = Path.Combine(profileConfigPath, "auth.json");
            _set = Settings.GetGlobalSettingsObject();
            _set.Save(configFile);
            _set.Auth.Save(authFile);
        }
    }
}
