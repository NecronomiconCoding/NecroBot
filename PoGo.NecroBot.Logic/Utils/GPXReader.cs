#region using directives

#region using directives

using System;
using System.Collections.Generic;
using System.Xml;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;

#endregion

// ReSharper disable All

#endregion

namespace PoGo.NecroBot.Logic.Utils
{
    public class GpxReader
    {
        private readonly XmlDocument _gpx = new XmlDocument();

        public string Author = "";
        public GpsBoundary Bounds = new GpsBoundary();

        public string Description = "";
        public string EMail = "";
        public string KeyWords = "";
        public List<Rte> Routes = new List<Rte>();
        public string Time = "";
        public List<Trk> Tracks = new List<Trk>();
        public string Url = "";
        public string UrlName = "";
        public List<Wpt> WayPoints = new List<Wpt>();

        public GpxReader(string xml, ISession session) : this(xml, session?.Translation)
        {
        }

        public GpxReader(string xml, ITranslation translation)
        {
            if (xml.Equals("")) return;
            _gpx.LoadXml(xml);
            if (_gpx.DocumentElement == null || !_gpx.DocumentElement.Name.Equals("gpx")) return;
            var gpxNodes = _gpx.GetElementsByTagName("gpx")[0].ChildNodes;
            foreach (XmlNode node in gpxNodes)
            {
                switch (node.Name)
                {
                    case "name":
                        Name = node.InnerText;
                        break;
                    case "desc":
                        Description = node.InnerText;
                        break;
                    case "author":
                        Author = node.InnerText;
                        break;
                    case "email":
                        EMail = node.InnerText;
                        break;
                    case "time":
                        Time = node.InnerText;
                        break;
                    case "keywords":
                        KeyWords = node.InnerText;
                        break;
                    case "bounds":
                        Bounds = new GpsBoundary();
                        if (node.Attributes != null)
                            foreach (XmlAttribute att in node.Attributes)
                            {
                                switch (att.Name)
                                {
                                    case "minlat":
                                        Bounds.Min.Lat = att.Value;
                                        break;
                                    case "minlon":
                                        Bounds.Min.Lon = att.Value;
                                        break;
                                    case "maxlat":
                                        Bounds.Max.Lat = att.Value;
                                        break;
                                    case "maxlon":
                                        Bounds.Max.Lon = att.Value;
                                        break;
                                }
                            }
                        break;
                    case "wpt":
                        var newWayPoint = new Wpt(node);
                        WayPoints.Add(newWayPoint);
                        break;
                    case "rte":
                        var newRoute = new Rte(node);
                        Routes.Add(newRoute);
                        break;
                    case "trk":
                        var track = new Trk(node);
                        Tracks.Add(track);
                        break;
                    case "url":
                        Url = node.InnerText;
                        break;
                    case "urlname":
                        UrlName = node.InnerText;
                        break;
                    case "topografix:active_point":
                    case "topografix:map":
                        break;
                    default:
                        if (translation != null)
                        {
                            Logger.Write(translation.GetTranslation(TranslationString.UnhandledGpxData),
                                LogLevel.Info);
                        }
                        break;
                }
            }
        }

        public string Name { get; set; } = "";

        public class Travelbug
        {
            public string GroundspeakName = "";
            public string Id;
            public string Reference;

            public Travelbug(XmlNode travelBugNode)
            {
                Id = travelBugNode.Attributes?["id"].Value;
                Reference = travelBugNode.Attributes?["ref"].Value;
                foreach (XmlNode tbChildNode in travelBugNode.ChildNodes)
                {
                    switch (tbChildNode.Name)
                    {
                        case "groundspeak:name":
                            GroundspeakName = tbChildNode.InnerText;
                            break;
                        default:
                            throw new Exception("Unhandled Child Node: " + tbChildNode.Name);
                    }
                }
            }
        }

