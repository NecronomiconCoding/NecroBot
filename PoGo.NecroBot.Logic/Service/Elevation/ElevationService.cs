using Caching;
using PoGo.NecroBot.Logic.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Service.Elevation
{
    public class ElevationService
    {
        private MapQuestElevationService mapQuestService;
        private GoogleElevationService googleService;
        private ISession _session;
        LRUCache<string, double> cache = new LRUCache<string, double>(capacity: 500);

        public ElevationService(ISession session)
        {
            _session = session;
            mapQuestService = new MapQuestElevationService(session, cache);
            googleService = new GoogleElevationService(session, cache);
        }

        public double GetElevation(double lat, double lng)
        {
            // First try Google service
            double elevation = googleService.GetElevation(lat, lng);
            if (elevation == 0)
            {
                // Fallback to MapQuest service
                elevation = mapQuestService.GetElevation(lat, lng);
            }

            return elevation;
        }
    }


}
