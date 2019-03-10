//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Localization;
using AdvantShop.Orders;
using AdvantShop.Taxes;

namespace AdvantShop.Payment
{
    /// <summary>
    /// Summary description for Robokassa
    /// </summary>
   [PaymentKey("Robokassa")]
    public class Robokassa : PaymentMethod
    {
        #region Receipt

        private class Receipt
        {
            public List<Item> items { get; set; }
        }

        private class Item
        {
            public string name { get; set; }
            public float quantity { get; set; }
            public float sum { get; set; }
            public string tax { get; set; }
        }

        /*
        none – без НДС;
        vat0 – НДС по ставке 0%;
        vat10 – НДС чека по ставке 10%;
        vat18 – НДС чека по ставке 18%;
        vat110 – НДС чека по расчетной ставке 10/110;
        vat118 – НДС чека по расчетной ставке 18/118.
        */
        private string GetVatType(TaxType? taxType)
        {
            if (!taxType.HasValue || taxType.Value == TaxType.Without)
                return "none";

            if (taxType.Value == TaxType.Zero)
                return "vat0";

            if (taxType.Value == TaxType.Ten)
                return "vat10";

            if (taxType.Value == TaxType.Eighteen)
                return "vat18";

            return "none";
        }
        #endregion

        public static Dictionary<string, string> GetCurrencies()
        {
            return Currencies;
        }

