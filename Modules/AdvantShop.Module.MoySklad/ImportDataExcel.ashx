<%@ WebHandler Language="C#" Class="ImportDataExcel" %>

using System;
using System.Web;
using AdvantShop.Module.MoySklad;
using Newtonsoft.Json;


public class ImportDataExcel : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.Cache.SetLastModified(DateTime.UtcNow);

        context.Response.ContentType = "application/json";
        context.Response.Write(JsonConvert.SerializeObject(MoySklad.ImportStatisticMoySkladExcel.Data));
        context.Response.End(); // ?
    }

    public bool IsReusable
    {
        get { return false; }
    }
}