//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.IO.Compression;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Core.Compress
{
    /// <summary>
    /// Summary description for CompressContent
    /// </summary>
    public class CompressContent : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += PreRequestHandlerExecute;
        }
        
        private static void PreRequestHandlerExecute(object sender, EventArgs e)
        {
            if (bool.Parse(SettingProvider.GetConfigSettingValue("EnableCompressContent")) == false)
                return;
            
            var app = (HttpApplication)sender;
           
            var request = app.Context.Request;
            var path = request.FilePath;
			
            if (request.RawUrl.ToLower().StartsWith("/api/"))
            {
                return;
            }
			
            if (path.Contains(".js") || path.Contains(".css") || path.Contains(".html"))
            {
                var response = app.Context.Response;
                
                var acceptEncoding = request.Headers[HttpConstants.HttpAcceptEncoding];
                if (string.IsNullOrEmpty(acceptEncoding))
                    return;

                var temp = response.Headers[HttpConstants.HttpContentEncoding];
                if (temp.IsNotEmpty())
                    return;

                acceptEncoding = acceptEncoding.ToLowerInvariant();
                if (acceptEncoding.Contains(HttpConstants.HttpContentEncodingGzip))
                {
                    response.Filter = new HttpCompressStream(response.Filter, CompressionMode.Compress, HttpCompressStream.CompressionType.GZip);
                    response.AddHeader(HttpConstants.HttpContentEncoding, HttpConstants.HttpContentEncodingGzip);
                }
                else if (acceptEncoding.Contains(HttpConstants.HttpContentEncodingDeflate))
                {
                    response.Filter = new HttpCompressStream(response.Filter, CompressionMode.Compress, HttpCompressStream.CompressionType.Deflate);
                    response.AddHeader(HttpConstants.HttpContentEncoding, HttpConstants.HttpContentEncodingDeflate);
                }
            }
        }

        public void Dispose()
        {
            // Nothing to dispose; 
        }
    }
}