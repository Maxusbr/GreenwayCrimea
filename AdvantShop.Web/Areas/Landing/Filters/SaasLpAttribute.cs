
using System.Web.Mvc;
using AdvantShop.Saas;

namespace AdvantShop.App.Landing.Filters
{
    public class SaasLpAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!SaasDataService.IsEnabledFeature(ESaasProperty.LandingPage))
            {
                filterContext.Result = new RedirectResult("/service/getfeature?id="+ ESaasProperty.LandingPage);
            }
        }
    }
}
