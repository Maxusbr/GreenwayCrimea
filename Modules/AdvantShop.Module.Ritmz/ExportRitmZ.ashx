<%@ WebHandler Language="C#" Class="AdvantShop.Modules.ExportRitmZ" %>

using System;
using System.Web;
using AdvantShop.Helpers;

namespace AdvantShop.Modules
{
    public class ExportRitmZ : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            
            DateTime? start = null;
            DateTime? end = null;

            if (!ModulesRepository.IsActiveModule(Ritmz.ModuleID))
            {
                return;
            }

            if (!string.IsNullOrEmpty(context.Request["b_date"]))
            {
                try
                {
                    start = SQLDataHelper.GetDateTime(context.Request["b_date"]);
                }
                catch
                {
                    start = null;
                }
            }

            if (!string.IsNullOrEmpty(context.Request["e_date"]))
            {
                try
                {
                    end = SQLDataHelper.GetDateTime(context.Request["e_date"]);
                }
                catch
                {
                    end = null;
                }
            }
            
            context.Response.Charset = "utf-8";
            context.Response.ContentType = "text/xml;";
           
            RitmZExportOrder.Export(start, end);
            RitmZExportOrder.WriteToResponce(context.Response, context.Server.MapPath(RitmZExportOrder.ExporDir + "/" + RitmZExportOrder.ExportFile));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}