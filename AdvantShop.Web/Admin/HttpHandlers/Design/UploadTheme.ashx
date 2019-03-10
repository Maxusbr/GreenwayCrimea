<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Design.UploadTheme" %>

using System;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Core.Services.Security;
using AdvantShop.Customers;
using AdvantShop.Design;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using Newtonsoft.Json;
using Resources;

namespace Admin.HttpHandlers.Design
{
    [AuthorizeRole(RoleAction.Design)]
    public class UploadTheme : AdminHandler, IHttpHandler
    {
        static void Msg(HttpContext context, string msg)
        {
            context.Response.Write(JsonConvert.SerializeObject(new {msg}));
        }

        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            context.Response.ContentType = "application/json";

            if (context.Request.Files.Count < 1 || context.Request.Files[0].FileName.IsNullOrEmpty())
            {
                context.Response.Write(Resource.Admin_ImportCsv_NoFile);
                return;
            }

            HttpPostedFile pf = context.Request.Files[0];

            if (!FileHelpers.CheckFileExtension(pf.FileName, EAdvantShopFileTypes.Zip))
            {
                context.Response.Write(Resource.Admin_ImportCsv_WrongFileExtention);
                return;
            }

            string designFolderPath = SettingsDesign.Template != TemplateService.DefaultTemplateId
                                          ? HttpContext.Current.Server.MapPath("~/Templates/" + SettingsDesign.Template + "/")
                                          : HttpContext.Current.Server.MapPath("~/");

            string filename = string.Format("{0}/design/{1}s/{2}", designFolderPath, context.Request["type"], pf.FileName);

            pf.SaveAs(filename);

            try
            {
                if (FileHelpers.UnZipFilesAndFolders(filename))
                {
                    Msg(context, Resource.Admin_ThemesSettings_SuccessAddingTheme);
                }
                else
                {
                    Msg(context, Resource.Admin_ThemesSettings_ErrorUnZip);
                }
                FileHelpers.DeleteFile(filename);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                Msg(context, Resource.Admin_ThemesSettings_ErrorHappens);
            }
        }
    }
}