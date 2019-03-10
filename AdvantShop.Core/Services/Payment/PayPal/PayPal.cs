//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Orders;
using AdvantShop.Repository;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Payment
{
    [PaymentKey("PayPal")]
    public class PayPal : PaymentMethod
    {

        public static readonly List<string> AvaliableCurrs = new List<string> {
                                                              "AUD", "CAD", "CZK", "DKK", "EUR", "HKD", "HUF", "ILS", "JPY"
                                                              , "MXN", "NOK", "NZD", "PLN", "GBP", "SGD", "SEK", "CHF",
                                                              "USD"
                                                          };

        public override ProcessType ProcessType
        {
            get { return ProcessType.FormPost; }
        }
        public override NotificationType NotificationType
        {
            get { return NotificationType.Handler | NotificationType.ReturnUrl; }
        }
        public override UrlStatus ShowUrls
        {
            get { return UrlStatus.CancelUrl | UrlStatus.ReturnUrl | UrlStatus.NotificationUrl; }
        }
        public string EMail { get; set; }

        public string PDTCode { get; set; }
        //public string ReturnUrl { get; set; }
        //public string CancelUrl { get; set; }
        public float CurrencyValue { get; set; }
        public string CurrencyCode { get; set; }
        public bool ShowTaxAndShipping { get; set; }
        public bool Sandbox { get; set; }
        private const string Command = "_xclick";
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {PayPalTemplate.EMail, EMail},
                               {PayPalTemplate.PDTCode, PDTCode},
                               //{PayPalTemplate.ReturnUrl, ReturnUrl},
                               //{PayPalTemplate.CancelUrl, CancelUrl},
                               {PayPalTemplate.CurrencyCode, CurrencyCode},
                               {PayPalTemplate.CurrencyValue, CurrencyValue.ToString(CultureInfo.InvariantCulture)},
                               {PayPalTemplate.ShowTaxAndShipping, ShowTaxAndShipping.ToString()},
                               {PayPalTemplate.Sandbox, Sandbox.ToString()}
                           };
            }
            set
            {
                if (value.ContainsKey(PayPalTemplate.EMail))

                    EMail = value[PayPalTemplate.EMail];
                PDTCode = !value.ContainsKey(PayPalTemplate.PDTCode) ? "" : value[PayPalTemplate.PDTCode];
                //ReturnUrl = !value.ContainsKey(PayPalTemplate.ReturnUrl) ? SettingsMain.SiteUrl : value[PayPalTemplate.ReturnUrl];
                //CancelUrl = !value.ContainsKey(PayPalTemplate.CancelUrl) ? SettingsMain.SiteUrl : value[PayPalTemplate.CancelUrl];

                if (!value.ContainsKey(PayPalTemplate.CurrencyCode))
                {
                    CurrencyCode = "USD";
                    var dollar = CurrencyService.Currency("USD");
                    CurrencyValue = dollar != null ? dollar.Rate : 1;
                }
                else
                {
                    CurrencyCode = value[PayPalTemplate.CurrencyCode];
                    if (!value.ContainsKey(PayPalTemplate.CurrencyValue))
                        CurrencyValue = 1;
                    else
                    {
                        float val;
                        CurrencyValue = float.TryParse(value[PayPalTemplate.CurrencyValue], out val) ? val : 1;
                    }
                }
                ShowTaxAndShipping = value.ElementOrDefault(PayPalTemplate.ShowTaxAndShipping).TryParseBool();
                Sandbox = value.ElementOrDefault(PayPalTemplate.Sandbox).TryParseBool();
            }
        }

        private string GetUrl()
        {
            return string.Format("https://{0}/cgi-bin/webscr", Sandbox ? "www.sandbox.paypal.com" : "www.paypal.com");
        }

        public override void ProcessForm(Order order)
        {
            //int index = 0;
            //if (!string.IsNullOrEmpty(order.BillingContact.Name)) index = order.BillingContact.Name.IndexOf(" ");
            //string firstName = string.Empty;
            //string lastName = string.Empty;
            //if (index > 0)
            //{
            //    firstName = order.BillingContact.Name.Substring(0, index).Trim();
            //    lastName = order.BillingContact.Name.Substring(index + 1).Trim();
            //}
            //else
            //    firstName = order.BillingContact.Name.Trim();

            var firstName = order.OrderCustomer.FirstName ?? "";
            var lastName = order.OrderCustomer.LastName ?? "";

            new PaymentFormHandler
                {
                    FormName = "_xclick",
                    Method = FormMethod.POST,
                    Url = GetUrl(),
                    InputValues = new Dictionary<string, string>
                                      {
                                          {"cmd", Command},
                                          {"business", EMail},
                                          {"charset", "utf-8"},
                                          {"currency_code", CurrencyCode},
                                          {"item_name", string.Format("Order #{0}", order.OrderID)},
                                          {"invoice", order.Number},
                                          {"amount", CurrencyService.ConvertCurrency(ShowTaxAndShipping ? order.Sum - order.TaxCost - order.ShippingCost : order.Sum, CurrencyValue,order.OrderCurrency.CurrencyValue).ToString("F2").Replace(",", ".")},
                                          {"tax", CurrencyService.ConvertCurrency(ShowTaxAndShipping ? order.TaxCost : 0, CurrencyValue,order.OrderCurrency.CurrencyValue).ToString("F2").Replace(",", ".")},
                                          {"shipping", CurrencyService.ConvertCurrency(ShowTaxAndShipping ? order.ShippingCost : 0, CurrencyValue,order.OrderCurrency.CurrencyValue).ToString("F2").Replace(",", ".")},
                                          {"address1", (order.OrderCustomer.GetCustomerAddress()).Replace("\n", "")},
                                          {"city", order.OrderCustomer.City ?? string.Empty},
                                          {"country", CountryService.GetIso2(order.OrderCustomer.Country ?? string.Empty) ?? string.Empty},
                                          {"lc", CountryService.GetIso2(order.OrderCustomer.Country ?? string.Empty) ?? string.Empty},
                                          //{"email", order.BillingContact.Email ?? string.Empty},
                                          {"email", order.OrderCustomer .Email ?? string.Empty},
                                          {"first_name", firstName ?? string.Empty},
                                          {"last_name", lastName ?? string.Empty},
                                          {"zip", order.OrderCustomer.Zip ?? string.Empty},
                                          {"state", order.OrderCustomer.Region ?? string.Empty},
                                          {"return", SuccessUrl},
                                          {"notify_url", NotificationUrl},
                                          {"cancel_return", CancelUrl}
                                      }
                }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            //int index = 0;
            //if (!string.IsNullOrEmpty(order.BillingContact.Name)) index = order.BillingContact.Name.IndexOf(" ");
            //string firstName = string.Empty;
            //string lastName = string.Empty;
            //if (index > 0)
            //{
            //    firstName = order.BillingContact.Name.Substring(0, index).Trim();
            //    lastName = order.BillingContact.Name.Substring(index + 1).Trim();
            //}
            //else
            //    firstName = order.BillingContact.Name.Trim();

            var firstName = order.OrderCustomer.FirstName ?? "";
            var lastName = order.OrderCustomer.LastName ?? "";

            return new PaymentFormHandler
               {
                   FormName = "_xclick",
                   Method = FormMethod.POST,
                   Url = GetUrl(),
                   Page = page,
                   InputValues = new Dictionary<string, string>
                                      {
                                          {"cmd", Command},
                                          {"business", EMail},
                                          {"charset", "utf-8"},
                                          {"currency_code", CurrencyCode},
                                          {"item_name", string.Format("Order #{0}", order.OrderID)},
                                          {"invoice", order.Number},
                                          {"amount", CurrencyService.ConvertCurrency(ShowTaxAndShipping ? order.Sum - order.TaxCost - order.ShippingCost : order.Sum, CurrencyValue,order.OrderCurrency.CurrencyValue).ToString("F2").Replace(",", ".")},
                                          {"tax", CurrencyService.ConvertCurrency(ShowTaxAndShipping ? order.TaxCost : 0, CurrencyValue,order.OrderCurrency.CurrencyValue).ToString("F2").Replace(",", ".")},
                                          {"shipping", CurrencyService.ConvertCurrency(ShowTaxAndShipping ? order.ShippingCost : 0, CurrencyValue,order.OrderCurrency.CurrencyValue).ToString("F2").Replace(",", ".")},
                                          {"address1", (order.OrderCustomer.GetCustomerAddress()).Replace("\n", "")},
                                          {"city", order.OrderCustomer.City ?? string.Empty},
                                          {"country", CountryService.GetIso2(order.OrderCustomer.Country ?? string.Empty) ?? string.Empty},
                                          {"lc", CountryService.GetIso2(order.OrderCustomer.Country ?? string.Empty) ?? string.Empty},
                                          //{"email", order.BillingContact.Email ?? string.Empty},
                                          {"email", order.OrderCustomer .Email ?? string.Empty},
                                          {"first_name", firstName ?? string.Empty},
                                          {"last_name", lastName ?? string.Empty},
                                          {"zip", order.OrderCustomer.Zip ?? string.Empty},
                                          {"state", order.OrderCustomer.Region ?? string.Empty},
                                          {"return", SuccessUrl},
                                          {"notify_url", NotificationUrl},
                                          {"cancel_return", CancelUrl}
                                      }
               }.ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {

            //if (Sandbox)
            //    return NotificationMessahges.TestMode;
            //if (string.IsNullOrEmpty(context.Request["tx"]))
            //    return NotificationMessahges.InvalidRequestData;
            //var tx = context.Request["tx"];
            try
            {
                bool isValidRequest = false;
                string orderNumber = string.Empty;
                string mcGross = string.Empty;

                if (!string.IsNullOrEmpty(GetPrm(context, "verify_sign")))
                {
                    string strFormValues = Encoding.ASCII.GetString(context.Request.BinaryRead(context.Request.ContentLength));

                    // Create the request back
                    var req = (HttpWebRequest)WebRequest.Create(GetUrl());

                    // Set values for the request back
                    req.Method = "POST";
                    req.ContentType = "application/x-www-form-urlencoded";
                    //now PayPal requires user-agent. otherwise, we can get 403 error
                    req.UserAgent = HttpContext.Current.Request.UserAgent;

                    string strNewValue = strFormValues + "&cmd=_notify-validate";
                    req.ContentLength = strNewValue.Length;

                    // Write the request back IPN strings
                    using (var stOut = new StreamWriter(req.GetRequestStream(), Encoding.ASCII))
                        stOut.Write(strNewValue);

                    // send the request, read the response
                    using (var strResponse = (HttpWebResponse)req.GetResponse())
                    {
                        using (var ipnResponseStream = strResponse.GetResponseStream())
                        {
                            var encode = Encoding.GetEncoding("utf-8");
                            using (var readStream = new StreamReader(ipnResponseStream, encode))
                            {
                                var read = new char[256];
                                // Reads 256 characters at a time.
                                int count = readStream.Read(read, 0, 256);
                                while (count > 0)
                                {
                                    // Dumps the 256 characters to a string and displays the string to the console.
                                    var ipnResponse = new String(read, 0, count);
                                    count = readStream.Read(read, 0, 256);
                                    // if IPN response was VERIFIED..perform VERIFIED handling
                                    //for this example - send email of raw IPN string
                                    if (ipnResponse == "VERIFIED")
                                        isValidRequest = true;
                                }
                                //tidy up, close streams
                                readStream.Close();
                                strResponse.Close();
                            }
                        }
                    }
                    orderNumber = GetPrm(context, "invoice");
                    mcGross = GetPrm(context, "mc_gross");
                }
                else if (!string.IsNullOrEmpty(GetPrm(context, "tx")))
                {
                    //var req = (HttpWebRequest)WebRequest.Create(GetUrl());
                    //req.Method = "POST";
                    //req.ContentType = "application/x-www-form-urlencoded";
                    //req.UserAgent = HttpContext.Current.Request.UserAgent;

                    //string formContent = string.Format("cmd=_notify-synch&at={0}&tx={1}", PDTCode, GetPrm(context, "tx"));
                    //req.ContentLength = formContent.Length;

                    //using (var sw = new StreamWriter(req.GetRequestStream(), Encoding.ASCII))
                    //    sw.Write(formContent);

                    //string response;
                    //using (var sr = new StreamReader(req.GetResponse().GetResponseStream()))
                    //    response = HttpUtility.UrlDecode(sr.ReadToEnd());

                    //var success = response.Split('\n').Select(l => l.Trim()).First();
                    //if (success != null && success.Equals("SUCCESS", StringComparison.OrdinalIgnoreCase))
                    if(context.Request["st"] == "Completed")
                    {
                        isValidRequest = true;
                        //this code because paypal didn't tell us why it send fail
                        orderNumber = context.Request["item_name"].Replace("Order%20%23", "");
                        mcGross = context.Request["amt"];

                        //orderNumber =
                        //    response.Split('\n').FirstOrDefault(
                        //        line => line.Contains('=') && line.Trim().Substring(0, line.IndexOf('=')) == "invoice");

                        //mcGross =
                        //    response.Split('\n').FirstOrDefault(
                        //        line => line.Contains('=') && line.Trim().Substring(0, line.IndexOf('=')) == "mc_gross");

                        //if (!string.IsNullOrEmpty(orderNumber))
                        //    orderNumber = orderNumber.Substring(orderNumber.IndexOf('=') + 1);

                        //if (!string.IsNullOrEmpty(mcGross))
                        //    mcGross = mcGross.Substring(mcGross.IndexOf('=') + 1);
                    }
                }

                if (isValidRequest && !string.IsNullOrEmpty(orderNumber) && !string.IsNullOrEmpty(mcGross))
                {
                    Order order = OrderService.GetOrder(OrderService.GetOrderIdByNumber(orderNumber));
                    if (order != null)
                    {
                        var provider = new CultureInfo(CultureInfo.InvariantCulture.LCID)
                        {
                            NumberFormat = {NumberDecimalSeparator = "."}
                        };
                        float summ;
                        float.TryParse(mcGross, NumberStyles.AllowDecimalPoint, provider, out summ);
                        if (summ < Math.Round(CurrencyService.ConvertCurrency(order.Sum, CurrencyValue, order.OrderCurrency.CurrencyValue), 2))
                            return NotificationMessahges.Fail;

                        string fileName = context.Request.FilePath.ToLower();
                        var ntFile = NotificationType.None;
                        if (fileName.Contains("paymentnotification"))
                            ntFile = NotificationType.Handler;

                        if (fileName.Contains("paymentreturnurl"))
                            ntFile = NotificationType.ReturnUrl;

                        if (((this.NotificationType & ntFile) == NotificationType.Handler) || ((this.NotificationType & ntFile) == NotificationType.ReturnUrl))
                            OrderService.PayOrder(order.OrderID, true);

                        return NotificationMessahges.SuccessfullPayment(orderNumber);
                    }
                }

                return NotificationMessahges.Fail;

                //string formContent = string.Format("cmd=_notify-validate");
                //req.ContentLength = formContent.Length;

                //using (var sw = new StreamWriter(req.GetRequestStream(), Encoding.ASCII))
                //    sw.Write(formContent);

                //string response;
                //using (var sr = new StreamReader(req.GetResponse().GetResponseStream()))
                //    response = HttpUtility.UrlDecode(sr.ReadToEnd());

                //var success = response.Split('\n').Select(l => l.Trim()).First();
                //if (success != null && success.Equals("SUCCESS", StringComparison.OrdinalIgnoreCase))
                //{
                //    var orderNumber =
                //        response.Split('\n').FirstOrDefault(
                //            line => line.Contains('=') && line.Trim().Substring(0, line.IndexOf('=')) == "invoice");
                //    if (!string.IsNullOrEmpty(orderNumber))
                //        OrderService.PayOrder(OrderService.GetOrderIdByNumber(orderNumber), true);

                //    return NotificationMessahges.SuccessfullPayment(orderNumber);
                //}
                //return NotificationMessahges.Fail;
            }
            catch (Exception ex)
            {
                return NotificationMessahges.LogError(ex);
            }
            //TODO ORDER PAYMENT TEST
        }

        private string GetPrm(HttpContext context, string sName)
        {
            string sValue;
            if (context != null && !string.IsNullOrEmpty(sName))
            {
                sValue = context.Request.Form[sName];
                if (string.IsNullOrEmpty(sValue)) sValue = context.Request.QueryString[sName];
                if (string.IsNullOrEmpty(sValue)) sValue = string.Empty;
            }
            else sValue = string.Empty;

            return sValue;
        }
    }
}