#region

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Protobuf;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.Exceptions;
using PokemonGo.RocketAPI.Extensions;
using PokemonGo.RocketAPI.GeneratedCode;
using PokemonGo.RocketAPI.Helpers;
using PokemonGo.RocketAPI.Login;
using static PokemonGo.RocketAPI.GeneratedCode.Response.Types;

#endregion

namespace PokemonGo.RocketAPI
{
    public class Client
    {
        private readonly HttpClient _httpClient;
        private string _apiUrl;
        private AuthType _authType = AuthType.Google;
        private Request.Types.UnknownAuth _unknownAuth;
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
            double pokemonLng, MiscEnums.Item pokeball)
        {
            var customRequest = new Request.Types.CatchPokemonRequest
            {
                EncounterId = encounterId,
                Pokeball = (int) pokeball,
                SpawnPointGuid = spawnPointGuid,
                HitPokemon = 1,
                NormalizedReticleSize = Utils.FloatAsUlong(1.950),
                SpinModifier = Utils.FloatAsUlong(1),
                NormalizedHitPosition = Utils.FloatAsUlong(1)
            };

            var catchPokemonRequest = RequestBuilder.GetRequest(_unknownAuth, CurrentLat, CurrentLng, CurrentAltitude,
                new Request.Types.Requests
                {
                    Type = (int) RequestType.CATCH_POKEMON,
                    Message = customRequest.ToByteString()
                });
            return
                await
                    _httpClient.PostProtoPayload<Request, CatchPokemonResponse>($"https://{_apiUrl}/rpc",
                        catchPokemonRequest);
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
            _authType = AuthType.Ptc;
        }

        public async Task<EncounterResponse> EncounterPokemon(ulong encounterId, string spawnPointGuid)
        {
            var customRequest = new Request.Types.EncounterRequest
            {
                EncounterId = encounterId,
                SpawnpointId = spawnPointGuid,
                PlayerLatDegrees = Utils.FloatAsUlong(CurrentLat),
                PlayerLngDegrees = Utils.FloatAsUlong(CurrentLng)
            };

            var encounterResponse = RequestBuilder.GetRequest(_unknownAuth, CurrentLat, CurrentLng, CurrentAltitude,
                new Request.Types.Requests
                {
                    Type = (int) RequestType.ENCOUNTER,
                    Message = customRequest.ToByteString()
                });
            return
                await
                    _httpClient.PostProtoPayload<Request, EncounterResponse>($"https://{_apiUrl}/rpc", encounterResponse);
        }

        public async Task<EvolvePokemonOut> EvolvePokemon(ulong pokemonId)
        {
            var customRequest = new EvolvePokemon
            {
                PokemonId = pokemonId
            };

            var releasePokemonRequest = RequestBuilder.GetRequest(_unknownAuth, CurrentLat, CurrentLng, CurrentAltitude,
                new Request.Types.Requests
                {
                    Type = (int) RequestType.EVOLVE_POKEMON,
                    Message = customRequest.ToByteString()
                });
            return
                await
                    _httpClient.PostProtoPayload<Request, EvolvePokemonOut>($"https://{_apiUrl}/rpc",
                        releasePokemonRequest);
        }

        public async Task<FortDetailsResponse> GetFort(string fortId, double fortLat, double fortLng)
        {
            var customRequest = new Request.Types.FortDetailsRequest
            {
                Id = ByteString.CopyFromUtf8(fortId),
                Latitude = Utils.FloatAsUlong(fortLat),
                Longitude = Utils.FloatAsUlong(fortLng)
            };

            var fortDetailRequest = RequestBuilder.GetRequest(_unknownAuth, CurrentLat, CurrentLng, CurrentAltitude,
                new Request.Types.Requests
                {
                    Type = (int) RequestType.FORT_DETAILS,
                    Message = customRequest.ToByteString()
                });
            return
                await
                    _httpClient.PostProtoPayload<Request, FortDetailsResponse>($"https://{_apiUrl}/rpc",
                        fortDetailRequest);
        }

        public async Task<GetInventoryResponse> GetInventory()
        {
            var inventoryRequest = RequestBuilder.GetRequest(_unknownAuth, CurrentLat, CurrentLng, CurrentAltitude,
                RequestType.GET_INVENTORY);
            return
                await
                    _httpClient.PostProtoPayload<Request, GetInventoryResponse>($"https://{_apiUrl}/rpc",
                        inventoryRequest);
        }

        public async Task<DownloadItemTemplatesResponse> GetItemTemplates()
        {
            var settingsRequest = RequestBuilder.GetRequest(_unknownAuth, CurrentLat, CurrentLng, CurrentAltitude,
                RequestType.DOWNLOAD_ITEM_TEMPLATES);
            return
                await
                    _httpClient.PostProtoPayload<Request, DownloadItemTemplatesResponse>($"https://{_apiUrl}/rpc",
                        settingsRequest);
        }


