//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using AdvantShop.Taxes;
using AdvantShop.Helpers;

namespace AdvantShop.Payment
{
    //
    // Документация: https://www.walletone.com/ru/merchant/documentation/#step5
    //
    [PaymentKey("WalletOneCheckout")]
    public class WalletOneCheckout : PaymentMethod
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
            get { return UrlStatus.CancelUrl | UrlStatus.ReturnUrl | UrlStatus.NotificationUrl; }
        }

        public string MerchantId { get; set; }
        public string SecretKey { get; set; }
        public string ExpiredDate { get; set; }
        public string PayWaysEnabled { get; set; }
        public string PayWaysDisabled { get; set; }
        public bool SendReceiptData { get; set; }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                {
                    {WalletOneCheckoutTemplate.MerchantId, MerchantId},
                    {WalletOneCheckoutTemplate.SecretKey, SecretKey},
                    {WalletOneCheckoutTemplate.PayWaysEnabled, PayWaysEnabled},
                    {WalletOneCheckoutTemplate.PayWaysDisabled, PayWaysDisabled},
                    {WalletOneCheckoutTemplate.SendReceiptData, SendReceiptData.ToString()},
                };
            }
            set
            {
                MerchantId = value.ElementOrDefault(WalletOneCheckoutTemplate.MerchantId);
                SecretKey = value.ElementOrDefault(WalletOneCheckoutTemplate.SecretKey);
                PayWaysEnabled = value.ElementOrDefault(WalletOneCheckoutTemplate.PayWaysEnabled, string.Empty);
                PayWaysDisabled = value.ElementOrDefault(WalletOneCheckoutTemplate.PayWaysDisabled, string.Empty);
                SendReceiptData = value.ElementOrDefault(WalletOneCheckoutTemplate.SendReceiptData).TryParseBool();
            }
        }

        public override void ProcessForm(Order order)
        {
            var merchantParams = GetFormParams(order);
            new PaymentFormHandler
            {
                FormName = "_xclick",
                Method = FormMethod.POST,
                Url = "https://wl.walletone.com/checkout/checkout/Index",
                InputValues = merchantParams
            }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            var merchantParams = GetFormParams(order);
            return new PaymentFormHandler
            {
                FormName = "_xclick",
                Method = FormMethod.POST,
                Url = "https://wl.walletone.com/checkout/checkout/Index",
                InputValues = merchantParams
            }.ProcessRequest(false, true);
        }

        public override string ProcessResponse(HttpContext context)
        {
            HttpRequest req = context.Request;
            if (ValidateRequest(req))
            {
                int orderID = 0;
                if (int.TryParse(req["WMI_PAYMENT_NO"], out orderID))
                {
                    if (req["WMI_ORDER_STATE"].ToLower() == "ACCEPTED".ToLower())
                    {
                        Order order = OrderService.GetOrder(orderID);
                        if (order != null)
                        {
                            OrderService.PayOrder(orderID, true);
                            return "WMI_RESULT=OK&WMI_DESCRIPTION=Order successfully processed";
                        }
                    }
                }
                return "WMI_RESULT=RETRY&WMI_DESCRIPTION=Invalid Order ID";
            }
            return "WMI_RESULT=RETRY&WMI_DESCRIPTION=Invalid WMI_SIGNATURE";
        }

        private Dictionary<string, string> GetFormParams(Order order)
        {
            string sum = Math.Round((order.Sum / order.OrderCurrency.CurrencyValue) * this.PaymentCurrency.Rate, 2).ToString().Replace(",", ".");

            var merchantParams = new Dictionary<string, string>
            {
                {"WMI_CULTURE_ID", "ru-RU"},
                {"WMI_CURRENCY_ID", order.OrderCurrency.CurrencyNumCode.ToString()},
                {"WMI_DESCRIPTION", "BASE64:" + Convert.ToBase64String(Encoding.UTF8.GetBytes(GetOrderDescription(order.Number)))}, //StringHelper.Translit(GetOrderDescription(order.Number))},
                { "WMI_FAIL_URL", FailUrl},
                {"WMI_MERCHANT_ID", MerchantId},
                {"WMI_PAYMENT_AMOUNT", sum},
                {"WMI_PAYMENT_NO", order.OrderID.ToString()},
                {"WMI_RECIPIENT_LOGIN", order.OrderCustomer.Email},
                {"WMI_SUCCESS_URL", SuccessUrl},
                {"WMI_CUSTOMER_FIRSTNAME",order.OrderCustomer.FirstName},
                {"WMI_CUSTOMER_LASTNAME",order.OrderCustomer.LastName},
                {"WMI_CUSTOMER_EMAIL", order.OrderCustomer.Email},
            };

            var payWay = PayWaysEnabled.Replace(",", "").Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (payWay.Count > 0)
            {
                for (var i = 0; i < payWay.Count; i++)
                {
                    merchantParams.Add("WMI_PTENABLED" + PaymentFormHandler.SameKey + i, payWay[i].Trim());
                }
            }

            var payDisWay = PayWaysDisabled.Replace(",", "").Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (payDisWay.Count > 0)
            {
                for (var i = 0; i < payDisWay.Count; i++)
                {
                    merchantParams.Add("WMI_PTDISABLED" + PaymentFormHandler.SameKey + i, payDisWay[i].Trim());
                }
            }

            if (SendReceiptData)
            {
                if (order.OrderCustomer != null)
                {
                    if (string.IsNullOrEmpty(order.OrderCustomer.Email) &&
                        order.OrderCustomer.StandardPhone != null && order.OrderCustomer.StandardPhone != 0)
                    {
                        merchantParams.Add("WMI_CUSTOMER_PHONE", "+" + order.OrderCustomer.StandardPhone.ToString());
                    }
                }

                var items =
                     OrderService.RecalculateItemsPriceIncludingAllDiscounts(order.OrderItems, order.ShippingCost, order.Sum)
                         .Select(x => new WalletOneCheckoutProductItem()
                         {
                             Title = (x.Name.Length > 128 ? x.Name.Substring(0, 128) : x.Name).Replace("'",""),
                             UnitPrice = (float)Math.Round(x.Price, 2),
                             Quantity = (float)Math.Round(x.Amount, 2),
                             SubTotal = (float)Math.Round((x.Price * x.Amount), 2),
                             TaxType = GetTaxType(x.TaxType),
                             Tax = (float)Math.Round(GetTax(x.TaxType, x.Price * x.Amount), 2),
                         }).ToList();

                if (order.ShippingCost > 0)
                {
                    items.Add(new WalletOneCheckoutProductItem()
                    {
                        Title = "Доставка",
                        UnitPrice = (float)Math.Round(order.ShippingCost, 2),
                        Quantity = 1,
                        SubTotal = (float)Math.Round(order.ShippingCost, 2),
                        TaxType = GetTaxType(order.ShippingTaxType),
                        Tax = (float)Math.Round(GetTax(order.ShippingTaxType, order.ShippingCost), 2),
                    });
                }

                merchantParams.Add("WMI_ORDER_ITEMS", Newtonsoft.Json.JsonConvert.SerializeObject(items));
            }

            var signatureData = new StringBuilder();
            foreach (var key in merchantParams.Keys.OrderBy(x => x))
            {
                signatureData.Append(merchantParams[key]);
            }

            // Формирование значения параметра WMI_SIGNATURE
            Byte[] bytes = Encoding.GetEncoding(1251).GetBytes(signatureData + SecretKey);
            Byte[] hash = new MD5CryptoServiceProvider().ComputeHash(bytes);
            string signature = Convert.ToBase64String(hash);
            merchantParams.Add("WMI_SIGNATURE", signature);
            return merchantParams;
        }

        private bool ValidateRequest(HttpRequest req)
        {

            var merchantParams = new Dictionary<string, string>();

            foreach (string key in req.Form.AllKeys)
            {
                if (key.ToLower() != "WMI_SIGNATURE".ToLower() && key.ToLower() != "PaymentMethodID".ToLower())
                {
                    merchantParams.Add(key, req.Form[key]);
                }
            }

            var signatureData = new StringBuilder();
            foreach (string key in merchantParams.Keys.OrderBy(s => s))
            {
                //foreach (var item in merchantParams[key].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                //{
                //    signatureData.Append(item);
                //}
                signatureData.Append(merchantParams[key]);
            }

            // Формирование значения параметра WMI_SIGNATURE
            var sign = signatureData + SecretKey;
            Byte[] bytes = Encoding.GetEncoding(1251).GetBytes(sign);
            Byte[] hash = new MD5CryptoServiceProvider().ComputeHash(bytes);
            string signature = Convert.ToBase64String(hash);

            return signature == req["WMI_SIGNATURE"];
        }

        /*
         tax_ru_1 — без НДС;
         tax_ru_2 — НДС по ставке 0%;
         tax_ru_3 — НДС чека по ставке 10%;
         tax_ru_4 — НДС чека по ставке 18%;
         tax_ru_5 — НДС чека по расчетной ставке 10/110;
         tax_ru_6 — НДС чека по расчетной ставке 18/118.
        */
        private string GetTaxType(TaxType? taxType)
        {
            if (taxType == null || taxType.Value == TaxType.Without)
                return "tax_ru_1";

            if (taxType.Value == TaxType.Zero)
                return "tax_ru_2";

            if (taxType.Value == TaxType.Ten)
                return "tax_ru_3";

            if (taxType.Value == TaxType.Eighteen)
                return "tax_ru_4";

            return "tax_ru_1";
        }

        private float GetTax(TaxType? taxType, float price)
        {
            var vat = 0;

            if (taxType == null || taxType.Value == TaxType.Without)
                vat = 0;
            else
            {
                if (taxType.Value == TaxType.Zero)
                    vat = 0;

                if (taxType.Value == TaxType.Ten)
                    vat = 10;

                if (taxType.Value == TaxType.Eighteen)
                    vat = 18;
            }

            var tax = price / (100 + vat) * vat;

            return tax;
        }
    }
}