<%@ WebHandler Language="C#" Class="AdvantShop.Admin.HttpHandlers.Design.CreateTheme" %>

using System;
using System.Web;
using AdvantShop.Core.Caching;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Customers;
using AdvantShop.Design;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Configuration;
using Newtonsoft.Json;
using Resources;
using AdvantShop.Core.Services.Security;

namespace AdvantShop.Admin.HttpHandlers.Design
{
    [AuthorizeRole(RoleAction.Design)]
    public class CreateTheme : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            context.Response.ContentType = "application/json";

            var name = context.Request["name"];
            var design = context.Request["design"];

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(design))
                return;

            name = name.Replace(':', '_').Replace('.', '_').Replace(' ', '_').Replace("//", "");

            eDesign designType;
            Enum.TryParse(design, true, out designType);

            var themes = DesignService.GetDesigns(designType);
            if (themes.Find(x => x.Name == name) != null)
            {
                context.Response.Write(JsonConvert.SerializeObject(new { error = true, msg = Resource.Admin_EditTheme_ErrorThemeExist }));
                return;
            }

            try
            {
                var designFolderPath = SettingsDesign.Template != TemplateService.DefaultTemplateId
                                            ? HttpContext.Current.Server.MapPath("~/Templates/" + SettingsDesign.Template + "/")
                                            : HttpContext.Current.Server.MapPath("~/");

                var themePath = string.Format("{0}design/{1}s/{2}/", designFolderPath, designType, name);

                FileHelpers.CreateDirectory(themePath);

                var cssFolder = themePath + "styles\\";

                FileHelpers.CreateDirectory(cssFolder);
                FileHelpers.CreateFile(cssFolder + "styles.css");

                var imagesFolder = themePath + "images\\";

                FileHelpers.CreateDirectory(imagesFolder);

                CacheManager.RemoveByPattern(CacheNames.GetDesignCacheObjectName(""));
            }
            catch (Exception ex)
            {
                context.Response.Write(JsonConvert.SerializeObject(new {error = true, msg = Resource.Admin_EditTheme_ErrorCreateTheme}));
                Debug.Log.Error(ex);
                return;
            }

            context.Response.Write(JsonConvert.SerializeObject(new{error = false, msg = string.Format("EditTheme.aspx?theme={0}&design={1}", name, designType)}));
        }
    }
}