        public async Task<GetMapObjectsResponse> GetMapObjects()
        {
            var customRequest = new Request.Types.MapObjectsRequest
            {
                CellIds =
                    ByteString.CopyFrom(
                        ProtoHelper.EncodeUlongList(S2Helper.GetNearbyCellIds(CurrentLng,
                            CurrentLat))),
                Latitude = Utils.FloatAsUlong(CurrentLat),
                Longitude = Utils.FloatAsUlong(CurrentLng),
                Unknown14 = ByteString.CopyFromUtf8("\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0")
            };

            var mapRequest = RequestBuilder.GetRequest(_unknownAuth, CurrentLat, CurrentLng, CurrentAltitude,
                new Request.Types.Requests
                {
                    Type = (int) RequestType.GET_MAP_OBJECTS,
                    Message = customRequest.ToByteString()
                },
                new Request.Types.Requests {Type = (int) RequestType.GET_HATCHED_OBJECTS},
                new Request.Types.Requests
                {
                    Type = (int) RequestType.GET_INVENTORY,
                    Message = new Request.Types.Time {Time_ = DateTime.UtcNow.ToUnixTime()}.ToByteString()
                },
                new Request.Types.Requests {Type = (int) RequestType.CHECK_AWARDED_BADGES},
                new Request.Types.Requests
                {
                    Type = (int) RequestType.DOWNLOAD_SETTINGS,
                    Message =
                        new Request.Types.SettingsGuid
                        {
                            Guid = ByteString.CopyFromUtf8("4a2e9bc330dae60e7b74fc85b98868ab4700802e")
                        }.ToByteString()
                });

            return
                await _httpClient.PostProtoPayload<Request, GetMapObjectsResponse>($"https://{_apiUrl}/rpc", mapRequest);
        }

        public async Task<GetPlayerResponse> GetProfile()
        {
            var profileRequest = RequestBuilder.GetInitialRequest(AccessToken, _authType, CurrentLat, CurrentLng,
                CurrentAltitude,
                new Request.Types.Requests {Type = (int) RequestType.GET_PLAYER});
            return
                await _httpClient.PostProtoPayload<Request, GetPlayerResponse>($"https://{_apiUrl}/rpc", profileRequest);
        }

        public async Task<DownloadSettingsResponse> GetSettings()
        {
            var settingsRequest = RequestBuilder.GetRequest(_unknownAuth, CurrentLat, CurrentLng, CurrentAltitude,
                RequestType.DOWNLOAD_SETTINGS);
            return
                await
                    _httpClient.PostProtoPayload<Request, DownloadSettingsResponse>($"https://{_apiUrl}/rpc",
                        settingsRequest);
        }

        public async Task<RecycleInventoryItemResponse> RecycleItem(ItemId itemId, int amount)
        {
            var customRequest = new RecycleInventoryItem
            {
                ItemId = (ItemId) Enum.Parse(typeof(ItemId), itemId.ToString()),
                Count = amount
            };

            var releasePokemonRequest = RequestBuilder.GetRequest(_unknownAuth, CurrentLat, CurrentLng, CurrentAltitude,
                new Request.Types.Requests
                {
                    Type = (int) RequestType.RECYCLE_INVENTORY_ITEM,
                    Message = customRequest.ToByteString()
                });
            return
                await
                    _httpClient.PostProtoPayload<Request, RecycleInventoryItemResponse>($"https://{_apiUrl}/rpc",
                        releasePokemonRequest);
        }


        public void SaveLatLng(double lat, double lng)
        {
            var latlng = lat + ":" + lng;
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Configs");
            File.WriteAllText(Directory.GetCurrentDirectory() + "\\Configs\\Coords.ini", latlng);
        }

        public async Task<FortSearchResponse> SearchFort(string fortId, double fortLat, double fortLng)
        {
            var customRequest = new Request.Types.FortSearchRequest
            {
                Id = ByteString.CopyFromUtf8(fortId),
                FortLatDegrees = Utils.FloatAsUlong(fortLat),
                FortLngDegrees = Utils.FloatAsUlong(fortLng),
                PlayerLatDegrees = Utils.FloatAsUlong(CurrentLat),
                PlayerLngDegrees = Utils.FloatAsUlong(CurrentLng)
            };

            var fortDetailRequest = RequestBuilder.GetRequest(_unknownAuth, CurrentLat, CurrentLng, CurrentAltitude,
                new Request.Types.Requests
                {
                    Type = (int) RequestType.FORT_SEARCH,
                    Message = customRequest.ToByteString()
                });
            return
                await
                    _httpClient.PostProtoPayload<Request, FortSearchResponse>($"https://{_apiUrl}/rpc",
                        fortDetailRequest);
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
            var serverRequest = RequestBuilder.GetInitialRequest(AccessToken, _authType, CurrentLat, CurrentLng,
                CurrentAltitude,
                RequestType.GET_PLAYER, RequestType.GET_HATCHED_OBJECTS, RequestType.GET_INVENTORY,
                RequestType.CHECK_AWARDED_BADGES, RequestType.DOWNLOAD_SETTINGS);
            var serverResponse = await _httpClient.PostProto(Resources.RpcUrl, serverRequest);

            if (serverResponse.Auth == null)
                throw new AccessTokenExpiredException();

            _unknownAuth = new Request.Types.UnknownAuth
            {
                Unknown71 = serverResponse.Auth.Unknown71,
                Timestamp = serverResponse.Auth.Timestamp,
                Unknown73 = serverResponse.Auth.Unknown73
            };

            _apiUrl = serverResponse.ApiUrl;
        }

