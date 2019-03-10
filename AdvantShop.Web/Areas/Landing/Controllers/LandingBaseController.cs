using System.IO;
using System.Web.Mvc;
using AdvantShop.Core.Services.Loging;
using AdvantShop.Core.Services.Loging.Events;
using AdvantShop.Core.Services.SEO;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Extensions;

namespace AdvantShop.App.Landing.Controllers
{
    public class LandingBaseController : BaseController
    {
        protected void SetNgController(NgControllers.NgControllersTypes controllerName)
        {
            LayoutExtensions.NgController = controllerName;
        }

        protected ActionResult Error404()
        {
            System.Web.HttpContext.Current.Server.TransferRequest("/error/notfound");
            return new EmptyResult();
        }

        protected bool ViewExist(string name)
        {
            var viewResult = ViewEngines.Engines.FindView(ControllerContext, name, null);
            return viewResult.View != null;
        }

        public string RenderPartialToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        protected void WriteLog(string name, string url, ePageType type)
        {
            if (Request.Browser.Crawler) return;

            var e = new Event
            {
                Name = name,
                Url = url,
                EvenType = type
            };

            var loger = LogingManager.GetEventLoger();
            loger.LogEvent(e);
        }
    }
}
