using System;
using System.Collections.Generic;
using System.Xml;
namespace PokemonGo.RocketAPI.Logic.Utils
{
    public class GPXReader
    {
        private XmlDocument _GPX = new XmlDocument();

        private string _Name = "";

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string Description = "";
        public string Author = "";
        public string EMail = "";
        public string Time = "";
        public string KeyWords = "";
        public string URL = "";
        public string URLName = "";
        public GPSBoundary Bounds = new GPSBoundary();
        public List<wpt> WayPoints = new List<wpt>();
        public List<rte> Routes = new List<rte>();
        public List<trk> Tracks = new List<trk>();

        public GPXReader(string XML)
        {
            if (!XML.Equals(""))
            {
                _GPX.LoadXml(XML);
                if ((_GPX.DocumentElement).Name.Equals("gpx"))
                {
                    XmlNodeList GPXNodes = ((_GPX.GetElementsByTagName("gpx"))[0]).ChildNodes;
                    foreach (XmlNode Node in GPXNodes)
                    {

                        switch (Node.Name)
                        {
                            case "name":
                                Name = Node.InnerText;
                                break;
                            case "desc":
                                Description = Node.InnerText;
                                break;
                            case "author":
                                Author = Node.InnerText;
                                break;
                            case "email":
                                EMail = Node.InnerText;
                                break;
                            case "time":
                                Time = Node.InnerText;
                                break;
                            case "keywords":
                                KeyWords = Node.InnerText;
                                break;
                            case "bounds":
                                Bounds = new GPSBoundary();
                                foreach (XmlAttribute Att in (Node).Attributes)
                                {
                                    switch (Att.Name)
                                    {
                                        case "minlat":
                                            Bounds.Min.lat = Att.Value;
                                            break;
                                        case "minlon":
                                            Bounds.Min.lon = Att.Value;
                                            break;
                                        case "maxlat":
                                            Bounds.Max.lat = Att.Value;
                                            break;
                                        case "maxlon":
                                            Bounds.Max.lon = Att.Value;
                                            break;
                                    }
                                }
                                break;
                            case "wpt":
                                wpt NewWayPoint = new wpt(Node);
                                WayPoints.Add(NewWayPoint);
                                break;
                            case "rte":
                                rte NewRoute = new rte(Node);
                                Routes.Add(NewRoute);
                                break;
                            case "trk":
                                trk Track = new trk(Node);
                                Tracks.Add(Track);
                                break;
                            case "url":
                                URL = Node.InnerText;
                                break;
                            case "urlname":
                                URLName = Node.InnerText;
                                break;
                            case "topografix:active_point":
                            case "topografix:map":
                                break;
                            default:
                                Logger.Write("Unhandled data in GPX file, attempting to skip.", LogLevel.Info);
                                break;
                        }
                    }
                }
            }
        }

        public class travelbug
        {
            public string ID = "";
            public string Reference = "";
            public string Groundspeak_Name = "";

            public travelbug(XmlNode TravelBugNode)
            {
                ID = TravelBugNode.Attributes["id"].Value.ToString();
                Reference = TravelBugNode.Attributes["ref"].Value.ToString();
                foreach (XmlNode TBChildNode in TravelBugNode.ChildNodes)
                {
                    switch (TBChildNode.Name)
                    {
                        case "groundspeak:name":
                            Groundspeak_Name = TBChildNode.InnerText;
                            break;
                        default:
                            throw new Exception("Unhandled Child Node: " + TBChildNode.Name);
                    }
                }
            }
        }

        public class cachelog
        {
            public string ID = "";
            public string Groundspeak_Date = "";
            public string Groundspeak_Type = "";
            public string Groundspeak_Finder = "";
            public string Groundspeak_FinderID = "";
            public string Groundspeak_Text = "";
            public string Groundspeak_TextEncoded = "";
            public GPSCoordinates Groundspeak_LogWayPoint = new GPSCoordinates();

