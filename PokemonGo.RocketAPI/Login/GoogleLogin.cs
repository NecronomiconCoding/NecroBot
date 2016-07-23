#region Usings

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PokemonGo.RocketAPI.Helpers;

#endregion

namespace PokemonGo.RocketAPI.Login
{
    public static class GoogleLogin
    {
        private const string OauthTokenEndpoint = "https://www.googleapis.com/oauth2/v4/token";
        private const string OauthEndpoint = "https://accounts.google.com/o/oauth2/device/code";
        private const string ClientId = "848232511240-73ri3t7plvk96pj4f85uj8otdat2alem.apps.googleusercontent.com";
        private const string ClientSecret = "NCjF1TLi2CcY6t5mt0ZveuL7";

        /// <summary>
        ///     Gets the access token from Google
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns>tokenResponse</returns>
        public static async Task<TokenResponseModel> GetAccessToken(DeviceCodeModel deviceCode)
        {
            //Poll until user submitted code..
            TokenResponseModel tokenResponse;
            do
            {
                await Task.Delay(2000);
                tokenResponse = await PollSubmittedToken(deviceCode.DeviceCode);
            } while (tokenResponse.AccessToken == null || tokenResponse.RefreshToken == null);

            Logger.Write($"Save the refresh token in your settings: {tokenResponse.RefreshToken}", LogLevel.None);

            return tokenResponse;
        }

        public static async Task<TokenResponseModel> GetAccessToken(string refreshToken)
        {
            return await HttpClientHelper.PostFormEncodedAsync<TokenResponseModel>(OauthTokenEndpoint,
                new KeyValuePair<string, string>("access_type", "offline"),
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("scope", "openid email https://www.googleapis.com/auth/userinfo.email"));
        }

        public static async Task<DeviceCodeModel> GetDeviceCode()
        {
            var deviceCode = await HttpClientHelper.PostFormEncodedAsync<DeviceCodeModel>(OauthEndpoint,
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("scope", "openid email https://www.googleapis.com/auth/userinfo.email"));

            Logger.Write($"Please visit {deviceCode.VerificationUrl} and enter {deviceCode.UserCode}", LogLevel.None);

            await Task.Delay(2000);
            try
            {
                Process.Start(@"http://www.google.com/device");
                var thread = new Thread(() => Clipboard.SetText(deviceCode.UserCode)); //Copy device code
                thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                thread.Start();
                thread.Join();
            }
            catch (Exception)
            {
                Logger.Write("Couldnt copy to clipboard, do it manually", LogLevel.Error);
                Logger.Write($"Goto: http://www.google.com/device & enter {deviceCode.UserCode}", LogLevel.Error);
            }

            return deviceCode;
        }

        private static async Task<TokenResponseModel> PollSubmittedToken(string deviceCode)
        {
            return await HttpClientHelper.PostFormEncodedAsync<TokenResponseModel>(OauthTokenEndpoint,
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret),
                new KeyValuePair<string, string>("code", deviceCode),
                new KeyValuePair<string, string>("grant_type", "http://oauth.net/grant_type/device/1.0"),
                new KeyValuePair<string, string>("scope", "openid email https://www.googleapis.com/auth/userinfo.email"));
        }


        internal class ErrorResponseModel
        {
            public string Error { get; set; }
            public string ErrorDescription { get; set; }
        }

        public class TokenResponseModel
        {
            public string AccessToken { get; set; }
            public string TokenType { get; set; }
            public int ExpiresIn { get; set; }
            public string RefreshToken { get; set; }
            public string IdToken { get; set; }
        }


        public class DeviceCodeModel
        {
            public string VerificationUrl { get; set; }
            public int ExpiresIn { get; set; }
            public int Interval { get; set; }
            public string DeviceCode { get; set; }
            public string UserCode { get; set; }
        }
    }
}