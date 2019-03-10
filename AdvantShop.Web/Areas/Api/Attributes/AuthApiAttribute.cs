using System.Web.Mvc;
using AdvantShop.Core.Services.Api;
using AdvantShop.Core.Services.Configuration.Settings;
using AdvantShop.Web.Infrastructure.ActionResults;

namespace AdvantShop.Areas.Api.Attributes
{
    public class AuthApiAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var apikey = filterContext.HttpContext.Request["apikey"];

            if (string.IsNullOrWhiteSpace(apikey) || string.IsNullOrWhiteSpace(SettingsApi.ApiKey) || apikey != SettingsApi.ApiKey)
            {
                filterContext.Result = new JsonNetResult {Data = new ApiResponse("error", "Check apikey")};
            }
        }
    }
}