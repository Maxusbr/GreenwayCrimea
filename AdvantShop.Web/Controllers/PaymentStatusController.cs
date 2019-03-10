using System;
using System.Web.Mvc;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Payment;
using AdvantShop.ViewModel.PaymentStatus;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Controllers
{
    public class PaymentStatusController : Controller
    {
        [LogRequest]
        public ActionResult Success(int advPaymentId)
        {
            var result = "";

            var method = PaymentService.GetPaymentMethod(advPaymentId);

            if (method != null && (method.NotificationType & NotificationType.ReturnUrl) == NotificationType.ReturnUrl)
            {
                var response = method.ProcessResponse(System.Web.HttpContext.Current);
                if (!String.IsNullOrWhiteSpace(response))
                {
                    result = LocalizationService.GetResource("PaymentStatus.Success.Status") + response;
                }
            }

            return View(new PaymentStatusViewModel() { Result = result });
        }

        [LogRequest]
        public void Notification(int advPaymentId)
        {
            var method = PaymentService.GetPaymentMethod(advPaymentId);
            if (method != null && (method.NotificationType & NotificationType.Handler) == NotificationType.Handler)
            {
                var paymentResponse = method.ProcessResponse(System.Web.HttpContext.Current);
                if (!string.IsNullOrWhiteSpace(paymentResponse))
                    Response.Write(paymentResponse);
            }
            else
            {
                Response.Write("payment method #" + advPaymentId + " not found");
            }
        }

        public ActionResult Fail()
        {
            return View();
        }

        public ActionResult Cancel()
        {
            return View();
        }
    }
}