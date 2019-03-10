//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Payment
{
    [PaymentKey("GoogleCheckout")]
    public class GoogleCheckout : PaymentMethod
    {
        public string MerchantID { get; set; }
        public float CurrencyValue { get; set; }
        public string CurrencyCode { get; set; }
        public bool Sandbox { get; set; }

        #region PaymentMethod Members
        
        public override ProcessType ProcessType
        {
            get { return ProcessType.FormPost; }
        }

        public override NotificationType NotificationType
        {
            get { return NotificationType.Handler; }
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {GoogleCheckoutTemplate.MerchantID, MerchantID},
                               //{GoogleCheckoutTemplate.CancelUrl, CancelUrl},
                               {GoogleCheckoutTemplate.Sandbox, Sandbox.ToString()},
                               {GoogleCheckoutTemplate.CurrencyValue, CurrencyValue.ToString()},
                               {GoogleCheckoutTemplate.CurrencyCode, CurrencyCode}
                           };
            }
            set
            {
                MerchantID = value.ContainsKey(GoogleCheckoutTemplate.MerchantID) ? value[GoogleCheckoutTemplate.MerchantID] : "";

                //CancelUrl = !value.ContainsKey(GoogleCheckoutTemplate.CancelUrl) ? SettingsMain.SiteUrl : value[GoogleCheckoutTemplate.CancelUrl];

                if (!value.ContainsKey(GoogleCheckoutTemplate.CurrencyCode))
                {
                    CurrencyCode = "USD";
                    Currency dollar = CurrencyService.Currency("USD");
                    CurrencyValue = dollar != null ? dollar.Rate : 1;
                }
                else
                {
                    CurrencyCode = value[GoogleCheckoutTemplate.CurrencyCode];
                    if (!value.ContainsKey(GoogleCheckoutTemplate.CurrencyValue))
                        CurrencyValue = 1;
                    else
                    {
                        float val;
                        CurrencyValue = float.TryParse(value[GoogleCheckoutTemplate.CurrencyValue], out val) ? val : 1;
                    }
                }
                bool boolVal;
                if (value.ContainsKey(GoogleCheckoutTemplate.Sandbox) &&
                    bool.TryParse(value[GoogleCheckoutTemplate.Sandbox], out boolVal))
                    Sandbox = boolVal;
            }
        }

        public override void ProcessForm(Order order)
        {
            new PaymentFormHandler
            {
                FormName = "pay",
                Method = FormMethod.POST,
                Url =
                    string.Format("https://{0}/api/checkout/v2/checkoutForm/Merchant/{1}",
                        Sandbox ? "sandbox.google.com/checkout" : "checkout.google.com", MerchantID),
                InputValues = new Dictionary<string, string>
                {
                    {"item_currency_1", CurrencyCode},
                    {"item_name_1",GetOrderDescription(order.Number)},
                    {"item_description_1",GetOrderDescription(order.Number)},
                    {"item_quantity_1", "1"},
                    {
                        "item_price_1",
                        CurrencyService.ConvertCurrency(order.Sum, CurrencyValue,
                            order.OrderCurrency.CurrencyValue).
                            ToString().Replace(",", ".")
                    },
                    {"item_merchant_id_1", order.Number},
                    {"return", SuccessUrl},
                    {"cancel_return", CancelUrl},
                    {"_charset_", ""}
                }
            }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            return new PaymentFormHandler
            {
                FormName = "pay",
                Method = FormMethod.POST,
                Page = page,
                Url =
                    string.Format("https://{0}/api/checkout/v2/checkoutForm/Merchant/{1}",
                        Sandbox ? "sandbox.google.com/checkout" : "checkout.google.com", MerchantID),
                InputValues = new Dictionary<string, string>
                {
                    {"item_currency_1", CurrencyCode},
                    {"item_name_1", GetOrderDescription(order.Number)},
                    {"item_description_1", GetOrderDescription(order.Number)},
                    {"item_quantity_1", "1"},
                    {
                        "item_price_1",
                        CurrencyService.ConvertCurrency(order.Sum, CurrencyValue,
                            order.OrderCurrency.CurrencyValue).
                            ToString().Replace(",", ".")
                    },
                    {"item_merchant_id_1", order.Number},
                    {"return", SuccessUrl},
                    {"cancel_return", CancelUrl},
                    {"_charset_", ""}
                }
            }.ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            if (Sandbox)
                return NotificationMessahges.TestMode;
            if (string.IsNullOrEmpty(context.Request["serial-number"]))
                return NotificationMessahges.InvalidRequestData;
            var client = new WebClient();
            try
            {
                string data =
                    Encoding.Default.GetString(
                        client.UploadValues(
                            string.Format("https://checkout.google.com/api/checkout/v2/reportsForm/Merchant/{0}",
                                          MerchantID),
                            new NameValueCollection
                                {
                                    {"_type", "notification-history-request"},
                                    {"serial-number", context.Request["serial-number"]}
                                }));
                Dictionary<string, string> values = GetResponseValues(data);
                if (values == null || !values.ContainsKey("_type") || values["_type"] != "notification-history-response" ||
                    !values.ContainsKey("merchant-item-id"))
                    return NotificationMessahges.InvalidRequestData;
                int orderID = OrderService.GetOrderIdByNumber(values["merchant-item-id"]);
                if (orderID == 0)
                    return NotificationMessahges.Fail;

                OrderService.PayOrder(orderID, true);
                return NotificationMessahges.SuccessfullPayment(values["merchant-item-id"]);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("at GoogleCheckout", ex);
                return NotificationMessahges.Fail;
            }
        }

        #endregion

        private static Dictionary<string, string> GetResponseValues(string responseText)
        {
            try
            {
                return
                    responseText.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries).Where(
                        vs => vs.Contains("=") && vs.IndexOf('=') != 0).Select(valString => valString.Split('=')).
                        ToDictionary(keyVal => keyVal[0], keyVal => keyVal[1]);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}