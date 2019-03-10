using System.Web.Mvc;
using System.Web.Routing;
using AdvantShop.Controllers;
using AdvantShop.Web.Infrastructure.Controllers;

namespace AdvantShop.Areas.Api.Controllers
{
    public partial class BaseApiController : BaseController
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
    }
}