<%@ WebHandler Language="C#" Class="ClearCache" %>

using System.Web;

public class ClearCache : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        AdvantShop.Core.Caching.CacheManager.Clean();
        context.Response.Write("ok");
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}