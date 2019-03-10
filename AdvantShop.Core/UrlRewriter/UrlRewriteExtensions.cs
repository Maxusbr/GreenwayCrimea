using System;
using System.Globalization;
using System.IO;
using System.Web;

namespace AdvantShop.Core.UrlRewriter
{
    public static class UrlRewriteExtensions
    {
        private const string _header = "If-Modified-Since";

        public static void StaticFile304(this HttpApplication app)
        {
            if (!app.Request.Url.AbsolutePath.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                return;

            var lastString = app.Request.Headers[_header];
            if (string.IsNullOrWhiteSpace(lastString)) return;

            var fileName = app.Request.PhysicalPath;
            
            var lastModified = File.GetLastWriteTime(fileName);
            var ifModifiedSince = lastString.TryParseDateTimeGMT();

            if (!ifModifiedSince.HasValue)
            {
                app.Request.Headers.Remove(_header);
            }
            if (ifModifiedSince.HasValue && (ifModifiedSince.Value >= lastModified))
            {
                app.Response.StatusCode = 304;
                app.Response.SuppressContent = true;
                return;
            }
            app.Response.Cache.SetLastModified(lastModified);
        }

        public static DateTime? TryParseDateTimeGMT(this string val)
        {
            DateTime intval;
            if (DateTime.TryParseExact(val, "R", CultureInfo.InvariantCulture, DateTimeStyles.None, out intval))
            {
                return intval.ToLocalTime();
            }
            return null;
        }

        public static void RewriteTo404(this HttpApplication app)
        {
            try
            {
                app.Context.Response.Clear();
                app.Context.Response.TrySkipIisCustomErrors = true;
                app.Context.Response.StatusCode = 404;
                app.Context.Response.StatusDescription = HttpWorkerRequest.GetStatusDescription(404);
                app.Context.Response.End();
            }
            catch
            {
            }
        }
    }
}
