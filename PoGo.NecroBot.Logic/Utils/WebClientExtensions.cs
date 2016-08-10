using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PoGo.NecroBot.Logic.Utils
{
    public static class WebClientExtensions
    {
        public static string DownloadString(this WebClient webClient, Uri uri)
        {
            webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            webClient.Encoding = Encoding.UTF8;
            byte[] rawData = null;
            string error;
            try
            {
                error = "loading";
                rawData = webClient.DownloadData(uri);
            }
            catch (NullReferenceException)
            {
                error = null;
            }
            catch (ArgumentNullException)
            {
                error = null;
            }
            catch (WebException)
            {
                error = null;
            }
            catch (SocketException)
            {
                error = null;
            }

            if ( error == null || rawData == null )
                return null;

            var encoding = WebUtils.GetEncodingFrom(webClient.ResponseHeaders, Encoding.UTF8);
            return encoding.GetString(rawData);
        }
    }
}
