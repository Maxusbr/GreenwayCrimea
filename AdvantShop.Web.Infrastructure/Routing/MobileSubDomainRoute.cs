using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AdvantShop.Configuration;

namespace AdvantShop.Web.Infrastructure.Routing
{
    public class MobileSubDomainRoute : Route
    {
        private readonly string[] _namespaces;
        private readonly string _subDomain;
        private readonly string _area;

        public MobileSubDomainRoute(string subDomain, string area, string url, object defaults, string[] namespaces)
            : base(url, new RouteValueDictionary(defaults), new MvcRouteHandler())
        {
            _namespaces = namespaces;
            _subDomain = subDomain;
            _area = area;
        }

        private bool IsSubDomainExist(HttpContextBase httpContext)
        {
            var url = httpContext.Request.Headers["HOST"];
            var index = url.IndexOf(".", StringComparison.Ordinal);

            if (index < 0)
                return false;

            var possibleSubDomain = url.Substring(0, index).ToLower();

            if (possibleSubDomain != _subDomain) return false;

            //SettingsDesign.IsMobileTemplate = true;

            return true;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            if (SettingsDesign.IsMobileTemplate) //  || IsSubDomainExist(httpContext)
            {
                var routeData = base.GetRouteData(httpContext);
                if (routeData == null) return null;
                if (_namespaces != null && _namespaces.Length > 0)
                {
                    routeData.DataTokens["Namespaces"] = _namespaces;
                }
                routeData.DataTokens["Area"] = _area;
                routeData.DataTokens["UseNamespaceFallback"] = bool.FalseString;

                return routeData;
            }

            return null;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            // Checks if the area to generate the route against is this same as the subdomain
            // If so we remove the area value so it won't be added to the URL as a query parameter
            if (!values.ContainsKey("Area")) return null;
            if (values["Area"].ToString().ToLower() != _area) return null;
            values.Remove("Area");
            return base.GetVirtualPath(requestContext, values);
        }
    }
}