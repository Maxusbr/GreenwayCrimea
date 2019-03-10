using System.Web;

namespace AdvantShop.Core.Services.SEO
{
    public static class GoogleTagManagerContext
    {
        private const string GtmContextKey = "GtmContextKey";

        public static GoogleTagManager Current
        {
            get
            {
                if (HttpContext.Current == null)
                    return new GoogleTagManager();

                var gtmContext = HttpContext.Current.Items[GtmContextKey] as GoogleTagManager;
                if (gtmContext != null) return gtmContext;

                var gtm = new GoogleTagManager();

                HttpContext.Current.Items[GtmContextKey] = gtm;

                return gtm;
            }
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Items[GtmContextKey] = value;
            }
        }
    }
}
