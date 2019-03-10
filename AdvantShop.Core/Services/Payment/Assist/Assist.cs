//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;

using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;


namespace AdvantShop.Payment
{
    /// <summary>
    /// Summary description for Assist
    /// </summary>
    [PaymentKey("Assist")]
    public class Assist : PaymentMethod
    {
        public override ProcessType ProcessType
        {
            get { return ProcessType.FormPost; }
        }
        public override NotificationType NotificationType
        {
            get { return NotificationType.ReturnUrl; }
        }

        public override UrlStatus ShowUrls
        {
            get { return UrlStatus.CancelUrl | UrlStatus.ReturnUrl | UrlStatus.NotificationUrl; }
        }

        public int MerchantID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string UrlWorkingMode { get; set; }

        //public string ReturnUrl { get; set; }
        //public string CancelUrl { get; set; }
        //public string FailUrl { get; set; }
        public string CurrencyCode { get; set; }
        public float CurrencyValue { get; set; }
        public bool Sandbox { get; set; }
        public bool Delay { get; set; }

        public bool CardPayment { get; set; }
        public bool WebMoneyPayment { get; set; }
        public bool PayCashPayment { get; set; }
        public bool QiwiBeelinePayment { get; set; }
        public bool AssistIdCcPayment { get; set; }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {AssistTemplate.MerchantID, MerchantID.ToString()},
                               {AssistTemplate.Login, Login},
                               {AssistTemplate.Password, Password},
                               {AssistTemplate.UrlWorkingMode, UrlWorkingMode},
                               //{AssistTemplate.ReturnUrl, ReturnUrl},
                               //{AssistTemplate.CancelUrl, CancelUrl},
                               //{AssistTemplate.FailUrl, FailUrl},
                               {AssistTemplate.Sandbox, Sandbox.ToString()},
                               {AssistTemplate.CurrencyCode, CurrencyCode},
                               {AssistTemplate.CurrencyValue, CurrencyValue.ToString()},
                               {AssistTemplate.Delay, Delay.ToString()},
                               //{AssistTemplate.CardPayment, CardPayment.ToString()},
                               //{AssistTemplate.WebMoneyPayment, WebMoneyPayment.ToString()},
                               //{AssistTemplate.PayCashPayment, PayCashPayment.ToString()},
                               //{AssistTemplate.QiwiBeelinePayment, QiwiBeelinePayment.ToString()},
                               //{AssistTemplate.AssistIdCcPayment, AssistIdCcPayment.ToString()}
                           };
            }
            set
            {
                MerchantID = value.ElementOrDefault(AssistTemplate.MerchantID).TryParseInt();
                Login = value.ElementOrDefault(AssistTemplate.Login);
                Password = value.ElementOrDefault(AssistTemplate.Password);
                UrlWorkingMode = value.ElementOrDefault(AssistTemplate.UrlWorkingMode);
                //ReturnUrl = !value.ContainsKey(AssistTemplate.ReturnUrl) ? SettingsMain.SiteUrl : value[AssistTemplate.ReturnUrl];
                //CancelUrl = !value.ContainsKey(AssistTemplate.CancelUrl) ? SettingsMain.SiteUrl : value[AssistTemplate.CancelUrl];
                //FailUrl = !value.ContainsKey(AssistTemplate.FailUrl) ? SettingsMain.SiteUrl : value[AssistTemplate.FailUrl];

                if (!value.ContainsKey(AssistTemplate.CurrencyCode))
                {
                    CurrencyCode = "USD";
                    var dollar = CurrencyService.Currency("USD");
                    CurrencyValue = dollar != null ? dollar.Rate : 1;
                }
                else
                {
                    CurrencyCode = value[AssistTemplate.CurrencyCode];

                    if (!value.ContainsKey(AssistTemplate.CurrencyValue))
                        CurrencyValue = 1;
                    else
                    {
                        float val;
                        CurrencyValue = float.TryParse(value[AssistTemplate.CurrencyValue], out val) ? val : 1;
                    }
                }

                Sandbox = value.ElementOrDefault(AssistTemplate.Sandbox).TryParseBool();
                Delay = value.ElementOrDefault(AssistTemplate.Delay).TryParseBool();


                //CardPayment = !bool.TryParse(value.ElementOrDefault(AssistTemplate.CardPayment), out boolVal) || boolVal;
                //WebMoneyPayment = !bool.TryParse(value.ElementOrDefault(AssistTemplate.WebMoneyPayment), out boolVal) || boolVal;
                //PayCashPayment = !bool.TryParse(value.ElementOrDefault(AssistTemplate.PayCashPayment), out boolVal) || boolVal;
                //QiwiBeelinePayment = !bool.TryParse(value.ElementOrDefault(AssistTemplate.QiwiBeelinePayment), out boolVal) || boolVal;
                //AssistIdCcPayment = !bool.TryParse(value.ElementOrDefault(AssistTemplate.AssistIdCcPayment), out boolVal) || boolVal;
            }
        }
        public override void ProcessForm(Order order)
        {
            new PaymentFormHandler
            {
                FormName = "pay",
                Method = FormMethod.POST,
                //Url = string.Format("https://{0}/pay/order.cfm", Sandbox ? "test.paysecure.ru" : PayUrl),//"secure.paysecure.ru"
                Url = Sandbox ? "https://test.paysecure.ru/pay/order.cfm" : UrlWorkingMode,//"secure.paysecure.ru"
                InputValues = new Dictionary<string, string>
                                      {
                                          {"Merchant_ID", MerchantID.ToString()},
                                          {"OrderNumber", order.Number},
                                          {"Delay", Delay ? "1" : "0"},
                                          {"OrderAmount", order.Sum.ToString().Replace(",", ".")},
                                          {"OrderCurrency", CurrencyCode},
                                          {"URL_RETURN", CancelUrl},
                                          {"URL_RETURN_OK", SuccessUrl},
                                          {"URL_RETURN_NO", FailUrl},
                                          //{"CardPayment", CardPayment ? "1" : "0"},
                                          //{"WMPayment", WebMoneyPayment ? "1" : "0"},
                                          //{"PayCashPayment", PayCashPayment ? "1" : "0"},
                                          //{"QIWIBeelinePayment", QiwiBeelinePayment ? "1" : "0"},
                                          //{"AssistIDCCPayment", AssistIdCcPayment ? "1" : "0"},
                                          {"TestMode", Sandbox ? "1" : "0"}
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
                 //Url = string.Format("https://{0}/pay/order.cfm", Sandbox ? "test.paysecure.ru" : PayUrl),//"secure.paysecure.ru"
                 Url = Sandbox ? "https://test.paysecure.ru/pay/order.cfm" : UrlWorkingMode,//"secure.paysecure.ru"
                 InputValues = new Dictionary<string, string>
                                      {
                                          {"Merchant_ID", MerchantID.ToString()},
                                          {"OrderNumber", order.Number},
                                          {"Delay", Delay ? "1" : "0"},
                                          {"OrderAmount", order.Sum.ToString().Replace(",", ".")},
                                          {"OrderCurrency", CurrencyCode},
                                          {"URL_RETURN", CancelUrl},
                                          {"URL_RETURN_OK", SuccessUrl},
                                          {"URL_RETURN_NO", FailUrl},
                                          //{"CardPayment", CardPayment ? "1" : "0"},
                                          //{"WebMoneyPayment", WebMoneyPayment ? "1" : "0"},
                                          //{"PayCashPayment", PayCashPayment ? "1" : "0"},
                                          //{"QiwiBeelinePayment", QiwiBeelinePayment ? "1" : "0"},
                                          //{"AssistIDCCPayment", AssistIdCcPayment ? "1" : "0"},
                                          {"TestMode", Sandbox ? "1" : "0"}
                                      }
             }.ProcessRequest();
        }

        public static readonly List<string> AvaliableCurrs = new List<string> {
                                                              "AUD", "BYR", "DKK", "USD", "EUR", "ISK", "KZT", "CAD", "CNY",
                                                              "TRY", "NOK", "RUB", "XDR", "SGD", "UAH", "GBP", "SEK", "CHF",
                                                              "JPY"
                                                          };

        public override string ProcessResponse(HttpContext context)
        {
            if (Sandbox)
                return NotificationMessahges.TestMode;
            if (string.IsNullOrEmpty(context.Request["ordernumber"]) && string.IsNullOrWhiteSpace(context.Request["billnumber"]))
                return NotificationMessahges.InvalidRequestData;
            string orderNumber = context.Request["ordernumber"];
            string billnumber = context.Request["billnumber"];

            try
            {
                if (Delay)
                {
                    //if 2 stage 
                    var client = new WebClient();
                    var data =
                        client.UploadValues(
                        //string.Format("https://{0}/charge/charge.cfm", Sandbox ? "test.paysecure.ru" : PayUrl),//"secure.paysecure.ru"
                            Sandbox ? "https://test.paysecure.ru/charge/charge.cfm" : UrlWorkingMode,//"secure.paysecure.ru"
                            new NameValueCollection
                            {
                                {"Billnumber",billnumber},
                                {"Merchant_ID", MerchantID.ToString()},
                                {"Login", Login},
                                {"Password", Password},
                                //XML
                                {"Format", "3"}
                            });


                    var xml = new XmlDocument();
                    var temp = Encoding.UTF8.GetString(data).ToLower();
                    var start = temp.IndexOf("<!doctype");
                    var end = temp.IndexOf("]>");
                    temp = temp.Remove(start, (end + 2) - start);
                    xml.LoadXml(temp);
                    if (xml.DocumentElement != null && xml.DocumentElement.Name != "result")
                        throw new Exception("Invalid XML response");
                    if (xml.DocumentElement != null)
                    {
                        var orders = xml.DocumentElement.SelectNodes(string.Format("descendant::order[ordernumber='{0}' and response_code='AS000' and orderstate='Approved' ]", orderNumber.ToLower()));
                        if (orders != null && orders.Count > 0)
                            OrderService.PayOrder(OrderService.GetOrderIdByNumber(orderNumber), true);
                        else
                            return NotificationMessahges.InvalidRequestData;
                    }
                }
                else
                {
                    //if 1 stage
                    OrderService.PayOrder(OrderService.GetOrderIdByNumber(orderNumber), true);
                }
                return NotificationMessahges.SuccessfullPayment(orderNumber);
            }
            catch (Exception ex)
            {
                return NotificationMessahges.LogError(ex);
            }
        }
    }
}