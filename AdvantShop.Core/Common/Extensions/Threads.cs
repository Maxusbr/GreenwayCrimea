using System.Globalization;
using System.Threading;
using AdvantShop.Localization;

namespace AdvantShop.Core.Common.Extensions
{
    public static class Threads
    {
        static public Thread SetCulture(this Thread val, string lang = "")
        {
            var culture = Culture.CurrentCulture(lang);
            val.CurrentCulture = culture;
            val.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            return val;
        }
    }
}