            public cachelog(XmlNode ChildNode)
            {
                ID = ChildNode.Attributes["id"].Value.ToString();
                foreach (XmlNode Node in ChildNode.ChildNodes)
                {
                    switch (Node.Name)
                    {
                        case "groundspeak:date":
                            Groundspeak_Date = Node.InnerText;
                            break;
                        case "groundspeak:type":
                            Groundspeak_Type = Node.InnerText;
                            break;
                        case "groundspeak:finder":
                            Groundspeak_Finder = Node.InnerText;
                            Groundspeak_FinderID = Node.Attributes["id"].Value.ToString();
                            break;
                        case "groundspeak:text":
                            Groundspeak_Text = Node.InnerText;
                            Groundspeak_TextEncoded = Node.Attributes["encoded"].Value.ToString();
                            break;
                        case "groundspeak:log_wpt":
                            Groundspeak_LogWayPoint.lat = Node.Attributes["lat"].Value.ToString();
                            Groundspeak_LogWayPoint.lon = Node.Attributes["lon"].Value.ToString();
                            break;
                        default:
                            throw new Exception("Unhandled Child Node: " + Node.Name);
                    }
                }
            }
        }

        public class cache
        {
            public string ID = "";
            public string Available = "";
            public string Archived = "";
            public string Xmlns = "";

            public string Groundspeak_Name = "";
            public string Groundspeak_PlacedBy = "";
            public string Groundspeak_Owner = "";
            public string Groundspeak_OwnerID = "";
            public string Groundspeak_Type = "";
            public string Groundspeak_Container = "";
            public string Groundspeak_Difficulty = "";
            public string Groundspeak_Terrain = "";
            public string Groundspeak_Country = "";
            public string Groundspeak_State = "";
            public string Groundspeak_ShortDescription = "";
            public bool Groundspeak_ShortDescriptionIsHTML = false;
            public string Groundspeak_LongDescription = "";
            public bool Groundspeak_LongDescriptionIsHTML = false;
            public string Groundspeak_EncodedHint = "";

            public List<cachelog> Groundspeak_Logs = new List<cachelog>();
            public List<travelbug> Groundspeak_Travelbugs = new List<travelbug>();
            public List<Attribute> Groundspeak_Attributes = new List<Attribute>();

            public cache(XmlNode Node)
            {
                #region Attributes

                foreach (XmlAttribute Attribute in Node.Attributes)
                {
                    switch (Attribute.Name)
                    {
                        case "id":
                            ID = Attribute.Value;
                            break;
                        case "available":
                            Available = Attribute.Value;
                            break;
                        case "archived":
                            Archived = Attribute.Value;
                            break;
                        case "xmlns:groundspeak":
                            Xmlns = Attribute.Value;
                            break;
                        default:
                            throw new Exception("Unhandled Attribute: " + Attribute.Name);
                    }
                }
                #endregion Attributes

                foreach (XmlNode ChildNode in Node.ChildNodes)
                {
                    switch (ChildNode.Name)
                    {
                        case "groundspeak:name":
                            Groundspeak_Name = ChildNode.InnerText;
                            break;
                        case "groundspeak:placed_by":
                            Groundspeak_PlacedBy = ChildNode.InnerText;
                            break;
                        case "groundspeak:owner":
                            Groundspeak_Owner = ChildNode.InnerText;
                            Groundspeak_OwnerID = ChildNode.Attributes["id"].Value.ToString();
                            break;
                        case "groundspeak:type":
                            Groundspeak_Type = ChildNode.InnerText;
                            break;
                        case "groundspeak:container":
                            Groundspeak_Container = ChildNode.InnerText;
                            break;
                        case "groundspeak:difficulty":
                            Groundspeak_Difficulty = ChildNode.InnerText;
                            break;
                        case "groundspeak:terrain":
                            Groundspeak_Terrain = ChildNode.InnerText;
                            break;
                        case "groundspeak:country":
                            Groundspeak_Country = ChildNode.InnerText;
                            break;
                        case "groundspeak:state":
                            Groundspeak_State = ChildNode.InnerText;
                            break;
                        case "groundspeak:short_description":
                            Groundspeak_ShortDescription = ChildNode.InnerText;
                            if (ChildNode.Attributes["html"].Value.Equals("True"))
                            {
                                Groundspeak_ShortDescriptionIsHTML = true;
                            }
                            break;
                        case "groundspeak:long_description":
                            Groundspeak_LongDescription = ChildNode.InnerText;
                            if (ChildNode.Attributes["html"].Value.Equals("True"))
                            {
                                Groundspeak_LongDescriptionIsHTML = true;
                            }
                            break;
                        case "groundspeak:encoded_hints":
                            Groundspeak_EncodedHint = ChildNode.InnerText;
                            break;
                        case "groundspeak:logs":
                            foreach (XmlNode LogNode in ChildNode.ChildNodes)
                            {
                                cachelog Groundspeak_LogEntry = new cachelog(LogNode);
                                Groundspeak_Logs.Add(Groundspeak_LogEntry);
                            }
                            break;
                        case "groundspeak:travelbugs":
                            foreach (XmlNode TravelBugNode in ChildNode.ChildNodes)
                            {
                                travelbug Travelbug = new travelbug(TravelBugNode);
                                Groundspeak_Travelbugs.Add(Travelbug);
                            }
                            break;
                        case "groundspeak:attributes":
                            foreach (XmlNode AttributeNode in ChildNode.ChildNodes)
                            {
                                Attribute CacheAttribute = new Attribute(AttributeNode);
                                Groundspeak_Attributes.Add(CacheAttribute);
                            }
                            break;
                        default:
                            throw new Exception("Unhandled Child Node: " + ChildNode.Name);
                    }
                }
            }

