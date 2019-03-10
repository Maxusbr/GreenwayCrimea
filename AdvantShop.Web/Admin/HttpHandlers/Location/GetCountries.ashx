<%@ WebHandler Language="C#" Class="GetCountries" %>

using System.Web;
using AdvantShop.Repository;

public class GetCountries : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        var countries = CountryService.GetCountriesByName(context.Request["q"]);
        if (countries.Count == 0)
        {
            context.Response.Write("\n");
        }
        else
        {
            for (int i = 0; i < countries.Count; i++)
            {
                context.Response.Write(countries[i] + "\n");
            }
        }
    }

    public bool IsReusable
    {
        get { return false; }
    }
}