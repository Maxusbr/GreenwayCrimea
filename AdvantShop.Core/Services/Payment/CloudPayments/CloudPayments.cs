using System;
using System.Text;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Linq;
using AdvantShop.Diagnostics;
using AdvantShop.Taxes;

namespace AdvantShop.Payment
{

    [PaymentKey("CloudPayments")]
    public class CloudPayments : PaymentMethod
    {
        public string PublicId { get; set; }
        public string ApiSecret { get; set; }
        public string Site { get; set; }

        public bool SendReceiptData { get; set; }
        public int TaxationSystem { get; set; }

        public override ProcessType ProcessType
        {
            get { return ProcessType.Javascript; }
        }

        public override NotificationType NotificationType
        {
            get { return NotificationType.Handler; }
        }


        public override UrlStatus ShowUrls
        {
            get { return UrlStatus.CancelUrl | UrlStatus.ReturnUrl | UrlStatus.NotificationUrl; }
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {CloudPaymentsTemplate.PublicId, PublicId},
                               {CloudPaymentsTemplate.APISecret, ApiSecret},
                    {CloudPaymentsTemplate.Site, Site},
                    {CloudPaymentsTemplate.SendReceiptData, SendReceiptData.ToString()},
                    {CloudPaymentsTemplate.TaxationSystem, TaxationSystem.ToString()}
                           };
            }
            set
            {
                PublicId = value.ElementOrDefault(CloudPaymentsTemplate.PublicId);
                ApiSecret = value.ElementOrDefault(CloudPaymentsTemplate.APISecret);
                Site = value.ElementOrDefault(CloudPaymentsTemplate.Site);
                SendReceiptData = value.ElementOrDefault(CloudPaymentsTemplate.SendReceiptData).TryParseBool();
                TaxationSystem = value.ElementOrDefault(CloudPaymentsTemplate.TaxationSystem).TryParseInt();
            }
        }

      

        public override string ProcessJavascript(Order order)
        {
            string format = @"<script src='https://widget.{8}/bundles/cloudpayments'></script>
                            <script>this.pay = function () {{
                            var widget = new cp.CloudPayments();
                            widget.charge({{ 
                            publicId: '{0}',
                            description: '{1}',
                            amount: {2},
                            currency: '{3}',
                            invoiceId: '{4}',
                            accountId: '{5}',
                            onSuccess: '{6}',
                            onFail: '{7}',
                            data: {9}
                            }},
                        function(options) {{ 
                            window.location = '{6}';
                            }},
                        function(reason, options) {{ 
                            window.location = '{7}'; 
                            }});
                        }};
                </script>";

            var receipt =  new
            {
                cloudPayments = new
                {
                    customerReceipt = new CloudPaymentsCustomerReceipt
                    {
                        Items = OrderService.RecalculateItemsPriceIncludingAllDiscounts(order.OrderItems, order.ShippingCost, order.Sum)
                        .Select(item => new CloudPaymentsItem()
                        {
                            label = item.Name.Length > 100 ? item.Name.Substring(0, 100) : item.Name,
                            price = item.Price,
                            quantity = item.Amount,
                            amount = item.Price * item.Amount,
                            vat = GetTaxType(item.TaxType)
                        }).ToList(),
                        taxationSystem = TaxationSystem,
                        email = order.OrderCustomer.Email,
                        phone = order.OrderCustomer.StandardPhone.ToString()
                    }
                }
            };

            if (order.ShippingCost > 0)
            {
                receipt.cloudPayments.customerReceipt.Items.Add(new CloudPaymentsItem()
                {
                    label = "Доставка",
                    price = order.ShippingCost,
                    quantity = 1,
                    amount = order.ShippingCost,
                    vat = GetTaxType(order.ShippingTaxType)
                });
            }

            var result = string.Format(format, PublicId, "Оплата заказа № " + order.Number, (order.Sum * GetCurrencyRate(order.OrderCurrency)).ToString("F2").Replace(",", "."),
                                       PaymentCurrency.Iso3, order.Number, order.OrderCustomer.Email, SuccessUrl, FailUrl, Site,
                                       SendReceiptData ? JsonConvert.SerializeObject(receipt) : "null");

            return result;
        }

        public override string ProcessJavascriptButton(Order order)
        {
            return "javascript:pay();";
        }

        public override string ButtonText
        {
            get { return "Оплатить"; }
        }

        public override string ProcessResponse(HttpContext context)
        {
            HttpRequest req = context.Request;


            string orderNumber = req["InvoiceId"];
            var orderid = OrderService.GetOrderIdByNumber(orderNumber);
            var order = OrderService.GetOrder(orderid);
            if (order == null)
            {
                Debug.Log.Info("Order " + orderNumber + " not found");
                return JsonConvert.SerializeObject(new { code = "Order " + orderNumber + " not found" });
            }

            if (Math.Round(order.Sum, 2) != Math.Round(req["Amount"].TryParseFloat(), 2))
            {
                Debug.Log.Info("Order sum is " + Math.Round(order.Sum, 2) + ", not " + req["Amount"]);
                return JsonConvert.SerializeObject(new { code = "Order sum is " + Math.Round(order.Sum, 2) + ", not " + req["Amount"] });
            }

            if (order.OrderCurrency.CurrencyCode != req["Currency"])
            {
                Debug.Log.Info("Order Currency is " + order.OrderCurrency.CurrencyCode + ", not " + req["Currency"]);
                return JsonConvert.SerializeObject(new { code = "Order Currency is " + order.OrderCurrency.CurrencyCode + ", not " + req["Currency"] });
            }

            var parameters = req.RawUrl.Split('?').LastOrDefault();
            var myToken = CreateToken(parameters, ApiSecret);

            if (myToken != req.Headers["Content-HMAC"])
            {
                Debug.Log.Info("My hash is " + myToken + ", headers hash is " + req.Headers["Content-HMAC"] + "for request: " + parameters);
                return JsonConvert.SerializeObject(new { code = "My hash is " + myToken + ", headers hash is " + req.Headers["Content-HMAC"] });
            }
            
            OrderService.PayOrder(order.OrderID, true);

            return JsonConvert.SerializeObject(new { code = 0 });

        }

        private string CreateToken(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

        /*
        null или не указано — НДС не облагается
        18 — НДС 18%
        10 — НДС 10%
        0 — НДС 0%
        110 — расчетный НДС 10/110
        118 — расчетный НДС 18.118             
        */
        private int? GetTaxType(TaxType? taxType)
        {
            if (taxType == null || taxType.Value == TaxType.Without)
                return null;

            if (taxType.Value == TaxType.Zero)
                return 0;

            if (taxType.Value == TaxType.Ten)
                return 10;

            if (taxType.Value == TaxType.Eighteen)
                return 18;
            
            return null;
        }
    }

    public class CloudPaymentsCustomerReceipt
    {
        public List<CloudPaymentsItem> Items { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public int taxationSystem { get; set; }

    }

    public class CloudPaymentsItem
    {
        public string label { get; set; }
        public float price { get; set; }
        public float quantity { get; set; }
        public float amount { get; set; }
        public int? vat { get; set; } // null, 0, 10, 18
    }
}