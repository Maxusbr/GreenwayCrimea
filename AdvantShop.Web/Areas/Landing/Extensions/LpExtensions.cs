using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.App.Landing.Domain;

namespace AdvantShop.App.Landing.Extensions
{
    public static class LpExtensions
    {
        public static MvcHtmlString SubBlock(this HtmlHelper helper, LpBlock block, string name)
        {
            return helper.Action("SubBlock", "Landing", new RouteValueDictionary() {{"blockId", block.Id}, {"name", name}});
        }

        public static MvcHtmlString SubBlock(this HtmlHelper helper, LpSubBlock subblock, string name)
        {
            return helper.Action("SubBlock", "Landing", new RouteValueDictionary() { { "blockId", subblock.LandingBlockId }, { "name", name } });
        }


        public static string LStaticUrl(this HtmlHelper helper)
        {
            return UrlService.GetUrl() + "landing/";
        }
    }
}
