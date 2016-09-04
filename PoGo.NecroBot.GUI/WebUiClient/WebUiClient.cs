#region using directives

using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace PoGo.NecroBot.GUI.WebUiClient
{
    [JsonObject(Title = "NecroBotWebUiClient", Description = "", ItemRequired = Required.DisallowNull)]
    public class WebUiClient
    {
        [JsonIgnore] private readonly string _basePath = Path.Combine(Directory.GetCurrentDirectory(), "WebUi");

        [JsonProperty(Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, Order = 4)] public readonly string HtlmPath;

        [JsonProperty(Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, Order = 2)] public readonly string RepoName;

        [JsonProperty(Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, Order = 1)] public readonly string RepoOwner;

        public WebUiClient(string owner, string repoName, string htlmPath)
        {
            RepoOwner = owner;
            RepoName = repoName;
            HtlmPath = htlmPath;
        }

        [JsonIgnore]
        public string HomeUri => "WebUi/" + RepoName + "-" + RepoOwner + "/index.html";

        public bool IsInstalled()
        {
            try
            {
                var indexHtmlPath = Path.Combine(_basePath,
                    RepoName + "-" + RepoOwner + Path.DirectorySeparatorChar + "index.html");
                return File.Exists(indexHtmlPath);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsUpToDate()
        {
            try
            {
                if (!IsInstalled())
                    return false;
                var referenceFile = Path.Combine(_basePath,
                    RepoName + "-" + RepoOwner + Path.DirectorySeparatorChar + RepoName + "-" + RepoOwner + ".reference");
                if (!File.Exists(referenceFile))
                    return false;
                var localReference = File.ReadAllText(referenceFile);
                var remoteReference = GetLastCommitRefFromGitHub(RepoOwner, RepoName);
                if (string.IsNullOrEmpty(remoteReference))
                    return true;
                return localReference == remoteReference;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void DonwloadAndInstall()
        {
            try
            {
                //Download source code from github
                var tmpPath = Path.Combine(Directory.GetCurrentDirectory(), "temp");
                if (!Directory.Exists(tmpPath))
                    Directory.CreateDirectory(tmpPath);
                var zipPath = Path.Combine(tmpPath, RepoName + "-" + RepoOwner + ".zip");
                if (File.Exists(zipPath))
                    File.Delete(zipPath);
                GetSourceCodeFromGitHub(zipPath, RepoOwner, RepoName);
                //Extract source code
                var extTmpPath = Path.Combine(tmpPath, RepoName + "-" + RepoOwner);
                if (Directory.Exists(extTmpPath))
                    Directory.Delete(extTmpPath, true);
                ZipFile.ExtractToDirectory(zipPath, extTmpPath);
                //Delete zip file
                if (File.Exists(zipPath))
                    File.Delete(zipPath);
                //Remove old version
                if (!Directory.Exists(_basePath))
                    Directory.CreateDirectory(_basePath);
                var finalPath = Path.Combine(_basePath, RepoName + "-" + RepoOwner);
                if (Directory.Exists(finalPath))
                    Directory.Delete(finalPath, true);
                //Copy new version
                var tmpFinalPath = Path.Combine(extTmpPath,
                    RepoName + "-master" + Path.DirectorySeparatorChar + HtlmPath);
                Directory.Move(tmpFinalPath, finalPath);
                //Remove temp content
                if (Directory.Exists(extTmpPath))
                    Directory.Delete(extTmpPath, true);
                //Add reference file
                var _ref = GetLastCommitRefFromGitHub(RepoOwner, RepoName);
                var referenceFile = Path.Combine(finalPath, RepoName + "-" + RepoOwner + ".reference");
                File.WriteAllText(referenceFile, _ref);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static void GetSourceCodeFromGitHub(string outputPath, string repoOwner, string repoName,
            string reference = null)
        {
            string url;
            if (reference != null)
                url = "https://github.com/" + repoOwner + "/" + repoName + "/archive/" + reference + ".zip";
            else
                url = "https://github.com/" + repoOwner + "/" + repoName + "/archive/master.zip";
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(url, outputPath);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static string GetLastCommitRefFromGitHub(string repoOwner, string repoName)
        {
            var uri = "https://api.github.com/repos/" + repoOwner + "/" + repoName + "/git/refs/heads/master";
            try
            {
                var request = WebRequest.CreateHttp(uri);
                request.Accept = "application/json";
                request.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.11; rv:47.0) Gecko/20100101 Firefox/47.0";
                request.Method = "GET";
                var resp = request.GetResponse();
                var reader = new StreamReader(resp.GetResponseStream());
                var fullresp = JObject.Parse(reader.ReadToEnd());
                return fullresp["object"]["sha"].ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}