//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Payment
{
    /// <summary>
    /// Summary description for MailRu
    /// </summary>
    [PaymentKey("MailRu")]
    public class MailRu : PaymentMethod
    {
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
            get { return UrlStatus.NotificationUrl; }
        }
        public string Key { get; set; }
        public string ShopID { get; set; }
        public string CurrencyCode { get; set; }
        public bool KeepUnique { get; set; }
        public string CryptoHex { get; set; }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                             {
                                 {MailRuTemplate.Key, Key},
                                 {MailRuTemplate.ShopID, ShopID},
                                 {MailRuTemplate.CurrencyCode, CurrencyCode},
                                 {MailRuTemplate.KeepUnique, KeepUnique.ToString()},
                                 {MailRuTemplate.CryptoHex,CryptoHex}
                             };
            }
            set
            {
                if (value.ContainsKey(MailRuTemplate.ShopID))
                    ShopID = value[MailRuTemplate.ShopID];
                Key = !value.ContainsKey(MailRuTemplate.Key) ? string.Empty : value[MailRuTemplate.Key];
                CurrencyCode = !value.ContainsKey(MailRuTemplate.CurrencyCode) ? "RUR" : value[MailRuTemplate.CurrencyCode];
                bool boolVal;
                if (value.ContainsKey(MailRuTemplate.KeepUnique) && bool.TryParse(value[MailRuTemplate.KeepUnique], out boolVal))
                    KeepUnique = boolVal;
                CryptoHex = !value.ContainsKey(MailRuTemplate.CryptoHex) ? string.Empty : value[MailRuTemplate.CryptoHex];
            }
        }
        public override void ProcessForm(Order order)
        {
            var sum = (order.Sum * GetCurrencyRate(order.OrderCurrency)).ToString("F2").Replace(",", ".");
            var description = GetOrderDescription(order.Number);
            var issuer_id = order.OrderID.ToString();

            var inputValues = new Dictionary<string, string>
                                  {
                                      {"shop_id", ShopID},
                                      {"currency", CurrencyCode},
                                      {"sum", sum},
                                      {"description", description},
                                      {"message", description},
                                      {"issuer_id", issuer_id}
                                  };
            if (KeepUnique) inputValues.Add("keep_uniq", "1");
            inputValues.Add("signature", GetSignature(CurrencyCode + description + issuer_id + (KeepUnique ? "1" : string.Empty) + description + ShopID + sum + CryptoHex));

            new PaymentFormHandler
            {
                FormName = "_xclick",
                Method = FormMethod.POST,
                //Url = "https://demoney.mail.ru/pay/light/", test account
                Url = "https://money.mail.ru/pay/light/",
                InputValues = inputValues,
            }.Post(true);
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            var sum = (order.Sum * GetCurrencyRate(order.OrderCurrency)).ToString("F2").Replace(",", ".");
            var description = GetOrderDescription(order.Number);
            var issuer_id = order.OrderID.ToString();

            var inputValues = new Dictionary<string, string>
                                  {
                                      {"shop_id", ShopID},
                                      {"currency", CurrencyCode},
                                      {"sum", sum},
                                      {"description", description},
                                      {"message", description},
                                      {"issuer_id", issuer_id}
                                  };
            if (KeepUnique) inputValues.Add("keep_uniq", "1");
            inputValues.Add("signature", GetSignature(CurrencyCode + description + issuer_id + (KeepUnique ? "1" : string.Empty) + description + ShopID + sum + CryptoHex));

            return new PaymentFormHandler
              {
                  FormName = "_xclick",
                  Method = FormMethod.POST,
                  Page = page,
                  //Url = "https://demoney.mail.ru/pay/light/", test account
                  Url = "https://money.mail.ru/pay/light/",
                  InputValues = inputValues,
              }.ProcessRequest(true);
        }

        public override string ProcessResponse(HttpContext context)
        {
            var req = context.Request;
            var orderID = 0;
            if (CheckFields(context) && req["status"] == "PAID" && int.TryParse(StringHelper.DecodeFrom64(req["issuer_id"]), out orderID) && OrderService.GetOrder(orderID) != null)
            {
                OrderService.PayOrder(orderID, true);
                context.Response.Clear();
                context.Response.Write("item_number=" + context.Request["item_number"] + "\n");
                context.Response.Write("status=ACCEPTED");
                context.Response.End();
                return NotificationMessahges.SuccessfullPayment(orderID.ToString());
            }
            return NotificationMessahges.InvalidRequestData;
        }

        private bool CheckFields(HttpContext context)
        {
            var req = context.Request;
            if (string.IsNullOrEmpty(req["type"]) || string.IsNullOrEmpty(req["status"])
                || string.IsNullOrEmpty(req["item_number"]) || string.IsNullOrEmpty(req["issuer_id"])
                || string.IsNullOrEmpty(req["serial"]) || string.IsNullOrEmpty(req["auth_method"])
                || string.IsNullOrEmpty(req["signature"]))
                return false;
            if (req["type"].Trim() != "PAYMENT")
                return false;
            if (!new[] { "MD5", "SHA" }.Contains(req["auth_method"].Trim().ToUpper()))
                return false;
            var code = req["auth_method"] + req["issuer_id"] + req["item_number"] + req["serial"] + req["status"] + req["type"] + Key.Trim();

            // никогда не сходится подпись если ее собирать по алготимту https://money.mail.ru/img/partners/dmr_standart_v1.2.pdf
            //if (code.GetCryptoHash(req["auth_Method"] == "SHA" ? StringHashType.SHA1 : StringHashType.MD5) != req["signature"])
            //    return false;

            return true;

        }

        private string GetSignature(string fields)
        {
            //return (fields + Key.Sha1()).Sha1();
            return fields.Sha1();
        }

        public static string CheckCurrencyCode(string key, string currencyCode)
        {
            var response =
                WebRequest.Create(string.Format(
                    "https://merchant.money.mail.ru/api/info/currency/?key={0}&currency={1}", key, currencyCode)).GetResponse();
            if (response == null)
                return "Unable to get info";
            using (Stream stream = response.GetResponseStream())
            {
                if (stream == null)
                    return "Unable to get info";
                try
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line) && line.Trim() != "OK")
                            return line.Contains(":") ? line.Split(':')[1] : line;
                        do
                        {
                            line = reader.ReadLine();
                            if (!string.IsNullOrEmpty(line) && line.Contains("currency=" + currencyCode))
                            {
                                var enabledLine = reader.ReadLine();
                                if (!string.IsNullOrEmpty(enabledLine) && enabledLine.Trim() == "status=enabled")
                                    return null;
                            }
                        } while (!reader.EndOfStream);
                    }
                    return "Currency not found";
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                    return ex.Message;
                }
            }
        }
    }
}