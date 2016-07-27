using PoGo.NecroBot.Logic.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Localization
{
    public interface ILocalizer
    {
        string GetFormat(TranslationString key);
        string GetFormat(TranslationString key, params object[] data);
    }

    public class Localizer : ILocalizer
    {
        public string GetFormat(TranslationString key)
        {
            return "";
        }

        public string GetFormat(TranslationString key, params object[] data)
        {
            return "";
        }
    }
}