        public async Task<TransferPokemonOut> TransferPokemon(ulong pokemonId)
        {
            var customRequest = new TransferPokemon
            {
                PokemonId = pokemonId
            };

            var releasePokemonRequest = RequestBuilder.GetRequest(_unknownAuth, CurrentLat, CurrentLng, CurrentAltitude,
                new Request.Types.Requests
                {
                    Type = (int) RequestType.RELEASE_POKEMON,
                    Message = customRequest.ToByteString()
                });
            return
                await
                    _httpClient.PostProtoPayload<Request, TransferPokemonOut>($"https://{_apiUrl}/rpc",
                        releasePokemonRequest);
        }

        public async Task<PlayerUpdateResponse> UpdatePlayerLocation(double lat, double lng, double alt)
        {
            SetCoordinates(lat, lng, alt);
            var customRequest = new Request.Types.PlayerUpdateProto
            {
                Lat = Utils.FloatAsUlong(CurrentLat),
                Lng = Utils.FloatAsUlong(CurrentLng)
            };

            var updateRequest = RequestBuilder.GetRequest(_unknownAuth, CurrentLat, CurrentLng, CurrentAltitude,
                new Request.Types.Requests
                {
                    Type = (int) RequestType.PLAYER_UPDATE,
                    Message = customRequest.ToByteString()
                });
            var updateResponse =
                await
                    _httpClient.PostProtoPayload<Request, PlayerUpdateResponse>($"https://{_apiUrl}/rpc", updateRequest);
            return updateResponse;
        }

        public async Task<UseItemCaptureRequest> UseCaptureItem(ulong encounterId, ItemId itemId, string spawnPointGuid)
        {
            var customRequest = new UseItemCaptureRequest
            {
                EncounterId = encounterId,
                ItemId = itemId,
                SpawnPointGuid = spawnPointGuid
            };

            var useItemRequest = RequestBuilder.GetRequest(_unknownAuth, CurrentLat, CurrentLng, CurrentAltitude,
                new Request.Types.Requests
                {
                    Type = (int) RequestType.USE_ITEM_CAPTURE,
                    Message = customRequest.ToByteString()
                });
            return
                await
                    _httpClient.PostProtoPayload<Request, UseItemCaptureRequest>($"https://{_apiUrl}/rpc",
                        useItemRequest);
        }

        public async Task<UseItemRequest> UseItemXpBoost(ItemId itemId) //changed from UseItem to UseItemXpBoost because of the RequestType
        {
            var customRequest = new UseItemRequest
            {
                ItemId = itemId,
            };

            var useItemRequest = RequestBuilder.GetRequest(_unknownAuth, CurrentLat, CurrentLng, CurrentAltitude,
                new Request.Types.Requests
                {
                    Type = (int)RequestType.USE_ITEM_XP_BOOST,
                    Message = customRequest.ToByteString()
                });
            return
                await
                    _httpClient.PostProtoPayload<Request, UseItemRequest>($"https://{_apiUrl}/rpc",
                        useItemRequest);
        }

        public async Task<UseItemRequest> UseItemIncense(ItemId itemId)
        {
            var customRequest = new UseItemRequest
            {
                ItemId = itemId,
            };

            var useItemRequest = RequestBuilder.GetRequest(_unknownAuth, CurrentLat, CurrentLng, CurrentAltitude,
                new Request.Types.Requests
                {
                    Type = (int)RequestType.USE_INCENSE,    //this should work for any incense they add as long as you pass the right itemId
                    Message = customRequest.ToByteString()
                });
            return
                await
                    _httpClient.PostProtoPayload<Request, UseItemRequest>($"https://{_apiUrl}/rpc",
                        useItemRequest);
        }

        public async Task<GetIncensePokemonRequest> getIncensedPokemon(ulong Lat, ulong Long)
        {
            var customRequest = new GetIncensePokemonRequest
            {
                player_latitude = Lat,
                player_longitude = Long
            };
            var sendRequest = RequestBuilder.GetRequest(_unknownAuth, CurrentLat, CurrentLng, CurrentAltitude,
                new Request.Types.Requests
                {
                    Type = (int)RequestType.GET_INCENSE_POKEMON,
                    Message = customRequest.ToByteString()
                });
            return
                await
                _httpClient.PostProtoPayload<Request, GetIncensePokemonRequest>($"https://{_apiUrl}/rpc",
                sendRequest);

        }
    }
}
