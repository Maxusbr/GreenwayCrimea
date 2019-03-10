using System.Web.Mvc;
using AdvantShop.ViewModel.ProductDetails;
using System.IO;
using AdvantShop.Configuration;

namespace AdvantShop.Controllers
{
    public class BaseClientProductController : BaseClientController
    {
        protected ViewResult CustomView(BaseProductViewModel model)
        {
            if (model.CustomViewPath != null && SettingsLandingPage.ActiveLandingPage)
            {
                var path = ("~/landings/templates/" + model.CustomViewPath);

                if (Directory.Exists(Server.MapPath(path)))
                {
                    var view = string.Format(path + "/views/product/{0}.cshtml", ControllerContext.RouteData.Values["action"]);

                    if (ViewExists(view))
                    {
                        return base.View(view, model);
                    }
                }
            }
            return base.View(model);
        }

        protected PartialViewResult CustomPartialView(BaseProductViewModel model)
        {
            if (model.CustomViewPath != null && SettingsLandingPage.ActiveLandingPage)
            {
                var path = "~/landings/templates/" + model.CustomViewPath;

                if (Directory.Exists(Server.MapPath(path)))
                {
                    var view = string.Format(path + "/views/product/{0}.cshtml", ControllerContext.RouteData.Values["action"]);

                    if (ViewExists(view))
                    {
                        return base.PartialView(view, model);
                    }
                }
            }
            return base.PartialView(model);
        }


        private bool ViewExists(string name)
        {
            ViewEngineResult result = ViewEngines.Engines.FindView(ControllerContext, name, null);
            return (result.View != null);
        }

    }
}