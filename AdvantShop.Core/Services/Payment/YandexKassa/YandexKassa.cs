//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Saas;
using AdvantShop.Taxes;

namespace AdvantShop.Payment
{
    [PaymentKey("YandexKassa")]
    public class YandexKassa : PaymentMethod
    {
        public string ShopId { get; set; }
        public string ScId { get; set; }
        public bool DemoMode { get; set; }
        public string YaPaymentType { get; set; }
        public string Password { get; set; }

        public bool SendReceiptData { get; set; }
        //public int VatType { get; set; }


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
            get { return UrlStatus.FailUrl | UrlStatus.ReturnUrl | UrlStatus.NotificationUrl; }
        }

        public override string NotificationUrl
        {
            get
            {
                return SaasDataService.IsSaasEnabled
                           ? "https://gate.advantshop.net/yandexkassa/" + StringHelper.EncodeTo64(base.NotificationUrl)
                                .Replace("/", "-slash-").Replace("+", "-plus-").Replace("=", "-equal-")
                           : base.NotificationUrl.Replace("http://", "https://");
            }
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {YandexKassaTemplate.ShopID, ShopId},
                               {YandexKassaTemplate.ScID, ScId},
                               {YandexKassaTemplate.DemoMode, DemoMode.ToString()},
                               {YandexKassaTemplate.YaPaymentType, YaPaymentType},
                               {YandexKassaTemplate.Password, Password},

                               {YandexKassaTemplate.SendReceiptData, SendReceiptData.ToString()},
                               //{YandexKassaTemplate.VatType, VatType.ToString()}
                           };
            }
            set
            {
                ShopId = value.ElementOrDefault(YandexKassaTemplate.ShopID);
                ScId = value.ElementOrDefault(YandexKassaTemplate.ScID);
                YaPaymentType = value.ElementOrDefault(YandexKassaTemplate.YaPaymentType);
                Password = value.ElementOrDefault(YandexKassaTemplate.Password);
                DemoMode = value.ElementOrDefault(YandexKassaTemplate.DemoMode).TryParseBool();

                SendReceiptData = value.ElementOrDefault(YandexKassaTemplate.SendReceiptData).TryParseBool();
                //VatType = value.ElementOrDefault(YandexKassaTemplate.VatType).TryParseInt();
            }
        }

        private PaymentFormHandler GetHandler(Order order, PageWithPaymentButton? page = null)
        {
            var receipt = SendReceiptData ? new Receipt()
            {
                customerContact = order.OrderCustomer.Email.IsNotEmpty() ? order.OrderCustomer.Email : order.OrderCustomer.StandardPhone.ToString(),
                items = OrderService.RecalculateItemsPriceIncludingAllDiscounts(order.OrderItems, order.ShippingCost, order.Sum)
                        .Select(item => new Item()
                        {
                            text = item.Name.Length > 100 ? item.Name.Substring(0, 100) : item.Name,
                            price = new Price { amount = item.Price },
                            quantity = (float)Math.Round(item.Amount, 2),
                            tax = GetVatType(item.TaxType)
                        }).ToList()
            }
            : null;

            if (order.ShippingCost > 0 && receipt != null)
            {
                receipt.items.Add(new Item() { price = new Price() { amount = order.ShippingCost }, quantity = 1, tax = GetVatType(order.ShippingTaxType), text = "Доставка" });
            }

            var orderSum = SendReceiptData ? receipt.items.Sum(x => x.price.amount * x.quantity) : order.Sum;

            var handler = new PaymentFormHandler
            {
                Url = DemoMode ? "https://demomoney.yandex.ru/eshop.xml" : "https://money.yandex.ru/eshop.xml",
                Method = FormMethod.POST,
                InputValues =
                {
                    {"shopId", ShopId},
                    {"scid", ScId},
                    {"sum", (orderSum * GetCurrencyRate(order.OrderCurrency)).ToString("F2").Replace(",",".")},
                    {"customerNumber", order.OrderCustomer.CustomerID.ToString().Normalize()},
                    {"orderNumber", order.OrderID.ToString(CultureInfo.InvariantCulture).Normalize()},
                    {"shopSuccessURL", HttpUtility.UrlEncode(SuccessUrl)},
                    {"shopFailURL", HttpUtility.UrlEncode(FailUrl)},
                    {"cps_email", order.OrderCustomer.Email ?? string.Empty},
                    {"paymentType", YaPaymentType},
                    {
                        "cps_phone",
                        order.OrderCustomer.StandardPhone.HasValue && order.OrderCustomer.StandardPhone.ToString().Length <= 15
                            ? order.OrderCustomer.StandardPhone.ToString()
                            : string.Empty
                    },
                    {"cms_name", "AdVantShop.NET"},
                    {"ym_merchant_receipt", receipt != null ?  Newtonsoft.Json.JsonConvert.SerializeObject(receipt) : null }
                }
            };

            if (page != null)
                handler.Page = page.Value;

            // https://tech.yandex.ru/money/doc/payment-solution/payment-form/payment-form-http-docpage/
            if (YaPaymentType == "KV")
            {
                if (SendReceiptData && receipt != null)
                {
                    for (var i = 0; i < receipt.items.Count; i++)
                    {
                        handler.InputValues.Add("goods_name_" + i, receipt.items[i].text.Reduce(255));
                        handler.InputValues.Add("goods_quantity_" + i, receipt.items[i].quantity.ToString("F2").Replace(",", "."));
                        handler.InputValues.Add("goods_cost_" + i, receipt.items[i].price.amount.ToString("F2").Replace(",", "."));
                        //handler.InputValues.Add("category_code_" + i, "11111");
                        handler.InputValues.Add("goods_description_" + i, receipt.items[i].text.Reduce(255));
                    }
                }
                else
                {
                    for (var i = 0; i < order.OrderItems.Count; i++)
                    {
                        handler.InputValues.Add("goods_name_" + i, order.OrderItems[i].Name.Reduce(255));
                        handler.InputValues.Add("goods_quantity_" + i, order.OrderItems[i].Amount.ToString("F2").Replace(",", "."));
                        handler.InputValues.Add("goods_cost_" + i, order.OrderItems[i].Price.ToString("F2").Replace(",", "."));
                        //handler.InputValues.Add("category_code_" + i, "11111");
                        handler.InputValues.Add("goods_description_" + i, order.OrderItems[i].Name.Reduce(255));
                    }
                }
                handler.InputValues.Add("seller_id", ShopId);
                handler.InputValues.Add("fixed_term", false.ToString());
            }

            return handler;
        }

        public override void ProcessForm(Order order)
        {
            GetHandler(order).Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            return GetHandler(order, page).ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            var typeRequest = TypeRequestYandex.checkOrder;
            var processingResult = ProcessingResult.ErrorParsing;
            var invoiceId = string.Empty;
            string error = string.Empty;

            try
            {
                ProcessingMd5(context, out processingResult, out typeRequest, out invoiceId, out error);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                processingResult = ProcessingResult.Exception;
            }

            var result = RendAnswer(typeRequest, processingResult, invoiceId, error);
            context.Response.Clear();
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.ContentType = "application/xml";
            context.Response.Write(result);
            //context.Response.End();
            return string.Empty;
        }

        private string RendAnswer(TypeRequestYandex typeRequest, ProcessingResult processingResult, string invoiceId, string error)
        {
            return string.Format(
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?><{0}Response performedDatetime=\"{1}\" code=\"{2}\" invoiceId=\"{3}\" shopId=\"{4}\" {5}/>",
                typeRequest, DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fzzz"), (int)processingResult,
                invoiceId, ShopId,
                error.IsNotEmpty() ? "ErrorAdvantShop=\"" + error + "\"" : string.Empty);
        }

        private bool IsCheckFields(Dictionary<string, string> parameters, TypeRequestYandex typeRequest)
        {
            decimal orderSumAmount;

            if (parameters["shopid"].Equals(ShopId, StringComparison.InvariantCultureIgnoreCase) &&
                parameters["invoiceid"].IsNotEmpty() && parameters["invoiceid"].All(char.IsDigit) &&
                parameters["ordernumber"].IsNotEmpty() && parameters["ordernumber"].All(char.IsDigit) &&
                parameters["ordersumamount"].IsNotEmpty() &&
                decimal.TryParse(parameters["ordersumamount"], NumberStyles.Float, CultureInfo.InvariantCulture, out orderSumAmount))
            {
                var ord = OrderService.GetOrder(parameters["ordernumber"].TryParseInt());

                if (ord != null &&
                    // Если это запрос "Уведомление о переводе", которые могут повторяться несколько раз (упомянуто в документации),
                    // тогда неважно заказ был уже отмечен оплаченным или уже отменен
                    (typeRequest == TypeRequestYandex.paymentAviso || (!ord.Payed && ord.OrderStatusId != OrderStatusService.CanceledOrderStatus)) &&
                    ord.OrderCustomer.CustomerID.ToString().Normalize().Equals(parameters["customernumber"], StringComparison.InvariantCultureIgnoreCase) &&
                    //orderSumAmount >= Math.Round((decimal)(ord.Sum * GetCurrencyRate(ord.OrderCurrency)), 2)
                    Math.Abs(orderSumAmount - (decimal)Math.Round(ord.Sum * GetCurrencyRate(ord.OrderCurrency), 2)) < 1
                    )
                {
                    return true;
                }
            }
            return false;
        }

        #region NVP/MD5

        private void ProcessingMd5(HttpContext context, out ProcessingResult processingResult,
            out TypeRequestYandex typeRequest, out string invoiceId, out string error)
        {
            var parameters = ReadParametersMd5(context, out typeRequest);

            invoiceId = parameters.ContainsKey("invoiceid") ? parameters["invoiceid"] : string.Empty;

            if (IsCheckMd5(parameters, out error))
            {
                if (IsCheckFields(parameters, typeRequest))
                {
                    if (typeRequest == TypeRequestYandex.paymentAviso)
                        OrderService.PayOrder(parameters["ordernumber"].TryParseInt(), true);

                    processingResult = ProcessingResult.Success;
                }
                else
                {
                    processingResult = typeRequest == TypeRequestYandex.checkOrder
                        ? ProcessingResult.TranslationFailure
                        : ProcessingResult.ErrorParsing;

                }
            }
            else
                processingResult = ProcessingResult.ErrorAuthorize;
        }

        private Dictionary<string, string> ReadParametersMd5(HttpContext context, out TypeRequestYandex typeRequest)
        {
            typeRequest = TypeRequestYandex.unknown;

            if (context.Request["action"].IsNotEmpty())
            {
                if (context.Request["action"].Equals("checkOrder", StringComparison.InvariantCultureIgnoreCase))
                    typeRequest = TypeRequestYandex.checkOrder;
                else if (context.Request["action"].Equals("paymentAviso", StringComparison.InvariantCultureIgnoreCase))
                    typeRequest = TypeRequestYandex.paymentAviso;
            }

            return context.Request.Params.AllKeys.ToDictionary(key => key.ToLower(), key => context.Request[key]);
        }

        private bool IsCheckMd5(Dictionary<string, string> parameters, out string error)
        {
            bool isValid = true;
            error = string.Empty;

            if (!parameters.ContainsKey("action"))
            {
                error += "Нет поля action. ";
                isValid = false;
            }

            if (!parameters.ContainsKey("ordersumamount"))
            {
                error += "Нет поля ordersumamount. ";
                isValid = false;
            }
            if (!parameters.ContainsKey("ordersumcurrencypaycash"))
            {
                error += "Нет поля ordersumcurrencypaycash. ";
                isValid = false;
            }
            if (!parameters.ContainsKey("ordersumbankpaycash"))
            {
                error += "Нет поля ordersumbankpaycash. ";
                isValid = false;
            }
            if (!parameters.ContainsKey("shopid"))
            {
                error += "Нет поля shopid. ";
                isValid = false;
            }
            if (!parameters.ContainsKey("invoiceid"))
            {
                error += "Нет поля invoiceid. ";
                isValid = false;
            }
            if (!parameters.ContainsKey("customernumber"))
            {
                error += "Нет поля customernumber. ";
                isValid = false;
            }

            if (!isValid)
                return false;


            string md5before = string.Format("{0};{1};{2};{3};{4};{5};{6};{7}",
                       parameters["action"],
                       parameters["ordersumamount"],
                       parameters["ordersumcurrencypaycash"],
                       parameters["ordersumbankpaycash"],
                       parameters["shopid"],
                       parameters["invoiceid"],
                       parameters["customernumber"],
                       Password);

            string md5 = string.Format("{0};{1};{2};{3};{4};{5};{6};{7}",
                       parameters["action"],
                       parameters["ordersumamount"],
                       parameters["ordersumcurrencypaycash"],
                       parameters["ordersumbankpaycash"],
                       parameters["shopid"],
                       parameters["invoiceid"],
                       parameters["customernumber"],
                       Password).Md5(false);

            if (parameters["md5"].ToLower() != md5before.Md5(false))
            {
                error = "Неверная цифровая подпись MD5. Возможно неверный пароль. md5before=" + md5before;
                return false;
            }
            else
            {
                return true;
            }

        }

        #endregion

        /*
         Без НДС - 1
         НДС по ставке 0% - 2
         НДС чека по ставке 10% - 3
         НДС чека по ставке 18% - 4
         НДС чека по расчетной ставке 10/110 - 5
         НДС чека по расчетной ставке 18/118 - 6
        */
        private int GetVatType(TaxType? taxType)
        {
            if (taxType == null)
                return 1;

            if (taxType.Value == TaxType.Without)
                return 1;

            if (taxType.Value == TaxType.Zero)
                return 2;

            if (taxType.Value == TaxType.Ten)
                return 3;

            if (taxType.Value == TaxType.Eighteen)
                return 4;

            return 1;
        }

        private enum TypeRequestYandex
        {
            //Do not change the register
            checkOrder,
            paymentAviso,
            unknown
        }

        private enum ProcessingResult : int
        {
            Success = 0,
            ErrorAuthorize = 1,
            TranslationFailure = 100,
            ErrorParsing = 200,
            Exception = 1000
        }

        private class Receipt
        {
            public string customerContact { get; set; }
            public List<Item> items { get; set; }
        }

        private class Item
        {
            public float quantity { get; set; }
            public Price price { get; set; }
            public int tax { get; set; }
            public string text { get; set; }
        }

        private class Price
        {
            public float amount { get; set; }
            public string currency { get { return "RUB"; } }
        }

    }
}