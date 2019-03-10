using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Webhook;

namespace AdvantShop.Web.Infrastructure.Filters
{
    public class WebhookAuthAttribute : ActionFilterAttribute
    {
        private EWebhookType _serviceType { get; set; }

        public WebhookAuthAttribute(EWebhookType serviceType)
        {
            _serviceType = serviceType;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var apikey = filterContext.RouteData.Values["apikey"] as string;

            var service = new WebhookSericeProvider().Get(_serviceType);

            if (WebhookRepository.IsSystemKey(apikey)) return;
            if (!service.Enabled)
            {
                filterContext.Result = new JsonResult
                {
                    Data = new { result = false, message = "service disabled" },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else if (_serviceType == EWebhookType.None || apikey.IsNullOrEmpty() || apikey != service.ApiKey)
            {
                filterContext.Result = new JsonResult
                {
                    Data = new { result = false, message = "check api key" },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
    }
}