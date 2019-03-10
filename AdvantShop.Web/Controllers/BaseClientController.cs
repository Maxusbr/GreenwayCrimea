using System.Web.Mvc;
using System.Web.Routing;
using AdvantShop.Core.Services.Loging;
using AdvantShop.Core.Services.Loging.Events;
using AdvantShop.Core.Services.SEO;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Extensions;
using AdvantShop.Web.Infrastructure.Filters;
using AdvantShop.Web.Infrastructure.Filters.Headers;

namespace AdvantShop.Controllers
{
    [IsStoreClosed]
    [LogUserActivity]
    public abstract class BaseClientController : BaseController
    {
        protected ActionResult Error404()
        {
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("action", "NotFound");

            IController errorController = new ErrorController();
            errorController.Execute(new RequestContext(HttpContext, routeData));
            return new EmptyResult();
        }

        protected void SetNgController(NgControllers.NgControllersTypes controllerName)
        {
            LayoutExtensions.NgController = controllerName;
        }

        protected void WriteLog(string name, string url, ePageType type)
        {
            if (Request.Browser.Crawler || Helpers.BrowsersHelper.IsBotByIp(Request.UserHostAddress))
                return;

            var @event = new Event
            {
                Name = name,
                Url = url,
                EvenType = type
            };

            var loger = LogingManager.GetEventLoger();
            loger.LogEvent(@event);
        }
    }
}