#pragma warning disable 1591, 0612, 3021

#region Designer generated code

#region

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;

#endregion

namespace PokemonGo.RocketAPI.GeneratedCode
{
    /// <summary>Holder for reflection information generated from Request.proto</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    public static partial class RequestReflection
    {
        #region Descriptor

        /// <summary>File descriptor for Request.proto</summary>
        public static pbr::FileDescriptor Descriptor
        {
            get { return descriptor; }
        }

        private static pbr::FileDescriptor descriptor;

        static RequestReflection()
        {
            var descriptorData = global::System.Convert.FromBase64String(
                string.Concat(
                    "Cg1SZXF1ZXN0LnByb3RvEiFQb2tlbW9uR28uUm9ja2V0QVBJLkdlbmVyYXRl",
                    "ZENvZGUihAwKB1JlcXVlc3QSEAoIdW5rbm93bjEYASABKAUSDgoGcnBjX2lk",
                    "GAMgASgDEkUKCHJlcXVlc3RzGAQgAygLMjMuUG9rZW1vbkdvLlJvY2tldEFQ",
                    "SS5HZW5lcmF0ZWRDb2RlLlJlcXVlc3QuUmVxdWVzdHMSRQoIdW5rbm93bjYY",
                    "BiABKAsyMy5Qb2tlbW9uR28uUm9ja2V0QVBJLkdlbmVyYXRlZENvZGUuUmVx",
                    "dWVzdC5Vbmtub3duNhIQCghsYXRpdHVkZRgHIAEoBhIRCglsb25naXR1ZGUY",
                    "CCABKAYSEAoIYWx0aXR1ZGUYCSABKAYSQQoEYXV0aBgKIAEoCzIzLlBva2Vt",
                    "b25Hby5Sb2NrZXRBUEkuR2VuZXJhdGVkQ29kZS5SZXF1ZXN0LkF1dGhJbmZv",
                    "EksKC3Vua25vd25hdXRoGAsgASgLMjYuUG9rZW1vbkdvLlJvY2tldEFQSS5H",
                    "ZW5lcmF0ZWRDb2RlLlJlcXVlc3QuVW5rbm93bkF1dGgSEQoJdW5rbm93bjEy",
                    "GAwgASgDGkYKC1Vua25vd25BdXRoEhEKCXVua25vd243MRgBIAEoDBIRCgl0",
                    "aW1lc3RhbXAYAiABKAMSEQoJdW5rbm93bjczGAMgASgMGikKCFJlcXVlc3Rz",
                    "EgwKBHR5cGUYASABKAUSDwoHbWVzc2FnZRgCIAEoDBocCghVbmtub3duMxIQ",
                    "Cgh1bmtub3duNBgBIAEoCRqKAQoIVW5rbm93bjYSEAoIdW5rbm93bjEYASAB",
                    "KAUSTgoIdW5rbm93bjIYAiABKAsyPC5Qb2tlbW9uR28uUm9ja2V0QVBJLkdl",
                    "bmVyYXRlZENvZGUuUmVxdWVzdC5Vbmtub3duNi5Vbmtub3duMhocCghVbmtu",
                    "b3duMhIQCgh1bmtub3duMRgBIAEoDBqQAQoIQXV0aEluZm8SEAoIcHJvdmlk",
                    "ZXIYASABKAkSRgoFdG9rZW4YAiABKAsyNy5Qb2tlbW9uR28uUm9ja2V0QVBJ",
                    "LkdlbmVyYXRlZENvZGUuUmVxdWVzdC5BdXRoSW5mby5KV1QaKgoDSldUEhAK",
                    "CGNvbnRlbnRzGAEgASgJEhEKCXVua25vd24xMxgCIAEoBRotChFQbGF5ZXJV",
                    "cGRhdGVQcm90bxILCgNMYXQYASABKAYSCwoDTG5nGAIgASgGGlwKEU1hcE9i",
                    "amVjdHNSZXF1ZXN0Eg8KB2NlbGxJZHMYASABKAwSEQoJdW5rbm93bjE0GAIg",
                    "ASgMEhAKCGxhdGl0dWRlGAMgASgGEhEKCWxvbmdpdHVkZRgEIAEoBhqDAQoR",
                    "Rm9ydFNlYXJjaFJlcXVlc3QSCgoCSWQYASABKAwSGAoQUGxheWVyTGF0RGVn",
                    "cmVlcxgCIAEoBhIYChBQbGF5ZXJMbmdEZWdyZWVzGAMgASgGEhYKDkZvcnRM",
                    "YXREZWdyZWVzGAQgASgGEhYKDkZvcnRMbmdEZWdyZWVzGAUgASgGGkUKEkZv",
                    "cnREZXRhaWxzUmVxdWVzdBIKCgJJZBgBIAEoDBIQCghMYXRpdHVkZRgCIAEo",
                    "BhIRCglMb25naXR1ZGUYAyABKAYacQoQRW5jb3VudGVyUmVxdWVzdBITCgtF",
                    "bmNvdW50ZXJJZBgBIAEoBhIUCgxTcGF3bnBvaW50SWQYAiABKAkSGAoQUGxh",
                    "eWVyTGF0RGVncmVlcxgDIAEoBhIYChBQbGF5ZXJMbmdEZWdyZWVzGAQgASgG",
                    "GrwBChNDYXRjaFBva2Vtb25SZXF1ZXN0EhMKC0VuY291bnRlcklkGAEgASgG",
                    "EhAKCFBva2ViYWxsGAIgASgFEh0KFU5vcm1hbGl6ZWRSZXRpY2xlU2l6ZRgD",
                    "IAEoBhIWCg5TcGF3blBvaW50R3VpZBgEIAEoCRISCgpIaXRQb2tlbW9uGAUg",
                    "ASgFEhQKDFNwaW5Nb2RpZmllchgGIAEoBhIdChVOb3JtYWxpemVkSGl0UG9z",
                    "aXRpb24YByABKAYaHAoMU2V0dGluZ3NHdWlkEgwKBGd1aWQYASABKAwaFAoE",
                    "VGltZRIMCgR0aW1lGAEgASgDYgZwcm90bzM="));
            descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
                new pbr::FileDescriptor[] {},
                new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[]
                {
                    new pbr::GeneratedClrTypeInfo(typeof(global::PokemonGo.RocketAPI.GeneratedCode.Request),
                        global::PokemonGo.RocketAPI.GeneratedCode.Request.Parser,
                        new[]
                        {
                            "Unknown1", "RpcId", "Requests", "Unknown6", "Latitude", "Longitude", "Altitude", "Auth",
                            "Unknownauth", "Unknown12"
                        }, null, null, new pbr::GeneratedClrTypeInfo[]
                        {
                            new pbr::GeneratedClrTypeInfo(
                                typeof(global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.UnknownAuth),
                                global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.UnknownAuth.Parser,
                                new[] {"Unknown71", "Timestamp", "Unknown73"}, null, null, null),
                            new pbr::GeneratedClrTypeInfo(
                                typeof(global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Requests),
                                global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Requests.Parser,
                                new[] {"Type", "Message"}, null, null, null),
                            new pbr::GeneratedClrTypeInfo(
                                typeof(global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Unknown3),
                                global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Unknown3.Parser,
                                new[] {"Unknown4"}, null, null, null),
                            new pbr::GeneratedClrTypeInfo(
                                typeof(global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Unknown6),
                                global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Unknown6.Parser,
                                new[] {"Unknown1", "Unknown2"}, null, null,
                                new pbr::GeneratedClrTypeInfo[]
                                {
                                    new pbr::GeneratedClrTypeInfo(
                                        typeof(
                                            global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Unknown6.Types.
                                                Unknown2),
                                        global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Unknown6.Types
                                            .Unknown2.Parser, new[] {"Unknown1"}, null, null, null)
                                }),
                            new pbr::GeneratedClrTypeInfo(
                                typeof(global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.AuthInfo),
                                global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.AuthInfo.Parser,
                                new[] {"Provider", "Token"}, null, null,
                                new pbr::GeneratedClrTypeInfo[]
                                {
                                    new pbr::GeneratedClrTypeInfo(
                                        typeof(
                                            global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.AuthInfo.Types.
                                                JWT),
                                        global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.AuthInfo.Types.JWT
                                            .Parser, new[] {"Contents", "Unknown13"}, null, null, null)
                                }),
                            new pbr::GeneratedClrTypeInfo(
                                typeof(global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.PlayerUpdateProto),
                                global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.PlayerUpdateProto.Parser,
                                new[] {"Lat", "Lng"}, null, null, null),
                            new pbr::GeneratedClrTypeInfo(
                                typeof(global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.MapObjectsRequest),
                                global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.MapObjectsRequest.Parser,
                                new[] {"CellIds", "Unknown14", "Latitude", "Longitude"}, null, null, null),
                            new pbr::GeneratedClrTypeInfo(
                                typeof(global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.FortSearchRequest),
                                global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.FortSearchRequest.Parser,
                                new[]
                                {"Id", "PlayerLatDegrees", "PlayerLngDegrees", "FortLatDegrees", "FortLngDegrees"},
                                null, null, null),
                            new pbr::GeneratedClrTypeInfo(
                                typeof(global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.FortDetailsRequest),
                                global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.FortDetailsRequest.Parser,
                                new[] {"Id", "Latitude", "Longitude"}, null, null, null),
                            new pbr::GeneratedClrTypeInfo(
                                typeof(global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.EncounterRequest),
                                global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.EncounterRequest.Parser,
                                new[] {"EncounterId", "SpawnpointId", "PlayerLatDegrees", "PlayerLngDegrees"}, null,
                                null, null),
                            new pbr::GeneratedClrTypeInfo(
                                typeof(global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.CatchPokemonRequest),
                                global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.CatchPokemonRequest.Parser,
                                new[]
                                {
                                    "EncounterId", "Pokeball", "NormalizedReticleSize", "SpawnPointGuid", "HitPokemon",
                                    "SpinModifier", "NormalizedHitPosition"
                                }, null, null, null),
                            new pbr::GeneratedClrTypeInfo(
                                typeof(global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.SettingsGuid),
                                global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.SettingsGuid.Parser,
                                new[] {"Guid"}, null, null, null),
                            new pbr::GeneratedClrTypeInfo(
                                typeof(global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Time),
                                global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Time.Parser, new[] {"Time_"},
                                null, null, null)
                        })
                }));
        }

        #endregion
    }

    #region Messages

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    public sealed partial class Request : pb::IMessage<Request>
    {
        /// <summary>Field number for the "unknown1" field.</summary>
        public const int Unknown1FieldNumber = 1;

        /// <summary>Field number for the "rpc_id" field.</summary>
        public const int RpcIdFieldNumber = 3;

        /// <summary>Field number for the "requests" field.</summary>
        public const int RequestsFieldNumber = 4;

        /// <summary>Field number for the "unknown6" field.</summary>
        public const int Unknown6FieldNumber = 6;

        /// <summary>Field number for the "latitude" field.</summary>
        public const int LatitudeFieldNumber = 7;

        /// <summary>Field number for the "longitude" field.</summary>
        public const int LongitudeFieldNumber = 8;

        /// <summary>Field number for the "altitude" field.</summary>
        public const int AltitudeFieldNumber = 9;

        /// <summary>Field number for the "auth" field.</summary>
        public const int AuthFieldNumber = 10;

        /// <summary>Field number for the "unknownauth" field.</summary>
        public const int UnknownauthFieldNumber = 11;

        /// <summary>Field number for the "unknown12" field.</summary>
        public const int Unknown12FieldNumber = 12;

        private static readonly pb::MessageParser<Request> _parser = new pb::MessageParser<Request>(() => new Request());

        private static readonly pb::FieldCodec<global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Requests>
            _repeated_requests_codec
                = pb::FieldCodec.ForMessage(34, global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Requests.Parser);

        private readonly pbc::RepeatedField<global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Requests> requests_
            = new pbc::RepeatedField<global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Requests>();

        private ulong altitude_;
        private global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.AuthInfo auth_;
        private ulong latitude_;
        private ulong longitude_;
        private long rpcId_;
        private int unknown1_;
        private long unknown12_;
        private global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Unknown6 unknown6_;
        private global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.UnknownAuth unknownauth_;

        public Request()
        {
            OnConstruction();
        }

        public Request(Request other) : this()
        {
            unknown1_ = other.unknown1_;
            rpcId_ = other.rpcId_;
            requests_ = other.requests_.Clone();
            Unknown6 = other.unknown6_ != null ? other.Unknown6.Clone() : null;
            latitude_ = other.latitude_;
            longitude_ = other.longitude_;
            altitude_ = other.altitude_;
            Auth = other.auth_ != null ? other.Auth.Clone() : null;
            Unknownauth = other.unknownauth_ != null ? other.Unknownauth.Clone() : null;
            unknown12_ = other.unknown12_;
        }

        public static pb::MessageParser<Request> Parser
        {
            get { return _parser; }
        }

        public static pbr::MessageDescriptor Descriptor
        {
            get { return global::PokemonGo.RocketAPI.GeneratedCode.RequestReflection.Descriptor.MessageTypes[0]; }
        }

        public int Unknown1
        {
            get { return unknown1_; }
            set { unknown1_ = value; }
        }

        public long RpcId
        {
            get { return rpcId_; }
            set { rpcId_ = value; }
        }

        public pbc::RepeatedField<global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Requests> Requests
        {
            get { return requests_; }
        }

        public global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Unknown6 Unknown6
        {
            get { return unknown6_; }
            set { unknown6_ = value; }
        }

        public ulong Latitude
        {
            get { return latitude_; }
            set { latitude_ = value; }
        }

        public ulong Longitude
        {
            get { return longitude_; }
            set { longitude_ = value; }
        }

        public ulong Altitude
        {
            get { return altitude_; }
            set { altitude_ = value; }
        }

        public global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.AuthInfo Auth
        {
            get { return auth_; }
            set { auth_ = value; }
        }

        public global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.UnknownAuth Unknownauth
        {
            get { return unknownauth_; }
            set { unknownauth_ = value; }
        }

        public long Unknown12
        {
            get { return unknown12_; }
            set { unknown12_ = value; }
        }

        pbr::MessageDescriptor pb::IMessage.Descriptor
        {
            get { return Descriptor; }
        }

        public Request Clone()
        {
            return new Request(this);
        }

        public bool Equals(Request other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            if (ReferenceEquals(other, this))
            {
                return true;
            }
            if (Unknown1 != other.Unknown1) return false;
            if (RpcId != other.RpcId) return false;
            if (!requests_.Equals(other.requests_)) return false;
            if (!Equals(Unknown6, other.Unknown6)) return false;
            if (Latitude != other.Latitude) return false;
            if (Longitude != other.Longitude) return false;
            if (Altitude != other.Altitude) return false;
            if (!Equals(Auth, other.Auth)) return false;
            if (!Equals(Unknownauth, other.Unknownauth)) return false;
            if (Unknown12 != other.Unknown12) return false;
            return true;
        }

        public void WriteTo(pb::CodedOutputStream output)
        {
            if (Unknown1 != 0)
            {
                output.WriteRawTag(8);
                output.WriteInt32(Unknown1);
            }
            if (RpcId != 0L)
            {
                output.WriteRawTag(24);
                output.WriteInt64(RpcId);
            }
            requests_.WriteTo(output, _repeated_requests_codec);
            if (unknown6_ != null)
            {
                output.WriteRawTag(50);
                output.WriteMessage(Unknown6);
            }
            if (Latitude != 0UL)
            {
                output.WriteRawTag(57);
                output.WriteFixed64(Latitude);
            }
            if (Longitude != 0UL)
            {
                output.WriteRawTag(65);
                output.WriteFixed64(Longitude);
            }
            if (Altitude != 0UL)
            {
                output.WriteRawTag(73);
                output.WriteFixed64(Altitude);
            }
            if (auth_ != null)
            {
                output.WriteRawTag(82);
                output.WriteMessage(Auth);
            }
            if (unknownauth_ != null)
            {
                output.WriteRawTag(90);
                output.WriteMessage(Unknownauth);
            }
            if (Unknown12 != 0L)
            {
                output.WriteRawTag(96);
                output.WriteInt64(Unknown12);
            }
        }

        public int CalculateSize()
        {
            var size = 0;
            if (Unknown1 != 0)
            {
                size += 1 + pb::CodedOutputStream.ComputeInt32Size(Unknown1);
            }
            if (RpcId != 0L)
            {
                size += 1 + pb::CodedOutputStream.ComputeInt64Size(RpcId);
            }
            size += requests_.CalculateSize(_repeated_requests_codec);
            if (unknown6_ != null)
            {
                size += 1 + pb::CodedOutputStream.ComputeMessageSize(Unknown6);
            }
            if (Latitude != 0UL)
            {
                size += 1 + 8;
            }
            if (Longitude != 0UL)
            {
                size += 1 + 8;
            }
            if (Altitude != 0UL)
            {
                size += 1 + 8;
            }
            if (auth_ != null)
            {
                size += 1 + pb::CodedOutputStream.ComputeMessageSize(Auth);
            }
            if (unknownauth_ != null)
            {
                size += 1 + pb::CodedOutputStream.ComputeMessageSize(Unknownauth);
            }
            if (Unknown12 != 0L)
            {
                size += 1 + pb::CodedOutputStream.ComputeInt64Size(Unknown12);
            }
            return size;
        }

        public void MergeFrom(Request other)
        {
            if (other == null)
            {
                return;
            }
            if (other.Unknown1 != 0)
            {
                Unknown1 = other.Unknown1;
            }
            if (other.RpcId != 0L)
            {
                RpcId = other.RpcId;
            }
            requests_.Add(other.requests_);
            if (other.unknown6_ != null)
            {
                if (unknown6_ == null)
                {
                    unknown6_ = new global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Unknown6();
                }
                Unknown6.MergeFrom(other.Unknown6);
            }
            if (other.Latitude != 0UL)
            {
                Latitude = other.Latitude;
            }
            if (other.Longitude != 0UL)
            {
                Longitude = other.Longitude;
            }
            if (other.Altitude != 0UL)
            {
                Altitude = other.Altitude;
            }
            if (other.auth_ != null)
            {
                if (auth_ == null)
                {
                    auth_ = new global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.AuthInfo();
                }
                Auth.MergeFrom(other.Auth);
            }
            if (other.unknownauth_ != null)
            {
                if (unknownauth_ == null)
                {
                    unknownauth_ = new global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.UnknownAuth();
                }
                Unknownauth.MergeFrom(other.Unknownauth);
            }
            if (other.Unknown12 != 0L)
            {
                Unknown12 = other.Unknown12;
            }
        }

        public void MergeFrom(pb::CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                switch (tag)
                {
                    default:
                        input.SkipLastField();
                        break;
                    case 8:
                    {
                        Unknown1 = input.ReadInt32();
                        break;
                    }
                    case 24:
                    {
                        RpcId = input.ReadInt64();
                        break;
                    }
                    case 34:
                    {
                        requests_.AddEntriesFrom(input, _repeated_requests_codec);
                        break;
                    }
                    case 50:
                    {
                        if (unknown6_ == null)
                        {
                            unknown6_ = new global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Unknown6();
                        }
                        input.ReadMessage(unknown6_);
                        break;
                    }
                    case 57:
                    {
                        Latitude = input.ReadFixed64();
                        break;
                    }
                    case 65:
                    {
                        Longitude = input.ReadFixed64();
                        break;
                    }
                    case 73:
                    {
                        Altitude = input.ReadFixed64();
                        break;
                    }
                    case 82:
                    {
                        if (auth_ == null)
                        {
                            auth_ = new global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.AuthInfo();
                        }
                        input.ReadMessage(auth_);
                        break;
                    }
                    case 90:
                    {
                        if (unknownauth_ == null)
                        {
                            unknownauth_ = new global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.UnknownAuth();
                        }
                        input.ReadMessage(unknownauth_);
                        break;
                    }
                    case 96:
                    {
                        Unknown12 = input.ReadInt64();
                        break;
                    }
                }
            }
        }

        public override bool Equals(object other)
        {
            return Equals(other as Request);
        }

        public override int GetHashCode()
        {
            var hash = 1;
            if (Unknown1 != 0) hash ^= Unknown1.GetHashCode();
            if (RpcId != 0L) hash ^= RpcId.GetHashCode();
            hash ^= requests_.GetHashCode();
            if (unknown6_ != null) hash ^= Unknown6.GetHashCode();
            if (Latitude != 0UL) hash ^= Latitude.GetHashCode();
            if (Longitude != 0UL) hash ^= Longitude.GetHashCode();
            if (Altitude != 0UL) hash ^= Altitude.GetHashCode();
            if (auth_ != null) hash ^= Auth.GetHashCode();
            if (unknownauth_ != null) hash ^= Unknownauth.GetHashCode();
            if (Unknown12 != 0L) hash ^= Unknown12.GetHashCode();
            return hash;
        }

        partial void OnConstruction();

        public override string ToString()
        {
            return pb::JsonFormatter.ToDiagnosticString(this);
        }

        #region Nested types

        /// <summary>Container for nested types declared in the Request message type.</summary>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public static partial class Types
        {
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public sealed partial class UnknownAuth : pb::IMessage<UnknownAuth>
            {
                /// <summary>Field number for the "unknown71" field.</summary>
                public const int Unknown71FieldNumber = 1;

                /// <summary>Field number for the "timestamp" field.</summary>
                public const int TimestampFieldNumber = 2;

                /// <summary>Field number for the "unknown73" field.</summary>
                public const int Unknown73FieldNumber = 3;

                private static readonly pb::MessageParser<UnknownAuth> _parser =
                    new pb::MessageParser<UnknownAuth>(() => new UnknownAuth());

                private long timestamp_;
                private pb::ByteString unknown71_ = pb::ByteString.Empty;
                private pb::ByteString unknown73_ = pb::ByteString.Empty;

                public UnknownAuth()
                {
                    OnConstruction();
                }

                public UnknownAuth(UnknownAuth other) : this()
                {
                    unknown71_ = other.unknown71_;
                    timestamp_ = other.timestamp_;
                    unknown73_ = other.unknown73_;
                }

                public static pb::MessageParser<UnknownAuth> Parser
                {
                    get { return _parser; }
                }

                public static pbr::MessageDescriptor Descriptor
                {
                    get { return global::PokemonGo.RocketAPI.GeneratedCode.Request.Descriptor.NestedTypes[0]; }
                }

                public pb::ByteString Unknown71
                {
                    get { return unknown71_; }
                    set { unknown71_ = pb::ProtoPreconditions.CheckNotNull(value, "value"); }
                }

                public long Timestamp
                {
                    get { return timestamp_; }
                    set { timestamp_ = value; }
                }

                public pb::ByteString Unknown73
                {
                    get { return unknown73_; }
                    set { unknown73_ = pb::ProtoPreconditions.CheckNotNull(value, "value"); }
                }

                pbr::MessageDescriptor pb::IMessage.Descriptor
                {
                    get { return Descriptor; }
                }

                public UnknownAuth Clone()
                {
                    return new UnknownAuth(this);
                }

                public bool Equals(UnknownAuth other)
                {
                    if (ReferenceEquals(other, null))
                    {
                        return false;
                    }
                    if (ReferenceEquals(other, this))
                    {
                        return true;
                    }
                    if (Unknown71 != other.Unknown71) return false;
                    if (Timestamp != other.Timestamp) return false;
                    if (Unknown73 != other.Unknown73) return false;
                    return true;
                }

                public void WriteTo(pb::CodedOutputStream output)
                {
                    if (Unknown71.Length != 0)
                    {
                        output.WriteRawTag(10);
                        output.WriteBytes(Unknown71);
                    }
                    if (Timestamp != 0L)
                    {
                        output.WriteRawTag(16);
                        output.WriteInt64(Timestamp);
                    }
                    if (Unknown73.Length != 0)
                    {
                        output.WriteRawTag(26);
                        output.WriteBytes(Unknown73);
                    }
                }

                public int CalculateSize()
                {
                    var size = 0;
                    if (Unknown71.Length != 0)
                    {
                        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Unknown71);
                    }
                    if (Timestamp != 0L)
                    {
                        size += 1 + pb::CodedOutputStream.ComputeInt64Size(Timestamp);
                    }
                    if (Unknown73.Length != 0)
                    {
                        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Unknown73);
                    }
                    return size;
                }

                public void MergeFrom(UnknownAuth other)
                {
                    if (other == null)
                    {
                        return;
                    }
                    if (other.Unknown71.Length != 0)
                    {
                        Unknown71 = other.Unknown71;
                    }
                    if (other.Timestamp != 0L)
                    {
                        Timestamp = other.Timestamp;
                    }
                    if (other.Unknown73.Length != 0)
                    {
                        Unknown73 = other.Unknown73;
                    }
                }

                public void MergeFrom(pb::CodedInputStream input)
                {
                    uint tag;
                    while ((tag = input.ReadTag()) != 0)
                    {
                        switch (tag)
                        {
                            default:
                                input.SkipLastField();
                                break;
                            case 10:
                            {
                                Unknown71 = input.ReadBytes();
                                break;
                            }
                            case 16:
                            {
                                Timestamp = input.ReadInt64();
                                break;
                            }
                            case 26:
                            {
                                Unknown73 = input.ReadBytes();
                                break;
                            }
                        }
                    }
                }

                public override bool Equals(object other)
                {
                    return Equals(other as UnknownAuth);
                }

                public override int GetHashCode()
                {
                    var hash = 1;
                    if (Unknown71.Length != 0) hash ^= Unknown71.GetHashCode();
                    if (Timestamp != 0L) hash ^= Timestamp.GetHashCode();
                    if (Unknown73.Length != 0) hash ^= Unknown73.GetHashCode();
                    return hash;
                }

                partial void OnConstruction();

                public override string ToString()
                {
                    return pb::JsonFormatter.ToDiagnosticString(this);
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public sealed partial class Requests : pb::IMessage<Requests>
            {
                /// <summary>Field number for the "type" field.</summary>
                public const int TypeFieldNumber = 1;

                /// <summary>Field number for the "message" field.</summary>
                public const int MessageFieldNumber = 2;

                private static readonly pb::MessageParser<Requests> _parser =
                    new pb::MessageParser<Requests>(() => new Requests());

                private pb::ByteString message_ = pb::ByteString.Empty;
                private int type_;

                public Requests()
                {
                    OnConstruction();
                }

                public Requests(Requests other) : this()
                {
                    type_ = other.type_;
                    message_ = other.message_;
                }

                public static pb::MessageParser<Requests> Parser
                {
                    get { return _parser; }
                }

                public static pbr::MessageDescriptor Descriptor
                {
                    get { return global::PokemonGo.RocketAPI.GeneratedCode.Request.Descriptor.NestedTypes[1]; }
                }

                public int Type
                {
                    get { return type_; }
                    set { type_ = value; }
                }

                public pb::ByteString Message
                {
                    get { return message_; }
                    set { message_ = pb::ProtoPreconditions.CheckNotNull(value, "value"); }
                }

                pbr::MessageDescriptor pb::IMessage.Descriptor
                {
                    get { return Descriptor; }
                }

                public Requests Clone()
                {
                    return new Requests(this);
                }

                public bool Equals(Requests other)
                {
                    if (ReferenceEquals(other, null))
                    {
                        return false;
                    }
                    if (ReferenceEquals(other, this))
                    {
                        return true;
                    }
                    if (Type != other.Type) return false;
                    if (Message != other.Message) return false;
                    return true;
                }

                public void WriteTo(pb::CodedOutputStream output)
                {
                    if (Type != 0)
                    {
                        output.WriteRawTag(8);
                        output.WriteInt32(Type);
                    }
                    if (Message.Length != 0)
                    {
                        output.WriteRawTag(18);
                        output.WriteBytes(Message);
                    }
                }

                public int CalculateSize()
                {
                    var size = 0;
                    if (Type != 0)
                    {
                        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Type);
                    }
                    if (Message.Length != 0)
                    {
                        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Message);
                    }
                    return size;
                }

                public void MergeFrom(Requests other)
                {
                    if (other == null)
                    {
                        return;
                    }
                    if (other.Type != 0)
                    {
                        Type = other.Type;
                    }
                    if (other.Message.Length != 0)
                    {
                        Message = other.Message;
                    }
                }

                public void MergeFrom(pb::CodedInputStream input)
                {
                    uint tag;
                    while ((tag = input.ReadTag()) != 0)
                    {
                        switch (tag)
                        {
                            default:
                                input.SkipLastField();
                                break;
                            case 8:
                            {
                                Type = input.ReadInt32();
                                break;
                            }
                            case 18:
                            {
                                Message = input.ReadBytes();
                                break;
                            }
                        }
                    }
                }

                public override bool Equals(object other)
                {
                    return Equals(other as Requests);
                }

                public override int GetHashCode()
                {
                    var hash = 1;
                    if (Type != 0) hash ^= Type.GetHashCode();
                    if (Message.Length != 0) hash ^= Message.GetHashCode();
                    return hash;
                }

                partial void OnConstruction();

                public override string ToString()
                {
                    return pb::JsonFormatter.ToDiagnosticString(this);
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public sealed partial class Unknown3 : pb::IMessage<Unknown3>
            {
                /// <summary>Field number for the "unknown4" field.</summary>
                public const int Unknown4FieldNumber = 1;

                private static readonly pb::MessageParser<Unknown3> _parser =
                    new pb::MessageParser<Unknown3>(() => new Unknown3());

                private string unknown4_ = "";

                public Unknown3()
                {
                    OnConstruction();
                }

                public Unknown3(Unknown3 other) : this()
                {
                    unknown4_ = other.unknown4_;
                }

                public static pb::MessageParser<Unknown3> Parser
                {
                    get { return _parser; }
                }

                public static pbr::MessageDescriptor Descriptor
                {
                    get { return global::PokemonGo.RocketAPI.GeneratedCode.Request.Descriptor.NestedTypes[2]; }
                }

                public string Unknown4
                {
                    get { return unknown4_; }
                    set { unknown4_ = pb::ProtoPreconditions.CheckNotNull(value, "value"); }
                }

                pbr::MessageDescriptor pb::IMessage.Descriptor
                {
                    get { return Descriptor; }
                }

                public Unknown3 Clone()
                {
                    return new Unknown3(this);
                }

                public bool Equals(Unknown3 other)
                {
                    if (ReferenceEquals(other, null))
                    {
                        return false;
                    }
                    if (ReferenceEquals(other, this))
                    {
                        return true;
                    }
                    if (Unknown4 != other.Unknown4) return false;
                    return true;
                }

                public void WriteTo(pb::CodedOutputStream output)
                {
                    if (Unknown4.Length != 0)
                    {
                        output.WriteRawTag(10);
                        output.WriteString(Unknown4);
                    }
                }

                public int CalculateSize()
                {
                    var size = 0;
                    if (Unknown4.Length != 0)
                    {
                        size += 1 + pb::CodedOutputStream.ComputeStringSize(Unknown4);
                    }
                    return size;
                }

                public void MergeFrom(Unknown3 other)
                {
                    if (other == null)
                    {
                        return;
                    }
                    if (other.Unknown4.Length != 0)
                    {
                        Unknown4 = other.Unknown4;
                    }
                }

                public void MergeFrom(pb::CodedInputStream input)
                {
                    uint tag;
                    while ((tag = input.ReadTag()) != 0)
                    {
                        switch (tag)
                        {
                            default:
                                input.SkipLastField();
                                break;
                            case 10:
                            {
                                Unknown4 = input.ReadString();
                                break;
                            }
                        }
                    }
                }

                public override bool Equals(object other)
                {
                    return Equals(other as Unknown3);
                }

                public override int GetHashCode()
                {
                    var hash = 1;
                    if (Unknown4.Length != 0) hash ^= Unknown4.GetHashCode();
                    return hash;
                }

                partial void OnConstruction();

                public override string ToString()
                {
                    return pb::JsonFormatter.ToDiagnosticString(this);
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public sealed partial class Unknown6 : pb::IMessage<Unknown6>
            {
                /// <summary>Field number for the "unknown1" field.</summary>
                public const int Unknown1FieldNumber = 1;

                /// <summary>Field number for the "unknown2" field.</summary>
                public const int Unknown2FieldNumber = 2;

                private static readonly pb::MessageParser<Unknown6> _parser =
                    new pb::MessageParser<Unknown6>(() => new Unknown6());

                private int unknown1_;
                private global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Unknown6.Types.Unknown2 unknown2_;

                public Unknown6()
                {
                    OnConstruction();
                }

                public Unknown6(Unknown6 other) : this()
                {
                    unknown1_ = other.unknown1_;
                    Unknown2 = other.unknown2_ != null ? other.Unknown2.Clone() : null;
                }

                public static pb::MessageParser<Unknown6> Parser
                {
                    get { return _parser; }
                }

                public static pbr::MessageDescriptor Descriptor
                {
                    get { return global::PokemonGo.RocketAPI.GeneratedCode.Request.Descriptor.NestedTypes[3]; }
                }

                public int Unknown1
                {
                    get { return unknown1_; }
                    set { unknown1_ = value; }
                }

                public global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Unknown6.Types.Unknown2 Unknown2
                {
                    get { return unknown2_; }
                    set { unknown2_ = value; }
                }

                pbr::MessageDescriptor pb::IMessage.Descriptor
                {
                    get { return Descriptor; }
                }

                public Unknown6 Clone()
                {
                    return new Unknown6(this);
                }

                public bool Equals(Unknown6 other)
                {
                    if (ReferenceEquals(other, null))
                    {
                        return false;
                    }
                    if (ReferenceEquals(other, this))
                    {
                        return true;
                    }
                    if (Unknown1 != other.Unknown1) return false;
                    if (!Equals(Unknown2, other.Unknown2)) return false;
                    return true;
                }

                public void WriteTo(pb::CodedOutputStream output)
                {
                    if (Unknown1 != 0)
                    {
                        output.WriteRawTag(8);
                        output.WriteInt32(Unknown1);
                    }
                    if (unknown2_ != null)
                    {
                        output.WriteRawTag(18);
                        output.WriteMessage(Unknown2);
                    }
                }

                public int CalculateSize()
                {
                    var size = 0;
                    if (Unknown1 != 0)
                    {
                        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Unknown1);
                    }
                    if (unknown2_ != null)
                    {
                        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Unknown2);
                    }
                    return size;
                }

                public void MergeFrom(Unknown6 other)
                {
                    if (other == null)
                    {
                        return;
                    }
                    if (other.Unknown1 != 0)
                    {
                        Unknown1 = other.Unknown1;
                    }
                    if (other.unknown2_ != null)
                    {
                        if (unknown2_ == null)
                        {
                            unknown2_ =
                                new global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Unknown6.Types.Unknown2();
                        }
                        Unknown2.MergeFrom(other.Unknown2);
                    }
                }

                public void MergeFrom(pb::CodedInputStream input)
                {
                    uint tag;
                    while ((tag = input.ReadTag()) != 0)
                    {
                        switch (tag)
                        {
                            default:
                                input.SkipLastField();
                                break;
                            case 8:
                            {
                                Unknown1 = input.ReadInt32();
                                break;
                            }
                            case 18:
                            {
                                if (unknown2_ == null)
                                {
                                    unknown2_ =
                                        new global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Unknown6.Types.
                                            Unknown2();
                                }
                                input.ReadMessage(unknown2_);
                                break;
                            }
                        }
                    }
                }

                public override bool Equals(object other)
                {
                    return Equals(other as Unknown6);
                }

                public override int GetHashCode()
                {
                    var hash = 1;
                    if (Unknown1 != 0) hash ^= Unknown1.GetHashCode();
                    if (unknown2_ != null) hash ^= Unknown2.GetHashCode();
                    return hash;
                }

                partial void OnConstruction();

                public override string ToString()
                {
                    return pb::JsonFormatter.ToDiagnosticString(this);
                }

                #region Nested types

                /// <summary>Container for nested types declared in the Unknown6 message type.</summary>
                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                public static partial class Types
                {
                    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                    public sealed partial class Unknown2 : pb::IMessage<Unknown2>
                    {
                        /// <summary>Field number for the "unknown1" field.</summary>
                        public const int Unknown1FieldNumber = 1;

                        private static readonly pb::MessageParser<Unknown2> _parser =
                            new pb::MessageParser<Unknown2>(() => new Unknown2());

                        private pb::ByteString unknown1_ = pb::ByteString.Empty;

                        public Unknown2()
                        {
                            OnConstruction();
                        }

                        public Unknown2(Unknown2 other) : this()
                        {
                            unknown1_ = other.unknown1_;
                        }

                        public static pb::MessageParser<Unknown2> Parser
                        {
                            get { return _parser; }
                        }

                        public static pbr::MessageDescriptor Descriptor
                        {
                            get
                            {
                                return
                                    global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.Unknown6.Descriptor
                                        .NestedTypes[0];
                            }
                        }

                        public pb::ByteString Unknown1
                        {
                            get { return unknown1_; }
                            set { unknown1_ = pb::ProtoPreconditions.CheckNotNull(value, "value"); }
                        }

                        pbr::MessageDescriptor pb::IMessage.Descriptor
                        {
                            get { return Descriptor; }
                        }

                        public Unknown2 Clone()
                        {
                            return new Unknown2(this);
                        }

                        public bool Equals(Unknown2 other)
                        {
                            if (ReferenceEquals(other, null))
                            {
                                return false;
                            }
                            if (ReferenceEquals(other, this))
                            {
                                return true;
                            }
                            if (Unknown1 != other.Unknown1) return false;
                            return true;
                        }

                        public void WriteTo(pb::CodedOutputStream output)
                        {
                            if (Unknown1.Length != 0)
                            {
                                output.WriteRawTag(10);
                                output.WriteBytes(Unknown1);
                            }
                        }

                        public int CalculateSize()
                        {
                            var size = 0;
                            if (Unknown1.Length != 0)
                            {
                                size += 1 + pb::CodedOutputStream.ComputeBytesSize(Unknown1);
                            }
                            return size;
                        }

                        public void MergeFrom(Unknown2 other)
                        {
                            if (other == null)
                            {
                                return;
                            }
                            if (other.Unknown1.Length != 0)
                            {
                                Unknown1 = other.Unknown1;
                            }
                        }

                        public void MergeFrom(pb::CodedInputStream input)
                        {
                            uint tag;
                            while ((tag = input.ReadTag()) != 0)
                            {
                                switch (tag)
                                {
                                    default:
                                        input.SkipLastField();
                                        break;
                                    case 10:
                                    {
                                        Unknown1 = input.ReadBytes();
                                        break;
                                    }
                                }
                            }
                        }

                        public override bool Equals(object other)
                        {
                            return Equals(other as Unknown2);
                        }

                        public override int GetHashCode()
                        {
                            var hash = 1;
                            if (Unknown1.Length != 0) hash ^= Unknown1.GetHashCode();
                            return hash;
                        }

                        partial void OnConstruction();

                        public override string ToString()
                        {
                            return pb::JsonFormatter.ToDiagnosticString(this);
                        }
                    }
                }

                #endregion
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public sealed partial class AuthInfo : pb::IMessage<AuthInfo>
            {
                /// <summary>Field number for the "provider" field.</summary>
                public const int ProviderFieldNumber = 1;

                /// <summary>Field number for the "token" field.</summary>
                public const int TokenFieldNumber = 2;

                private static readonly pb::MessageParser<AuthInfo> _parser =
                    new pb::MessageParser<AuthInfo>(() => new AuthInfo());

                private string provider_ = "";
                private global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.AuthInfo.Types.JWT token_;

                public AuthInfo()
                {
                    OnConstruction();
                }

                public AuthInfo(AuthInfo other) : this()
                {
                    provider_ = other.provider_;
                    Token = other.token_ != null ? other.Token.Clone() : null;
                }

                public static pb::MessageParser<AuthInfo> Parser
                {
                    get { return _parser; }
                }

                public static pbr::MessageDescriptor Descriptor
                {
                    get { return global::PokemonGo.RocketAPI.GeneratedCode.Request.Descriptor.NestedTypes[4]; }
                }

                public string Provider
                {
                    get { return provider_; }
                    set { provider_ = pb::ProtoPreconditions.CheckNotNull(value, "value"); }
                }

                public global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.AuthInfo.Types.JWT Token
                {
                    get { return token_; }
                    set { token_ = value; }
                }

                pbr::MessageDescriptor pb::IMessage.Descriptor
                {
                    get { return Descriptor; }
                }

                public AuthInfo Clone()
                {
                    return new AuthInfo(this);
                }

                public bool Equals(AuthInfo other)
                {
                    if (ReferenceEquals(other, null))
                    {
                        return false;
                    }
                    if (ReferenceEquals(other, this))
                    {
                        return true;
                    }
                    if (Provider != other.Provider) return false;
                    if (!Equals(Token, other.Token)) return false;
                    return true;
                }

                public void WriteTo(pb::CodedOutputStream output)
                {
                    if (Provider.Length != 0)
                    {
                        output.WriteRawTag(10);
                        output.WriteString(Provider);
                    }
                    if (token_ != null)
                    {
                        output.WriteRawTag(18);
                        output.WriteMessage(Token);
                    }
                }

                public int CalculateSize()
                {
                    var size = 0;
                    if (Provider.Length != 0)
                    {
                        size += 1 + pb::CodedOutputStream.ComputeStringSize(Provider);
                    }
                    if (token_ != null)
                    {
                        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Token);
                    }
                    return size;
                }

                public void MergeFrom(AuthInfo other)
                {
                    if (other == null)
                    {
                        return;
                    }
                    if (other.Provider.Length != 0)
                    {
                        Provider = other.Provider;
                    }
                    if (other.token_ != null)
                    {
                        if (token_ == null)
                        {
                            token_ = new global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.AuthInfo.Types.JWT();
                        }
                        Token.MergeFrom(other.Token);
                    }
                }

                public void MergeFrom(pb::CodedInputStream input)
                {
                    uint tag;
                    while ((tag = input.ReadTag()) != 0)
                    {
                        switch (tag)
                        {
                            default:
                                input.SkipLastField();
                                break;
                            case 10:
                            {
                                Provider = input.ReadString();
                                break;
                            }
                            case 18:
                            {
                                if (token_ == null)
                                {
                                    token_ =
                                        new global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.AuthInfo.Types.JWT();
                                }
                                input.ReadMessage(token_);
                                break;
                            }
                        }
                    }
                }

                public override bool Equals(object other)
                {
                    return Equals(other as AuthInfo);
                }

                public override int GetHashCode()
                {
                    var hash = 1;
                    if (Provider.Length != 0) hash ^= Provider.GetHashCode();
                    if (token_ != null) hash ^= Token.GetHashCode();
                    return hash;
                }

                partial void OnConstruction();

                public override string ToString()
                {
                    return pb::JsonFormatter.ToDiagnosticString(this);
                }

                #region Nested types

                /// <summary>Container for nested types declared in the AuthInfo message type.</summary>
                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                public static partial class Types
                {
                    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                    public sealed partial class JWT : pb::IMessage<JWT>
                    {
                        /// <summary>Field number for the "contents" field.</summary>
                        public const int ContentsFieldNumber = 1;

                        /// <summary>Field number for the "unknown13" field.</summary>
                        public const int Unknown13FieldNumber = 2;

                        private static readonly pb::MessageParser<JWT> _parser =
                            new pb::MessageParser<JWT>(() => new JWT());

                        private string contents_ = "";
                        private int unknown13_;

                        public JWT()
                        {
                            OnConstruction();
                        }

                        public JWT(JWT other) : this()
                        {
                            contents_ = other.contents_;
                            unknown13_ = other.unknown13_;
                        }

                        public static pb::MessageParser<JWT> Parser
                        {
                            get { return _parser; }
                        }

                        public static pbr::MessageDescriptor Descriptor
                        {
                            get
                            {
                                return
                                    global::PokemonGo.RocketAPI.GeneratedCode.Request.Types.AuthInfo.Descriptor
                                        .NestedTypes[0];
                            }
                        }

                        public string Contents
                        {
                            get { return contents_; }
                            set { contents_ = pb::ProtoPreconditions.CheckNotNull(value, "value"); }
                        }

                        public int Unknown13
                        {
                            get { return unknown13_; }
                            set { unknown13_ = value; }
                        }

                        pbr::MessageDescriptor pb::IMessage.Descriptor
                        {
                            get { return Descriptor; }
                        }

                        public JWT Clone()
                        {
                            return new JWT(this);
                        }

                        public bool Equals(JWT other)
                        {
                            if (ReferenceEquals(other, null))
                            {
                                return false;
                            }
                            if (ReferenceEquals(other, this))
                            {
                                return true;
                            }
                            if (Contents != other.Contents) return false;
                            if (Unknown13 != other.Unknown13) return false;
                            return true;
                        }

                        public void WriteTo(pb::CodedOutputStream output)
                        {
                            if (Contents.Length != 0)
                            {
                                output.WriteRawTag(10);
                                output.WriteString(Contents);
                            }
                            if (Unknown13 != 0)
                            {
                                output.WriteRawTag(16);
                                output.WriteInt32(Unknown13);
                            }
                        }

                        public int CalculateSize()
                        {
                            var size = 0;
                            if (Contents.Length != 0)
                            {
                                size += 1 + pb::CodedOutputStream.ComputeStringSize(Contents);
                            }
                            if (Unknown13 != 0)
                            {
                                size += 1 + pb::CodedOutputStream.ComputeInt32Size(Unknown13);
                            }
                            return size;
                        }

                        public void MergeFrom(JWT other)
                        {
                            if (other == null)
                            {
                                return;
                            }
                            if (other.Contents.Length != 0)
                            {
                                Contents = other.Contents;
                            }
                            if (other.Unknown13 != 0)
                            {
                                Unknown13 = other.Unknown13;
                            }
                        }

                        public void MergeFrom(pb::CodedInputStream input)
                        {
                            uint tag;
                            while ((tag = input.ReadTag()) != 0)
                            {
                                switch (tag)
                                {
                                    default:
                                        input.SkipLastField();
                                        break;
                                    case 10:
                                    {
                                        Contents = input.ReadString();
                                        break;
                                    }
                                    case 16:
                                    {
                                        Unknown13 = input.ReadInt32();
                                        break;
                                    }
                                }
                            }
                        }

                        public override bool Equals(object other)
                        {
                            return Equals(other as JWT);
                        }

                        public override int GetHashCode()
                        {
                            var hash = 1;
                            if (Contents.Length != 0) hash ^= Contents.GetHashCode();
                            if (Unknown13 != 0) hash ^= Unknown13.GetHashCode();
                            return hash;
                        }

                        partial void OnConstruction();

                        public override string ToString()
                        {
                            return pb::JsonFormatter.ToDiagnosticString(this);
                        }
                    }
                }

                #endregion
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public sealed partial class PlayerUpdateProto : pb::IMessage<PlayerUpdateProto>
            {
                /// <summary>Field number for the "Lat" field.</summary>
                public const int LatFieldNumber = 1;

                /// <summary>Field number for the "Lng" field.</summary>
                public const int LngFieldNumber = 2;

                private static readonly pb::MessageParser<PlayerUpdateProto> _parser =
                    new pb::MessageParser<PlayerUpdateProto>(() => new PlayerUpdateProto());

                private ulong lat_;
                private ulong lng_;

                public PlayerUpdateProto()
                {
                    OnConstruction();
                }

                public PlayerUpdateProto(PlayerUpdateProto other) : this()
                {
                    lat_ = other.lat_;
                    lng_ = other.lng_;
                }

                public static pb::MessageParser<PlayerUpdateProto> Parser
                {
                    get { return _parser; }
                }

                public static pbr::MessageDescriptor Descriptor
                {
                    get { return global::PokemonGo.RocketAPI.GeneratedCode.Request.Descriptor.NestedTypes[5]; }
                }

                public ulong Lat
                {
                    get { return lat_; }
                    set { lat_ = value; }
                }

                public ulong Lng
                {
                    get { return lng_; }
                    set { lng_ = value; }
                }

                pbr::MessageDescriptor pb::IMessage.Descriptor
                {
                    get { return Descriptor; }
                }

                public PlayerUpdateProto Clone()
                {
                    return new PlayerUpdateProto(this);
                }

                public bool Equals(PlayerUpdateProto other)
                {
                    if (ReferenceEquals(other, null))
                    {
                        return false;
                    }
                    if (ReferenceEquals(other, this))
                    {
                        return true;
                    }
                    if (Lat != other.Lat) return false;
                    if (Lng != other.Lng) return false;
                    return true;
                }

                public void WriteTo(pb::CodedOutputStream output)
                {
                    if (Lat != 0UL)
                    {
                        output.WriteRawTag(9);
                        output.WriteFixed64(Lat);
                    }
                    if (Lng != 0UL)
                    {
                        output.WriteRawTag(17);
                        output.WriteFixed64(Lng);
                    }
                }

                public int CalculateSize()
                {
                    var size = 0;
                    if (Lat != 0UL)
                    {
                        size += 1 + 8;
                    }
                    if (Lng != 0UL)
                    {
                        size += 1 + 8;
                    }
                    return size;
                }

                public void MergeFrom(PlayerUpdateProto other)
                {
                    if (other == null)
                    {
                        return;
                    }
                    if (other.Lat != 0UL)
                    {
                        Lat = other.Lat;
                    }
                    if (other.Lng != 0UL)
                    {
                        Lng = other.Lng;
                    }
                }

                public void MergeFrom(pb::CodedInputStream input)
                {
                    uint tag;
                    while ((tag = input.ReadTag()) != 0)
                    {
                        switch (tag)
                        {
                            default:
                                input.SkipLastField();
                                break;
                            case 9:
                            {
                                Lat = input.ReadFixed64();
                                break;
                            }
                            case 17:
                            {
                                Lng = input.ReadFixed64();
                                break;
                            }
                        }
                    }
                }

                public override bool Equals(object other)
                {
                    return Equals(other as PlayerUpdateProto);
                }

                public override int GetHashCode()
                {
                    var hash = 1;
                    if (Lat != 0UL) hash ^= Lat.GetHashCode();
                    if (Lng != 0UL) hash ^= Lng.GetHashCode();
                    return hash;
                }

                partial void OnConstruction();

                public override string ToString()
                {
                    return pb::JsonFormatter.ToDiagnosticString(this);
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public sealed partial class MapObjectsRequest : pb::IMessage<MapObjectsRequest>
            {
                /// <summary>Field number for the "cellIds" field.</summary>
                public const int CellIdsFieldNumber = 1;

                /// <summary>Field number for the "unknown14" field.</summary>
                public const int Unknown14FieldNumber = 2;

                /// <summary>Field number for the "latitude" field.</summary>
                public const int LatitudeFieldNumber = 3;

                /// <summary>Field number for the "longitude" field.</summary>
                public const int LongitudeFieldNumber = 4;

                private static readonly pb::MessageParser<MapObjectsRequest> _parser =
                    new pb::MessageParser<MapObjectsRequest>(() => new MapObjectsRequest());

                private pb::ByteString cellIds_ = pb::ByteString.Empty;
                private ulong latitude_;
                private ulong longitude_;
                private pb::ByteString unknown14_ = pb::ByteString.Empty;

                public MapObjectsRequest()
                {
                    OnConstruction();
                }

                public MapObjectsRequest(MapObjectsRequest other) : this()
                {
                    cellIds_ = other.cellIds_;
                    unknown14_ = other.unknown14_;
                    latitude_ = other.latitude_;
                    longitude_ = other.longitude_;
                }

                public static pb::MessageParser<MapObjectsRequest> Parser
                {
                    get { return _parser; }
                }

                public static pbr::MessageDescriptor Descriptor
                {
                    get { return global::PokemonGo.RocketAPI.GeneratedCode.Request.Descriptor.NestedTypes[6]; }
                }

                public pb::ByteString CellIds
                {
                    get { return cellIds_; }
                    set { cellIds_ = pb::ProtoPreconditions.CheckNotNull(value, "value"); }
                }

                public pb::ByteString Unknown14
                {
                    get { return unknown14_; }
                    set { unknown14_ = pb::ProtoPreconditions.CheckNotNull(value, "value"); }
                }

                public ulong Latitude
                {
                    get { return latitude_; }
                    set { latitude_ = value; }
                }

                public ulong Longitude
                {
                    get { return longitude_; }
                    set { longitude_ = value; }
                }

                pbr::MessageDescriptor pb::IMessage.Descriptor
                {
                    get { return Descriptor; }
                }

                public MapObjectsRequest Clone()
                {
                    return new MapObjectsRequest(this);
                }

                public bool Equals(MapObjectsRequest other)
                {
                    if (ReferenceEquals(other, null))
                    {
                        return false;
                    }
                    if (ReferenceEquals(other, this))
                    {
                        return true;
                    }
                    if (CellIds != other.CellIds) return false;
                    if (Unknown14 != other.Unknown14) return false;
                    if (Latitude != other.Latitude) return false;
                    if (Longitude != other.Longitude) return false;
                    return true;
                }

                public void WriteTo(pb::CodedOutputStream output)
                {
                    if (CellIds.Length != 0)
                    {
                        output.WriteRawTag(10);
                        output.WriteBytes(CellIds);
                    }
                    if (Unknown14.Length != 0)
                    {
                        output.WriteRawTag(18);
                        output.WriteBytes(Unknown14);
                    }
                    if (Latitude != 0UL)
                    {
                        output.WriteRawTag(25);
                        output.WriteFixed64(Latitude);
                    }
                    if (Longitude != 0UL)
                    {
                        output.WriteRawTag(33);
                        output.WriteFixed64(Longitude);
                    }
                }

                public int CalculateSize()
                {
                    var size = 0;
                    if (CellIds.Length != 0)
                    {
                        size += 1 + pb::CodedOutputStream.ComputeBytesSize(CellIds);
                    }
                    if (Unknown14.Length != 0)
                    {
                        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Unknown14);
                    }
                    if (Latitude != 0UL)
                    {
                        size += 1 + 8;
                    }
                    if (Longitude != 0UL)
                    {
                        size += 1 + 8;
                    }
                    return size;
                }

                public void MergeFrom(MapObjectsRequest other)
                {
                    if (other == null)
                    {
                        return;
                    }
                    if (other.CellIds.Length != 0)
                    {
                        CellIds = other.CellIds;
                    }
                    if (other.Unknown14.Length != 0)
                    {
                        Unknown14 = other.Unknown14;
                    }
                    if (other.Latitude != 0UL)
                    {
                        Latitude = other.Latitude;
                    }
                    if (other.Longitude != 0UL)
                    {
                        Longitude = other.Longitude;
                    }
                }

                public void MergeFrom(pb::CodedInputStream input)
                {
                    uint tag;
                    while ((tag = input.ReadTag()) != 0)
                    {
                        switch (tag)
                        {
                            default:
                                input.SkipLastField();
                                break;
                            case 10:
                            {
                                CellIds = input.ReadBytes();
                                break;
                            }
                            case 18:
                            {
                                Unknown14 = input.ReadBytes();
                                break;
                            }
                            case 25:
                            {
                                Latitude = input.ReadFixed64();
                                break;
                            }
                            case 33:
                            {
                                Longitude = input.ReadFixed64();
                                break;
                            }
                        }
                    }
                }

                public override bool Equals(object other)
                {
                    return Equals(other as MapObjectsRequest);
                }

                public override int GetHashCode()
                {
                    var hash = 1;
                    if (CellIds.Length != 0) hash ^= CellIds.GetHashCode();
                    if (Unknown14.Length != 0) hash ^= Unknown14.GetHashCode();
                    if (Latitude != 0UL) hash ^= Latitude.GetHashCode();
                    if (Longitude != 0UL) hash ^= Longitude.GetHashCode();
                    return hash;
                }

                partial void OnConstruction();

                public override string ToString()
                {
                    return pb::JsonFormatter.ToDiagnosticString(this);
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public sealed partial class FortSearchRequest : pb::IMessage<FortSearchRequest>
            {
                /// <summary>Field number for the "Id" field.</summary>
                public const int IdFieldNumber = 1;

                /// <summary>Field number for the "PlayerLatDegrees" field.</summary>
                public const int PlayerLatDegreesFieldNumber = 2;

                /// <summary>Field number for the "PlayerLngDegrees" field.</summary>
                public const int PlayerLngDegreesFieldNumber = 3;

                /// <summary>Field number for the "FortLatDegrees" field.</summary>
                public const int FortLatDegreesFieldNumber = 4;

                /// <summary>Field number for the "FortLngDegrees" field.</summary>
                public const int FortLngDegreesFieldNumber = 5;

                private static readonly pb::MessageParser<FortSearchRequest> _parser =
                    new pb::MessageParser<FortSearchRequest>(() => new FortSearchRequest());

                private ulong fortLatDegrees_;
                private ulong fortLngDegrees_;
                private pb::ByteString id_ = pb::ByteString.Empty;
                private ulong playerLatDegrees_;
                private ulong playerLngDegrees_;

                public FortSearchRequest()
                {
                    OnConstruction();
                }

                public FortSearchRequest(FortSearchRequest other) : this()
                {
                    id_ = other.id_;
                    playerLatDegrees_ = other.playerLatDegrees_;
                    playerLngDegrees_ = other.playerLngDegrees_;
                    fortLatDegrees_ = other.fortLatDegrees_;
                    fortLngDegrees_ = other.fortLngDegrees_;
                }

                public static pb::MessageParser<FortSearchRequest> Parser
                {
                    get { return _parser; }
                }

                public static pbr::MessageDescriptor Descriptor
                {
                    get { return global::PokemonGo.RocketAPI.GeneratedCode.Request.Descriptor.NestedTypes[7]; }
                }

                public pb::ByteString Id
                {
                    get { return id_; }
                    set { id_ = pb::ProtoPreconditions.CheckNotNull(value, "value"); }
                }

                public ulong PlayerLatDegrees
                {
                    get { return playerLatDegrees_; }
                    set { playerLatDegrees_ = value; }
                }

                public ulong PlayerLngDegrees
                {
                    get { return playerLngDegrees_; }
                    set { playerLngDegrees_ = value; }
                }

                public ulong FortLatDegrees
                {
                    get { return fortLatDegrees_; }
                    set { fortLatDegrees_ = value; }
                }

                public ulong FortLngDegrees
                {
                    get { return fortLngDegrees_; }
                    set { fortLngDegrees_ = value; }
                }

                pbr::MessageDescriptor pb::IMessage.Descriptor
                {
                    get { return Descriptor; }
                }

                public FortSearchRequest Clone()
                {
                    return new FortSearchRequest(this);
                }

                public bool Equals(FortSearchRequest other)
                {
                    if (ReferenceEquals(other, null))
                    {
                        return false;
                    }
                    if (ReferenceEquals(other, this))
                    {
                        return true;
                    }
                    if (Id != other.Id) return false;
                    if (PlayerLatDegrees != other.PlayerLatDegrees) return false;
                    if (PlayerLngDegrees != other.PlayerLngDegrees) return false;
                    if (FortLatDegrees != other.FortLatDegrees) return false;
                    if (FortLngDegrees != other.FortLngDegrees) return false;
                    return true;
                }

                public void WriteTo(pb::CodedOutputStream output)
                {
                    if (Id.Length != 0)
                    {
                        output.WriteRawTag(10);
                        output.WriteBytes(Id);
                    }
                    if (PlayerLatDegrees != 0UL)
                    {
                        output.WriteRawTag(17);
                        output.WriteFixed64(PlayerLatDegrees);
                    }
                    if (PlayerLngDegrees != 0UL)
                    {
                        output.WriteRawTag(25);
                        output.WriteFixed64(PlayerLngDegrees);
                    }
                    if (FortLatDegrees != 0UL)
                    {
                        output.WriteRawTag(33);
                        output.WriteFixed64(FortLatDegrees);
                    }
                    if (FortLngDegrees != 0UL)
                    {
                        output.WriteRawTag(41);
                        output.WriteFixed64(FortLngDegrees);
                    }
                }

                public int CalculateSize()
                {
                    var size = 0;
                    if (Id.Length != 0)
                    {
                        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Id);
                    }
                    if (PlayerLatDegrees != 0UL)
                    {
                        size += 1 + 8;
                    }
                    if (PlayerLngDegrees != 0UL)
                    {
                        size += 1 + 8;
                    }
                    if (FortLatDegrees != 0UL)
                    {
                        size += 1 + 8;
                    }
                    if (FortLngDegrees != 0UL)
                    {
                        size += 1 + 8;
                    }
                    return size;
                }

                public void MergeFrom(FortSearchRequest other)
                {
                    if (other == null)
                    {
                        return;
                    }
                    if (other.Id.Length != 0)
                    {
                        Id = other.Id;
                    }
                    if (other.PlayerLatDegrees != 0UL)
                    {
                        PlayerLatDegrees = other.PlayerLatDegrees;
                    }
                    if (other.PlayerLngDegrees != 0UL)
                    {
                        PlayerLngDegrees = other.PlayerLngDegrees;
                    }
                    if (other.FortLatDegrees != 0UL)
                    {
                        FortLatDegrees = other.FortLatDegrees;
                    }
                    if (other.FortLngDegrees != 0UL)
                    {
                        FortLngDegrees = other.FortLngDegrees;
                    }
                }

                public void MergeFrom(pb::CodedInputStream input)
                {
                    uint tag;
                    while ((tag = input.ReadTag()) != 0)
                    {
                        switch (tag)
                        {
                            default:
                                input.SkipLastField();
                                break;
                            case 10:
                            {
                                Id = input.ReadBytes();
                                break;
                            }
                            case 17:
                            {
                                PlayerLatDegrees = input.ReadFixed64();
                                break;
                            }
                            case 25:
                            {
                                PlayerLngDegrees = input.ReadFixed64();
                                break;
                            }
                            case 33:
                            {
                                FortLatDegrees = input.ReadFixed64();
                                break;
                            }
                            case 41:
                            {
                                FortLngDegrees = input.ReadFixed64();
                                break;
                            }
                        }
                    }
                }

                public override bool Equals(object other)
                {
                    return Equals(other as FortSearchRequest);
                }

                public override int GetHashCode()
                {
                    var hash = 1;
                    if (Id.Length != 0) hash ^= Id.GetHashCode();
                    if (PlayerLatDegrees != 0UL) hash ^= PlayerLatDegrees.GetHashCode();
                    if (PlayerLngDegrees != 0UL) hash ^= PlayerLngDegrees.GetHashCode();
                    if (FortLatDegrees != 0UL) hash ^= FortLatDegrees.GetHashCode();
                    if (FortLngDegrees != 0UL) hash ^= FortLngDegrees.GetHashCode();
                    return hash;
                }

                partial void OnConstruction();

                public override string ToString()
                {
                    return pb::JsonFormatter.ToDiagnosticString(this);
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public sealed partial class FortDetailsRequest : pb::IMessage<FortDetailsRequest>
            {
                /// <summary>Field number for the "Id" field.</summary>
                public const int IdFieldNumber = 1;

                /// <summary>Field number for the "Latitude" field.</summary>
                public const int LatitudeFieldNumber = 2;

                /// <summary>Field number for the "Longitude" field.</summary>
                public const int LongitudeFieldNumber = 3;

                private static readonly pb::MessageParser<FortDetailsRequest> _parser =
                    new pb::MessageParser<FortDetailsRequest>(() => new FortDetailsRequest());

                private pb::ByteString id_ = pb::ByteString.Empty;
                private ulong latitude_;
                private ulong longitude_;

                public FortDetailsRequest()
                {
                    OnConstruction();
                }

                public FortDetailsRequest(FortDetailsRequest other) : this()
                {
                    id_ = other.id_;
                    latitude_ = other.latitude_;
                    longitude_ = other.longitude_;
                }

                public static pb::MessageParser<FortDetailsRequest> Parser
                {
                    get { return _parser; }
                }

                public static pbr::MessageDescriptor Descriptor
                {
                    get { return global::PokemonGo.RocketAPI.GeneratedCode.Request.Descriptor.NestedTypes[8]; }
                }

                public pb::ByteString Id
                {
                    get { return id_; }
                    set { id_ = pb::ProtoPreconditions.CheckNotNull(value, "value"); }
                }

                public ulong Latitude
                {
                    get { return latitude_; }
                    set { latitude_ = value; }
                }

                public ulong Longitude
                {
                    get { return longitude_; }
                    set { longitude_ = value; }
                }

                pbr::MessageDescriptor pb::IMessage.Descriptor
                {
                    get { return Descriptor; }
                }

                public FortDetailsRequest Clone()
                {
                    return new FortDetailsRequest(this);
                }

                public bool Equals(FortDetailsRequest other)
                {
                    if (ReferenceEquals(other, null))
                    {
                        return false;
                    }
                    if (ReferenceEquals(other, this))
                    {
                        return true;
                    }
                    if (Id != other.Id) return false;
                    if (Latitude != other.Latitude) return false;
                    if (Longitude != other.Longitude) return false;
                    return true;
                }

                public void WriteTo(pb::CodedOutputStream output)
                {
                    if (Id.Length != 0)
                    {
                        output.WriteRawTag(10);
                        output.WriteBytes(Id);
                    }
                    if (Latitude != 0UL)
                    {
                        output.WriteRawTag(17);
                        output.WriteFixed64(Latitude);
                    }
                    if (Longitude != 0UL)
                    {
                        output.WriteRawTag(25);
                        output.WriteFixed64(Longitude);
                    }
                }

                public int CalculateSize()
                {
                    var size = 0;
                    if (Id.Length != 0)
                    {
                        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Id);
                    }
                    if (Latitude != 0UL)
                    {
                        size += 1 + 8;
                    }
                    if (Longitude != 0UL)
                    {
                        size += 1 + 8;
                    }
                    return size;
                }

                public void MergeFrom(FortDetailsRequest other)
                {
                    if (other == null)
                    {
                        return;
                    }
                    if (other.Id.Length != 0)
                    {
                        Id = other.Id;
                    }
                    if (other.Latitude != 0UL)
                    {
                        Latitude = other.Latitude;
                    }
                    if (other.Longitude != 0UL)
                    {
                        Longitude = other.Longitude;
                    }
                }

                public void MergeFrom(pb::CodedInputStream input)
                {
                    uint tag;
                    while ((tag = input.ReadTag()) != 0)
                    {
                        switch (tag)
                        {
                            default:
                                input.SkipLastField();
                                break;
                            case 10:
                            {
                                Id = input.ReadBytes();
                                break;
                            }
                            case 17:
                            {
                                Latitude = input.ReadFixed64();
                                break;
                            }
                            case 25:
                            {
                                Longitude = input.ReadFixed64();
                                break;
                            }
                        }
                    }
                }

                public override bool Equals(object other)
                {
                    return Equals(other as FortDetailsRequest);
                }

                public override int GetHashCode()
                {
                    var hash = 1;
                    if (Id.Length != 0) hash ^= Id.GetHashCode();
                    if (Latitude != 0UL) hash ^= Latitude.GetHashCode();
                    if (Longitude != 0UL) hash ^= Longitude.GetHashCode();
                    return hash;
                }

                partial void OnConstruction();

                public override string ToString()
                {
                    return pb::JsonFormatter.ToDiagnosticString(this);
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public sealed partial class EncounterRequest : pb::IMessage<EncounterRequest>
            {
                /// <summary>Field number for the "EncounterId" field.</summary>
                public const int EncounterIdFieldNumber = 1;

                /// <summary>Field number for the "SpawnpointId" field.</summary>
                public const int SpawnpointIdFieldNumber = 2;

                /// <summary>Field number for the "PlayerLatDegrees" field.</summary>
                public const int PlayerLatDegreesFieldNumber = 3;

                /// <summary>Field number for the "PlayerLngDegrees" field.</summary>
                public const int PlayerLngDegreesFieldNumber = 4;

                private static readonly pb::MessageParser<EncounterRequest> _parser =
                    new pb::MessageParser<EncounterRequest>(() => new EncounterRequest());

                private ulong encounterId_;
                private ulong playerLatDegrees_;
                private ulong playerLngDegrees_;
                private string spawnpointId_ = "";

                public EncounterRequest()
                {
                    OnConstruction();
                }

                public EncounterRequest(EncounterRequest other) : this()
                {
                    encounterId_ = other.encounterId_;
                    spawnpointId_ = other.spawnpointId_;
                    playerLatDegrees_ = other.playerLatDegrees_;
                    playerLngDegrees_ = other.playerLngDegrees_;
                }

                public static pb::MessageParser<EncounterRequest> Parser
                {
                    get { return _parser; }
                }

                public static pbr::MessageDescriptor Descriptor
                {
                    get { return global::PokemonGo.RocketAPI.GeneratedCode.Request.Descriptor.NestedTypes[9]; }
                }

                public ulong EncounterId
                {
                    get { return encounterId_; }
                    set { encounterId_ = value; }
                }

                public string SpawnpointId
                {
                    get { return spawnpointId_; }
                    set { spawnpointId_ = pb::ProtoPreconditions.CheckNotNull(value, "value"); }
                }

                public ulong PlayerLatDegrees
                {
                    get { return playerLatDegrees_; }
                    set { playerLatDegrees_ = value; }
                }

                public ulong PlayerLngDegrees
                {
                    get { return playerLngDegrees_; }
                    set { playerLngDegrees_ = value; }
                }

                pbr::MessageDescriptor pb::IMessage.Descriptor
                {
                    get { return Descriptor; }
                }

                public EncounterRequest Clone()
                {
                    return new EncounterRequest(this);
                }

                public bool Equals(EncounterRequest other)
                {
                    if (ReferenceEquals(other, null))
                    {
                        return false;
                    }
                    if (ReferenceEquals(other, this))
                    {
                        return true;
                    }
                    if (EncounterId != other.EncounterId) return false;
                    if (SpawnpointId != other.SpawnpointId) return false;
                    if (PlayerLatDegrees != other.PlayerLatDegrees) return false;
                    if (PlayerLngDegrees != other.PlayerLngDegrees) return false;
                    return true;
                }

                public void WriteTo(pb::CodedOutputStream output)
                {
                    if (EncounterId != 0UL)
                    {
                        output.WriteRawTag(9);
                        output.WriteFixed64(EncounterId);
                    }
                    if (SpawnpointId.Length != 0)
                    {
                        output.WriteRawTag(18);
                        output.WriteString(SpawnpointId);
                    }
                    if (PlayerLatDegrees != 0UL)
                    {
                        output.WriteRawTag(25);
                        output.WriteFixed64(PlayerLatDegrees);
                    }
                    if (PlayerLngDegrees != 0UL)
                    {
                        output.WriteRawTag(33);
                        output.WriteFixed64(PlayerLngDegrees);
                    }
                }

                public int CalculateSize()
                {
                    var size = 0;
                    if (EncounterId != 0UL)
                    {
                        size += 1 + 8;
                    }
                    if (SpawnpointId.Length != 0)
                    {
                        size += 1 + pb::CodedOutputStream.ComputeStringSize(SpawnpointId);
                    }
                    if (PlayerLatDegrees != 0UL)
                    {
                        size += 1 + 8;
                    }
                    if (PlayerLngDegrees != 0UL)
                    {
                        size += 1 + 8;
                    }
                    return size;
                }

                public void MergeFrom(EncounterRequest other)
                {
                    if (other == null)
                    {
                        return;
                    }
                    if (other.EncounterId != 0UL)
                    {
                        EncounterId = other.EncounterId;
                    }
                    if (other.SpawnpointId.Length != 0)
                    {
                        SpawnpointId = other.SpawnpointId;
                    }
                    if (other.PlayerLatDegrees != 0UL)
                    {
                        PlayerLatDegrees = other.PlayerLatDegrees;
                    }
                    if (other.PlayerLngDegrees != 0UL)
                    {
                        PlayerLngDegrees = other.PlayerLngDegrees;
                    }
                }

                public void MergeFrom(pb::CodedInputStream input)
                {
                    uint tag;
                    while ((tag = input.ReadTag()) != 0)
                    {
                        switch (tag)
                        {
                            default:
                                input.SkipLastField();
                                break;
                            case 9:
                            {
                                EncounterId = input.ReadFixed64();
                                break;
                            }
                            case 18:
                            {
                                SpawnpointId = input.ReadString();
                                break;
                            }
                            case 25:
                            {
                                PlayerLatDegrees = input.ReadFixed64();
                                break;
                            }
                            case 33:
                            {
                                PlayerLngDegrees = input.ReadFixed64();
                                break;
                            }
                        }
                    }
                }

                public override bool Equals(object other)
                {
                    return Equals(other as EncounterRequest);
                }

                public override int GetHashCode()
                {
                    var hash = 1;
                    if (EncounterId != 0UL) hash ^= EncounterId.GetHashCode();
                    if (SpawnpointId.Length != 0) hash ^= SpawnpointId.GetHashCode();
                    if (PlayerLatDegrees != 0UL) hash ^= PlayerLatDegrees.GetHashCode();
                    if (PlayerLngDegrees != 0UL) hash ^= PlayerLngDegrees.GetHashCode();
                    return hash;
                }

                partial void OnConstruction();

                public override string ToString()
                {
                    return pb::JsonFormatter.ToDiagnosticString(this);
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public sealed partial class CatchPokemonRequest : pb::IMessage<CatchPokemonRequest>
            {
                /// <summary>Field number for the "EncounterId" field.</summary>
                public const int EncounterIdFieldNumber = 1;

                /// <summary>Field number for the "Pokeball" field.</summary>
                public const int PokeballFieldNumber = 2;

                /// <summary>Field number for the "NormalizedReticleSize" field.</summary>
                public const int NormalizedReticleSizeFieldNumber = 3;

                /// <summary>Field number for the "SpawnPointGuid" field.</summary>
                public const int SpawnPointGuidFieldNumber = 4;

                /// <summary>Field number for the "HitPokemon" field.</summary>
                public const int HitPokemonFieldNumber = 5;

                /// <summary>Field number for the "SpinModifier" field.</summary>
                public const int SpinModifierFieldNumber = 6;

                /// <summary>Field number for the "NormalizedHitPosition" field.</summary>
                public const int NormalizedHitPositionFieldNumber = 7;

                private static readonly pb::MessageParser<CatchPokemonRequest> _parser =
                    new pb::MessageParser<CatchPokemonRequest>(() => new CatchPokemonRequest());

                private ulong encounterId_;
                private int hitPokemon_;
                private ulong normalizedHitPosition_;
                private ulong normalizedReticleSize_;
                private int pokeball_;
                private string spawnPointGuid_ = "";
                private ulong spinModifier_;

                public CatchPokemonRequest()
                {
                    OnConstruction();
                }

                public CatchPokemonRequest(CatchPokemonRequest other) : this()
                {
                    encounterId_ = other.encounterId_;
                    pokeball_ = other.pokeball_;
                    normalizedReticleSize_ = other.normalizedReticleSize_;
                    spawnPointGuid_ = other.spawnPointGuid_;
                    hitPokemon_ = other.hitPokemon_;
                    spinModifier_ = other.spinModifier_;
                    normalizedHitPosition_ = other.normalizedHitPosition_;
                }

                public static pb::MessageParser<CatchPokemonRequest> Parser
                {
                    get { return _parser; }
                }

                public static pbr::MessageDescriptor Descriptor
                {
                    get { return global::PokemonGo.RocketAPI.GeneratedCode.Request.Descriptor.NestedTypes[10]; }
                }

                public ulong EncounterId
                {
                    get { return encounterId_; }
                    set { encounterId_ = value; }
                }

                public int Pokeball
                {
                    get { return pokeball_; }
                    set { pokeball_ = value; }
                }

                public ulong NormalizedReticleSize
                {
                    get { return normalizedReticleSize_; }
                    set { normalizedReticleSize_ = value; }
                }

                public string SpawnPointGuid
                {
                    get { return spawnPointGuid_; }
                    set { spawnPointGuid_ = pb::ProtoPreconditions.CheckNotNull(value, "value"); }
                }

                public int HitPokemon
                {
                    get { return hitPokemon_; }
                    set { hitPokemon_ = value; }
                }

                public ulong SpinModifier
                {
                    get { return spinModifier_; }
                    set { spinModifier_ = value; }
                }

                public ulong NormalizedHitPosition
                {
                    get { return normalizedHitPosition_; }
                    set { normalizedHitPosition_ = value; }
                }

                pbr::MessageDescriptor pb::IMessage.Descriptor
                {
                    get { return Descriptor; }
                }

                public CatchPokemonRequest Clone()
                {
                    return new CatchPokemonRequest(this);
                }

                public bool Equals(CatchPokemonRequest other)
                {
                    if (ReferenceEquals(other, null))
                    {
                        return false;
                    }
                    if (ReferenceEquals(other, this))
                    {
                        return true;
                    }
                    if (EncounterId != other.EncounterId) return false;
                    if (Pokeball != other.Pokeball) return false;
                    if (NormalizedReticleSize != other.NormalizedReticleSize) return false;
                    if (SpawnPointGuid != other.SpawnPointGuid) return false;
                    if (HitPokemon != other.HitPokemon) return false;
                    if (SpinModifier != other.SpinModifier) return false;
                    if (NormalizedHitPosition != other.NormalizedHitPosition) return false;
                    return true;
                }

                public void WriteTo(pb::CodedOutputStream output)
                {
                    if (EncounterId != 0UL)
                    {
                        output.WriteRawTag(9);
                        output.WriteFixed64(EncounterId);
                    }
                    if (Pokeball != 0)
                    {
                        output.WriteRawTag(16);
                        output.WriteInt32(Pokeball);
                    }
                    if (NormalizedReticleSize != 0UL)
                    {
                        output.WriteRawTag(25);
                        output.WriteFixed64(NormalizedReticleSize);
                    }
                    if (SpawnPointGuid.Length != 0)
                    {
                        output.WriteRawTag(34);
                        output.WriteString(SpawnPointGuid);
                    }
                    if (HitPokemon != 0)
                    {
                        output.WriteRawTag(40);
                        output.WriteInt32(HitPokemon);
                    }
                    if (SpinModifier != 0UL)
                    {
                        output.WriteRawTag(49);
                        output.WriteFixed64(SpinModifier);
                    }
                    if (NormalizedHitPosition != 0UL)
                    {
                        output.WriteRawTag(57);
                        output.WriteFixed64(NormalizedHitPosition);
                    }
                }

                public int CalculateSize()
                {
                    var size = 0;
                    if (EncounterId != 0UL)
                    {
                        size += 1 + 8;
                    }
                    if (Pokeball != 0)
                    {
                        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Pokeball);
                    }
                    if (NormalizedReticleSize != 0UL)
                    {
                        size += 1 + 8;
                    }
                    if (SpawnPointGuid.Length != 0)
                    {
                        size += 1 + pb::CodedOutputStream.ComputeStringSize(SpawnPointGuid);
                    }
                    if (HitPokemon != 0)
                    {
                        size += 1 + pb::CodedOutputStream.ComputeInt32Size(HitPokemon);
                    }
                    if (SpinModifier != 0UL)
                    {
                        size += 1 + 8;
                    }
                    if (NormalizedHitPosition != 0UL)
                    {
                        size += 1 + 8;
                    }
                    return size;
                }

                public void MergeFrom(CatchPokemonRequest other)
                {
                    if (other == null)
                    {
                        return;
                    }
                    if (other.EncounterId != 0UL)
                    {
                        EncounterId = other.EncounterId;
                    }
                    if (other.Pokeball != 0)
                    {
                        Pokeball = other.Pokeball;
                    }
                    if (other.NormalizedReticleSize != 0UL)
                    {
                        NormalizedReticleSize = other.NormalizedReticleSize;
                    }
                    if (other.SpawnPointGuid.Length != 0)
                    {
                        SpawnPointGuid = other.SpawnPointGuid;
                    }
                    if (other.HitPokemon != 0)
                    {
                        HitPokemon = other.HitPokemon;
                    }
                    if (other.SpinModifier != 0UL)
                    {
                        SpinModifier = other.SpinModifier;
                    }
                    if (other.NormalizedHitPosition != 0UL)
                    {
                        NormalizedHitPosition = other.NormalizedHitPosition;
                    }
                }

                public void MergeFrom(pb::CodedInputStream input)
                {
                    uint tag;
                    while ((tag = input.ReadTag()) != 0)
                    {
                        switch (tag)
                        {
                            default:
                                input.SkipLastField();
                                break;
                            case 9:
                            {
                                EncounterId = input.ReadFixed64();
                                break;
                            }
                            case 16:
                            {
                                Pokeball = input.ReadInt32();
                                break;
                            }
                            case 25:
                            {
                                NormalizedReticleSize = input.ReadFixed64();
                                break;
                            }
                            case 34:
                            {
                                SpawnPointGuid = input.ReadString();
                                break;
                            }
                            case 40:
                            {
                                HitPokemon = input.ReadInt32();
                                break;
                            }
                            case 49:
                            {
                                SpinModifier = input.ReadFixed64();
                                break;
                            }
                            case 57:
                            {
                                NormalizedHitPosition = input.ReadFixed64();
                                break;
                            }
                        }
                    }
                }

                public override bool Equals(object other)
                {
                    return Equals(other as CatchPokemonRequest);
                }

                public override int GetHashCode()
                {
                    var hash = 1;
                    if (EncounterId != 0UL) hash ^= EncounterId.GetHashCode();
                    if (Pokeball != 0) hash ^= Pokeball.GetHashCode();
                    if (NormalizedReticleSize != 0UL) hash ^= NormalizedReticleSize.GetHashCode();
                    if (SpawnPointGuid.Length != 0) hash ^= SpawnPointGuid.GetHashCode();
                    if (HitPokemon != 0) hash ^= HitPokemon.GetHashCode();
                    if (SpinModifier != 0UL) hash ^= SpinModifier.GetHashCode();
                    if (NormalizedHitPosition != 0UL) hash ^= NormalizedHitPosition.GetHashCode();
                    return hash;
                }

                partial void OnConstruction();

                public override string ToString()
                {
                    return pb::JsonFormatter.ToDiagnosticString(this);
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public sealed partial class SettingsGuid : pb::IMessage<SettingsGuid>
            {
                /// <summary>Field number for the "guid" field.</summary>
                public const int GuidFieldNumber = 1;

                private static readonly pb::MessageParser<SettingsGuid> _parser =
                    new pb::MessageParser<SettingsGuid>(() => new SettingsGuid());

                private pb::ByteString guid_ = pb::ByteString.Empty;

                public SettingsGuid()
                {
                    OnConstruction();
                }

                public SettingsGuid(SettingsGuid other) : this()
                {
                    guid_ = other.guid_;
                }

                public static pb::MessageParser<SettingsGuid> Parser
                {
                    get { return _parser; }
                }

                public static pbr::MessageDescriptor Descriptor
                {
                    get { return global::PokemonGo.RocketAPI.GeneratedCode.Request.Descriptor.NestedTypes[11]; }
                }

                public pb::ByteString Guid
                {
                    get { return guid_; }
                    set { guid_ = pb::ProtoPreconditions.CheckNotNull(value, "value"); }
                }

                pbr::MessageDescriptor pb::IMessage.Descriptor
                {
                    get { return Descriptor; }
                }

                public SettingsGuid Clone()
                {
                    return new SettingsGuid(this);
                }

                public bool Equals(SettingsGuid other)
                {
                    if (ReferenceEquals(other, null))
                    {
                        return false;
                    }
                    if (ReferenceEquals(other, this))
                    {
                        return true;
                    }
                    if (Guid != other.Guid) return false;
                    return true;
                }

                public void WriteTo(pb::CodedOutputStream output)
                {
                    if (Guid.Length != 0)
                    {
                        output.WriteRawTag(10);
                        output.WriteBytes(Guid);
                    }
                }

                public int CalculateSize()
                {
                    var size = 0;
                    if (Guid.Length != 0)
                    {
                        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Guid);
                    }
                    return size;
                }

                public void MergeFrom(SettingsGuid other)
                {
                    if (other == null)
                    {
                        return;
                    }
                    if (other.Guid.Length != 0)
                    {
                        Guid = other.Guid;
                    }
                }

                public void MergeFrom(pb::CodedInputStream input)
                {
                    uint tag;
                    while ((tag = input.ReadTag()) != 0)
                    {
                        switch (tag)
                        {
                            default:
                                input.SkipLastField();
                                break;
                            case 10:
                            {
                                Guid = input.ReadBytes();
                                break;
                            }
                        }
                    }
                }

                public override bool Equals(object other)
                {
                    return Equals(other as SettingsGuid);
                }

                public override int GetHashCode()
                {
                    var hash = 1;
                    if (Guid.Length != 0) hash ^= Guid.GetHashCode();
                    return hash;
                }

                partial void OnConstruction();

                public override string ToString()
                {
                    return pb::JsonFormatter.ToDiagnosticString(this);
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public sealed partial class Time : pb::IMessage<Time>
            {
                /// <summary>Field number for the "time" field.</summary>
                public const int Time_FieldNumber = 1;

                private static readonly pb::MessageParser<Time> _parser = new pb::MessageParser<Time>(() => new Time());
                private long time_;

                public Time()
                {
                    OnConstruction();
                }

                public Time(Time other) : this()
                {
                    time_ = other.time_;
                }

                public static pb::MessageParser<Time> Parser
                {
                    get { return _parser; }
                }

                public static pbr::MessageDescriptor Descriptor
                {
                    get { return global::PokemonGo.RocketAPI.GeneratedCode.Request.Descriptor.NestedTypes[12]; }
                }

                public long Time_
                {
                    get { return time_; }
                    set { time_ = value; }
                }

                pbr::MessageDescriptor pb::IMessage.Descriptor
                {
                    get { return Descriptor; }
                }

                public Time Clone()
                {
                    return new Time(this);
                }

                public bool Equals(Time other)
                {
                    if (ReferenceEquals(other, null))
                    {
                        return false;
                    }
                    if (ReferenceEquals(other, this))
                    {
                        return true;
                    }
                    if (Time_ != other.Time_) return false;
                    return true;
                }

                public void WriteTo(pb::CodedOutputStream output)
                {
                    if (Time_ != 0L)
                    {
                        output.WriteRawTag(8);
                        output.WriteInt64(Time_);
                    }
                }

                public int CalculateSize()
                {
                    var size = 0;
                    if (Time_ != 0L)
                    {
                        size += 1 + pb::CodedOutputStream.ComputeInt64Size(Time_);
                    }
                    return size;
                }

                public void MergeFrom(Time other)
                {
                    if (other == null)
                    {
                        return;
                    }
                    if (other.Time_ != 0L)
                    {
                        Time_ = other.Time_;
                    }
                }

                public void MergeFrom(pb::CodedInputStream input)
                {
                    uint tag;
                    while ((tag = input.ReadTag()) != 0)
                    {
                        switch (tag)
                        {
                            default:
                                input.SkipLastField();
                                break;
                            case 8:
                            {
                                Time_ = input.ReadInt64();
                                break;
                            }
                        }
                    }
                }

                public override bool Equals(object other)
                {
                    return Equals(other as Time);
                }

                public override int GetHashCode()
                {
                    var hash = 1;
                    if (Time_ != 0L) hash ^= Time_.GetHashCode();
                    return hash;
                }

                partial void OnConstruction();

                public override string ToString()
                {
                    return pb::JsonFormatter.ToDiagnosticString(this);
                }
            }
        }

        #endregion
    }

    #endregion
}

#endregion Designer generated code