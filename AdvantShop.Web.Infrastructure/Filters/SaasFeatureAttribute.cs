using AdvantShop.Core.UrlRewriter;
using AdvantShop.Saas;
using System.Web.Mvc;
using System;
using AdvantShop.Web.Infrastructure.ActionResults;

namespace AdvantShop.Web.Infrastructure.Filters
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
            if (!SaasFeaturesExclusion.IsAvailblePage(filterContext.HttpContext.Request.Url.AbsoluteUri))
            {                
                filterContext.Result = new TransferResult(UrlService.GetAbsoluteLink("adminv2/service/getfeature/" + saasMarker));
                
                filterContext.RouteData.Values.Add("clentcontroller", "jopa");
            }

            base.OnActionExecuting(filterContext);
        }
        
        private ActionResult View(object p, object model)
        {
            throw new NotImplementedException();
        }
    }
   
}