            public cache()
            {
            }
        }


        //WayPoint contains Caches and other Objects

        public class wpt
        {
            public GPSCoordinates Coordinates = new GPSCoordinates();
            public string Name = "";
            public string Desc = "";
            public string Time = "";
            public string URLName = "";
            public string URL = "";
            public string Sym = "";
            public string Type = "";
            public string Ele = "";
            public string Cmt = "";
            public string Opencaching_Awesomeness = "";
            public string Opencaching_Difficulty = "";
            public string Opencaching_Terrain = "";
            public string Opencaching_Size = "";
            public string Opencaching_VerificationPhrase = "";
            public string Opencaching_VerificationNumber = "";
            public string Opencaching_VerificationQR = "";
            public string Opencaching_VerificationChirp = "";
            public string Opencaching_SeriesID = "";
            public string Opencaching_SeriesName = "";
            public List<string> Opencaching_Tags = new List<string>();

            public cache Groundspeak_Cache = new cache();

            public wpt(XmlNode Node)
            {
                Coordinates.lat = Node.Attributes["lat"].Value.ToString();
                Coordinates.lon = Node.Attributes["lon"].Value.ToString();
                foreach (XmlNode ChildNode in Node.ChildNodes)
                {
                    switch (ChildNode.Name)
                    {
                        case "time":
                            Time = ChildNode.InnerText;
                            break;
                        case "name":
                            Name = ChildNode.InnerText;
                            break;
                        case "desc":
                            Desc = ChildNode.InnerText;
                            break;
                        case "url":
                            URL = ChildNode.InnerText;
                            break;
                        case "urlname":
                            URLName = ChildNode.InnerText;
                            break;
                        case "sym":
                            Sym = ChildNode.InnerText;
                            break;
                        case "type":
                            Type = ChildNode.InnerText;
                            break;
                        case "ele":
                            Ele = ChildNode.InnerText;
                            break;
                        case "cmt":
                            Cmt = ChildNode.InnerText;
                            break;
                        case "groundspeak:cache":
                            Groundspeak_Cache = new cache(ChildNode);
                            break;
                        case "ox:opencaching":
                            foreach (XmlNode OpenCachingChildNode in ChildNode.ChildNodes)
                            {
                                switch (OpenCachingChildNode.Name)
                                {
                                    case "ox:ratings":
                                        foreach (XmlNode OpenCachingRatingsChildNode in OpenCachingChildNode.ChildNodes)
                                        {
                                            switch (OpenCachingRatingsChildNode.Name)
                                            {
                                                case "ox:awesomeness":
                                                    Opencaching_Awesomeness = (OpenCachingRatingsChildNode).InnerText;
                                                    break;
                                                case "ox:difficulty":
                                                    Opencaching_Difficulty = (OpenCachingRatingsChildNode).InnerText;
                                                    break;
                                                case "ox:terrain":
                                                    Opencaching_Terrain = (OpenCachingRatingsChildNode).InnerText;
                                                    break;
                                                case "ox:size":
                                                    Opencaching_Size = (OpenCachingRatingsChildNode).InnerText;
                                                    break;
                                                default:
                                                    throw new Exception("Unhandled for Child Object: " + OpenCachingRatingsChildNode.Name);
                                            }
                                        }
                                        break;
                                    case "ox:tags":
                                        foreach (XmlNode OpenCachingTagNode in OpenCachingChildNode.ChildNodes)
                                        {
                                            switch (OpenCachingTagNode.Name)
                                            {
                                                case "ox:tag":
                                                    Opencaching_Tags.Add((OpenCachingTagNode).InnerXml);
                                                    break;
                                                default:
                                                    throw new Exception("Unhandled for Child Object: " + OpenCachingTagNode.Name);
                                            }
                                        }

                                        break;
                                    case "ox:verification":
                                        foreach (XmlNode OpenCachingVerificationNode in OpenCachingChildNode.ChildNodes)
                                        {
                                            switch (OpenCachingVerificationNode.Name)
                                            {
                                                case "ox:phrase":
                                                    Opencaching_VerificationPhrase = OpenCachingChildNode.InnerText;
                                                    break;
                                                case "ox:number":
                                                    Opencaching_VerificationNumber = OpenCachingChildNode.InnerText;
                                                    break;
                                                case "ox:QR":
                                                    Opencaching_VerificationQR = OpenCachingChildNode.InnerText;
                                                    break;
                                                case "ox:chirp":
                                                    Opencaching_VerificationChirp = OpenCachingChildNode.InnerText;
                                                    break;
                                                default:
                                                    throw new Exception("Unhandled for Child Object: " + OpenCachingVerificationNode.Name);
                                            }
                                        }
                                        break;
                                    case "ox:series":
                                        Opencaching_SeriesName = OpenCachingChildNode.InnerText;
                                        Opencaching_SeriesID = (OpenCachingChildNode).Attributes["id"].Value.ToString();
                                        break;
                                    default:
                                        throw new Exception("Unhandled for Child Object: " + OpenCachingChildNode.Name);
                                }
                            }
                            break;
                        default:
                            throw new Exception("Unhandled for Child Object: " + ChildNode.Name);
                    }
                }
            }
        }

