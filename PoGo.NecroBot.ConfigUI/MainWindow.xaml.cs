using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using PoGo.NecroBot.ConfigUI.Models;
using PoGo.NecroBot.Logic;
using PoGo.NecroBot.Logic.Utils;
using PokemonGo.RocketAPI.Enums;

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

        public bool IsGoogleAuthShowing
        {
            get { return (bool)GetValue(IsGoogleAuthShowingProperty); }
            set { SetValue(IsGoogleAuthShowingProperty, value); }
        }
        public static readonly DependencyProperty IsGoogleAuthShowingProperty =
            DependencyProperty.Register("IsGoogleAuthShowing", typeof(bool), typeof(MainWindow), new PropertyMetadata(true));

        public bool IsCustomDevicePackage
        {
            get { return (bool)GetValue(IsCustomDevicePackageProperty); }
            set { SetValue(IsCustomDevicePackageProperty, value); }
        }
        public static readonly DependencyProperty IsCustomDevicePackageProperty =
            DependencyProperty.Register("IsCustomDevicePackage", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        public List<string> DevicePackageCollection
        {
            get { return (List<string>)GetValue(DevicePackageCollectionProperty); }
            set { SetValue(DevicePackageCollectionProperty, value); }
        }
        public static readonly DependencyProperty DevicePackageCollectionProperty =
            DependencyProperty.Register("DevicePackageCollection", typeof(List<string>), typeof(MainWindow), new PropertyMetadata(new List<string>()));

        public List<string> OperatorsCollection
        {
            get { return (List<string>)GetValue(OperatorsCollectionProperty); }
            set { SetValue(OperatorsCollectionProperty, value); }
        }
        public static readonly DependencyProperty OperatorsCollectionProperty =
            DependencyProperty.Register("OperatorsCollection", typeof(List<string>), typeof(MainWindow), new PropertyMetadata(new List<string>() { "or", "and" }));

        public List<string> IpCvCollection
        {
            get { return (List<string>)GetValue(IpCvCollectionProperty); }
            set { SetValue(IpCvCollectionProperty, value); }
        }
        public static readonly DependencyProperty IpCvCollectionProperty =
            DependencyProperty.Register("IpCvCollection", typeof(List<string>), typeof(MainWindow), new PropertyMetadata(new List<string>() { "iv", "cp" }));




        public MainWindow()
        {
            this.DataContext = this;
          //  InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DevicePackageCollection.Add("custom");
            DevicePackageCollection.Add("random");
            foreach (string key in DeviceInfoHelper.DeviceInfoSets.Keys)
                DevicePackageCollection.Add(key);
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

        private void AuthType_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            IsGoogleAuthShowing = (AuthType.Google == (AuthType)e.AddedItems[0]);
        }

        private void DevicePackage_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            IsCustomDevicePackage = e.AddedItems[0].Equals("custom");
        }

    }
}
