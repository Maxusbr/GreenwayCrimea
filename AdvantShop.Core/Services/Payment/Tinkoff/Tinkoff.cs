//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;
using AdvantShop.Core.Services.Payment.Tinkoff;

namespace AdvantShop.Payment
{
    /// <summary>
    /// Tinkoff интернет-эквайринг. 
    /// Документация: https://oplata.tinkoff.ru/landing/develop/documentation/termins_and_operations
    /// </summary>
    [PaymentKey("Tinkoff")]
    public class Tinkoff : PaymentMethod
    {
        public override ProcessType ProcessType
        {
            get { return ProcessType.ServerRequest; }
        }

        public override NotificationType NotificationType
        {
            get { return NotificationType.Handler; }
        }

        public override UrlStatus ShowUrls
        {
            get { return UrlStatus.FailUrl | UrlStatus.ReturnUrl | UrlStatus.NotificationUrl; }
        }


        public string TerminalKey { get; set; }
        public string SecretKey { get; set; }
        public bool SendReceiptData { get; set; }
        public string Taxation { get; set; }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                {
                    {TinkoffTemplate.TerminalKey, TerminalKey},
                    {TinkoffTemplate.SecretKey, SecretKey},
                    {TinkoffTemplate.SendReceiptData, SendReceiptData.ToString()},
                    {TinkoffTemplate.Taxation, Taxation},
                };
            }
            set
            {
                TerminalKey = value.ElementOrDefault(TinkoffTemplate.TerminalKey);
                SecretKey = value.ElementOrDefault(TinkoffTemplate.SecretKey);
                SendReceiptData = value.ElementOrDefault(TinkoffTemplate.SendReceiptData).TryParseBool();
                Taxation = value.ElementOrDefault(TinkoffTemplate.Taxation);
            }
        }

        public override string ProcessServerRequest(Order order)
        {
            var service = new TinkoffService(TerminalKey, SecretKey, SendReceiptData);
            var response = service.Init(order, GetOrderDescription(order.Number), Taxation, GetCurrencyRate);

            if (response != null)
                return response.PaymentURL;

            return "";
        }

        public override string ProcessResponse(HttpContext context)
        {
            string bodyPost = null;

            context.Request.InputStream.Seek(0, SeekOrigin.Begin);
            bodyPost = (new StreamReader(context.Request.InputStream)).ReadToEnd();

            if (!string.IsNullOrEmpty(bodyPost))
            {

                var service = new TinkoffService(TerminalKey, SecretKey, SendReceiptData);

                var notify = service.ReadNotifyData(bodyPost);

                if (notify == null || notify.OrderId.IsNullOrEmpty() || notify.TerminalKey != TerminalKey)
                {
                    return NotificationMessahges.InvalidRequestData;
                }

                if (notify.Token.IsNullOrEmpty() || notify.Token != service.GenerateToken(service.AsDictionary(notify)))
                {
                    return NotificationMessahges.InvalidRequestData;
                }

                if (notify.Success.Equals("true", StringComparison.OrdinalIgnoreCase) && notify.Status == "CONFIRMED")
                {
                    var order = notify.OrderId.IsNotEmpty() ? OrderService.GetOrder(notify.OrderId.TryParseInt()) : null;

                    if (
                        order != null &&
                        notify.Amount == Convert.ToInt32(Math.Round(order.Sum * GetCurrencyRate(order.OrderCurrency), 2) * 100).ToString())
                    {
                        OrderService.PayOrder(order.OrderID, true);
                    }
                }
            }

            return "OK";
        }

    }
}
