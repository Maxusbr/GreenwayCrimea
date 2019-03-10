//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Payment
{
    [PaymentKey("WorldPay")]
    public class WorldPay : PaymentMethod
    {
        private string Url
        {
            get
            {
                return Sandbox
                           ? "https://test.sagepay.com/gateway/service/vspform-register.vsp"
                           : "https://live.sagepay.com/gateway/service/vspform-register.vsp";
            }
        }
       
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
            get
            {
                return UrlStatus.NotificationUrl;
            }
        }
        public bool Sandbox { get; set; }
        public int InstID { get; set; }
        public float CurrencyValue { get; set; }
        public string CurrencyCode { get; set; }

        public static readonly List<string> AvaliableCurrs = new List<string>
                                                          {
                                                              "USD","ARS","AUD","BRL","CAD",
                                                              "CHF","CLP","CNY","COP","CZK",
                                                              "DKK","EUR","GBP","HKD","HUF",
                                                              "IDR","JPY","KES","KRW","MXP",
                                                              "MYR","NOK","NZD","PHP","PLN",
                                                              "PTE","SEK","SGD","SKK","THB",
                                                              "TWD","USD","VND","ZAR"
                                                          };
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {WorldPayTemplate.InstID, InstID.ToString()},
                               {WorldPayTemplate.Sandbox, Sandbox.ToString()},
                               {WorldPayTemplate.CurrencyCode, CurrencyCode},
                               {WorldPayTemplate.CurrencyValue, CurrencyValue.ToString()}
                           };
            }
            set
            {
                int intval;
                if (int.TryParse(value.ElementOrDefault(WorldPayTemplate.InstID), out intval))
                    InstID = intval;

                bool boolval;
                Sandbox = !bool.TryParse(value.ElementOrDefault(WorldPayTemplate.Sandbox), out boolval) || boolval;
                CurrencyCode = value.ElementOrDefault(WorldPayTemplate.CurrencyCode, "USD");
                float decVal;
                if (float.TryParse(value.ElementOrDefault(WorldPayTemplate.CurrencyValue), out decVal))
                {
                    CurrencyValue = decVal;
                }
                else
                {
                    var usd = CurrencyService.Currency(CurrencyCode);
                    CurrencyValue = usd != null ? usd.Rate : 1;
                }
            }
        }
        public override void ProcessForm(Order order)
        {
            var paymentNo = order.OrderID.ToString();
            var sum = string.Format("{0:0.00}", order.Sum / CurrencyValue);
            var description = string.Format("Order #{0} payment", order.Number);

            new PaymentFormHandler
                {
                    Url = Url,
                    InputValues = new Dictionary<string, string>
                                      {

                                          {"instId", InstID.ToString()},
                                          {"cartId", paymentNo},
                                          {"Amount", sum},
                                          {"currency", CurrencyCode},
                                          {"desc", description},
                                          {"SuccessURL", SuccessUrl},
                                          {"FailureURL", FailUrl},
                                          //FIELDS TO ENCRYPTION instId:amount:currency:cartId
                                          {"signature", (InstID.ToString() + sum + CurrencyCode + paymentNo).Md5()},
                                          {"MC_code", (sum + CurrencyCode + paymentNo).Md5()}
                                      }
                }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            var paymentNo = order.OrderID.ToString();
            var sum = string.Format("{0:0.00}", order.Sum / CurrencyValue);
            var description = string.Format("Order #{0} payment", order.Number);

            return new PaymentFormHandler
              {
                  Url = Url,
                  Page = page,
                  InputValues = new Dictionary<string, string>
                                      {

                                          {"instId", InstID.ToString()},
                                          {"cartId", paymentNo},
                                          {"Amount", sum},
                                          {"currency", CurrencyCode},
                                          {"desc", description},
                                          {"SuccessURL", SuccessUrl},
                                          {"FailureURL", FailUrl},
                                          //FIELDS TO ENCRYPTION instId:amount:currency:cartId
                                          {"signature", (InstID.ToString() + sum + CurrencyCode + paymentNo).Md5()},
                                          {"MC_code", (sum + CurrencyCode + paymentNo).Md5()}
                                      }
              }.ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            //!!!!!!!!!! UNSAFE !!!!!!!!!!!!!!
            var req = context.Request;
            if (!CheckData(req))
                return NotificationMessahges.InvalidRequestData;
            var paymentNumber = req["cartId"];
            int orderID = 0;
            if (int.TryParse(paymentNumber, out orderID) &&
                OrderService.GetOrder(orderID) != null)
            {
                var order = OrderService.GetOrder(orderID);
                if (order != null && req["amount"] == string.Format("{0:0.00}", order.Sum / CurrencyValue))
                {
                    OrderService.PayOrder(orderID, true);
                    return NotificationMessahges.SuccessfullPayment(order.Number);
                }
            }
            return NotificationMessahges.Fail;
        }

        private static bool CheckData(HttpRequest req)
        {
            return !new[]
                        {
                            "cartId",
                            "instId",
                            "amount",
                            "transId",
                            "currency",
                            "MC_code"
                        }.Any(item => string.IsNullOrEmpty(req[item]))
                        && (req["amount"] + req["currency"] + req["cartId"]).Md5() == req["MC_code"];

        }
    }
}