<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.DownloadFile" %>

using System;
using System.Web;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Helpers;
using AdvantShop.Configuration;

namespace Admin.HttpHandlers
{
    public class DownloadFile : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;
            
            var file = context.Request["file"];
            if (string.IsNullOrEmpty(file))
                return;
            
            if (!FileHelpers.CheckFileExtension(file, EAdvantShopFileTypes.FileInRootFolder))
            {
                context.Response.Write("Access denied");
                context.Response.StatusCode = 403;
                context.Response.End();
                return;
            }
            
            if (!System.IO.File.Exists(SettingsGeneral.AbsolutePath + file))
                return;
            CommonHelper.WriteResponseFile(SettingsGeneral.AbsolutePath + file, file);
        }
    }
}