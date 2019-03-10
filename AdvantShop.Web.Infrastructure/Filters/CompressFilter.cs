using System.IO.Compression;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Compress;

namespace AdvantShop.Web.Infrastructure.Filters
{
    public class CompressFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction ||
                SettingProvider.GetConfigSettingValue("EnableCompressContent") == "false")
                return;

            var request = filterContext.HttpContext.Request;
            var response = filterContext.HttpContext.Response;
            var acceptEncoding = request.Headers[HttpConstants.HttpAcceptEncoding];

            if (response == null || response.Filter == null || string.IsNullOrEmpty(acceptEncoding)) 
                return;

            if (filterContext.RouteData.Values.ContainsValue("error"))
                return;

            if (filterContext.HttpContext.Request.RawUrl.ToLower().StartsWith("/api/"))
                return;

            acceptEncoding = acceptEncoding.ToLowerInvariant();
            var temp = response.Headers[HttpConstants.HttpContentEncoding];
            if (temp.IsNotEmpty()) return;

            if (acceptEncoding.Contains(HttpConstants.HttpContentEncodingGzip))
            {
                response.AppendHeader(HttpConstants.HttpContentEncoding, HttpConstants.HttpContentEncodingGzip);
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
            else if (acceptEncoding.Contains(HttpConstants.HttpContentEncodingDeflate))
            {
                response.AppendHeader(HttpConstants.HttpContentEncoding, HttpConstants.HttpContentEncodingDeflate);
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }
        }
    }
}