        public class Cachelog
        {
            public string GroundspeakDate = "";
            public string GroundspeakFinder = "";
            public string GroundspeakFinderId = "";
            public GpsCoordinates GroundspeakLogWayPoint = new GpsCoordinates();
            public string GroundspeakText = "";
            public string GroundspeakTextEncoded = "";
            public string GroundspeakType = "";
            public string Id;

            public Cachelog(XmlNode childNode)
            {
                Id = childNode.Attributes?["id"].Value;
                foreach (XmlNode node in childNode.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "groundspeak:date":
                            GroundspeakDate = node.InnerText;
                            break;
                        case "groundspeak:type":
                            GroundspeakType = node.InnerText;
                            break;
                        case "groundspeak:finder":
                            GroundspeakFinder = node.InnerText;
                            GroundspeakFinderId = node.Attributes?["id"].Value;
                            break;
                        case "groundspeak:text":
                            GroundspeakText = node.InnerText;
                            GroundspeakTextEncoded = node.Attributes?["encoded"].Value;
                            break;
                        case "groundspeak:log_wpt":
                            GroundspeakLogWayPoint.Lat = node.Attributes?["lat"].Value;
                            GroundspeakLogWayPoint.Lon = node.Attributes?["lon"].Value;
                            break;
                        default:
                            throw new Exception("Unhandled Child Node: " + node.Name);
                    }
                }
            }
        }

        public class Cache
        {
            public string Archived = "";
            public string Available = "";
            public List<Attribute> GroundspeakAttributes = new List<Attribute>();
            public string GroundspeakContainer = "";
            public string GroundspeakCountry = "";
            public string GroundspeakDifficulty = "";
            public string GroundspeakEncodedHint = "";

            public List<Cachelog> GroundspeakLogs = new List<Cachelog>();
            public string GroundspeakLongDescription = "";
            public bool GroundspeakLongDescriptionIsHtml;

            public string GroundspeakName = "";
            public string GroundspeakOwner = "";
            public string GroundspeakOwnerId = "";
            public string GroundspeakPlacedBy = "";
            public string GroundspeakShortDescription = "";
            public bool GroundspeakShortDescriptionIsHtml;
            public string GroundspeakState = "";
            public string GroundspeakTerrain = "";
            public List<Travelbug> GroundspeakTravelbugs = new List<Travelbug>();
            public string GroundspeakType = "";
            public string Id = "";
            public string Xmlns = "";

            public Cache(XmlNode node)
            {
                #region Attributes

                if (node.Attributes == null) return;
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    switch (attribute.Name)
                    {
                        case "id":
                            Id = attribute.Value;
                            break;
                        case "available":
                            Available = attribute.Value;
                            break;
                        case "archived":
                            Archived = attribute.Value;
                            break;
                        case "xmlns:groundspeak":
                            Xmlns = attribute.Value;
                            break;
                        default:
                            throw new Exception("Unhandled Attribute: " + attribute.Name);
                    }
                }

                #endregion Attributes

