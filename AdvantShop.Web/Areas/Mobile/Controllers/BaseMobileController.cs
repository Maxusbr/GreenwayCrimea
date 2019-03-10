using System;
using System.Web.Mvc;
using System.Web.Routing;
using AdvantShop.Areas.Mobile.Extensions;
using AdvantShop.Core.Services.Loging;
using AdvantShop.Core.Services.Loging.Events;
using AdvantShop.Core.Services.SEO;
using AdvantShop.Customers;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Extensions;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Areas.Mobile.Controllers
{
    [IsStoreClosed]
    [LogUserActivity]
    public partial class BaseMobileController : BaseController
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

        public void SetTitle(string title)
        {
            MobileLayoutExtensions.TitleText = title;
        }

        protected void WriteLog(string name, string url, ePageType type)
        {
            if (Request.Browser.Crawler) return;

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