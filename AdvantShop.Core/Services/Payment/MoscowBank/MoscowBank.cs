using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Localization;
using AdvantShop.Orders;

namespace AdvantShop.Payment
{
    [PaymentKey("MoscowBank")]
    public class MoscowBank : PaymentMethod
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
            get { return UrlStatus.NotificationUrl; }
        }

        public string Merchant { get; set; }
        public string Terminal { get; set; }
        public string MerchName { get; set; }
        public string Email { get; set; }
        public string Key { get; set; }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                {
                    {MoscowBankTemplate.Merchant, Merchant},
                    {MoscowBankTemplate.Terminal, Terminal},
                    {MoscowBankTemplate.MerchName, MerchName},
                    {MoscowBankTemplate.Email, Email},
                    {MoscowBankTemplate.Key, Key},
                };
            }
            set
            {
                Merchant = value.ElementOrDefault(MoscowBankTemplate.Merchant);
                Terminal = value.ElementOrDefault(MoscowBankTemplate.Terminal);
                MerchName = value.ElementOrDefault(MoscowBankTemplate.MerchName);
                Email = value.ElementOrDefault(MoscowBankTemplate.Email);
                Key = value.ElementOrDefault(MoscowBankTemplate.Key);
            }
        }

        public override void ProcessForm(Order order)
        {
            string sum = (order.Sum*GetCurrencyRate(order.OrderCurrency)).ToString("#0.##").Replace(',', '.');
            // порядок из формата
            var values = new Dictionary<string, string>
            {
                {"AMOUNT", sum},
                {"CURRENCY", PaymentCurrency.Iso3},
                {"ORDER", GetOrderIdString(order.OrderID)},
                {"DESC", string.Format("order #{0} payment", order.Number)},
                {"MERCH_NAME", MerchName},
                {"MERCH_URL", SettingsMain.SiteUrl.ToLower()},
                {"MERCHANT", Merchant},
                {"TERMINAL", Terminal},
                {"EMAIL", Email},
                {"TRTYPE", "1"},
                {"COUNTRY", Culture.Language == Culture.SupportLanguage.Russian ? "RU" : "EN"},
                {"MERCH_GMT", (DateTime.Now - DateTime.UtcNow).Hours.ToString()},
                {"TIMESTAMP", DateTime.UtcNow.ToString("yyyyMMddHHmmss")},
                {"NONCE", GenerateNonce()},
                {"BACKREF", SuccessUrl}
            };
            values.Add("P_SIGN", GetPSign(values));
            new PaymentFormHandler
            {
                FormName = "_xclick",
                Method = FormMethod.POST,
                Url = "https://3ds2.mmbank.ru/cgi-bin/cgi_link",
                InputValues = values
            }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            string sum = (order.Sum*GetCurrencyRate(order.OrderCurrency)).ToString("#0.##").Replace(',', '.');
            // порядок из формата
            var values = new Dictionary<string, string>
            {
                {"AMOUNT", sum},
                {"CURRENCY", PaymentCurrency.Iso3},
                {"ORDER", GetOrderIdString(order.OrderID)},
                {"DESC", string.Format("order #{0} payment", order.Number)},
                {"MERCH_NAME", MerchName},
                {"MERCH_URL", SettingsMain.SiteUrl.ToLower()},
                {"MERCHANT", Merchant},
                {"TERMINAL", Terminal},
                {"EMAIL", Email},
                {"TRTYPE", "1"},
                {"COUNTRY", Culture.Language == Culture.SupportLanguage.Russian ? "RU" : "EN"},
                {"MERCH_GMT", (DateTime.Now - DateTime.UtcNow).Hours.ToString()},
                {"TIMESTAMP", DateTime.UtcNow.ToString("yyyyMMddHHmmss")},
                {"NONCE", GenerateNonce()},
                {"BACKREF", SuccessUrl}
            };
            values.Add("P_SIGN", GetPSign(values));
            return new PaymentFormHandler
            {
                FormName = "_xclick",
                Method = FormMethod.POST,
                Page = page,
                Url = "https://3ds2.mmbank.ru/cgi-bin/cgi_link",
                InputValues = values
            }.ProcessRequest();
        }

        // возвращает пустую строку, т.к. платежной системой на returnurl не передаются параметры
        public override string ProcessResponse(HttpContext context)
        {
            HttpRequest req = context.Request;
            int orderID = 0;
            if (CheckFields(req) && int.TryParse(req["ORDER"], out orderID))
            {
                Order order = OrderService.GetOrder(orderID);
                if (order != null)
                {
                    OrderService.PayOrder(orderID, true);
                    return string.Empty;
                    //return NotificationMessahges.SuccessfullPayment(orderID.ToString());
                }
            }
            return string.Empty;
            //return NotificationMessahges.InvalidRequestData;
        }

        private string GetPSign(Dictionary<string, string> values)
        {
            var sb = new StringBuilder();
            foreach (var val in values)
            {
                if (val.Value.Length > 0)
                    sb.AppendFormat("{0}{1}", val.Value.Length, val.Value);
                else
                    sb.Append("-");
            }

            var input = sb.ToString();
            var privateKey = Key;

            // если вдруг нечетное число символов в ключе
            if (privateKey.Length%2 != 0)
                privateKey += "0";
            // для макирования необходим массив байт с кодами шестнадцатеричных чисел ключа
            var bytes = new List<byte>();
            for (int i = 0; i < privateKey.Length; i += 2)
            {
                var hex = privateKey.Substring(i, 2);
                bytes.Add((byte) int.Parse(hex, NumberStyles.AllowHexSpecifier));
            }
            var bitesArr = bytes.ToArray();

            var encoding = new UTF8Encoding();

            var hmac = new HMACSHA1(bitesArr);
            var hash = hmac.ComputeHash(encoding.GetBytes(input));

            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        private string GenerateNonce()
        {
            var rand = new Random(DateTime.Now.Millisecond);
            // 8-32 байта
            int length = rand.Next(8, 33);

            var bytes = new byte[length];
            rand.NextBytes(bytes);

            var sb = new StringBuilder();
            foreach (var b in bytes)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        private bool CheckFields(HttpRequest req)
        {
            var fields = new string[]
            {
                "ACTION",
                "ORDER",
                "AMOUNT",
                "CURRENCY",
                "RRN",
                "INT_REF",
                "TRTYPE",
                "TERMINAL",
                "TIMESTAMP",
                "NONCE",
                "P_SIGN"
            };
            if (fields.Any(val => string.IsNullOrEmpty(req[val])))
                return false;

            //ACTION - Код ответа e-Commerce Gateway:
            //0 – Транзакция успешно завершена
            //1 – Обнаружена повторная операция
            //2 – Транзакция отклонена
            //3 – Ошибка обработки транзакции
            if (req["ACTION"] != "0")
                return false;

            // порядок из формата
            var values = new Dictionary<string, string>
            {
                {"ORDER", req["ORDER"]},
                {"AMOUNT", req["AMOUNT"]},
                {"CURRENCY", req["CURRENCY"]},
                {"RRN", req["RRN"]},
                {"INT_REF", req["INT_REF"]},
                {"TRTYPE", req["TRTYPE"]},
                {"TERMINAL", req["TERMINAL"]},
                {"TIMESTAMP", req["TIMESTAMP"]},
                {"NONCE", req["NONCE"]}
            };
            return GetPSign(values).ToLower() == req["P_SIGN"].ToLower();
        }

        /// <summary>
        /// Номер заказа, длиной не меньше шести символов
        /// </summary>
        /// <param name="orderId">Номер заказа</param>
        /// <returns></returns>
        private string GetOrderIdString(int orderId)
        {
            return orderId.ToString().Length < 6 ? orderId.ToString().PadLeft(6, '0') : orderId.ToString();
        }
    }
}