                foreach (XmlNode childNode in node.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "groundspeak:name":
                            GroundspeakName = childNode.InnerText;
                            break;
                        case "groundspeak:placed_by":
                            GroundspeakPlacedBy = childNode.InnerText;
                            break;
                        case "groundspeak:owner":
                            GroundspeakOwner = childNode.InnerText;
                            GroundspeakOwnerId = childNode.Attributes?["id"].Value;
                            break;
                        case "groundspeak:type":
                            GroundspeakType = childNode.InnerText;
                            break;
                        case "groundspeak:container":
                            GroundspeakContainer = childNode.InnerText;
                            break;
                        case "groundspeak:difficulty":
                            GroundspeakDifficulty = childNode.InnerText;
                            break;
                        case "groundspeak:terrain":
                            GroundspeakTerrain = childNode.InnerText;
                            break;
                        case "groundspeak:country":
                            GroundspeakCountry = childNode.InnerText;
                            break;
                        case "groundspeak:state":
                            GroundspeakState = childNode.InnerText;
                            break;
                        case "groundspeak:short_description":
                            GroundspeakShortDescription = childNode.InnerText;
                            if (childNode.Attributes != null && childNode.Attributes["html"].Value.Equals("True"))
                            {
                                GroundspeakShortDescriptionIsHtml = true;
                            }
                            break;
                        case "groundspeak:long_description":
                            GroundspeakLongDescription = childNode.InnerText;
                            if (childNode.Attributes != null && childNode.Attributes["html"].Value.Equals("True"))
                            {
                                GroundspeakLongDescriptionIsHtml = true;
                            }
                            break;
                        case "groundspeak:encoded_hints":
                            GroundspeakEncodedHint = childNode.InnerText;
                            break;
                        case "groundspeak:logs":
                            foreach (XmlNode logNode in childNode.ChildNodes)
                            {
                                var groundspeakLogEntry = new Cachelog(logNode);
                                GroundspeakLogs.Add(groundspeakLogEntry);
                            }
                            break;
                        case "groundspeak:travelbugs":
                            foreach (XmlNode travelBugNode in childNode.ChildNodes)
                            {
                                var travelbug = new Travelbug(travelBugNode);
                                GroundspeakTravelbugs.Add(travelbug);
                            }
                            break;
                        case "groundspeak:attributes":
                            foreach (XmlNode attributeNode in childNode.ChildNodes)
                            {
                                var cacheAttribute = new Attribute(attributeNode);
                                GroundspeakAttributes.Add(cacheAttribute);
                            }
                            break;
                        default:
                            throw new Exception("Unhandled Child Node: " + childNode.Name);
                    }
                }
            }

            public Cache()
            {
            }
        }


        //WayPoint contains Caches and other Objects

        public class Wpt
        {
            public string Cmt = "";
            public GpsCoordinates Coordinates = new GpsCoordinates();
            public string Desc = "";
            public string Ele = "";

            public Cache GroundspeakCache = new Cache();
            public string Name = "";
            public string OpencachingAwesomeness = "";
            public string OpencachingDifficulty = "";
            public string OpencachingSeriesId = "";
            public string OpencachingSeriesName = "";
            public string OpencachingSize = "";
            public List<string> OpencachingTags = new List<string>();
            public string OpencachingTerrain = "";
            public string OpencachingVerificationChirp = "";
            public string OpencachingVerificationNumber = "";
            public string OpencachingVerificationPhrase = "";
            public string OpencachingVerificationQr = "";
            public string Sym = "";
            public string Time = "";
            public string Type = "";
            public string Url = "";
            public string UrlName = "";

            public Wpt(XmlNode node)
            {
                Coordinates.Lat = node.Attributes?["lat"].Value;
                Coordinates.Lon = node.Attributes?["lon"].Value;
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "time":
                            Time = childNode.InnerText;
                            break;
                        case "name":
                            Name = childNode.InnerText;
                            break;
                        case "desc":
                            Desc = childNode.InnerText;
                            break;
                        case "url":
                            Url = childNode.InnerText;
                            break;
                        case "urlname":
                            UrlName = childNode.InnerText;
                            break;
                        case "sym":
                            Sym = childNode.InnerText;
                            break;
                        case "type":
                            Type = childNode.InnerText;
                            break;
                        case "ele":
                            Ele = childNode.InnerText;
                            break;
                        case "cmt":
                            Cmt = childNode.InnerText;
                            break;
                        case "groundspeak:cache":
                            GroundspeakCache = new Cache(childNode);
                            break;
                        case "ox:opencaching":
                            foreach (XmlNode openCachingChildNode in childNode.ChildNodes)
                            {
                                switch (openCachingChildNode.Name)
                                {
                                    case "ox:ratings":
                                        foreach (XmlNode openCachingRatingsChildNode in openCachingChildNode.ChildNodes)
                                        {
                                            switch (openCachingRatingsChildNode.Name)
                                            {
                                                case "ox:awesomeness":
                                                    OpencachingAwesomeness = openCachingRatingsChildNode.InnerText;
                                                    break;
                                                case "ox:difficulty":
                                                    OpencachingDifficulty = openCachingRatingsChildNode.InnerText;
                                                    break;
                                                case "ox:terrain":
                                                    OpencachingTerrain = openCachingRatingsChildNode.InnerText;
                                                    break;
                                                case "ox:size":
                                                    OpencachingSize = openCachingRatingsChildNode.InnerText;
                                                    break;
                                                default:
                                                    throw new Exception("Unhandled for Child Object: " +
                                                                        openCachingRatingsChildNode.Name);
                                            }
                                        }
                                        break;
                                    case "ox:tags":
                                        foreach (XmlNode openCachingTagNode in openCachingChildNode.ChildNodes)
                                        {
                                            switch (openCachingTagNode.Name)
                                            {
                                                case "ox:tag":
                                                    OpencachingTags.Add(openCachingTagNode.InnerXml);
                                                    break;
                                                default:
                                                    throw new Exception("Unhandled for Child Object: " +
                                                                        openCachingTagNode.Name);
                                            }
                                        }

                                        break;
                                    case "ox:verification":
                                        foreach (XmlNode openCachingVerificationNode in openCachingChildNode.ChildNodes)
                                        {
                                            switch (openCachingVerificationNode.Name)
                                            {
                                                case "ox:phrase":
                                                    OpencachingVerificationPhrase = openCachingChildNode.InnerText;
                                                    break;
                                                case "ox:number":
                                                    OpencachingVerificationNumber = openCachingChildNode.InnerText;
                                                    break;
                                                case "ox:QR":
                                                    OpencachingVerificationQr = openCachingChildNode.InnerText;
                                                    break;
                                                case "ox:chirp":
                                                    OpencachingVerificationChirp = openCachingChildNode.InnerText;
                                                    break;
                                                default:
                                                    throw new Exception("Unhandled for Child Object: " +
                                                                        openCachingVerificationNode.Name);
                                            }
                                        }
                                        break;
                                    case "ox:series":
                                        OpencachingSeriesName = openCachingChildNode.InnerText;
                                        OpencachingSeriesId = openCachingChildNode.Attributes?["id"].Value;
                                        break;
                                    default:
                                        throw new Exception("Unhandled for Child Object: " + openCachingChildNode.Name);
                                }
                            }
                            break;
                        default:
                            throw new Exception("Unhandled for Child Object: " + childNode.Name);
                    }
                }
            }
        }

        public class GpsBoundary
        {
            public GpsCoordinates Max = new GpsCoordinates();
            public GpsCoordinates Min = new GpsCoordinates();
        }

        public class GpsCoordinates
        {
            public string Lat = "";
            public string Lon = "";
        }

        public class Attribute : IComparable<Attribute>
        {
            public string Description = "";
            public string Id = "";
            public string Inc = "";

            public Attribute(XmlNode attributeNode)
            {
                Id = attributeNode.Attributes?["id"].Value;
                Inc = attributeNode.Attributes?["inc"].Value;
                Description = attributeNode.InnerText;
            }

            public Attribute()
            {
            }


            public int CompareTo(Attribute other)
            {
                return string.Compare(Description, other.Description, StringComparison.Ordinal);
            }
        }

        //Route ans Route Points

        public class Rte
        {
            public string Desc = "";
            public string Name = "";
            public string Number = "";
            // private readonly List<Rtept> _routePoints = new List<Rtept>();
            public string Url = "";
            public string UrlName = "";

            public Rte(XmlNode node)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "name":
                            Name = childNode.InnerText;
                            break;
                        case "desc":
                            Desc = childNode.InnerText;
                            break;
                        case "number":
                            Number = childNode.InnerText;
                            break;
                        case "rtept":
                            //var routePoint = new Rtept(childNode);
                            //_routePoints.Add(routePoint);
                            break;
                        case "url":
                            Url = childNode.InnerText;
                            break;
                        case "urlname":
                            UrlName = childNode.InnerText;
                            break;
                        case "topografix:color":
                            break;
                        default:
                            throw new Exception("Unhandled for Child Object: " + childNode.Name);
                    }
                }
            }
        }

        public class Rtept
        {
            public string Cmt = "";
            public string Desc = "";
            public string Ele = "";
            public string Lat;
            public string Lon;
            public string Name = "";
            public string Sym = "";
            public string Time = "";
            public string Type = "";
            public string Url = "";
            public string UrlName = "";

            public Rtept(XmlNode node)
            {
                Lat = node.Attributes?["lat"].Value;
                Lon = node.Attributes?["lon"].Value;
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "ele":
                            Ele = childNode.InnerText;
                            break;
                        case "time":
                            Time = childNode.InnerText;
                            break;
                        case "name":
                            Name = childNode.InnerText;
                            break;
                        case "cmt":
                            Cmt = childNode.InnerText;
                            break;
                        case "desc":
                            Desc = childNode.InnerText;
                            break;
                        case "sym":
                            Sym = childNode.InnerText;
                            break;
                        case "type":
                            Type = childNode.InnerText;
                            break;
                        case "url":
                            Url = childNode.InnerText;
                            break;
                        case "urlname":
                            UrlName = childNode.InnerText;
                            break;
                        case "topografix:leg":
                            break;
                        default:
                            throw new Exception("Unhandled for Child Object: " + childNode.Name);
                    }
                }
            }
        }

        //Tracks

        public class Trk
        {
            public string Desc = "";
            public string Name = "";
            public string Number = "";
            public List<Trkseg> Segments = new List<Trkseg>();
            public string Url = "";
            public string UrlName = "";

            public Trk(XmlNode node)
            {
                foreach (XmlNode childNode in node)
                {
                    switch (childNode.Name)
                    {
                        case "name":
                            Name = childNode.InnerText;
                            break;
                        case "desc":
                            Desc = childNode.InnerText;
                            break;
                        case "number":
                            Number = childNode.InnerText;
                            break;
                        case "trkseg":
                            var segment = new Trkseg(childNode);
                            Segments.Add(segment);
                            break;
                        case "url":
                            Url = childNode.InnerText;
                            break;
                        case "urlname":
                            UrlName = childNode.InnerText;
                            break;
                        case "topografix:color":
                            break;
                        default:
                            throw new Exception("Unhandled for Child Object: " + childNode.Name);
                    }
                }
            }
        }

        public class Trkseg
        {
            public List<Trkpt> TrackPoints = new List<Trkpt>();

            public Trkseg(XmlNode node)
            {
                foreach (XmlNode childNode in node)
                {
                    switch (childNode.Name)
                    {
                        case "trkpt":
                            var trackPoint = new Trkpt(childNode);
                            TrackPoints.Add(trackPoint);
                            break;
                        default:
                            throw new Exception("Unhandled for Child Object: " + childNode.Name);
                    }
                }
            }
        }

        public class Trkpt
        {
            public string Cmt;
            public string Desc;
            public string Ele;
            public string Lat;
            public string Lon;
            public string Name;
            public string Sym;
            public string Time;

            public Trkpt(XmlNode node)
            {
                Lat = node.Attributes?["lat"].Value;
                Lon = node.Attributes?["lon"].Value;
                foreach (XmlNode childNode in node)
                {
                    switch (childNode.Name)
                    {
                        case "sym":
                            Sym = childNode.InnerText;
                            break;
                        case "ele":
                            Ele = childNode.InnerText;
                            break;
                        case "time":
                            Time = childNode.InnerText;
                            break;
                        case "cmt":
                            Cmt = childNode.InnerText;
                            break;
                        case "name":
                            Name = childNode.InnerText;
                            break;
                        case "desc":
                            Desc = childNode.InnerText;
                            break;
                        default:
                            throw new Exception("Unhandled for Child Object: " + childNode.Name);
                    }
                }
            }

            public override string ToString()
            {
                return "Latitude: " + Lat + " Longitude: " + Lon + " Elevation: " + Ele;
            }
        }
    }
}