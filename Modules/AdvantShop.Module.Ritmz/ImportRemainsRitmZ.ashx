<%@ WebHandler Language="C#" Class=" AdvantShop.Modules.ImportRemainsRitmZ" %>

using System.Collections.Specialized;
using System.Web;

namespace AdvantShop.Modules
{
    public class ImportRemainsRitmZ : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {

            //номенклатура
            string items = string.Empty;
            //склад
            string storage = null;
            NameValueCollection nvc = context.Request.Form;

            if (!ModulesRepository.IsActiveModule(Ritmz.ModuleID))
            {
                return;
            }
            
            if (!string.IsNullOrEmpty(nvc["items"]))
            {
                try
                {
                    items = nvc["items"];
                }
                catch
                {
                    items = string.Empty;
                }
            }

            if (!string.IsNullOrEmpty(nvc["storage"]))
            {
                try
                {
                    storage = nvc["storage"];
                }
                catch
                {
                    storage = string.Empty;
                }
            }

            context.Response.Charset = "utf-8";
            context.Response.ContentType = "text/xml;";
            RitmzImportRemains.GetRemains(items, storage);
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