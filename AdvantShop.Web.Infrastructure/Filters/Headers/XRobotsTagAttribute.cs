using System;
using System.Text;
using System.Web.Mvc;

namespace AdvantShop.Web.Infrastructure.Filters.Headers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class XRobotsTagAttribute : HttpHeaderAttributeBase
    {
        public bool NoIndex { get; set; }
        public bool NoFollow { get; set; }
        public bool NoSnippet { get; set; }
        public bool NoArchive { get; set; }
        public bool NoOdp { get; set; }
        public bool NoTranslate { get; set; }
        public bool NoImageIndex { get; set; }

        public override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            var sb = new StringBuilder();
            sb.Append(NoIndex ? "noindex, " : string.Empty);
            sb.Append(NoFollow ? "nofollow, " : string.Empty);
            sb.Append(NoSnippet && !NoIndex ? "nosnippet, " : string.Empty);
            sb.Append(NoArchive && !NoIndex ? "noarchive, " : string.Empty);
            sb.Append(NoOdp && !NoIndex ? "noodp, " : string.Empty);
            sb.Append(NoTranslate && !NoIndex ? "notranslate, " : string.Empty);
            sb.Append(NoImageIndex ? "noimageindex" : string.Empty);
            var value = sb.ToString().TrimEnd(' ', ',');

            if (value.Length == 0) return;
            response.Headers[HeaderConstants.XRobotsTagHeader] = value;
        }
    }
}