        public class GPSBoundary
        {
            public GPSCoordinates Min = new GPSCoordinates();
            public GPSCoordinates Max = new GPSCoordinates();

            public GPSBoundary()
            {
            }
        }

        public class GPSCoordinates
        {
            public string lat = "";
            public string lon = "";

            public GPSCoordinates()
            {
            }
        }

        public class Attribute : IComparable<Attribute>
        {
            public string ID = "";
            public string Inc = "";
            public string Description = "";

            public Attribute(XmlNode AttributeNode)
            {
                ID = AttributeNode.Attributes["id"].Value.ToString();
                Inc = AttributeNode.Attributes["inc"].Value.ToString();
                Description = AttributeNode.InnerText;
            }

            public Attribute()
            {
            }


            public int CompareTo(Attribute other)
            {
                return this.Description.CompareTo(other.Description);

            }
        }

        //Route ans Route Points

        public class rte
        {
            public string Name = "";
            public string Desc = "";
            public string Number = "";
            public string URL = "";
            public string URLName = "";
            List<rtept> RoutePoints = new List<rtept>();

            public rte(XmlNode Node)
            {
                foreach (XmlNode ChildNode in Node.ChildNodes)
                {
                    switch (ChildNode.Name)
                    {
                        case "name":
                            Name = ChildNode.InnerText;
                            break;
                        case "desc":
                            Desc = ChildNode.InnerText;
                            break;
                        case "number":
                            Number = ChildNode.InnerText;
                            break;
                        case "rtept":
                            rtept RoutePoint = new rtept(ChildNode);
                            RoutePoints.Add(RoutePoint);
                            break;
                        case "url":
                            URL = ChildNode.InnerText;
                            break;
                        case "urlname":
                            URLName = ChildNode.InnerText;
                            break;
                        case "topografix:color":
                            break;
                        default:
                            throw new Exception("Unhandled for Child Object: " + ChildNode.Name);
                    }
                }
            }
        }

