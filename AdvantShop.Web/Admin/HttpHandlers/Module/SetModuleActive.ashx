<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Modules.SetModuleActive" %>

using System.Text;
using System.Web;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Core.Modules;

using AdvantShop.Trial;
using Newtonsoft.Json;

namespace Admin.HttpHandlers.Modules
{
    public class SetModuleActive : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            var active = false;

            if (string.IsNullOrEmpty(context.Request["modulestringid"]) || !bool.TryParse(context.Request["active"], out active))
            {
                ReturnResult(context, false, "error");
            }

            ModulesRepository.SetActiveModule(context.Request["modulestringid"], active);
            
            TrialService.TrackEvent(active ? TrialEvents.ActivateModule : TrialEvents.DeactivateModule, context.Request["modulestringid"]);

            if (context.Request["modulestringid"] == "YaMetrika" && active)
            {
                TrialService.TrackEvent(TrialEvents.SetUpYandexMentrika, string.Empty);
            }

            ReturnResult(context, active, active ? Resources.Resource.Admin_Module_Active : Resources.Resource.Admin_Module_NotActive);
        }

        private static void ReturnResult(HttpContext context, bool active, string result)
        {
            context.Response.ContentType = "application/JSON";
            context.Response.ContentEncoding = Encoding.UTF8;

            context.Response.Write(JsonConvert.SerializeObject(new
            {
                active,
                state = result,
            }));
            context.Response.End();
        }
    }
}