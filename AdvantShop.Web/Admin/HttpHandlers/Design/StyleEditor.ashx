<%@ WebHandler Language="C#" Class="AdvantShop.Admin.HttpHandlers.Design.StyleEditor" %>

using System;
using System.IO;
using System.Web;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Core.Services.Security;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using Newtonsoft.Json;
using Resources;

namespace AdvantShop.Admin.HttpHandlers.Design
{
    [AuthorizeRole(RoleAction.Design)]
    public class StyleEditor : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            context.Response.ContentType = "application/json";

            var css = context.Request["css"] ?? string.Empty;

            try
            {
                var path = HttpContext.Current.Server.MapPath("~/userfiles/extra.css");

                using (TextWriter writer = new StreamWriter(path, false))
                {
                    writer.Write(css);
                }
                
                context.Response.Write(JsonConvert.SerializeObject(new { result = Resource.Admin_StylesEditor_FileSaved }));
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                context.Response.Write(JsonConvert.SerializeObject(new { error = Resource.Admin_StylesEditor_ErrorSave }));
            }            
        }
    }
}