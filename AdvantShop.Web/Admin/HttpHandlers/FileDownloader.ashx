<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.FileDownloader" %>

using System.IO;
using System.Web;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Helpers;

namespace Admin.HttpHandlers
{
    public class FileDownloader : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            var filePath = context.Request["file"];
            if (filePath == null || !filePath.Contains("Export/") && !filePath.Contains("content/price_temp"))
            {
                context.Response.Write("Access denied");
                context.Response.StatusCode = 403;
                context.Response.End();
                return;
            }
            if (!FileHelpers.CheckFileExtension(filePath, EAdvantShopFileTypes.FileInRootFolder))
            {
                context.Response.Write("Access denied");
                context.Response.StatusCode = 403;
                context.Response.End();
                return;
            }
            if (!File.Exists(context.Server.MapPath(filePath)))
            {
                context.Response.Write("Access denied");
                context.Response.StatusCode = 403;
                context.Response.End();
                return;
            }
            var fileName = filePath.Substring(filePath.LastIndexOf("/") + 1, filePath.Length - filePath.LastIndexOf("/") - 1);
            CommonHelper.WriteResponseFile(context.Server.MapPath(filePath), fileName);
        }
    }
}