using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Security.AccessControl;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PoGo.NecroBot.Logic.Settings {
    public abstract class SettingsFile {
        [JsonIgnore]
        protected string FilePath { get; set; }
        [JsonIgnore]
        private CancellationTokenSource CancelToken { get; set; }
        [JsonIgnore]
        private bool IsEmpty
        {
            get
            {
                EnsureFileExists();
                string contents = File.ReadAllText(FilePath);
                return string.IsNullOrEmpty(contents);
            }
        }

        #region "Maintenance Operations"
        private void CancelOperationIfInProgress() {
            if (CancelToken != null) {
                CancelToken.Cancel();
            }
        }
        private void EnsureDirectoryExists() {
            string _directory = Path.GetDirectoryName(FilePath);

            if (!Directory.Exists(_directory))
                Directory.CreateDirectory(_directory);
        }

        private void EnsureFileExists() {
            EnsureDirectoryExists();

            if (!File.Exists(FilePath))
                File.WriteAllText(FilePath, "");
        }
        #endregion

        #region "IO Operations"
        protected virtual void LoadOrInitializeFile() {
            if (IsEmpty) {
                SaveFile();
            } else {
                LoadFile();
            }                
        }

        protected async void LoadOrInitializeFileAsync() {
            LoadOrInitializeFile();
        }

        protected void SaveFile() {
            string data = Serialize();
            File.WriteAllText(FilePath, data);
        }

        protected void LoadFile() {
            string data = File.ReadAllText(FilePath);
            Deserialize(data);
        }

        protected async Task SaveFileAsync() {
            CancelOperationIfInProgress();
            SaveFile();
        }

        protected async Task LoadFileAsync() {
            CancelOperationIfInProgress();
            LoadFile();
        }
        #endregion

        #region "Serialization Operations"
        private string Serialize() {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new StringEnumConverter { CamelCaseText = true });
        }

        private void Deserialize(string data) {
            if (string.IsNullOrEmpty(data))
                return;

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
            JsonConvert.PopulateObject(data, this, settings);
        }
        #endregion
    }

    public class NoFilePathException : Exception {
        public NoFilePathException() : base() { }
        public NoFilePathException(string message) : base(message) { }
        public NoFilePathException(string message, Exception inner) : base(message, inner) { }
    }

    public class NoWriteAccessException : Exception {
        public NoWriteAccessException() : base() { }
        public NoWriteAccessException(string message) : base(message) { }
        public NoWriteAccessException(string message, Exception inner) : base(message, inner) { }
    }
}
