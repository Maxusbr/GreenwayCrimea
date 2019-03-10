<%@ WebHandler Language="C#" Class="DeleteCityExcluded" %>

using System;
using System.Web;
using AdvantShop.Helpers;

public class DeleteCityExcluded : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            int cityID = SQLDataHelper.GetInt(context.Request["cityID"]);
            int methodId = SQLDataHelper.GetInt(context.Request["methodId"]);
            string subject = context.Request["subject"];
            
            if (subject == "shipping")
            {
                AdvantShop.ShippingPaymentGeoMaping.DeleteShippingCityExcluded(methodId, cityID);
            }

        }
        catch (Exception e)
        {
            AdvantShop.Diagnostics.Debug.Log.Error(e);
        }

        context.Response.ContentType = "text/plain";
        context.Response.Write("sussecce");
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}