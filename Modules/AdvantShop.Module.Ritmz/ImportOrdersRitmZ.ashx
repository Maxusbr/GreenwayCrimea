<%@ WebHandler Language="C#" Class="AdvantShop.Modules.ImportOrdersRitmZ" %>

using System;
using System.Collections.Specialized;
using System.Web;

using AdvantShop.Helpers;

namespace AdvantShop.Modules
{
    public class ImportOrdersRitmZ : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            DateTime? start = null;
            DateTime? end = null;
            NameValueCollection nvc = context.Request.Form;

            if (!ModulesRepository.IsActiveModule(Ritmz.ModuleID))
            {
                return;
            }
            
            if (!string.IsNullOrEmpty(nvc["b_date"]))
            {
                try
                {
                    start = SQLDataHelper.GetDateTime(nvc["b_date"]);
                }
                catch
                {
                    start = null;
                }
            }

            if (!string.IsNullOrEmpty(nvc["e_date"]))
            {
                try
                {
                    end = SQLDataHelper.GetDateTime(nvc["e_date"]);
                }
                catch
                {
                    end = null;
                }
            }

            context.Response.Charset = "utf-8";
            context.Response.ContentType = "text/xml;";
            RitmzImportOrders.Import(start, end, context.Server.MapPath(RitmZExportOrder.ExporDir + "/" + RitmZExportOrder.ExportFile));
            RitmzImportOrders.WriteToResponce(context.Response, context.Server.MapPath(RitmZExportOrder.ExporDir + "/" + RitmZExportOrder.ExportFile));
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