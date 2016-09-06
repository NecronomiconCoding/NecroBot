#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;

#endregion

namespace PoGo.NecroBot.GUI.WebUiClient
{
    public partial class WebUiClientSelector
    {
        private readonly bool _isSilentUpdate;
        private readonly WebUiClientConfig _settings;

        private CancellationTokenSource _cts;

        private string _currentWebUiClient;
        private bool _isCloseEvent;

        public WebUiClientSelector(WebUiClientConfig settings, bool isSilentUpdate = false)
        {
            _settings = settings;
            _isSilentUpdate = isSilentUpdate;
            AutoUpdateWebUiClient = settings.AutoUpdateWebUiClient;
            WebUiClients = settings.WebUiClients;
            CurrentWebUiClient = settings.CurrentWebUiClient;
            InitializeComponent();
            DataContext = this;
        }

        public bool AutoUpdateWebUiClient { get; set; }
        public Dictionary<string, WebUiClient> WebUiClients { get; set; }

        public string CurrentWebUiClient
        {
            get { return _currentWebUiClient; }
            set
            {
                try
                {
                    if (WebUiClients != null && value != null)
                    {
                        if (WebUiClients[value].IsUpToDate())
                        {
                            BtnDialogInstallUpdate.Visibility = Visibility.Hidden;
                            BtnDialogUninstall.Visibility = Visibility.Visible;
                        }
                        else if (WebUiClients[value].IsInstalled())
                        {
                            BtnDialogInstallUpdate.Visibility = Visibility.Visible;
                            BtnDialogUninstall.Visibility = Visibility.Visible;
                            BtnDialogInstallUpdate.Content = "Update";
                        }
                        else
                        {
                            BtnDialogInstallUpdate.Visibility = Visibility.Visible;
                            BtnDialogUninstall.Visibility = Visibility.Hidden;
                            BtnDialogInstallUpdate.Content = "Install";
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
                _currentWebUiClient = value;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AutoUpdateWebUiClient = _settings.AutoUpdateWebUiClient;
            WebUiClients = _settings.WebUiClients;
            CurrentWebUiClient = _settings.CurrentWebUiClient;

            if (_isSilentUpdate)
                btnDialogInstallUpdate_Click(this, null);
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (!WebUiClients[CurrentWebUiClient].IsInstalled())
            {
                MessageBox.Show(
                    "The current selected WebUi is not installed, install it first or close the selector dialog to ignore and continue.",
                    "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _settings.CurrentWebUiClient = CurrentWebUiClient;
            _settings.AutoUpdateWebUiClient = AutoUpdateWebUiClient;
            _settings.Save();
            DialogResult = true;
        }

        private void btnDialogUninstall_Click(object sender, RoutedEventArgs e)
        {
            WebUiClients[CurrentWebUiClient].Uninstall();

            if (WebUiClients[CurrentWebUiClient].IsUpToDate())
            {
                BtnDialogInstallUpdate.Visibility = Visibility.Hidden;
                BtnDialogUninstall.Visibility = Visibility.Visible;
            }
            else if (WebUiClients[CurrentWebUiClient].IsInstalled())
            {
                BtnDialogInstallUpdate.Visibility = Visibility.Visible;
                BtnDialogUninstall.Visibility = Visibility.Visible;
                BtnDialogInstallUpdate.Content = "Update";
            }
            else
            {
                BtnDialogInstallUpdate.Visibility = Visibility.Visible;
                BtnDialogUninstall.Visibility = Visibility.Hidden;
                BtnDialogInstallUpdate.Content = "Install";
            }
        }

        private void ReportProgress(int value)
        {
            ProgressBar1.Value = value;
        }

        private async void btnDialogInstallUpdate_Click(object sender, RoutedEventArgs e)
        {
            BtnDialogOk.Visibility = Visibility.Hidden;
            BtnDialogInstallUpdate.Visibility = Visibility.Hidden;
            BtnDialogUninstall.Visibility = Visibility.Hidden;

            ComboBox1.IsEnabled = false;
            CheckBox1.IsEnabled = false;

            if (!WebUiClients[CurrentWebUiClient].IsInstalled() || !WebUiClients[CurrentWebUiClient].IsUpToDate())
            {
                BtnDialogCancel.Visibility = Visibility.Visible;
                ProgressBar1.Value = 0;
                ProgressBar1.Visibility = Visibility.Visible;

                _cts = new CancellationTokenSource();

                var progressIndicator = new Progress<int>(ReportProgress);
                try
                {
                    await WebUiClients[CurrentWebUiClient].DownloadAndInstall(_cts.Token, progressIndicator);

                    if (_isSilentUpdate)
                    {
                        DialogResult = true;
                        return;
                    }
                }
                catch (OperationCanceledException ex)
                {
                    if (_isSilentUpdate && !_isCloseEvent)
                    {
                        if (!_isCloseEvent)
                            DialogResult = true;
                        return;
                    }

                    MessageBox.Show(ex.Message, "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    if (_isSilentUpdate)
                    {
                        if (!_isCloseEvent)
                            DialogResult = true;
                        return;
                    }

                    MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                BtnDialogCancel.Visibility = Visibility.Hidden;
                ProgressBar1.Visibility = Visibility.Hidden;
            }

            if (_isSilentUpdate)
            {
                DialogResult = true;
                return;
            }

            if (WebUiClients[CurrentWebUiClient].IsUpToDate())
            {
                BtnDialogInstallUpdate.Visibility = Visibility.Hidden;
                BtnDialogUninstall.Visibility = Visibility.Visible;
            }
            else if (WebUiClients[CurrentWebUiClient].IsInstalled())
            {
                BtnDialogInstallUpdate.Visibility = Visibility.Visible;
                BtnDialogUninstall.Visibility = Visibility.Visible;
                BtnDialogInstallUpdate.Content = "Update";
            }
            else
            {
                BtnDialogInstallUpdate.Visibility = Visibility.Visible;
                BtnDialogUninstall.Visibility = Visibility.Hidden;
                BtnDialogInstallUpdate.Content = "Install";
            }

            BtnDialogOk.Visibility = Visibility.Visible;
            ComboBox1.IsEnabled = true;
            CheckBox1.IsEnabled = true;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (_cts == null || _cts.IsCancellationRequested) return;
            _isCloseEvent = true;
            _cts.Cancel();
        }

        private void btnDialogCancel_Click(object sender, RoutedEventArgs e)
        {
            if (_cts != null && !_cts.IsCancellationRequested)
                _cts.Cancel();
        }
    }
}