        public static readonly Dictionary<string, string> Currencies = new Dictionary<string, string>
        {
            {"MtsR", "МТС"},
            {"MPBeelineR", "Билайн"},
            {"BANKR", "RUR Банковская карта"},
            {"OceanBankR", "RUR Океан Банк"},
            {"TerminalsPinpayR", "Pinpay"},
            {"TerminalsComepayR", "Кампэй"},
            {"TerminalsMElementR", "Мобил Элемент"},
            {"TerminalsNovoplatR", "Новоплат"},
            {"TerminalsUnikassaR", "Уникасса"},
            {"ElecsnetR", "Элекснет"},
            {"ContactR", "RUR Contact"},
            {"IFreeR", "RUR SMS"},
            {"VTB24R", "RUR ВТБ24"},
            {"TerminalsPkbR", "Петрокоммерц"},
            {"RapidaInR", "RUR Евросеть"},
            {"AlfaBankR", "Альфа-Клик"},
            {"EasyPayB", "EasyPay"},
            {"QiwiR", "QIWI Кошелек"},
            {"MoneyMailR", "RUR MoneyMail"},
            {"RuPayR", "RUR RBK Money"},
            {"TeleMoneyR", "RUR TeleMoney"},
            {"WebCredsR", "RUR WebCreds"},
            {"ZPaymentR", "RUR Z-Payment"},
            {"VKontakteMerchantR", "RUR ВКонтакте"},
            {"W1R", "RUR Единый Кошелек"},
            {"WMBM", "WMB"},
            {"WMEM", "WME"},
            {"WMGM", "WMG"},
            {"WMRM", "WMR"},
            {"WMUM", "WMU"},
            {"WMZM", "WMZ"},
            {"MailRuR", "Деньги@Mail.Ru"},
            {"PCR", "Яндекс.Деньги"}
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
        public string MerchantLogin { get; set; }
        public string Password { get; set; }
        public string PasswordNotify { get; set; }
        public string CurrencyLabel { get; set; }
        public bool SendReceiptData { get; set; }
        public bool IsTest { get; set; }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {RobokassaTemplate.MerchantLogin, MerchantLogin},
                               //{RobokassaTemplate.CurrencyLabel, CurrencyLabel},
                               {RobokassaTemplate.Password, Password},
                               {RobokassaTemplate.PasswordNotify, PasswordNotify},
                               {RobokassaTemplate.SendReceiptData, SendReceiptData.ToString()},
                               {RobokassaTemplate.IsTest, IsTest.ToString()},
                           };
            }
            set
            {
                if (value.ContainsKey(RobokassaTemplate.MerchantLogin))
                    MerchantLogin = value[RobokassaTemplate.MerchantLogin];
                Password = value.ElementOrDefault(RobokassaTemplate.Password);
                PasswordNotify = value.ElementOrDefault(RobokassaTemplate.PasswordNotify);
                //CurrencyLabel = !value.ContainsKey(RobokassaTemplate.CurrencyLabel)
                //                    ? "RUR"
                //                    : value[RobokassaTemplate.CurrencyLabel];
                SendReceiptData = value.ElementOrDefault(RobokassaTemplate.SendReceiptData).TryParseBool();
                IsTest = value.ElementOrDefault(RobokassaTemplate.IsTest).TryParseBool();
            }
        }

        public override void ProcessForm(Order order)
        {
             var receipt = SendReceiptData
                ? new Receipt
                {
                    items = OrderService.RecalculateItemsPriceIncludingAllDiscounts(order.OrderItems.Where(x => x.Price > 0).ToList(), order.ShippingCost, order.Sum)
                       .Select(item => new Item()
                       {
                           name = item.Name.Length > 64 ? item.Name.Substring(0, 64) : item.Name,
                           sum = (float)Math.Round(item.Price * item.Amount, 2),
                           quantity = item.Amount,
                           tax = GetVatType(item.TaxType)
                       }).ToList()
                }
                : null;

            if (order.ShippingCost > 0 && receipt != null)
            {
                receipt.items.Add(new Item { name = "Доставка", sum = order.ShippingCost, quantity = 1, tax = GetVatType(order.ShippingTaxType) });
            }

            var receiptString = receipt != null ? HttpUtility.UrlEncode(Newtonsoft.Json.JsonConvert.SerializeObject(receipt)) : null;
            var sum = (order.Sum * GetCurrencyRate(order.OrderCurrency)).ToString(CultureInfo.InvariantCulture);

            new PaymentFormHandler
            {
                FormName = "_xclick",
                Method = FormMethod.POST,
                Url = "https://auth.robokassa.ru/Merchant/Index.aspx",
                InputValues = new Dictionary<string, string>
                {
                    {"MrchLogin", MerchantLogin},
                    {"OutSum", sum},
                    {"InvId", order.OrderID.ToString()},
                    {"Desc", GetOrderDescription(order.Number)},
                    {"IncCurrLabel", CurrencyLabel},
                    {"IsTest", IsTest ? "1" : "0"},
                    {"Culture", Culture.Language == Culture.SupportLanguage.Russian ? "ru" : "en"},
                    {
                        "SignatureValue",
                        (MerchantLogin + ":" + sum + ":" + order.OrderID + ":" + (receiptString.IsNotEmpty() ? receiptString + ":" : string.Empty) + Password).Md5()
                    },
                    {"receipt", receiptString }
                }
            }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            var receipt = SendReceiptData 
                ? new Receipt
                {
                    items = OrderService.RecalculateItemsPriceIncludingAllDiscounts(order.OrderItems.Where(x => x.Price > 0).ToList(), order.ShippingCost, order.Sum)
                       .Select(item => new Item()
                       {
                           name = item.Name.Length > 64 ? item.Name.Substring(0, 64) : item.Name,
                           sum = (float)Math.Round(item.Price * item.Amount, 2),
                           quantity = item.Amount,
                           tax = GetVatType(item.TaxType)
                       }).ToList()
                }
                : null;

            if (order.ShippingCost > 0 && receipt != null)
            {
                receipt.items.Add(new Item { name = "Доставка", sum = order.ShippingCost, quantity = 1, tax = GetVatType(order.ShippingTaxType) });
            }

            var receiptString = receipt != null ? HttpUtility.UrlEncode(Newtonsoft.Json.JsonConvert.SerializeObject(receipt)) : null;
            var sum = (order.Sum * GetCurrencyRate(order.OrderCurrency)).ToString(CultureInfo.InvariantCulture);

            return new PaymentFormHandler
            {
                FormName = "_xclick",
                Method = FormMethod.POST,
                Page = page,
                Url = "https://auth.robokassa.ru/Merchant/Index.aspx",
                InputValues = new Dictionary<string, string>
                {
                    {"MrchLogin", MerchantLogin},
                    {"OutSum", sum},
                    {"InvId", order.OrderID.ToString()},
                    {"Desc", GetOrderDescription(order.Number)},
                    {"IncCurrLabel", CurrencyLabel},
                    {"IsTest", IsTest ? "1" : "0"},
                    {"Culture", Culture.Language == Culture.SupportLanguage.Russian ? "ru" : "en"},
                    {
                        "SignatureValue",
                        (MerchantLogin + ":" + sum + ":" + order.OrderID + ":" + (receiptString.IsNotEmpty() ? receiptString + ":" : string.Empty) + Password).Md5()
                    },
                    {"receipt", receiptString }
                }
            }.ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            if (context.Request.Url.AbsolutePath.Contains("paymentnotification"))
                return ProcessResponseNotify(context);
            return ProcessResponseReturn(context);
        }

        private string ProcessResponseReturn(HttpContext context)
        {
            var req = context.Request;
            int orderId = 0;

            if (int.TryParse(req["InvId"], out orderId))
            {
                if (CheckFields(req))
                {

                    Order order = OrderService.GetOrder(orderId);
                    if (order != null)
                    {
                        OrderService.PayOrder(orderId, true);
                        return NotificationMessahges.SuccessfullPayment(orderId.ToString());
                    }
                }
                return NotificationMessahges.InvalidRequestData;
            }
            return string.Empty;
        }

        private bool CheckFields(HttpRequest req)
        {
            if (string.IsNullOrEmpty(req["OutSum"]) || string.IsNullOrEmpty(req["InvId"]) || string.IsNullOrEmpty(req["Culture"]) ||
                string.IsNullOrEmpty(req["SignatureValue"]))
                return false;
            if (req["SignatureValue"].ToLower() !=
                (req["OutSum"].Trim() + ":" + req["InvId"] + ":" + Password).Md5(false))
                return false;
            return true;
        }

        private string ProcessResponseNotify(HttpContext context)
        {
            var req = context.Request;
            int orderId = 0;
            if (CheckFieldsExt(req) && int.TryParse(req["InvId"], out orderId))
            {
                Order order = OrderService.GetOrder(orderId);
                if (order != null)
                {
                    OrderService.PayOrder(orderId, true);
                    return string.Format("OK{0}", req["InvId"]);
                }
            }
            return NotificationMessahges.InvalidRequestData;
        }

        private bool CheckFieldsExt(HttpRequest req)
        {
            if (string.IsNullOrEmpty(req["OutSum"]) || string.IsNullOrEmpty(req["InvId"]) || string.IsNullOrEmpty(req["SignatureValue"]))
                return false;
            if (req["SignatureValue"].ToLower() !=
                (req["OutSum"].Trim() + ":" + req["InvId"] + ":" + PasswordNotify).Md5(false))
                return false;
            return true;
        }
    }
}