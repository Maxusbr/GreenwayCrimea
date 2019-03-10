//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Helpers;
using AdvantShop.Orders;

namespace AdvantShop.Payment
{
    /// <summary>
    /// Summary description for AuthorizeNet
    /// </summary>
    [PaymentKey("AuthorizeNet")]
    public class AuthorizeNet : PaymentMethod
    {
        public string Login { get; set; }
        public string TransactionKey { get; set; }
        public float CurrencyValue { get; set; }
        //TODO do we need returnurl?
        //public string ReturnUrl { get; set; }
        public bool Sandbox { get; set; }

        #region PaymentMethod Members

        public override ProcessType ProcessType
        {
            get { return ProcessType.FormPost; }
        }

        public override NotificationType NotificationType
        {
            get { return NotificationType.ReturnUrl; }
        }
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {AuthorizeNetTemplate.Login, Login},
                               {AuthorizeNetTemplate.TransactionKey, TransactionKey},
                               //{AuthorizeNetTemplate.ReturnUrl, ReturnUrl},
                               {AuthorizeNetTemplate.CurrencyValue, CurrencyValue.ToString()},
                               {AuthorizeNetTemplate.Sandbox, Sandbox.ToString()},
                           };
            }
            set
            {
                Login = value.ElementOrDefault(AuthorizeNetTemplate.Login);
                TransactionKey = value.ElementOrDefault(AuthorizeNetTemplate.TransactionKey);
                //ReturnUrl = !value.ContainsKey(AuthorizeNetTemplate.ReturnUrl)
                //                ? SettingsMain.SiteUrl
                //                : value[AuthorizeNetTemplate.ReturnUrl];
                if (!value.ContainsKey(AuthorizeNetTemplate.CurrencyValue))
                    CurrencyValue = 1;
                else
                {
                    float val;
                    CurrencyValue = float.TryParse(value[AuthorizeNetTemplate.CurrencyValue], out val) ? val : 1;
                }
                Sandbox = value.ElementOrDefault(AuthorizeNetTemplate.Sandbox).TryParseBool();
            }
        }

        public override void ProcessForm(Order order)
        {
            int sequence = new Random().Next(0, 1000);

            // a time stamp is generated (using a function from simlib.asp)
            int timeStamp = SQLDataHelper.GetInt((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);

            //generate a fingerprint
            string fingerprint = HMAC_MD5(TransactionKey,
                                          Login + "^" + sequence + "^" + timeStamp + "^" + order.Sum + "^");

            //TODO deal with POST

            new PaymentFormHandler
                {
                    FormName = "_xclick",
                    Method = FormMethod.POST,
                    Url =
                        string.Format("https://{0}/gateway/transact.dll",
                                      Sandbox ? "test.authorize.net" : "secure.authorize.net"),
                    InputValues = new Dictionary<string, string>
                                      {
                                          {"x_login", Login},
                                          {"x_fp_sequence", sequence.ToString()},
                                          {"x_fp_timestamp", timeStamp.ToString()},
                                          {"x_fp_hash", fingerprint},
                                          {"x_relay_url", SuccessUrl},
                                          {"x_invoice_num", order.Number},
                                          {"x_test_request", "false"},
                                          {"x_show_form", "PAYMENT_FORM"},
                                          {"x_description", string.Format("Order #{0}", order.Number)},
                                          {"x_amount", order.Sum.ToString()},
                                          {"x_first_name", order.OrderCustomer.FirstName ?? ""},
                                          {"x_last_name", order.OrderCustomer.LastName ?? ""},
                                          {"x_address", order.OrderCustomer.GetCustomerAddress()},
                                          {"x_city", order.OrderCustomer.City ?? ""},
                                          {"x_zip", order.OrderCustomer.Zip ?? ""},
                                          {"x_phone", order.OrderCustomer.StandardPhone.ToString()},
                                          {"x_email", order.OrderCustomer.Email ?? ""},
                                      }
                }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            int sequence = new Random().Next(0, 1000);

            // a time stamp is generated (using a function from simlib.asp)
            int timeStamp = SQLDataHelper.GetInt((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);

            //generate a fingerprint
            string fingerprint = HMAC_MD5(TransactionKey,
                                          Login + "^" + sequence + "^" + timeStamp + "^" + order.Sum + "^");

            //TODO deal with POST

            return new PaymentFormHandler
              {
                  FormName = "_xclick",
                  Method = FormMethod.POST,
                  Page = page,
                  Url =
                      string.Format("https://{0}/gateway/transact.dll",
                                    Sandbox ? "test.authorize.net" : "secure.authorize.net"),
                  InputValues = new Dictionary<string, string>
                                      {
                                          {"x_login", Login},
                                          {"x_fp_sequence", sequence.ToString()},
                                          {"x_fp_timestamp", timeStamp.ToString()},
                                          {"x_fp_hash", fingerprint},
                                          {"x_relay_url", SuccessUrl},
                                          {"x_invoice_num", order.Number},
                                          {"x_test_request", "false"},
                                          {"x_show_form", "PAYMENT_FORM"},
                                          {"x_description", string.Format("Order #{0}", order.Number)},
                                          {"x_amount", order.Sum.ToString()},
                                          {"x_first_name", order.OrderCustomer.FirstName ?? ""},
                                          {"x_last_name", order.OrderCustomer.LastName ?? ""},
                                          {"x_address", order.OrderCustomer.GetCustomerAddress()},
                                          {"x_city", order.OrderCustomer.City ?? ""},
                                          {"x_zip", order.OrderCustomer.Zip ?? ""},
                                          {"x_phone", order.OrderCustomer.StandardPhone.ToString()},
                                          {"x_email", order.OrderCustomer.Email ?? ""},
                                      }
              }.ProcessRequest();
        }

        #endregion

        private static string HMAC_MD5(string key, string value)
        {
            // The first two lines take the input values and convert them from strings to Byte arrays
            byte[] HMACkey = (new ASCIIEncoding()).GetBytes(key);
            byte[] HMACdata = (new ASCIIEncoding()).GetBytes(value);

            // create a HMACMD5 object with the key set
            var myhmacMD5 = new HMACMD5(HMACkey);

            // calculate the hash (returns a byte array)
            byte[] HMAChash = myhmacMD5.ComputeHash(HMACdata);

            // loop through the byte array and add append each piece to a string to obtain a hash string
            string fingerprint = "";
            for (int i = 0; i <= HMAChash.Length - 1; i++)
            {
                fingerprint += HMAChash[i].ToString("x").PadLeft(2, '0');
            }

            return fingerprint;
        }

        public override string ProcessResponse(HttpContext context)
        {
            if (Sandbox)
                return NotificationMessahges.TestMode;
            var response = new global::AuthorizeNet.SIMResponse(context.Request.Form);
            //TODO find out hash key
            if (!response.Validate("YOUR_MERCHANT_HASH_CODE", Login))
                return NotificationMessahges.InvalidRequestData;

            //TODO ORDER PAYMENT TEST
            OrderService.PayOrder(OrderService.GetOrderIdByNumber(response.InvoiceNumber), true);
            return NotificationMessahges.SuccessfullPayment(response.InvoiceNumber);
        }
    }
}