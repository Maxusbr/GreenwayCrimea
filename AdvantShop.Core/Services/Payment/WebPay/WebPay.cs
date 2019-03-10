using System;
using System.Collections.Generic;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;
using AdvantShop.Diagnostics;

namespace AdvantShop.Payment
{
    // docs:  https://webpay.by/wp-content/uploads/2016/08/WebPay-Developer-Guide-2.1.1_RU.pdf

    [PaymentKey("WebPay")]
    public class WebPay : PaymentMethod
    {
        public override ProcessType ProcessType
        {
            get { return ProcessType.FormPost; }
        }

        public override NotificationType NotificationType
        {
            get { return NotificationType.ReturnUrl | NotificationType.Handler; }
        }
        public override UrlStatus ShowUrls
        {
            get { return UrlStatus.CancelUrl | UrlStatus.ReturnUrl | UrlStatus.NotificationUrl; }
        }

        public bool TestMode { get; set; }
        public string StoreId { get; set; }
        public string SecretKey { get; set; }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                {
                    {WebPayTemplate.TestMode, TestMode.ToString()},
                    {WebPayTemplate.StoreId, StoreId},
                    {WebPayTemplate.SecretKey, SecretKey}
                };
            }
            set
            {
                TestMode = value.ElementOrDefault(WebPayTemplate.TestMode).TryParseBool();
                StoreId = value.ElementOrDefault(WebPayTemplate.StoreId);
                SecretKey = value.ElementOrDefault(WebPayTemplate.SecretKey);
            }
        }
        
        public override void ProcessForm(Order order)
        {
            GetForm(order).Post();
        }
        
        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            return GetForm(order).ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            if (context.Request.Url.AbsolutePath.Contains("paymentnotification"))
                return ProcessResponseNotify(context);
            return ProcessResponseReturn(context);
        }

        private string ProcessResponseNotify(HttpContext context)
        {
            var req = context.Request;
            var orderId = 0;
            if (int.TryParse(req["site_order_id"], out orderId) && CheckSignature(req))
            {
                var order = OrderService.GetOrder(orderId);
                if (order != null)
                {
                    OrderService.PayOrder(orderId, true);
                    return "OK";
                }
            }
            return NotificationMessahges.InvalidRequestData;
        }

        private string ProcessResponseReturn(HttpContext context)
        {
            var req = context.Request;
            var orderId = 0;
            if (int.TryParse(req["wsb_order_num"], out orderId) && !string.IsNullOrEmpty(req["wsb_tid"]))
            {
                var order = OrderService.GetOrder(orderId);
                if (order != null)
                {
                    OrderService.PayOrder(orderId, true);
                    return NotificationMessahges.SuccessfullPayment(order.Number);
                }
            }
            Debug.Log.Info("WebPay reurn url failed: " + req.Url);

            return NotificationMessahges.InvalidRequestData;
        }

        private PaymentFormHandler GetForm(Order order)
        {
            var seed = new Random().Next(100, 100000);

            var paymentCurrencyIso3 = order.OrderCurrency.CurrencyCode.ToUpper();
            var currency = "BYN";

            if (paymentCurrencyIso3 == "BYN" || paymentCurrencyIso3 == "USD" || paymentCurrencyIso3 == "EUR" ||
                paymentCurrencyIso3 == "RUB")
            {
                currency = paymentCurrencyIso3;
            }
            else if (paymentCurrencyIso3 == "RUR")
            {
                currency = "RUB";
            }

            var formHandler = new PaymentFormHandler
            {
                Url = TestMode ? "https://securesandbox.webpay.by" : "https://payment.webpay.by",
                InputValues = new Dictionary<string, string>
                {
                    {"*scart", ""},
                    {"wsb_storeid", StoreId},
                    {"wsb_order_num", order.OrderID.ToString() },
                    {"wsb_currency_id", currency },               // BYN, USD, EUR, RUB
                    {"wsb_version", "2" },                        // Версия формы оплаты.
                    {"wsb_language_id", "russian" },
                    {"wsb_seed", seed.ToString() },
                    {"wsb_return_url", SuccessUrl },
                    {"wsb_cancel_return_url", FailUrl },
                    {"wsb_notify_url", NotificationUrl },
                    {"wsb_test", TestMode ? "1" : "0" },
                    {"wsb_customer_name", order.OrderCustomer.LastName + " " + order.OrderCustomer.FirstName },
                }
            };
            
            var i = 0;
            foreach (var orderItem in OrderService.RecalculateItemsPriceIncludingAllDiscounts(order.OrderItems, order.ShippingCost, order.Sum))
            {
                formHandler.InputValues.Add("wsb_invoice_item_name[" + i + "]", orderItem.Name);
                formHandler.InputValues.Add("wsb_invoice_item_quantity[" + i + "]", orderItem.Amount.ToString("F0").Replace(",", "."));
                formHandler.InputValues.Add("wsb_invoice_item_price[" + i + "]", orderItem.Price.ToString("F2").Replace(",", "."));
                i++;
            }
            if (order.ShippingCost > 0)
            {
                formHandler.InputValues.Add("wsb_invoice_item_name[" + i + "]", "Доставка");
                formHandler.InputValues.Add("wsb_invoice_item_quantity[" + i + "]", 1.ToString("F0").Replace(",", "."));
                formHandler.InputValues.Add("wsb_invoice_item_price[" + i + "]", order.ShippingCost.ToString("F2").Replace(",", "."));
            }

            formHandler.InputValues.Add("wsb_total", order.Sum.ToString("F2").Replace(",", "."));

            formHandler.InputValues.Add("wsb_signature", GetSignature(formHandler.InputValues));

            return formHandler;
        }

        private string GetSignature(Dictionary<string, string> dic)
        {
            var sig = dic["wsb_seed"] +
                      dic["wsb_storeid"] +
                      dic["wsb_order_num"] +
                      dic["wsb_test"] +
                      dic["wsb_currency_id"] +
                      dic["wsb_total"] +
                      SecretKey;

            return sig.Sha1();
        }

        private bool CheckSignature(HttpRequest req)
        {
            var s = (req["batch_timestamp"] +
                     req["currency_id"] +
                     req["amount"] +
                     req["payment_method"] +
                     req["order_id"] +
                     req["site_order_id"] +
                     req["transaction_id"] +
                     req["payment_type"] +
                     req["rrn"] +
                     SecretKey).Md5(false);

            var result = s == req["wsb_signature"];
            if (!result)
                Debug.Log.Info("WebPay signature check failed: s: " + s + ", signature: " + req["wsb_signature"]);

            return result;
        }
    }
}