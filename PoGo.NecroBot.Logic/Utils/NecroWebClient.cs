using System;
using System.Net;

namespace PoGo.NecroBot.Logic.Utils
{
    public class NecroWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = 10000;
            return w;
        }
    }
}
