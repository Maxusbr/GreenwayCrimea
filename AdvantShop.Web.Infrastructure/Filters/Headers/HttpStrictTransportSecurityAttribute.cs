using System;
using System.Web;
using System.Web.Mvc;

namespace AdvantShop.Web.Infrastructure.Filters.Headers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class HttpStrictTransportSecurityAttribute : HttpHeaderAttributeBase
    {
        public TimeSpan MaxAge { get; set; }
        public bool IncludeSubdomains { get; set; }
        public bool Preload { get; set; }
        public bool UpgradeInsecureRequests { get; set; }
        public bool HttpsOnly { get; set; }

        public override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var response = filterContext.HttpContext.Response;
            if (HttpsOnly && !request.IsSecureConnection)
            {
                return;
            }

            if (UpgradeInsecureRequests && !UaSupportsUpgradeInsecureRequests(filterContext.HttpContext))
            {
                return;
            }
            if (MaxAge < TimeSpan.Zero) return;

            if (Preload && (MaxAge.TotalSeconds < 10886400 || !IncludeSubdomains))
            {
                return;
            }

            var seconds = (int)MaxAge.TotalSeconds;

            var includeSubdomains = (IncludeSubdomains ? "; includeSubDomains" : "");
            var preload = (Preload ? "; preload" : "");
            var value = string.Format("max-age={0}{1}{2}", seconds, includeSubdomains, preload);

            response.Headers[HeaderConstants.StrictTransportSecurityHeader] = value;
        }

        private bool UaSupportsUpgradeInsecureRequests(HttpContextBase env)
        {
            var upgradeHeader = env.Request.Headers["Upgrade-Insecure-Requests"];

            return upgradeHeader.Equals("1", StringComparison.Ordinal);
        }
    }
}
