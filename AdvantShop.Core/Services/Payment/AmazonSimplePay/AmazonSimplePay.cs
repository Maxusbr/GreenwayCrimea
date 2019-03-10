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
using AdvantShop.Orders;
using App_Code.AdvantShop.Payment.AmazonSimplePay;

namespace AdvantShop.Payment
{
    [PaymentKey("AmazonSimplePay")]
    public class AmazonSimplePay : PaymentMethod
    {
        private string Url
        {
            get
            {
                return Sandbox
                           ? "https://authorize.payments-sandbox.amazon.com/pba/paypipeline"
                           : "https://authorize.payments.amazon.com/pba/paypipeline";
            }
        }
        
        public string AccessKey { get; set; }
        // public string AccountId { get; set; }
        public string SecretKey { get; set; }
        public bool Sandbox { get; set; }

        public override UrlStatus ShowUrls
        {
            get { return UrlStatus.None; }
        }

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
                               {AmazonSimplePayTemplate.AccessKey , AccessKey},
                              // {AmazonSimplePayTemplate.AccountId , AccountId},
                               {AmazonSimplePayTemplate.SecretKey , SecretKey},
                               {AmazonSimplePayTemplate.Sandbox , Sandbox.ToString()}
                           };
            }
            set
            {
                AccessKey = value.ElementOrDefault(AmazonSimplePayTemplate.AccessKey);
                // AccountId = value.ElementOrDefault(AmazonSimplePayTemplate.AccountId);
                SecretKey = value.ElementOrDefault(AmazonSimplePayTemplate.SecretKey);
                Sandbox = value.ElementOrDefault(AmazonSimplePayTemplate.Sandbox).TryParseBool();
            }
        }

        public override string ProcessResponse(HttpContext context)
        {
            var req = context.Request;
            if (!CheckData(req) || req["status"] != "PS")
            {
                return NotificationMessahges.InvalidRequestData;
            }
            var paymentNumber = req["referenceId"];
            try
            {
                int orderID = 0;
                if (int.TryParse(paymentNumber, out orderID) && OrderService.GetOrder(orderID) != null && req["pg_result"].Trim() == "1")
                {
                    OrderService.PayOrder(orderID, true);
                    return NotificationMessahges.SuccessfullPayment(paymentNumber);
                }
            }
            catch (Exception ex)
            {
                return NotificationMessahges.LogError(ex);
            }
            return NotificationMessahges.Fail;
        }

        private static readonly string[] ResponseParams = {"addressLine1",
                                                              "addressLine2",
                                                              "addressName",
                                                              "buyerEmail",
                                                              "buyerNames",
                                                              "city",
                                                              "country",
                                                              "errorMessage",
                                                              "operation",
                                                              "paymentMethod",
                                                              "paymentReason",
                                                              "phoneNumber",
                                                              "recipientEmail",
                                                              "recipientName",
                                                              "referenceId",
                                                              "signature",
                                                              "state",
                                                              "status",
                                                              "transactionAmount",
                                                              "transactionDate",
                                                              "transactionId",
                                                              "zip"
                                                          };
        private bool CheckData(HttpRequest req)
        {
            return !new[] {
                            "referenceId",
                            "transactionId",
                            "status",
                            "transactionAmount"
                        }.Any(field => string.IsNullOrEmpty(req[field]))
                   &&
                   new AmazonResponseUtils(SecretKey).ValidateRequest((IDictionary<string, string>)req.QueryString,
                                                                      req.Url.AbsolutePath, "GET");
        }

        public override void ProcessForm(Order order)
        {
            var gatewayUrl = new Uri(Url);
            var sum = String.Format(CultureInfo.InvariantCulture, "USD {0:0.00}", order.Sum);
            var pars = new Dictionary<string, string>
                           {
                               {"immediateReturn", "1"},
                               {"signatureVersion", "2"},
                               {"signatureMethod", "HmacSHA256"},
                               {"accessKey", AccessKey},
                               {"amount", sum},
                               {"description", Configuration.SettingsMain.ShopName},
                              // {"amazonPaymentsAccountId", AccountId},
                               {"returnUrl", SuccessUrl},
                               {"referenceId", order.OrderID.ToString()}
                           };
            pars.Add("signature", AmazonUtils.SignParameters(pars, SecretKey, "POST", gatewayUrl.Host, gatewayUrl.AbsolutePath));
            new PaymentFormHandler
            {
                Url = Url,
                InputValues = pars
            }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            var gatewayUrl = new Uri(Url);
            var sum = String.Format(CultureInfo.InvariantCulture, "USD {0:0.00}", order.Sum);
            var pars = new Dictionary<string, string>
                           {
                               {"immediateReturn", "1"},
                               {"signatureVersion", "2"},
                               {"signatureMethod", "HmacSHA256"},
                               {"accessKey", AccessKey},
                               {"amount", sum},
                               {"description", Configuration.SettingsMain.ShopName},
                              // {"amazonPaymentsAccountId", AccountId},
                               {"returnUrl", SuccessUrl},
                               {"referenceId", order.OrderID.ToString()}
                           };
            pars.Add("signature", AmazonUtils.SignParameters(pars, SecretKey, "POST", gatewayUrl.Host, gatewayUrl.AbsolutePath));
            return new PaymentFormHandler
              {
                  Url = Url,
                  InputValues = pars,
                  Page = page
              }.ProcessRequest();
        }


    }
}