using System.IO;
using System.Web;

namespace AdvantShop.Web.Infrastructure.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetRequestRawData(this HttpRequestBase request)
        {
            if (request == null || request.Url == null)
                return "";

            var data = request.Url.ToString();

            try
            {
                if (request.InputStream != null && request.HttpMethod == "POST" && request.ContentLength <= 1000000)
                {
                    var s = request.InputStream;
                    s.Seek(0, SeekOrigin.Begin);

                    var stream = new StreamReader(s);
                    //using (var stream = new StreamReader(s))
                    //{
                        var postData = stream.ReadToEnd();
                        if (!string.IsNullOrEmpty(postData))
                            data += "  \n" + postData;
                    //}
                    s.Seek(0, SeekOrigin.Begin);
                }
            }
            catch
            {
            }

            return data;
        }
    }
}