        public class rtept
        {
            public string Lat = "";
            public string Lon = "";
            public string Ele = "";
            public string Time = "";
            public string Name = "";
            public string Cmt = "";
            public string Desc = "";
            public string Sym = "";
            public string Type = "";
            public string URL = "";
            public string URLName = "";

            public rtept(XmlNode Node)
            {
                Lat = Node.Attributes["lat"].Value.ToString();
                Lon = Node.Attributes["lon"].Value.ToString();
                foreach (XmlNode ChildNode in Node.ChildNodes)
                {
                    switch (ChildNode.Name)
                    {
                        case "ele":
                            Ele = ChildNode.InnerText;
                            break;
                        case "time":
                            Time = ChildNode.InnerText;
                            break;
                        case "name":
                            Name = ChildNode.InnerText;
                            break;
                        case "cmt":
                            Cmt = ChildNode.InnerText;
                            break;
                        case "desc":
                            Desc = ChildNode.InnerText;
                            break;
                        case "sym":
                            Sym = ChildNode.InnerText;
                            break;
                        case "type":
                            Type = ChildNode.InnerText;
                            break;
                        case "url":
                            URL = ChildNode.InnerText;
                            break;
                        case "urlname":
                            URLName = ChildNode.InnerText;
                            break;
                        case "topografix:leg":
                            break;
                        default:
                            throw new Exception("Unhandled for Child Object: " + ChildNode.Name);
                    }
                }
            }
        }

        //Tracks

        public class trk
        {
            public string Name = "";
            public string Desc = "";
            public string Number = "";
            public string URL = "";
            public string URLName = "";
            public List<trkseg> Segments = new List<trkseg>();

            public trk(XmlNode Node)
            {
                foreach (XmlNode ChildNode in Node)
                {
                    switch (ChildNode.Name)
                    {
                        case "name":
                            Name = ChildNode.InnerText;
                            break;
                        case "desc":
                            Desc = ChildNode.InnerText;
                            break;
                        case "number":
                            Number = ChildNode.InnerText;
                            break;
                        case "trkseg":
                            trkseg Segment = new trkseg(ChildNode);
                            Segments.Add(Segment);
                            break;
                        case "url":
                            URL = ChildNode.InnerText;
                            break;
                        case "urlname":
                            URLName = ChildNode.InnerText;
                            break;
                        case "topografix:color":
                            break;
                        default:
                            throw new Exception("Unhandled for Child Object: " + ChildNode.Name);
                    }
                }
            }
        }

        public class trkseg
        {
            public List<trkpt> TrackPoints = new List<trkpt>();

            public trkseg(XmlNode Node)
            {
                foreach (XmlNode ChildNode in Node)
                {
                    switch (ChildNode.Name)
                    {
                        case "trkpt":
                            trkpt TrackPoint = new trkpt(ChildNode);
                            TrackPoints.Add(TrackPoint);
                            break;
                        default:
                            throw new Exception("Unhandled for Child Object: " + ChildNode.Name);
                    }
                }
            }
        }

        public class trkpt
        {
            public string Lat = "";
            public string Lon = "";
            public string Sym = "";
            public string Ele = "0";
            public string Time = "";
            public string Cmt = "";
            public string Name = "";
            public string Desc = "";

            public trkpt(XmlNode Node)
            {
                Lat = Node.Attributes["lat"].Value.ToString();
                Lon = Node.Attributes["lon"].Value.ToString();
                foreach (XmlNode ChildNode in Node)
                {
                    switch (ChildNode.Name)
                    {
                        case "sym":
                            Sym = ChildNode.InnerText;
                            break;
                        case "ele":
                            Ele = ChildNode.InnerText;
                            break;
                        case "time":
                            Time = ChildNode.InnerText;
                            break;
                        case "cmt":
                            Cmt = ChildNode.InnerText;
                            break;
                        case "name":
                            Name = ChildNode.InnerText;
                            break;
                        case "desc":
                            Desc = ChildNode.InnerText;
                            break;
                        default:
                            throw new Exception("Unhandled for Child Object: " + ChildNode.Name);
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