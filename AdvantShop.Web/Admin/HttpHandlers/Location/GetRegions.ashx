<%@ WebHandler Language="C#" Class="GetRegions" %>

using System.Web;
using AdvantShop.Repository;

public class GetRegions : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        if (string.IsNullOrWhiteSpace(context.Request["q"]))
        {
            context.Response.Write("\n");
            return;
        }
        
        var cities = RegionService.GetRegionsByName(context.Request["q"]);
        if (cities.Count == 0)
        {
            context.Response.Write("\n");
        }
        else
        {
            for (int i = 0; i < cities.Count; i++)
            {
                context.Response.Write(cities[i] + "\n");
            }
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}