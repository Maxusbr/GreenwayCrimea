using System.Web.Mvc;
using AdvantShop.Web.Infrastructure.Extensions;

namespace AdvantShop.Controllers
{
    public partial class ModulesController : BaseClientController
    {
        public ActionResult RenderModules(string key, object routeValues = null)
        {
            var model = ModulesExtensions.GetModuleRoutes(key, routeValues);
            return PartialView("_Module", model);
        }
    }
}