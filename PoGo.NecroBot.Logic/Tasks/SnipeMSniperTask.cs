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
            string pth = Path.Combine(session.LogicSettings.ProfilePath, "SnipeMS.json");
            try
            {
                if (session.LogicSettings.CatchPokemon == true &&
                    session.LogicSettings.SnipeAtPokestops == false &&
                    session.LogicSettings.UseSnipeLocationServer == false &&
                    session.LogicSettings.EnableHumanWalkingSnipe == false
                    )//extra security
                {
                    if (!await SnipePokemonTask.CheckPokeballsToSnipe(session.LogicSettings.MinPokeballsWhileSnipe + 1, session, cancellationToken))
                        return;

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
                File.Delete(pth);//fixing deserialize errors
                ErrorEvent ee = new ErrorEvent { Message = ex.Message };
                if (ex.InnerException != null) ee.Message = ex.InnerException.Message;
                session.EventDispatcher.Send(ee);
            }
        }


    }
}
