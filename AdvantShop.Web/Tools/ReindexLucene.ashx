<%@ WebHandler Language="C#" Class="ReindexLucene" %>

using System.Web;
using AdvantShop.FullSearch;

public class ReindexLucene : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        LuceneSearch.CreateAllIndex();
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