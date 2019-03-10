<%@ WebHandler Language="C#" Class="ConvertPhone" %>

using System.Web;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using Newtonsoft.Json;

public class ConvertPhone : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        var temp = context.Request["source"];
        context.Response.ContentType = "application/json";
        context.Response.Write(JsonConvert.SerializeObject(StringHelper.ConvertToStandardPhone(temp)));
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}