
using System.Web.Mvc;
using System;
using AdvantShop.Saas;
using AdvantShop.Web.Admin.Controllers;
using AdvantShop.Web.Admin.Controllers.Shared;

namespace AdvantShop.Web.Admin.Filters
{
    public class SaasFeatureAttribute : ActionFilterAttribute
    {
        private ESaasProperty saasMarker;
        public SaasFeatureAttribute(ESaasProperty marker)
        {
            saasMarker = marker;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!SaasDataService.IsEnabledFeature(saasMarker))
            {                
                filterContext.Result = (ViewResult)new ServiceController().GetFeature(saasMarker.ToString());                
            }

            base.OnActionExecuting(filterContext);
        }

        private ActionResult View(object p, object model)
        {
            throw new NotImplementedException();
        }
    }
}
