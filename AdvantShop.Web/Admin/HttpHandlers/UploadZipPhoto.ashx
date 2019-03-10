<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.UploadZipPhoto" %>

using System;
using System.Web;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using Newtonsoft.Json;
using Resources;

namespace Admin.HttpHandlers
{
    public class UploadZipPhoto : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            if (context.Request.Files.Count < 1 || context.Request.Files[0].FileName.IsNullOrEmpty())
            {
                ReturnResult(context, "error", Resource.Admin_ImportCsv_NoFile);
                return;
            }

            HttpPostedFile pf = context.Request.Files[0];

            if (!FileHelpers.CheckFileExtension(pf.FileName, EAdvantShopFileTypes.Zip))
            {
                ReturnResult(context, "error", Resource.Admin_ImportCsv_WrongFileExtention);
                return;
            }

            FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));
            var path = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, pf.FileName);
            try
            {
                FileHelpers.DeleteFilesFromPath(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));
                pf.SaveAs(path);
                var res = FileHelpers.UnZipFile(path);
                FileHelpers.DeleteFile(path);
                if (!res)
                {
                    ReturnResult(context, "error", Resource.Admin_ImportCsv_ErrorAtUnZip);
                    return;
                }
            }
            catch (Exception ex)
            {
                FileHelpers.DeleteFile(path);
                Debug.Log.Error(ex);
                ReturnResult(context, "error", Resource.Admin_ImportCsv_ErrorAtUploadFile);
            }
            ReturnResult(context, "", "Ok");
        }

        private void ReturnResult(HttpContext context, string error, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(new {error = error, msg = message}));
            context.Response.End();
        }
    }
}