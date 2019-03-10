using AdvantShop.Configuration;
using AdvantShop.Core.UrlRewriter;
using System;
using System.Web.Mvc;

namespace AdvantShop.Web.Infrastructure.Filters.Headers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class XFrameOptionsAttribute : HttpHeaderAttributeBase
    {
        public XFrameOptionsPolicy Policy { get; set; }
        public string AllowUrl { get; set; }

        public XFrameOptionsAttribute(XFrameOptionsPolicy disabled)
        {
            this.Policy = disabled;
        }

        public override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            var strCurrentUrl = filterContext.HttpContext.Request.Url.ToString().ToLower();

            UrlService.ESocialType socialType;
            if ((socialType = UrlService.IsSocialUrl(strCurrentUrl)) != UrlService.ESocialType.none)
            {
                return;
            }


            switch (Policy)
            {
                case XFrameOptionsPolicy.Disabled:
                    return;
                case XFrameOptionsPolicy.Deny:
                    response.Headers[HeaderConstants.XFrameOptionsHeader] = "Deny";
                    return;
                case XFrameOptionsPolicy.SameOrigin:
                    response.Headers[HeaderConstants.XFrameOptionsHeader] = "SameOrigin";
                    return;
                case XFrameOptionsPolicy.AllowFrom:
                    response.Headers[HeaderConstants.XFrameOptionsHeader] = "ALLOW-FROM " + AllowUrl;
                    return;
                default:
                    throw new NotImplementedException("Wrong XFrameOptionsPolicy " + Policy);
            }
        }
    }

    public enum XFrameOptionsPolicy
    {
        //Specifies that the X-Frame-Options header should not be set in the HTTP response.
        Disabled,

        //Specifies that the X-Frame-Options header should be set in the HTTP response, instructing the browser to not
        //display the page when it is loaded in an iframe.
        Deny,

        //Specifies that the X-Frame-Options header should be set in the HTTP response, instructing the browser to
        //display the page when it is loaded in an iframe - but only if the iframe is from the same origin as the page.
        SameOrigin,

        //The page can only be displayed in a frame on the specified origin. not work in chrome and safari
        AllowFrom
    }
}
