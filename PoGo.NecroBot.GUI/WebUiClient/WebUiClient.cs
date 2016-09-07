#region using directives

using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace PoGo.NecroBot.GUI.WebUiClient
{
    [JsonObject(Title = "NecroBotWebUiClient", Description = "", ItemRequired = Required.DisallowNull)]
    public class WebUiClient
    {
        [JsonIgnore] private readonly string _basePath = Path.Combine(Directory.GetCurrentDirectory(), "WebUi");

        [JsonProperty(Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, Order = 1)]
        public readonly string RepoOwner;

        [JsonProperty(Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, Order = 2)]
        public readonly string RepoName;

        [JsonProperty(Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, Order = 3)]
        public readonly string HtlmRootPath;

        [JsonProperty(Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, Order = 4)]
        public readonly string HtlmDefaultFilePath;

        private DateTime _lastCommitCheckDateTime = DateTime.Now;

        private string _lastCommitCheckRef = "";

        public WebUiClient(string owner, string repoName, string htlmRootPath, string htlmDefaultFilePath)
        {
            RepoOwner = owner;
            RepoName = repoName;
            HtlmRootPath = htlmRootPath;
            HtlmDefaultFilePath = htlmDefaultFilePath;
        }

        [JsonIgnore]
        public string HomeUri => "WebUi/" + RepoName + "-" + RepoOwner + "/" + HtlmDefaultFilePath;

        public bool IsInstalled()
        {
            var indexHtmlPath = Path.Combine(_basePath,
                RepoName + "-" + RepoOwner + Path.DirectorySeparatorChar + "index.html");
            return File.Exists(indexHtmlPath);
        }

        public bool IsUpToDate()
        {
            if (!IsInstalled())
                return false;
            var referenceFile = Path.Combine(_basePath,
                RepoName + "-" + RepoOwner + Path.DirectorySeparatorChar + RepoName + ".reference");
            if (!File.Exists(referenceFile))
                return false;
            var localReference = File.ReadAllText(referenceFile);
            var remoteReference = GetLastCommitRef(RepoOwner, RepoName);
            if (string.IsNullOrEmpty(remoteReference))
                return true;
            return localReference == remoteReference;
        }

        public void Uninstall()
        {
            if (!IsInstalled())
                return;
            var finalPath = Path.Combine(_basePath, RepoName + "-" + RepoOwner);
            if (Directory.Exists(finalPath))
                Directory.Delete(finalPath, true);
        }

        public async Task DownloadAndInstall(CancellationToken ct, IProgress<int> progress = null)
        {
            var tmpPath = Path.Combine(_basePath, "Temp");
            var zipPath = Path.Combine(tmpPath, RepoName + "-" + RepoOwner + ".zip");
            var extTmpPath = Path.Combine(tmpPath, RepoName + "-" + RepoOwner);
            var finalPath = Path.Combine(_basePath, RepoName + "-" + RepoOwner);
            var tmpFinalPath = Path.Combine(extTmpPath,
                RepoName + "-master" + Path.DirectorySeparatorChar + HtlmRootPath);
            var referenceFile = Path.Combine(finalPath, RepoName + ".reference");
            try
            {
                //Download source code from github
                if (!Directory.Exists(tmpPath))
                    Directory.CreateDirectory(tmpPath);

                if (File.Exists(zipPath))
                    File.Delete(zipPath);

                var url = "https://github.com/" + RepoOwner + "/" + RepoName + "/archive/master.zip";

                const int bufferSize = 2048;
                var bufferBytes = new byte[bufferSize];

                var webRequest = (HttpWebRequest) WebRequest.Create(url);
                var response = (HttpWebResponse) await webRequest.GetResponseAsync();
                var totalSize = response.ContentLength;
                var downloadsize = 0;
                var responseStream = response.GetResponseStream();
                var fakepercentComplete = 0;
                var lastfakepercentcomplete = DateTime.Now;

                using (var filestream = File.Open(zipPath, FileMode.Create, FileAccess.Write))
                {
                    while (true)
                    {
                        if (ct.IsCancellationRequested)
                        {
                            response.Close();
                            filestream.Close();
                            ct.ThrowIfCancellationRequested();
                        }

                        var readSize = await responseStream.ReadAsync(bufferBytes, 0, bufferBytes.Length, ct);
                        if (readSize > 0)
                        {
                            await filestream.WriteAsync(bufferBytes, 0, readSize, ct);

                            //ReportProgress
                            if (totalSize > 0)
                            {
                                downloadsize += readSize;
                                var percentComplete = (int) (downloadsize/(float) totalSize*100);
                                var fixpercentComplete = (int) (percentComplete*0.8);
                                progress?.Report(fixpercentComplete);
                            }

                            //Fake ReportProgress
                            else if (DateTime.Now > lastfakepercentcomplete.AddSeconds(2))
                            {
                                lastfakepercentcomplete = DateTime.Now;
                                fakepercentComplete += 5;

                                if (fakepercentComplete >= 80)
                                    fakepercentComplete = 20;

                                progress?.Report(fakepercentComplete);
                            }
                        }
                        else
                        {
                            response.Close();
                            filestream.Close();
                            break;
                        }
                    }
                }

                //ThrowIfCancellationRequested
                progress?.Report(80);
                ct.ThrowIfCancellationRequested();

                //Extract source code
                if (Directory.Exists(extTmpPath))
                    Directory.Delete(extTmpPath, true);

                ZipFile.ExtractToDirectory(zipPath, extTmpPath);

                //Delete zip file
                if (File.Exists(zipPath))
                    File.Delete(zipPath);

                //ReportProgress and ThrowIfCancellationRequested
                progress?.Report(90);
                ct.ThrowIfCancellationRequested();

                //Remove old version
                if (Directory.Exists(finalPath))
                    Directory.Delete(finalPath, true);

                //Copy new version
                if (!Directory.Exists(_basePath))
                    Directory.CreateDirectory(_basePath);
                Directory.Move(tmpFinalPath, finalPath);

                //Remove temp content
                if (Directory.Exists(tmpPath))
                    Directory.Delete(tmpPath, true);

                //Add reference file
                var _ref = GetLastCommitRef(RepoOwner, RepoName);
                File.WriteAllText(referenceFile, _ref);

                //ReportProgress 
                progress?.Report(100);
            }
            catch (Exception)
            {
                //Delete zip file
                if (File.Exists(zipPath))
                    File.Delete(zipPath);

                //Remove temp content
                if (Directory.Exists(tmpPath))
                    Directory.Delete(tmpPath, true);

                throw;
            }
        }

        private string GetLastCommitRef(string repoOwner, string repoName)
        {
            if (DateTime.Now < _lastCommitCheckDateTime.AddMinutes(5) && !string.IsNullOrEmpty(_lastCommitCheckRef))
                return _lastCommitCheckRef;
            try
            {
                var uri = "https://api.github.com/repos/" + repoOwner + "/" + repoName + "/git/refs/heads/master";
                var request = WebRequest.CreateHttp(uri);
                request.Accept = "application/json";
                request.UserAgent =
                    "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.11; rv:47.0) Gecko/20100101 Firefox/47.0";
                request.Method = "GET";
                var resp = request.GetResponse();
                var reader = new StreamReader(resp.GetResponseStream());
                var fullresp = JObject.Parse(reader.ReadToEnd());
                _lastCommitCheckRef = fullresp["object"]["sha"].ToString();
                return _lastCommitCheckRef;
            }
            catch
            {
                return null;
            }
        }
    }
}