//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;

namespace AdvantShop.Payment
{
    [PaymentKey("Dibs")]
    public class Dibs : PaymentMethod
    {
        public static Dictionary<string, string> GetCurrencies()
        {
            return Currencies;
        }
        public static readonly Dictionary<string, string> Currencies = new Dictionary<string, string>
        {
            {"208", "Danish Kroner (DKK)"},
            {"978", "Euro (EUR)"},
            {"840", "US Dollar $ (USD)"},
            {"826", "English Pound £ (GBP)"},
            {"752", "Swedish Kroner (SEK)"},
            {"036", "Australian Dollar (AUD)"},
            {"124", "Canadian Dollar (CAD)"},
            {"352", "Icelandic Kroner (ISK)"},
            {"392", "Japanese Yen (JPY)"},
            {"554", "New Zealand Dollar (NZD)"},
            {"578", "Norwegian Kroner (NOK)"},
            {"756", "Swiss Franc (CHF)"},
            {"949", "Turkish Lire (TRY)"}
        };

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

        public string Merchant { get; set; }
        public string Account { get; set; }
        public string Currency { get; set; }
        public float CurrencyRate { get; set; }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {DibsTemplate.Merchant, Merchant},
                               
                               {DibsTemplate.Currency, Currency},
                               {DibsTemplate.CurrencyRate, CurrencyRate.ToString()}
                           };
            }
            set
            {
                Merchant = value.ElementOrDefault(DibsTemplate.Merchant);
                Currency = value.ElementOrDefault(DibsTemplate.Currency);
                CurrencyRate = value.ElementOrDefault(DibsTemplate.CurrencyRate).TryParseFloat();
            }
        }

        public override void ProcessForm(Order order)
        {
            string sum = Math.Round(order.Sum * CurrencyRate).ToString();

            new PaymentFormHandler
                {
                    FormName = "_xclick",
                    Method = FormMethod.GET,
                    Url = "https://payment.architrade.com/paymentweb/start.action",
                    InputValues = new Dictionary<string, string>
                                      {
                                          {"merchant", Merchant},
                                          {"amount", sum},
                                          {"accepturl", this.SuccessUrl},
                                          {"orderid", order.OrderID.ToString()},
                                          {"currency", Currency},
                                          {"test", "yes"},
                                          //{"acquirerinfo", ""},
                                          {"acquirerlang", CultureInfo.CurrentCulture.TwoLetterISOLanguageName},
                                          //{"voucher", order.OrderCustomer.FirstName + " " + order.OrderCustomer.LastName},
                                          //{"uniqueoid", order.BillingContact.Country + "," + order.BillingContact.City + "," + order.BillingContact.Address},
                                          //{"ticketrule", order.OrderCustomer.MobilePhone},
                                          //{"priceinfo1", order.OrderCustomer.CustomerIP},
                                          //{"preauth", order.OrderCustomer.CustomerIP},
                                          //{"postype", order.OrderCustomer.CustomerIP},
                                          //{"ordline0-1", order.OrderCustomer.CustomerIP},
                                          //{"ordertext", order.OrderCustomer.CustomerIP},
                                          //{"md5key", order.OrderCustomer.CustomerIP},
                                          //{"maketicket", order.OrderCustomer.CustomerIP},
                                          {"lang", CultureInfo.CurrentCulture.TwoLetterISOLanguageName},
                                          //{"ip", order.OrderCustomer.CustomerIP},
                                          //{"delivery1", order.OrderCustomer.CustomerIP},
                                          //{"decorator", order.OrderCustomer.CustomerIP},
                                          //{"capturenow", order.OrderCustomer.CustomerIP},
                                          //{"cancelurl", order.OrderCustomer.CustomerIP},
                                          //{"callbackurl", order.OrderCustomer.CustomerIP},
                                          //{"calcfee", "foo"},
                                          //{"account", order.OrderCustomer.CustomerIP},
                                          //{"HTTP_COOKIE", order.OrderCustomer.CustomerIP},
                                      }
                }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            string sum = Math.Round(order.Sum * CurrencyRate * 100).ToString();
            return new PaymentFormHandler
             {
                 FormName = "_xclick",
                 Method = FormMethod.GET,
                 Page = page,
                 Url = "https://payment.architrade.com/paymentweb/start.action",
                 InputValues = new Dictionary<string, string>
                                      {
                                          {"merchant", Merchant},
                                          {"amount", sum},
                                          {"accepturl", this.SuccessUrl},
                                          {"orderid", order.OrderID.ToString()},
                                          {"currency", Currency},
                                          {"test", "yes"},
                                          //{"acquirerinfo", ""},
                                          {"acquirerlang", CultureInfo.CurrentCulture.TwoLetterISOLanguageName},
                                          //{"voucher", order.OrderCustomer.FirstName + " " + order.OrderCustomer.LastName},
                                          //{"uniqueoid", order.BillingContact.Country + "," + order.BillingContact.City + "," + order.BillingContact.Address},
                                          //{"ticketrule", order.OrderCustomer.MobilePhone},
                                          //{"priceinfo1", order.OrderCustomer.CustomerIP},
                                          //{"preauth", order.OrderCustomer.CustomerIP},
                                          //{"postype", order.OrderCustomer.CustomerIP},
                                          //{"ordline0-1", order.OrderCustomer.CustomerIP},
                                          //{"ordertext", order.OrderCustomer.CustomerIP},
                                          //{"md5key", order.OrderCustomer.CustomerIP},
                                          //{"maketicket", order.OrderCustomer.CustomerIP},
                                          {"lang", CultureInfo.CurrentCulture.TwoLetterISOLanguageName},
                                          //{"ip", order.OrderCustomer.CustomerIP},
                                          //{"delivery1", order.OrderCustomer.CustomerIP},
                                          //{"decorator", order.OrderCustomer.CustomerIP},
                                          //{"capturenow", order.OrderCustomer.CustomerIP},
                                          //{"cancelurl", order.OrderCustomer.CustomerIP},
                                          //{"callbackurl", order.OrderCustomer.CustomerIP},
                                          //{"calcfee", "foo"},
                                          //{"account", order.OrderCustomer.CustomerIP},
                                          //{"HTTP_COOKIE", order.OrderCustomer.CustomerIP},

                                      }
             }.ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            HttpRequest req = context.Request;
            int orderID = 0;
            if (CheckFields(req) && int.TryParse(req["orderid"], out orderID))
            {
                Order order = OrderService.GetOrder(orderID);
                if (order != null)
                {
                    OrderService.PayOrder(orderID, true);
                    return NotificationMessahges.SuccessfullPayment(orderID.ToString());
                }
            }
            return NotificationMessahges.InvalidRequestData;
        }

        private bool CheckFields(HttpRequest req)
        {
            if (string.IsNullOrEmpty(req["transact"]) || string.IsNullOrEmpty(req["orderid"]) ||
                string.IsNullOrEmpty(req["statuscode"]))
                return false;
            
            return true;
        }
    }
}