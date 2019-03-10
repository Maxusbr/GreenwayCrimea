//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace AdvantShop.Payment
{

    [PaymentKey("Paymaster")]
    public class Paymaster : PaymentMethod
    {
        public override ProcessType ProcessType
        {
            get { return ProcessType.FormPost; }
        }

        public override NotificationType NotificationType
        {
            get { return NotificationType.Handler; }
        }

        public override UrlStatus ShowUrls
        {
            get { return UrlStatus.CancelUrl | UrlStatus.ReturnUrl | UrlStatus.NotificationUrl; }
        }

        public string MerchantId { get; set; }
        public string SecretWord { get; set; }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {PaymasterTemplate.MerchantId, MerchantId},
                               {PaymasterTemplate.SecretWord, SecretWord},
                           };
            }
            set
            {
                MerchantId = value.ElementOrDefault(PaymasterTemplate.MerchantId);
                SecretWord = value.ElementOrDefault(PaymasterTemplate.SecretWord, string.Empty);
            }
        }

        public override void ProcessForm(Order order)
        {
            throw new NotImplementedException();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            var merchantParams = GetFormParams(order);
            return new PaymentFormHandler
            {
                FormName = "_xclick",
                Method = FormMethod.GET,
                Url = " https://paymaster.ru/Payment/Init",
                InputValues = merchantParams
            }.ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            HttpRequest req = context.Request;


            if (req["LMI_PREREQUEST"] == "1")
            {
                return "YES";
            }

            if (ValidateRequest(req))
            {
                int orderID = 0;
                if (int.TryParse(req["LMI_PAYMENT_NO"], out orderID))
                {
                    Order order = OrderService.GetOrder(orderID);
                    if (order != null)
                    {
                        OrderService.PayOrder(orderID, true);
                        return "OK"; // любой ответ со статусом 200
                    }

                }
                throw new Exception("Order #" + req["LMI_PAYMENT_NO"] + " not found"); // обязательно выкидывать Exception. Любой ответ со статусом 200, воспринимается как OK
            }
            throw new Exception("Invalid signature for order #" + req["LMI_PAYMENT_NO"]);
        }

        private Dictionary<string, string> GetFormParams(Order order)
        {
            string sum = Math.Round(order.Sum, 2).ToString().Replace(",", ".");

            var merchantParams = new Dictionary<string, string>
            {
                {"LMI_MERCHANT_ID", MerchantId},
                {"LMI_PAYMENT_AMOUNT", sum},
                {"LMI_CURRENCY", order.OrderCurrency.CurrencyNumCode.ToString()},
                {"LMI_PAYMENT_NO", order.OrderID.ToString()},
                {"LMI_PAYMENT_DESC", GetOrderDescription(order.Number)},
                {"LMI_INVOICE_CONFIRMATION_URL", NotificationUrl},
                {"LMI_PAYMENT_NOTIFICATION_URL", NotificationUrl},
                {"LMI_SUCCESS_URL", SuccessUrl},
                {"LMI_FAILURE_URL", FailUrl},
                {"LMI_PAYER_EMAIL", order.OrderCustomer.Email},
            };

            return merchantParams;
        }

        private bool ValidateRequest(HttpRequest req)
        {
            var paramString = new[] { req["LMI_MERCHANT_ID"],
                                      req["LMI_PAYMENT_NO"],
                                      req["LMI_SYS_PAYMENT_ID"],
                                      req["LMI_SYS_PAYMENT_DATE"],
                                      req["LMI_PAYMENT_AMOUNT"],
                                      req["LMI_CURRENCY"],
                                      req["LMI_PAID_AMOUNT"],
                                      req["LMI_PAID_CURRENCY"],
                                      req["LMI_PAYMENT_SYSTEM"],
                                      req["LMI_SIM_MODE"],
                                        }.Select(str => str != null ? str : "").AggregateString(";");

            var strWithPass = paramString + ";" + SecretWord;

            var hashBytes =  new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(strWithPass));

            string signature = Convert.ToBase64String(hashBytes);

            return signature == req["LMI_HASH"];
        }
    }
}