//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;

namespace AdvantShop.Payment
{
    [PaymentKey("NetPay")]
    public class NetPay : PaymentMethod
    {
        public override ProcessType ProcessType
        {
            get { return ProcessType.FormPost; }
        }

        //public override NotificationType NotificationType
        //{
        //    get { return NotificationType.ReturnUrl; }
        //}

        public string ApiKey { get; set; }
        public string AuthSign { get; set; }
        public bool TestMode { get; set; }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {NetPayTemplate.ApiKey, ApiKey},
                               {NetPayTemplate.AuthSign, AuthSign},
                               {NetPayTemplate.TestMode, TestMode.ToString()}
                           };
            }
            set
            {
                ApiKey = value.ElementOrDefault(NetPayTemplate.ApiKey);
                AuthSign = value.ElementOrDefault(NetPayTemplate.AuthSign);
                TestMode = value.ElementOrDefault(NetPayTemplate.TestMode).TryParseBool();
            }
        }
        public override void ProcessForm(Order order)
        {
            //1.	data (шифрованные данные с информацией об оплате)
            //2.	expire (дата в формате YYYY-MM-DD’V’HH:mm:ss)
            //3.	auth (auth код, полученый при регистрации)
             
            var expire = DateTime.Now.AddHours(1).ToString("yyyy-MM-dd'V'HH:mm:ss");

            new PaymentFormHandler
                {
                    Url = TestMode
                              ? "https://demo.net2pay.ru/billingService/paypage"
                              : "https://my.net2pay.ru/billingService/paypage",
                    InputValues = new Dictionary<string, string>
                        {
                            {"data", EncodeData(order, expire)},
                            {"expire", expire},
                            {"auth", AuthSign}
                        }
                }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            //1.	data (шифрованные данные с информацией об оплате)
            //2.	expire (дата в формате YYYY-MM-DD’V’HH:mm:ss)
            //3.	auth (auth код, полученый при регистрации)

            var expire = DateTime.Now.AddHours(1).ToString("yyyy-MM-dd'V'HH:mm:ss");

            return new PaymentFormHandler
                {
                    Url = TestMode
                              ? "https://demo.net2pay.ru/billingService/paypage"
                              : "https://my.net2pay.ru/billingService/paypage",
                    Page = page,
                    InputValues = new Dictionary<string, string>
                        {
                            {"data", EncodeData(order, expire)},
                            {"expire", HttpUtility.UrlEncode(expire)},
                            {"auth", AuthSign}
                        }
                }.ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            //var req = context.Request;

            //if (req["orderID"].IsNotEmpty())
            //{
            //    var orderId = req["orderID"].TryParseInt();
            //    var order = OrderService.GetOrder(orderId);
            //    if (order != null)
            //    {
            //        OrderService.PayOrder(orderId, true);
            //        return NotificationMessahges.SuccessfullPayment(orderId.ToString());
            //    }
            //}
            return string.Empty;
        }

        public bool CheckData(HttpRequest req)
        {
            return true;
        }

        private string EncodeData(Order order, string expire)
        {
            //- amount - сумма к оплате (для разделения десятичной части числа должна использоваться десятичная точка);
            //- currency - валюта оплаты (в настоящий момент доступны USD, RUB, EUR)
            //- orderID - уникальный идентификатор платежа;
            //- successUrl - ссылка запроса GET, переход по которой будет осуществлен при успешной
            //  оплате (при отсутствии ссылки, переход осуществляется на страницу результата оплаты системы Net Pay);
            //- failUrl - ссылка запроса GET, переход по которой будет осуществлен при неуспешной
            //  оплате (при отсутствии ссылки, переход осуществляется на страницу результата оплаты системы NetPay);
            
            var encoding = Encoding.UTF8;
            var sb = new StringBuilder();

            var apiKeyHashed = Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(encoding.GetBytes(ApiKey)));
            var cryptoWord = Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(encoding.GetBytes(apiKeyHashed + expire)));
            
            var cryptoKey = encoding.GetBytes(cryptoWord.Substring(0, 16));

            sb.Append(EncryptStringUseAes(string.Format("amount={0}", order.Sum.ToString().Replace(',', '.')), cryptoKey, cryptoKey));

            var currencyCode = order.OrderCurrency != null && order.OrderCurrency.CurrencyCode != "RUR" ? order.OrderCurrency.CurrencyCode : "RUB";
            sb.Append("&" + EncryptStringUseAes(string.Format("currency={0}", currencyCode), cryptoKey, cryptoKey));

            sb.Append("&" + EncryptStringUseAes(string.Format("orderID={0}", order.OrderID.ToString()), cryptoKey, cryptoKey));

            var successUrl = string.Format("http://{0}/paymentreturnurl/{1}", SettingsMain.SiteUrl.ToLower().Replace("http://", "").Replace("https://", ""), 100);
            sb.AppendFormat("&" + EncryptStringUseAes(string.Format("successUrl={0}", successUrl), cryptoKey, cryptoKey));

            var failUrl = string.Format("http://{0}/PaymentFailUrl.aspx", SettingsMain.SiteUrl.ToLower().Replace("http://", "").Replace("https://", ""));
            sb.AppendFormat("&" + EncryptStringUseAes(string.Format("failUrl={0}", failUrl), cryptoKey, cryptoKey));

            return HttpUtility.UrlEncode(sb.ToString());
        }


        private string EncryptStringUseAes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                return string.Empty;
            if (Key == null || Key.Length <= 0)
                return string.Empty;
            if (IV == null || IV.Length <= 0)
                return string.Empty;
            byte[] encrypted;
            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.ECB;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return Convert.ToBase64String(encrypted);

        }
    }
}