using System.Web;
using System.Web.Mvc;
using AdvantShop.Core.Services.SEO;

namespace AdvantShop.Extensions
{
    public static class GoogleTagManagerExtenstions
    {
        public static HtmlString GoggleTagManagerCounter(this HtmlHelper helper)
        {
            var counter = GoogleTagManagerContext.Current.RenderCounter();
            return new HtmlString(counter);
        }
    }
}