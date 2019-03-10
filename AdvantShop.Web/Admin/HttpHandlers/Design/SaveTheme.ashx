<%@ WebHandler Language="C#" Class="AdvantShop.Admin.HttpHandlers.Design.SaveTheme" %>

using System.IO;
using System.Web;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Security;
using AdvantShop.Customers;
using AdvantShop.Design;
using Newtonsoft.Json;
using Resources;

namespace AdvantShop.Admin.HttpHandlers.Design
{
    [AuthorizeRole(RoleAction.Design)]
    public class SaveTheme : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            context.Response.ContentType = "application/json";

            var css = context.Request["css"];
            var design = context.Request["design"];
            var theme = context.Request["theme"];

            if (string.IsNullOrEmpty(css) || string.IsNullOrEmpty(design) || string.IsNullOrEmpty(theme))
                return;

            var designFolderPath = SettingsDesign.Template != TemplateService.DefaultTemplateId
                                            ? HttpContext.Current.Server.MapPath("~/Templates/" + SettingsDesign.Template + "/")
                                            : HttpContext.Current.Server.MapPath("~/");

            var cssPath = string.Format("{0}design/{1}s/{2}/styles/styles.css", designFolderPath, design, theme);
            
            if (!File.Exists(cssPath))
            {
                context.Response.Write(JsonConvert.SerializeObject(new { error = Resource.Admin_EditThemeCantSave }));
                return;
            }

            using (TextWriter writer = new StreamWriter(cssPath, false))
            {
                writer.Write(css);
            }

            context.Response.Write(JsonConvert.SerializeObject(new { result = Resource.Admin_EditThemeSaved }));
        }
    }
}