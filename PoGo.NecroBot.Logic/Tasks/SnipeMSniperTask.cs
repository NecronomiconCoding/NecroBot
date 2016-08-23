using PoGo.NecroBot.Logic.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Model.Settings;
using PoGo.NecroBot.Logic.Event;
using POGOProtos.Enums;
using Newtonsoft.Json;
using System.IO;

namespace PoGo.NecroBot.Logic.Tasks
{
    public class MSniperInfo
    {
        public PokemonId Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public static class SnipeMSniperTask
    {
        public static async Task CheckMSniperLocation(ISession session, CancellationToken cancellationToken)
        {
            try
            {
                if (session.LogicSettings.CatchPokemon)//config is checking
                {
                    string pth = Path.Combine(session.LogicSettings.ProfilePath, "SnipeMS.json");
                    if (!File.Exists(pth))
                        return;
                    StreamReader sr = new StreamReader(pth, Encoding.UTF8);
                    string jsn = sr.ReadToEnd();
                    sr.Close();
                    List<MSniperInfo> MSniperLocation = JsonConvert.DeserializeObject<List<MSniperInfo>>(jsn);
                    File.Delete(pth);
                    foreach (var location in MSniperLocation)
                    {
                        session.EventDispatcher.Send(new SnipeScanEvent
                        {
                            Bounds = new Location(location.Latitude, location.Longitude),
                            PokemonId = location.Id,
                            Source = "MSniper"
                        });
                        await SnipePokemonTask.Snipe(session, session.LogicSettings.PokemonToSnipe.Pokemon, location.Latitude, location.Longitude, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                session.EventDispatcher.Send(new ErrorEvent { Message = ex.Message });
            }
        }


    }
}
