using Caching;
using GeoCoordinatePortable;
using PoGo.NecroBot.Logic.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Service.Elevation
{
    public abstract class BaseElevationService
    {
        protected ISession _session;
        protected LRUCache<string, double> _cache;
        protected string _apiKey;

        public abstract double GetElevationFromWebService(double lat, double lng);

        public BaseElevationService(ISession session, LRUCache<string, double> cache)
        {
            _session = session;
            _cache = cache;
        }

        public string GetCacheKey(double lat, double lng)
        {
            return Math.Round(lat, 3) + "," + Math.Round(lng, 3);
        }

        public string GetCacheKey(GeoCoordinate position)
        {
            return GetCacheKey(position.Latitude, position.Longitude);
        }

        public double GetElevation(double lat, double lng)
        {
            string cacheKey = GetCacheKey(lat, lng);
            double elevation;
            bool success = _cache.TryGetValue(cacheKey, out elevation);
            if (!success)
            {
                elevation = GetElevationFromWebService(lat, lng);
                if (elevation == 0)
                {
                    // Error getting elevation so just return 0.
                    return 0;
                }
                else
                {
                    _cache.Add(cacheKey, elevation);
                }
            }

            // Always return a slightly random elevation.
            return GetRandomElevation(elevation);
        }

        public void UpdateElevation(ref GeoCoordinate position)
        {
            double elevation = GetElevation(position.Latitude, position.Longitude);
            // Only update the position elevation if we got a non-zero elevation.
            if (elevation != 0)
            {
                position.Altitude = elevation;
            }
        }

        public double GetRandomElevation(double elevation)
        {
            return elevation + (new Random().NextDouble() * 5);
        }
    }
}
