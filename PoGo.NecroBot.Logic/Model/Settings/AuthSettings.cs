using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.Utils;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    public class AuthSettings
    {
        [JsonIgnore]
        private string _filePath;

        public AuthConfig AuthConfig = new AuthConfig();
        public ProxyConfig ProxyConfig = new ProxyConfig();
        public DeviceConfig DeviceConfig = new DeviceConfig();

        public AuthSettings()
        {
            InitializePropertyDefaultValues(this);
        }

        public void InitializePropertyDefaultValues(object obj)
        {
            FieldInfo[] fields = obj.GetType().GetFields();

            foreach (FieldInfo field in fields)
            {
                var d = field.GetCustomAttribute<DefaultValueAttribute>();

                if (d != null)
                    field.SetValue(obj, d.Value);
            }
        }

        public void Load(string path)
        {
            try
            {
                _filePath = path;

                if (File.Exists(_filePath))
                {
                    // if the file exists, load the settings
                    var input = File.ReadAllText(_filePath);

                    var settings = new JsonSerializerSettings();
                    settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                    JsonConvert.PopulateObject(input, this, settings);
                }
                // Do some post-load logic to determine what device info to be using - if 'custom' is set we just take what's in the file without question
                if (!this.DeviceConfig.DevicePackageName.Equals("random", StringComparison.InvariantCultureIgnoreCase) && !this.DeviceConfig.DevicePackageName.Equals("custom", StringComparison.InvariantCultureIgnoreCase))
                {
                    // User requested a specific device package, check to see if it exists and if so, set it up - otherwise fall-back to random package
                    string keepDevId = this.DeviceConfig.DeviceId;
                    SetDevInfoByKey(this.DeviceConfig.DevicePackageName);
                    this.DeviceConfig.DeviceId = keepDevId;
                }
                if (this.DeviceConfig.DevicePackageName.Equals("random", StringComparison.InvariantCultureIgnoreCase))
                {
                    // Random is set, so pick a random device package and set it up - it will get saved to disk below and re-used in subsequent sessions
                    Random rnd = new Random();
                    int rndIdx = rnd.Next(0, DeviceInfoHelper.DeviceInfoSets.Keys.Count - 1);
                    this.DeviceConfig.DevicePackageName = DeviceInfoHelper.DeviceInfoSets.Keys.ToArray()[rndIdx];
                    SetDevInfoByKey(this.DeviceConfig.DevicePackageName);
                }
                if (string.IsNullOrEmpty(this.DeviceConfig.DeviceId) || this.DeviceConfig.DeviceId == "8525f5d8201f78b5")
                    this.DeviceConfig.DeviceId = this.RandomString(16, "0123456789abcdef"); // changed to random hex as full alphabet letters could have been flagged

                // Jurann: Note that some device IDs I saw when adding devices had smaller numbers, only 12 or 14 chars instead of 16 - probably not important but noted here anyway

                Save(_filePath);
            }
            catch (JsonReaderException exception)
            {
                if (exception.Message.Contains("Unexpected character") && exception.Message.Contains("PtcUsername"))
                    Logger.Write("JSON Exception: You need to properly configure your PtcUsername using quotations.",
                        LogLevel.Error);
                else if (exception.Message.Contains("Unexpected character") && exception.Message.Contains("PtcPassword"))
                    Logger.Write(
                        "JSON Exception: You need to properly configure your PtcPassword using quotations.",
                        LogLevel.Error);
                else if (exception.Message.Contains("Unexpected character") &&
                         exception.Message.Contains("GoogleUsername"))
                    Logger.Write(
                        "JSON Exception: You need to properly configure your GoogleUsername using quotations.",
                        LogLevel.Error);
                else if (exception.Message.Contains("Unexpected character") &&
                         exception.Message.Contains("GooglePassword"))
                    Logger.Write(
                        "JSON Exception: You need to properly configure your GooglePassword using quotations.",
                        LogLevel.Error);
                else
                    Logger.Write("JSON Exception: " + exception.Message, LogLevel.Error);
            }
        }

        public void Save(string fullPath)
        {
            var jsonSerializeSettings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Include,
                Formatting = Formatting.Indented,
                Converters = new JsonConverter[] { new StringEnumConverter { CamelCaseText = true } }
            };

            var output = JsonConvert.SerializeObject(this, jsonSerializeSettings);

            var folder = Path.GetDirectoryName(fullPath);
            if (folder != null && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            File.WriteAllText(fullPath, output);
        }

        public void Save()
        {
            if (!string.IsNullOrEmpty(_filePath))
            {
                Save(_filePath);
            }
        }

        public void checkProxy(ITranslation translator)
        {
            using (var tempWebClient = new NecroWebClient())
            {
                string unproxiedIP = WebClientExtensions.DownloadString(tempWebClient, new Uri("https://api.ipify.org/?format=text"));
                if (ProxyConfig.UseProxy)
                {
                    tempWebClient.Proxy = this.InitProxy();
                    string proxiedIPres = WebClientExtensions.DownloadString(tempWebClient, new Uri("https://api.ipify.org/?format=text"));
                    string proxiedIP = proxiedIPres == null?"INVALID PROXY": proxiedIPres;
                    Logger.Write(translator.GetTranslation(TranslationString.Proxied, unproxiedIP, proxiedIP), LogLevel.Info, (unproxiedIP == proxiedIP) ? ConsoleColor.Red : ConsoleColor.Green);

                    if (unproxiedIP == proxiedIP || proxiedIPres == null)
                    {
                        Logger.Write(translator.GetTranslation(TranslationString.FixProxySettings), LogLevel.Info, ConsoleColor.Red);
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Logger.Write(translator.GetTranslation(TranslationString.Unproxied, unproxiedIP), LogLevel.Info, ConsoleColor.Red);
                }
            }
        }

        private string RandomString(int length, string alphabet = "abcdefghijklmnopqrstuvwxyz0123456789")
        {
            var outOfRange = Byte.MaxValue + 1 - (Byte.MaxValue + 1) % alphabet.Length;

            return string.Concat(
                Enumerable
                    .Repeat(0, Int32.MaxValue)
                    .Select(e => this.RandomByte())
                    .Where(randomByte => randomByte < outOfRange)
                    .Take(length)
                    .Select(randomByte => alphabet[randomByte % alphabet.Length])
                );
        }

        private byte RandomByte()
        {
            using (var randomizationProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[1];
                randomizationProvider.GetBytes(randomBytes);
                return randomBytes.Single();
            }
        }

        private void SetDevInfoByKey(string devKey)
        {
            if (DeviceInfoHelper.DeviceInfoSets.ContainsKey(this.DeviceConfig.DevicePackageName))
            {
                this.DeviceConfig.AndroidBoardName = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["AndroidBoardName"];
                this.DeviceConfig.AndroidBootloader = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["AndroidBootloader"];
                this.DeviceConfig.DeviceBrand = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["DeviceBrand"];
                this.DeviceConfig.DeviceId = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["DeviceId"];
                this.DeviceConfig.DeviceModel = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["DeviceModel"];
                this.DeviceConfig.DeviceModelBoot = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["DeviceModelBoot"];
                this.DeviceConfig.DeviceModelIdentifier = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["DeviceModelIdentifier"];
                this.DeviceConfig.FirmwareBrand = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["FirmwareBrand"];
                this.DeviceConfig.FirmwareFingerprint = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["FirmwareFingerprint"];
                this.DeviceConfig.FirmwareTags = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["FirmwareTags"];
                this.DeviceConfig.FirmwareType = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["FirmwareType"];
                this.DeviceConfig.HardwareManufacturer = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["HardwareManufacturer"];
                this.DeviceConfig.HardwareModel = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["HardwareModel"];
            }
            else
            {
                throw new ArgumentException("Invalid device info package! Check your auth.config file and make sure a valid DevicePackageName is set. For simple use set it to 'random'. If you have a custom device, then set it to 'custom'.");
            }
        }

        private WebProxy InitProxy()
        {
            if (!ProxyConfig.UseProxy) return null;

            WebProxy prox = new WebProxy(new System.Uri($"http://{ProxyConfig.UseProxyHost}:{ProxyConfig.UseProxyPort}"), false, null);

            if (ProxyConfig.UseProxyAuthentication)
                prox.Credentials = new NetworkCredential(ProxyConfig.UseProxyUsername, ProxyConfig.UseProxyPassword);

            return prox;
        }
    }
}