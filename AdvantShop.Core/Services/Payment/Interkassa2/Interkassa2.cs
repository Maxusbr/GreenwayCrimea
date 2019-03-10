using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Linq;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;
using AdvantShop.Diagnostics;

namespace AdvantShop.Payment
{
    [PaymentKey("Interkassa2")]
    public class Interkassa2 : PaymentMethod
    {
        public string ShopId { get; set; }
        public string SecretKey { get; set; }
        public bool IsCheckSign { get; set; }
        

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

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {Interkassa2Template.ShopId, ShopId},
                               {Interkassa2Template.IsCheckSign, IsCheckSign.ToString()},
                               {Interkassa2Template.SecretKey, SecretKey}
                           };
            }
            set
            {
                ShopId = value.ElementOrDefault(Interkassa2Template.ShopId);
                IsCheckSign = value.ElementOrDefault(Interkassa2Template.IsCheckSign).TryParseBool();
                SecretKey = value.ElementOrDefault(Interkassa2Template.SecretKey);
            }
        }


        public override void ProcessForm(Order order)
        {
            var formHandler = new PaymentFormHandler
            {
                Url = "https://sci.interkassa.com/",
                InputValues = new Dictionary<string, string>
                {
                    {"ik_co_id", ShopId},
                    {"ik_pm_no", order.OrderID.ToString()},
                    {"ik_am", (order.Sum*GetCurrencyRate(order.OrderCurrency)).ToString("F2").Replace(",", ".")},
                    {"ik_desc", GetOrderDescription(order.Number)},
                    {"ik_cur", order.OrderCurrency.CurrencyCode}
                }
            };

            if (IsCheckSign)
                formHandler.InputValues.Add("ik_sign", GetSign(formHandler.InputValues));

            formHandler.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            var formHandler = new PaymentFormHandler
            {
                Url = "https://sci.interkassa.com/",
                InputValues = new Dictionary<string, string>
                {
                    {"ik_co_id", ShopId},
                    {"ik_pm_no", order.OrderID.ToString()},
                    {"ik_am", (order.Sum*GetCurrencyRate(order.OrderCurrency)).ToString("F2").Replace(",", ".")},
                    {"ik_desc", GetOrderDescription(order.Number)},
                    {"ik_cur", order.OrderCurrency.CurrencyCode}
                }
            };


            if (IsCheckSign)
                formHandler.InputValues.Add("ik_sign", GetSign(formHandler.InputValues));

            return formHandler.ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            var req = context.Request;
            var orderID = 0;
            if (CheckFields(context) && req["ik_inv_st"] == "success" && int.TryParse(req["ik_pm_no"], out orderID))
            {
                var order = OrderService.GetOrder(orderID);
                if (order != null)
                {
                    OrderService.PayOrder(orderID, true);
                    return NotificationMessahges.SuccessfullPayment(orderID.ToString());
                }
            }
            else
            {
                Debug.Log.Error("exeption in interkassa 2.0. ik_inv_st:" + req["ik_inv_st"] + ", ik_pm_no:" + req["ik_pm_no"]);
            }
            return NotificationMessahges.InvalidRequestData;
        }

        private string GetSign(Dictionary<string, string> inputValues)
        {
            var sortedList = inputValues.OrderBy(v => v.Key).Select(v => v.Value).ToList();
            sortedList.Add(SecretKey);

            return
                Convert.ToBase64String(
                    new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(string.Join(":", sortedList))));
        }

        private bool CheckFields(HttpContext context)
        {
            // check summ
            return true;
        }
    }
}