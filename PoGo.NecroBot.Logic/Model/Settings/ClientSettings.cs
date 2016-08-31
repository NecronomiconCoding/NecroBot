using System;
using PoGo.NecroBot.Logic.Utils;
using PokemonGo.RocketAPI;
using PokemonGo.RocketAPI.Enums;
using Google.Protobuf;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    public class ClientSettings : ISettings
    {
        // Never spawn at the same position.
        private readonly Random _rand = new Random();
        private readonly GlobalSettings _settings;

        public ClientSettings(GlobalSettings settings)
        {
            _settings = settings;
        }


        public string GoogleUsername => _settings.Auth.AuthConfig.GoogleUsername;
        public string GooglePassword => _settings.Auth.AuthConfig.GooglePassword;

        #region Auth Config Values

        public bool UseProxy
        {
            get { return _settings.Auth.ProxyConfig.UseProxy; }
            set { _settings.Auth.ProxyConfig.UseProxy = value; }
        }

        public string UseProxyHost
        {
            get { return _settings.Auth.ProxyConfig.UseProxyHost; }
            set { _settings.Auth.ProxyConfig.UseProxyHost = value; }
        }

        public string UseProxyPort
        {
            get { return _settings.Auth.ProxyConfig.UseProxyPort; }
            set { _settings.Auth.ProxyConfig.UseProxyPort = value; }
        }

        public bool UseProxyAuthentication
        {
            get { return _settings.Auth.ProxyConfig.UseProxyAuthentication; }
            set { _settings.Auth.ProxyConfig.UseProxyAuthentication = value; }
        }

        public string UseProxyUsername
        {
            get { return _settings.Auth.ProxyConfig.UseProxyUsername; }
            set { _settings.Auth.ProxyConfig.UseProxyUsername = value; }
        }

        public string UseProxyPassword
        {
            get { return _settings.Auth.ProxyConfig.UseProxyPassword; }
            set { _settings.Auth.ProxyConfig.UseProxyPassword = value; }
        }

        public string GoogleRefreshToken
        {
            get { return null; }
            set { GoogleRefreshToken = null; }
        }

        public ByteString SessionHash { get; set; }

        AuthType ISettings.AuthType
        {
            get { return _settings.Auth.AuthConfig.AuthType; }

            set { _settings.Auth.AuthConfig.AuthType = value; }
        }

        string ISettings.GoogleUsername
        {
            get { return _settings.Auth.AuthConfig.GoogleUsername; }

            set { _settings.Auth.AuthConfig.GoogleUsername = value; }
        }

        string ISettings.GooglePassword
        {
            get { return _settings.Auth.AuthConfig.GooglePassword; }

            set { _settings.Auth.AuthConfig.GooglePassword = value; }
        }

        string ISettings.PtcUsername
        {
            get { return _settings.Auth.AuthConfig.PtcUsername; }

            set { _settings.Auth.AuthConfig.PtcUsername = value; }
        }

        string ISettings.PtcPassword
        {
            get { return _settings.Auth.AuthConfig.PtcPassword; }

            set { _settings.Auth.AuthConfig.PtcPassword = value; }
        }

        #endregion Auth Config Values

        #region Device Config Values

        string DevicePackageName
        {
            get { return _settings.Auth.DeviceConfig.DevicePackageName; }
            set { _settings.Auth.DeviceConfig.DevicePackageName = value; }
        }
        string ISettings.DeviceId
        {
            get { return _settings.Auth.DeviceConfig.DeviceId; }
            set { _settings.Auth.DeviceConfig.DeviceId = value; }
        }
        string ISettings.AndroidBoardName
        {
            get { return _settings.Auth.DeviceConfig.AndroidBoardName; }
            set { _settings.Auth.DeviceConfig.AndroidBoardName = value; }
        }
        string ISettings.AndroidBootloader
        {
            get { return _settings.Auth.DeviceConfig.AndroidBootloader; }
            set { _settings.Auth.DeviceConfig.AndroidBootloader = value; }
        }
        string ISettings.DeviceBrand
        {
            get { return _settings.Auth.DeviceConfig.DeviceBrand; }
            set { _settings.Auth.DeviceConfig.DeviceBrand = value; }
        }
        string ISettings.DeviceModel
        {
            get { return _settings.Auth.DeviceConfig.DeviceModel; }
            set { _settings.Auth.DeviceConfig.DeviceModel = value; }
        }
        string ISettings.DeviceModelIdentifier
        {
            get { return _settings.Auth.DeviceConfig.DeviceModelIdentifier; }
            set { _settings.Auth.DeviceConfig.DeviceModelIdentifier = value; }
        }
        string ISettings.DeviceModelBoot
        {
            get { return _settings.Auth.DeviceConfig.DeviceModelBoot; }
            set { _settings.Auth.DeviceConfig.DeviceModelBoot = value; }
        }
        string ISettings.HardwareManufacturer
        {
            get { return _settings.Auth.DeviceConfig.HardwareManufacturer; }
            set { _settings.Auth.DeviceConfig.HardwareManufacturer = value; }
        }
        string ISettings.HardwareModel
        {
            get { return _settings.Auth.DeviceConfig.HardwareModel; }
            set { _settings.Auth.DeviceConfig.HardwareModel = value; }
        }
        string ISettings.FirmwareBrand
        {
            get { return _settings.Auth.DeviceConfig.FirmwareBrand; }
            set { _settings.Auth.DeviceConfig.FirmwareBrand = value; }
        }
        string ISettings.FirmwareTags
        {
            get { return _settings.Auth.DeviceConfig.FirmwareTags; }
            set { _settings.Auth.DeviceConfig.FirmwareTags = value; }
        }
        string ISettings.FirmwareType
        {
            get { return _settings.Auth.DeviceConfig.FirmwareType; }
            set { _settings.Auth.DeviceConfig.FirmwareType = value; }
        }
        string ISettings.FirmwareFingerprint
        {
            get { return _settings.Auth.DeviceConfig.FirmwareFingerprint; }
            set { _settings.Auth.DeviceConfig.FirmwareFingerprint = value; }
        }

        #endregion Device Config Values

        double ISettings.DefaultLatitude
        {
            get
            {
                return _settings.LocationConfig.DefaultLatitude + _rand.NextDouble() * ((double)_settings.LocationConfig.MaxSpawnLocationOffset / 111111);
            }

            set { _settings.LocationConfig.DefaultLatitude = value; }
        }

        double ISettings.DefaultLongitude
        {
            get
            {
                return _settings.LocationConfig.DefaultLongitude +
                       _rand.NextDouble() *
                       ((double)_settings.LocationConfig.MaxSpawnLocationOffset / 111111 / Math.Cos(_settings.LocationConfig.DefaultLatitude));
            }

            set { _settings.LocationConfig.DefaultLongitude = value; }
        }

        double ISettings.DefaultAltitude
        {
            get
            {
                return
                    LocationUtils.getElevation(null, _settings.LocationConfig.DefaultLatitude, _settings.LocationConfig.DefaultLongitude) +
                    _rand.NextDouble() *
                    ((double)5 / Math.Cos(LocationUtils.getElevation(null, _settings.LocationConfig.DefaultLatitude, _settings.LocationConfig.DefaultLongitude)));
            }


            set { }
        }
    }
}