using System;
using System.Collections.Generic;
using C5;
using GeoCoordinatePortable;

namespace PoGo.NecroBot.Logic.Model.Google.GoogleObjects
{
    public class Polyline
    {
        public string points { get; set; }

        public List<GeoCoordinate> DecodePoly()
        {
            var poly = new List<GeoCoordinate>();
            if (string.IsNullOrEmpty(points))
                throw new ArgumentNullException("polyline");

            char[] polylineChars = points.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            while (index < polylineChars.Length)
            {
                // calculate next latitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylineChars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length)
                    break;

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                //calculate next longitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylineChars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length && next5bits >= 32)
                    break;

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                poly.Add(new GeoCoordinate()
                {
                    Latitude = Convert.ToDouble(currentLat) / 1E5,
                    Longitude = Convert.ToDouble(currentLng) / 1E5
                });
            }

            return poly;
        }
    }
}