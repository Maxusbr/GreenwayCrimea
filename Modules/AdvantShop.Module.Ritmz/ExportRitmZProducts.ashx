<%@ WebHandler Language="C#" Class="AdvantShop.Modules.ExportRitmZProducts" %>

using System.Web;

namespace AdvantShop.Modules
{
    public class ExportRitmZProducts : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (!ModulesRepository.IsActiveModule(Ritmz.ModuleID))
            {
                return;
            }
            
            context.Response.Charset = "utf-8";
            context.Response.ContentType = "text/xml;";
        
            RitmzExportProducts.Export();
            RitmzExportProducts.WriteToResponce(context.Response,
                                                context.Server.MapPath(RitmzExportProducts.ExporDir + "/" +
                                                                       RitmzExportProducts.ExportFile));
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}