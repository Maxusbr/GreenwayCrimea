//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Taxes;
using Newtonsoft.Json;

namespace AdvantShop.Payment
{
    [PaymentKey("PayAnyWay")]
    public class PayAnyWay : PaymentMethod
    {
        public string MerchantId { get; set; }
        public string CurrencyLabel { get; set; }
        public float CurrencyValue { get; set; }
        public string Signature { get; set; }
        public bool TestMode { get; set; }
        public string UnitId { get; set; }
        public string LimitIds { get; set; }
        public bool UseKassa { get; set; }

        public override UrlStatus ShowUrls
        {
            get { return UrlStatus.NotificationUrl | UrlStatus.ReturnUrl | UrlStatus.FailUrl; }
        }

        public override ProcessType ProcessType
        {
            get { return ProcessType.FormPost; }
        }

        public override NotificationType NotificationType
        {
            get { return NotificationType.ReturnUrl | NotificationType.Handler; }
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {PayAnyWayTemplate.MerchantId, MerchantId},
                               {PayAnyWayTemplate.CurrencyLabel, CurrencyLabel},
                               {PayAnyWayTemplate.CurrencyValue, CurrencyValue.ToString()},
                               {PayAnyWayTemplate.Signature, Signature},
                               {PayAnyWayTemplate.TestMode, TestMode.ToString()},
                               {PayAnyWayTemplate.UnitId, UnitId},
                               {PayAnyWayTemplate.LimitIds, LimitIds},
                               {PayAnyWayTemplate.UseKassa, UseKassa.ToString()},
                           };
            }
            set
            {
                if (value.ContainsKey(PayAnyWayTemplate.MerchantId))
                    MerchantId = value[PayAnyWayTemplate.MerchantId];
                Signature = value.ElementOrDefault(PayAnyWayTemplate.Signature);
                CurrencyLabel = !value.ContainsKey(PayAnyWayTemplate.CurrencyLabel)
                                    ? "RUB"
                                    : value[PayAnyWayTemplate.CurrencyLabel];
                float decVal = 0;
                CurrencyValue = value.ContainsKey(PayAnyWayTemplate.CurrencyValue) &&
                               float.TryParse(value[PayAnyWayTemplate.CurrencyValue], out decVal)
                                   ? (decVal > 0 ? decVal : 1)
                                   : 1;
                bool boolVal;
                TestMode = !bool.TryParse(value.ElementOrDefault(PayAnyWayTemplate.TestMode), out boolVal) || boolVal;
                UnitId = value.ElementOrDefault(PayAnyWayTemplate.UnitId);
                LimitIds = value.ElementOrDefault(PayAnyWayTemplate.LimitIds);
                UseKassa = value.ElementOrDefault(PayAnyWayTemplate.UseKassa).TryParseBool();
            }
        }

        private PaymentFormHandler RendPaymentFormHandler(Order order)
        {
            var sum = (order.Sum * CurrencyValue).ToString("F2").Replace(",", ".");

            var payment = new PaymentFormHandler
            {
                FormName = "_xclick",
                Method = FormMethod.POST,
                Url = "https://www.moneta.ru/assistant.htm",
                InputValues = new Dictionary<string, string>
                {
                    {"MNT_ID", MerchantId},
                    {"MNT_TRANSACTION_ID", order.OrderID.ToString()},
                    {"MNT_CURRENCY_CODE", CurrencyLabel},
                    {"MNT_AMOUNT", sum},
                    {"MNT_TEST_MODE", TestMode ? "1" : "0"},
                    {"MNT_DESCRIPTION", GetOrderDescription(order.Number)},
                    {
                        "MNT_SIGNATURE",
                        string.Format("{0}{1}{2}{3}{4}{5}", MerchantId, order.OrderID.ToString(), sum, CurrencyLabel,
                            TestMode ? "1" : "0", Signature).Md5()
                    },
                    {"MNT_SUCCESS_URL", SuccessUrl},
                    {"MNT_FAIL_URL", FailUrl},
                }
            };

            if (!string.IsNullOrEmpty(UnitId))
            {
                payment.InputValues.Add("paymentSystem.unitId", UnitId);
            }

            if (!string.IsNullOrEmpty(LimitIds))
            {
                payment.InputValues.Add("paymentSystem.limitIds", LimitIds);
            }

            //if (UseKassa)
            //{
            //    var productsJson = GetProductsJson(order);

            //    // суммарный объем​ ​CUSTOM​ ​полей​ ​не​ ​должен​ ​превышать​ ​1,5​ ​Килобайта,​ ​
            //    // суммарный​ ​объем​ ​всего​ ​запроса​ ​- не​ ​должен​ ​превышать​ ​2​ ​Килобайта. 
            //    if (Encoding.UTF8.GetBytes(productsJson).Length < 1500)
            //    {
            //        payment.InputValues.Add("MNT_CUSTOM1", "1");
            //        payment.InputValues.Add("MNT_CUSTOM2", productsJson);
            //    }
            //}

            return payment;
        }

        public override void ProcessForm(Order order)
        {
            RendPaymentFormHandler(order).Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            return RendPaymentFormHandler(order).ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            var req = context.Request;

            int orderId;
            if (int.TryParse(req["MNT_TRANSACTION_ID"], out orderId))
            {
                var order = OrderService.GetOrder(orderId);
                if (order != null)
                {
                    string sum = (order.Sum * CurrencyValue).ToString("F2").Replace(",", ".");

                    if (CheckData(req, order, sum) && req["MNT_AMOUNT"] == sum && req["MNT_CURRENCY_CODE"] == CurrencyLabel && req["MNT_TEST_MODE"] == (TestMode ? "1" : "0"))
                    {
                        if (!order.Payed)
                            OrderService.PayOrder(orderId, true);

                        if (UseKassa)
                            SendXmlResponse(context, order);

                        return "SUCCESS";
                    }
                    //Добавил костыль, потому что с сервиса приходит в url только id заказа, подтверждение оплаты заказа приходит не по прямому url, а фоновым запросом.
                    else if (order.Payed)
                    {
                        if (UseKassa)
                            SendXmlResponse(context, order);

                        return "SUCCESS";
                    }
                    Debug.Log.Info("PayAnyWay check data fail " + context.Request.Url.ToString());
                }
            }
            return "FAIL";
        }

        public bool CheckData(HttpRequest req, Order order, string sum)
        {
            var fields = new string[]
            {
                "MNT_ID",
                "MNT_TRANSACTION_ID",
                "MNT_AMOUNT",
                "MNT_CURRENCY_CODE",
                "MNT_TEST_MODE",
                "MNT_SIGNATURE"
            };

            return !fields.Any(val => string.IsNullOrEmpty(req[val]))
                   && string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                       MerchantId,
                       order.OrderID.ToString(),
                       req["MNT_OPERATION_ID"] ?? "",
                       sum,
                       CurrencyLabel,
                       req["MNT_SUBSCRIBER_ID"] ?? "",
                       TestMode ? "1" : "0",
                       Signature).Md5(false) == req["MNT_SIGNATURE"];

            //fields.Aggregate<string, StringBuilder, string>(new StringBuilder(), (str, field) => str.Append(field == "MNT_ID" ? MerchantId : field == "MNT_CURRENCY_CODE" ? CurrencyLabel : field == "MNT_SUBSCRIBER_ID" ? req["MNT_SUBSCRIBER_ID"] : field == "MNT_SIGNATURE" ? Signature : req[field]), Strings.ToString).Md5(true) != req["MNT_SIGNATURE"]);
        }

        private string GetProductsJson(Order order)
        {
            var items =
                    OrderService.RecalculateItemsPriceIncludingAllDiscounts(order.OrderItems, order.ShippingCost, order.Sum)
                        .Select(x => new PayAnyWayItem()
                        {
                            n = HttpUtility.HtmlEncode(x.Name).Length > 100 ? HttpUtility.HtmlEncode(x.Name).Substring(0, 100) : HttpUtility.HtmlEncode(x.Name),
                            p = x.Price.ToString("F2").Replace(",", "."),
                            q = x.Amount.ToString("F2").Replace(",", "."),
                            t = GetTaxType(x.TaxType)
                        }).ToList();

            if (order.ShippingCost > 0)
            {
                items.Add(new PayAnyWayItem()
                {
                    n = "Доставка",
                    p = order.ShippingCost.ToString("F2").Replace(",", "."),
                    q = 1.ToString("F2").Replace(",", "."),
                    t = GetTaxType(order.ShippingTaxType)
                });
            }

            var productsJson = JsonConvert.SerializeObject(new { customer = order.OrderCustomer.Email, items = items });

            return productsJson;
        }

        private string GetProductsInXml(Order order)
        {
            var items =
                    OrderService.RecalculateItemsPriceIncludingAllDiscounts(order.OrderItems, order.ShippingCost, order.Sum)
                        .Select(x => new PayAnyWayItemInXml()
                        {
                            name = x.Name.Length > 100 ? x.Name.Substring(0, 100).Replace("\"", "") : x.Name.Replace("\"", ""),
                            price = x.Price.ToString("F2").Replace(",", "."),
                            quantity = x.Amount.ToString("F2").Replace(",", "."),
                            vatTag = GetTaxType(x.TaxType)
                        }).ToList();

            if (order.ShippingCost > 0)
            {
                items.Add(new PayAnyWayItemInXml()
                {
                    name = "Доставка",
                    price = order.ShippingCost.ToString("F2").Replace(",", "."),
                    quantity = 1.ToString("F2").Replace(",", "."),
                    vatTag = GetTaxType(order.ShippingTaxType)
                });
            }

            var productsJson = JsonConvert.SerializeObject(items);

            return productsJson;
        }

        // https://www.payanyway.ru/info/p/ru/public/merchants/Assistant54FZ.pdf
        private void SendXmlResponse(HttpContext context, Order order)
        {
            var req = context.Request;
            
            // md5(MNT_RESULT_CODE + MNT_ID + MNT_TRANSACTION_ID + Код проверки целостности данных)
            var signature = ("200" + req["MNT_ID"] + req["MNT_TRANSACTION_ID"] + Signature).Md5();
            var productsJson = GetProductsInXml(order);

            var responseText = 
                string.Format(@"<?xml version=""1.0"" encoding=""UTF-8"" ?>
<MNT_RESPONSE>
<MNT_ID>{0}</MNT_ID>
<MNT_TRANSACTION_ID>{1}</MNT_TRANSACTION_ID>
<MNT_RESULT_CODE>200</MNT_RESULT_CODE>
<MNT_SIGNATURE>{2}</MNT_SIGNATURE>
<MNT_ATTRIBUTES>
<ATTRIBUTE>
<KEY>INVENTORY</KEY>
<VALUE>{3}</VALUE>
</ATTRIBUTE>
<ATTRIBUTE>
<KEY>CUSTOMER</KEY>
<VALUE>{4}</VALUE>
</ATTRIBUTE>
<ATTRIBUTE>
<KEY>DELIVERY</KEY>
<VALUE>0</VALUE>
</ATTRIBUTE>
</MNT_ATTRIBUTES>
</MNT_RESPONSE>
", req["MNT_ID"], req["MNT_TRANSACTION_ID"], signature, productsJson, order.OrderCustomer.Email);

            context.Response.Clear();
            context.Response.ContentType = "application/xml";
            context.Response.Write(responseText);
            context.Response.End();
        }

        /*
         * 1104 НДС 0%
           1103 НДС 10%
           1102 НДС 18%
           1105 НДС не облагается
           1107 НДС с рассч. ставкой 10%
           1106 НДС с рассч. ставкой 18%
         */
        private string GetTaxType(TaxType? taxType)
        {
            if (taxType == null)
                return "1104";

            if (taxType.Value == TaxType.Without)
                return "1105";

            if (taxType.Value == TaxType.Zero)
                return "1104";

            if (taxType.Value == TaxType.Ten)
                return "1103";

            if (taxType.Value == TaxType.Eighteen)
                return "1102";

            return "1104";
        }
    }
}