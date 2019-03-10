using System.Web;
using System.Web.Mvc;

namespace AdvantShop.Areas.Mobile.Extensions
{
    public static class MobileLayoutExtensions
    {
        public static string TitleText
        {
            get { return HttpContext.Current.Items["Page_TitleText"] as string; }
            set { HttpContext.Current.Items["Page_TitleText"] = value; }
        }

        public static MvcHtmlString GetPageTitleText(this HtmlHelper helper)
        {
            return MvcHtmlString.Create(TitleText);
        }
    }
}