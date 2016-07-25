#region

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Protobuf;
using PokemonGo.RocketAPI.Exceptions;
using PokemonGo.RocketAPI.Extensions;
using PokemonGo.RocketAPI.Helpers;
using PokemonGo.RocketAPI.Login;
using PokemonGoDesktop.API.Common;
using PokemonGoDesktop.API.Proto;
using PokemonGoDesktop.API.Proto.Services;

#endregion

namespace PokemonGo.RocketAPI
{
    public class Client
    {
        private readonly HttpClient _httpClient;
        private string _apiUrl;
        private AuthType _authType = AuthType.Google;
        private AuthTicket _authTicket; //the old Ferox _unknownAuth object
        Random rand = null;

        public Client(ISettings settings)
        {
            Settings = settings;

            DirectoryInfo di = Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Configs");

            Tuple<double, double> latLngFromFile = GetLatLngFromFile();

            if (latLngFromFile != null)
            {
                SetCoordinates(latLngFromFile.Item1, latLngFromFile.Item2, Settings.DefaultAltitude);
            }
            else
            {
                SetCoordinates(Settings.DefaultLatitude, Settings.DefaultLongitude, Settings.DefaultAltitude);
            }

            //Setup HttpClient and create default headers
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                AllowAutoRedirect = false
            };
            _httpClient = new HttpClient(new RetryHandler(handler));
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Niantic App");
            //"Dalvik/2.1.0 (Linux; U; Android 5.1.1; SM-G900F Build/LMY48G)");
            _httpClient.DefaultRequestHeaders.ExpectContinue = false;
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type",
                "application/x-www-form-urlencoded");
        }

        /// <summary>
        /// Gets the lat LNG from file.
        /// </summary>
        /// <returns>Tuple&lt;System.Double, System.Double&gt;.</returns>
        public static Tuple<double, double> GetLatLngFromFile()
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "\\Configs\\Coords.ini") &&
                File.ReadAllText(Directory.GetCurrentDirectory() + "\\Configs\\Coords.ini").Contains(":"))
            {
                var latlngFromFile = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Configs\\Coords.ini");
                var latlng = latlngFromFile.Split(':');
                if (latlng[0].Length != 0 && latlng[1].Length != 0)
                {
                    try
                    {
                        double temp_lat = Convert.ToDouble(latlng[0]);
                        double temp_long = Convert.ToDouble(latlng[1]);

                        if (temp_lat >= -90 && temp_lat <= 90 && temp_long >= -180 && temp_long <= 180)
                        {
                            return new Tuple<double, double>(temp_lat, temp_long);
                        }
                        else
                        {
                            Logger.Write("Coordinates in \"Coords.ini\" file are invalid, using the default coordinates ",
                            LogLevel.Warning);
                            return null;
                        }
                    }
                    catch (FormatException)
                    {
                        Logger.Write("Coordinates in \"Coords.ini\" file are invalid, using the default coordinates ",
                            LogLevel.Warning);
                        return null;
                    }
                }

            }

            return null;
        }

        public ISettings Settings { get; }
        public string AccessToken { get; set; }

        public double CurrentLat { get; private set; }
        public double CurrentLng { get; private set; }
        public double CurrentAltitude { get; private set; }

        public async Task<CatchPokemonResponse> CatchPokemon(ulong encounterId, string spawnPointGuid, double pokemonLat,
            double pokemonLng, ItemId pokeball)
        {
            CatchPokemonMessage catchPokemon = new CatchPokemonMessage()
            {
                EncounterId = encounterId,
                Pokeball = pokeball,
                SpawnPointId = spawnPointGuid,
                HitPokemon = true,
                NormalizedReticleSize = 1.950,
                SpinModifier = 1,
                NormalizedHitPosition = 1
            };

            return await AwaitableOnResponseFor<CatchPokemonMessage, CatchPokemonResponse>(catchPokemon, RequestType.CatchPokemon);
        }

        private async Task<TResponseTypeMessage> AwaitableOnResponseFor<TRequestMessageType, TResponseTypeMessage>(TRequestMessageType requestMessage, RequestType requestType)
            where TRequestMessageType : IRequestMessage, IMessage
            where TResponseTypeMessage : IResponseMessage, IMessage, IMessage<TResponseTypeMessage>, new()
        {
            //builds the general envelope with the provided request
            var requestEnvelope = RequestEnvelopeBuilder.GetRequestEnvelope(_authTicket, CurrentLat, CurrentLng, CurrentAltitude)
                .WithMessage(new Request()
                {
                    RequestType = requestType,
                    RequestMessage = requestMessage.ToByteString()
                });

            //awaits for the IResponseMessage
            return
                await
                    _httpClient.PostProtoPayload<TResponseTypeMessage>($"https://{_apiUrl}/rpc",
                        requestEnvelope);
        }

        private async Task<TResponseTypeMessage> AwaitableOnResponseFor<TResponseTypeMessage>(RequestType requestType)
            where TResponseTypeMessage : IResponseMessage, IMessage, IMessage<TResponseTypeMessage>, new()
        {
            //builds the general envelope with only the request ID
            var requestEnvelope = RequestEnvelopeBuilder.GetRequestEnvelope(_authTicket, CurrentLat, CurrentLng, CurrentAltitude, requestType);

            //awaits for the IResponseMessage
            return
                await
                    _httpClient.PostProtoPayload<TResponseTypeMessage>($"https://{_apiUrl}/rpc",
                        requestEnvelope);
        }

        public async Task DoGoogleLogin()
        {
            _authType = AuthType.Google;

            string googleRefreshToken = string.Empty;
            if (File.Exists(Directory.GetCurrentDirectory() + "\\Configs\\GoogleAuth.ini"))
            {
                googleRefreshToken = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Configs\\GoogleAuth.ini");
            }

            GoogleLogin.TokenResponseModel tokenResponse;
            if (googleRefreshToken != string.Empty)
            {
                tokenResponse = await GoogleLogin.GetAccessToken(googleRefreshToken);
                AccessToken = tokenResponse?.id_token;
            }

            if (AccessToken == null)
            {
                var deviceCode = await GoogleLogin.GetDeviceCode();
                tokenResponse = await GoogleLogin.GetAccessToken(deviceCode);
                googleRefreshToken = tokenResponse?.refresh_token;
                File.WriteAllText(Directory.GetCurrentDirectory() + "\\Configs\\GoogleAuth.ini", googleRefreshToken);
                Logger.Write("Refreshtoken " + tokenResponse?.refresh_token + " saved");
                AccessToken = tokenResponse?.id_token;
            }
        }

        public async Task DoPtcLogin(string username, string password)
        {
            AccessToken = await PtcLogin.GetAccessToken(username, password);
            _authType = AuthType.PTC;
        }

        public async Task<EncounterResponse> EncounterPokemon(ulong encounterId, string spawnPointGuid)
        {
            EncounterMessage encounterPokemonMessage = new EncounterMessage()
            {
                EncounterId = encounterId,
                SpawnPointId = spawnPointGuid,
                PlayerLatitude = CurrentLat,
                PlayerLongitude = CurrentLng,
            };

            return await AwaitableOnResponseFor<EncounterMessage, EncounterResponse>(encounterPokemonMessage, RequestType.Encounter);
        }

        public async Task<EvolvePokemonResponse> EvolvePokemon(ulong pokemonId)
        {
            EvolvePokemonMessage evolvePokemonMessage = new EvolvePokemonMessage
            {
                PokemonId = pokemonId
            };

            return await AwaitableOnResponseFor<EvolvePokemonMessage, EvolvePokemonResponse>(evolvePokemonMessage, RequestType.EvolvePokemon);
        }

		public async Task<FortDetailsResponse> GetFort(string fortId, double fortLat, double fortLng)
		{
			FortDetailsMessage fortDetailsMessage = new FortDetailsMessage
			{
				FortId = fortId,
				Latitude = fortLat,
				Longitude = fortLng
			};

			return await AwaitableOnResponseFor<FortDetailsMessage, FortDetailsResponse>(fortDetailsMessage, RequestType.FortDetails);
		}

        public async Task<GetInventoryResponse> GetInventory()
        {
            return await AwaitableOnResponseFor<GetInventoryResponse>(RequestType.GetInventory);
        }

        public async Task<DownloadItemTemplatesResponse> GetItemTemplates()
        {
            return await AwaitableOnResponseFor<DownloadItemTemplatesResponse>(RequestType.DownloadItemTemplates);
        }

        public async Task<GetMapObjectsResponse> GetMapObjects()
        {
            GetMapObjectsMessage mapObjectMessage = new GetMapObjectsMessage()
            {
                Latitude = CurrentLat,
                Longitude = CurrentLng,
            };

            mapObjectMessage.SinceTimestampMs.Add(new long[21]);
            mapObjectMessage.CellId.Add(S2Helper.GetNearbyCellIds(CurrentLng,
                            CurrentLat));

            RequestEnvelope envelope = RequestEnvelopeBuilder.GetRequestEnvelope(_authTicket, CurrentLat, CurrentLng, CurrentAltitude);

            envelope.WithMessage(new Request() { RequestMessage = mapObjectMessage.ToByteString(), RequestType = RequestType.GetMapObjects })
                .WithMessage(new Request() { RequestType = RequestType.GetHatchedEggs })
                .WithMessage(new Request() { RequestType = RequestType.GetInventory, RequestMessage = new GetInventoryMessage() { LastTimestampMs = DateTime.UtcNow.ToUnixTime() }.ToByteString() })
                .WithMessage(new Request() { RequestType = RequestType.CheckAwardedBadges })
                .WithMessage(new Request() { RequestType = RequestType.DownloadSettings, RequestMessage = new DownloadSettingsMessage() { Hash = "4a2e9bc330dae60e7b74fc85b98868ab4700802e" }.ToByteString() });
    

            return
                await _httpClient.PostProtoPayload<GetMapObjectsResponse>($"https://{_apiUrl}/rpc", envelope);
        }

        public async Task<GetPlayerResponse> GetPlayer()
        {
            var profileRequest = RequestEnvelopeBuilder.GetInitialRequestEnvelope(AccessToken, _authType, CurrentLat, CurrentLng, CurrentAltitude)
                .WithMessage(new Request() { RequestType = RequestType.GetPlayer });
            return
                await _httpClient.PostProtoPayload<GetPlayerResponse>($"https://{_apiUrl}/rpc", profileRequest);
        }

        public async Task<DownloadSettingsResponse> GetSettings()
        {
            return await AwaitableOnResponseFor<DownloadSettingsResponse>(RequestType.DownloadSettings);
        }

        public async Task<RecycleInventoryItemResponse> RecycleItem(ItemId itemId, int amount)
        {
            RecycleInventoryItemMessage recycleObjectMessage = new RecycleInventoryItemMessage
            {
                ItemId = itemId,
                Count = amount
            };

            return await AwaitableOnResponseFor<RecycleInventoryItemMessage, RecycleInventoryItemResponse>(recycleObjectMessage, RequestType.RecycleInventoryItem);
        }

        public void SaveLatLng(double lat, double lng)
        {
            var latlng = lat + ":" + lng;
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Configs");
            File.WriteAllText(Directory.GetCurrentDirectory() + "\\Configs\\Coords.ini", latlng);
        }

        public async Task<FortSearchResponse> SearchFort(string fortId, double fortLat, double fortLng)
        {
            FortSearchMessage fortSearchMessage = new FortSearchMessage()
            {	
                FortId = fortId,
                FortLatitude = fortLat,
                FortLongitude = fortLng,
                PlayerLatitude = CurrentLat,
                PlayerLongitude = CurrentLng
            };

            return await AwaitableOnResponseFor<FortSearchMessage, FortSearchResponse>(fortSearchMessage, RequestType.FortSearch);
        }

        /// <summary>
        ///     For GUI clients only. GUI clients don't use the DoGoogleLogin, but call the GoogleLogin class directly
        /// </summary>
        /// <param name="type"></param>
        public void SetAuthType(AuthType type)
        {
            _authType = type;
        }

        private void CalcNoisedCoordinates(double lat, double lng, out double latNoise, out double lngNoise)
        {
            double mean = 0.0;// just for fun
            double stdDev = 2.09513120352; //-> so 50% of the noised coordinates will have a maximal distance of 4 m to orginal ones

            if (rand == null)
            {
                rand = new Random();
            }
            double u1 = rand.NextDouble();
            double u2 = rand.NextDouble();
            double u3 = rand.NextDouble();
            double u4 = rand.NextDouble();

            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            double randNormal = mean + stdDev * randStdNormal;
            double randStdNormal2 = Math.Sqrt(-2.0 * Math.Log(u3)) * Math.Sin(2.0 * Math.PI * u4);
            double randNormal2 = mean + stdDev * randStdNormal2;

            latNoise = lat + randNormal / 100000.0;
            lngNoise = lng + randNormal2 / 100000.0;
        }

        private void SetCoordinates(double lat, double lng, double altitude)
        {
            if (double.IsNaN(lat) || double.IsNaN(lng)) return;

            double latNoised = 0.0;
            double lngNoised = 0.0;
            CalcNoisedCoordinates(lat, lng, out latNoised, out lngNoised);
            CurrentLat = latNoised;
            CurrentLng = lngNoised;
            CurrentAltitude = altitude;
            SaveLatLng(lat, lng);
        }


        public async Task SetServer()
        {
            var serverRequest = RequestEnvelopeBuilder.GetInitialRequestEnvelope(AccessToken, _authType, CurrentLat, CurrentLng,
                CurrentAltitude,
                RequestType.GetPlayerProfile, RequestType.GetHatchedEggs, RequestType.GetInventory,
                RequestType.CheckAwardedBadges, RequestType.DownloadSettings);

            var serverResponse = await _httpClient.PostProto(Resources.RpcUrl, serverRequest);

            if (serverResponse.AuthTicket == null)
                throw new AccessTokenExpiredException(serverResponse.Error);

            _authTicket = serverResponse.AuthTicket;

            _apiUrl = serverResponse.ApiUrl;
        }

        public async Task<ReleasePokemonResponse> TransferPokemon(ulong pokemonId)
        {
            ReleasePokemonMessage releasePokemonMessage = new ReleasePokemonMessage()
            {
                PokemonId = pokemonId
            };

            return await AwaitableOnResponseFor<ReleasePokemonMessage, ReleasePokemonResponse>(releasePokemonMessage, RequestType.ReleasePokemon);
        }

        public async Task<PlayerUpdateResponse> UpdatePlayerLocation(double lat, double lng, double alt)
        {
            SetCoordinates(lat, lng, alt);
            PlayerUpdateMessage playerUpdateMessage = new PlayerUpdateMessage()
            {
                Latitude = CurrentLat,
                Longitude = CurrentLng
            };

            return await AwaitableOnResponseFor<PlayerUpdateMessage, PlayerUpdateResponse>(playerUpdateMessage, RequestType.PlayerUpdate);
        }

        public async Task<UseItemCaptureResponse> UseCaptureItem(ulong encounterId, ItemId itemId, string spawnPointGuid)
        {
            UseItemCaptureMessage useItemCaptureMessage = new UseItemCaptureMessage()
            {
                EncounterId = encounterId,
                ItemId = itemId,
                SpawnPointGuid = spawnPointGuid
            };

            return await AwaitableOnResponseFor<UseItemCaptureMessage, UseItemCaptureResponse>(useItemCaptureMessage, RequestType.UseItemCapture);
        }

        public async Task<UseItemXpBoostResponse> UseItemXpBoost(ItemId itemId) //changed from UseItem to UseItemXpBoost because of the RequestType
        {
            UseItemXpBoostMessage useXpBoostMessage = new UseItemXpBoostMessage()
            {
                ItemId = itemId,
            };

            return await AwaitableOnResponseFor<UseItemXpBoostMessage, UseItemXpBoostResponse>(useXpBoostMessage, RequestType.UseItemXpBoost);
        }
    }
}
