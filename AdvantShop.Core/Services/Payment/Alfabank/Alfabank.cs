//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment.Alfabank;
using AdvantShop.Orders;

namespace AdvantShop.Payment
{
    [PaymentKey("Alfabank")]
    public class Alfabank : PaymentMethod
    {
        public const string AlfabankOrderId = "alfabankorderId";

        public string UserName { get; set; }
        public string Password { get; set; }
        public string MerchantLogin { get; set; }
        public string UseTestMode { get; set; }

        public override ProcessType ProcessType
        {
            get { return ProcessType.ServerRequest; }
        }

        public override NotificationType NotificationType
        {
            get { return NotificationType.ReturnUrl; }
        }

        public override UrlStatus ShowUrls
        {
            get { return UrlStatus.FailUrl | UrlStatus.ReturnUrl; }
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {AlfabankTemplate.UserName, UserName},
                               {AlfabankTemplate.Password, Password},
                               {AlfabankTemplate.MerchantLogin, MerchantLogin},
                               {AlfabankTemplate.UseTestMode, UseTestMode}
                           };
            }
            set
            {
                UserName = value.ElementOrDefault(AlfabankTemplate.UserName);
                Password = value.ElementOrDefault(AlfabankTemplate.Password);
                MerchantLogin = value.ElementOrDefault(AlfabankTemplate.MerchantLogin);
                UseTestMode = value.ElementOrDefault(AlfabankTemplate.UseTestMode);
            }
        }

        public override string ProcessServerRequest(Order order)
        {
            var service = new AlfabankService(UserName, Password, MerchantLogin, UseTestMode);
            var response = service.Register(order, GetOrderDescription(order.Number), GetCurrencyRate, SuccessUrl, FailUrl);

            if (response != null)
                return response.FormUrl;

            return "";
        }

        public override string ProcessResponse(HttpContext context)
        {
            if (context.Request[AlfabankService.ReturnUrlParamNameMerchantOrder].IsNullOrEmpty())
            {
                return NotificationMessahges.InvalidRequestData;
            }

            var orderNumber = context.Request[AlfabankService.ReturnUrlParamNameMerchantOrder];

            var order = orderNumber.IsNotEmpty() ? OrderService.GetOrderByNumber(orderNumber) : null;

            if (order == null)
                return NotificationMessahges.InvalidRequestData;

            var service = new AlfabankService(UserName, Password, MerchantLogin, UseTestMode);
            var response = service.GetOrderStatus(context.Request[AlfabankService.ReturnUrlParamNameAlfaOrder], context.Request[AlfabankService.ReturnUrlParamNameMerchantOrder]);

            if (response == null || response.ErrorCode != 0 || response.OrderStatus != "2")
                return NotificationMessahges.InvalidRequestData;

            OrderService.PayOrder(order.OrderID, true);
            return NotificationMessahges.SuccessfullPayment(order.Number);
        }

    }
}
