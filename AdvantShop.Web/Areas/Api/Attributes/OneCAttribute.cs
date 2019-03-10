using System.Web.Mvc;
using AdvantShop.Core.Services.Api;
using AdvantShop.Core.Services.Configuration.Settings;
using AdvantShop.Web.Infrastructure.ActionResults;

namespace AdvantShop.Areas.Api.Attributes
{
    public class OneCAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Settings1C.Enabled)
            {
                filterContext.Result = new JsonNetResult {Data = new ApiResponse("error", "1С не активна в настройках API")};
            }
        }
    }
}