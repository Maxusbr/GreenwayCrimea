using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace AdvantShop.Payment
{
    [PaymentKey("GateLine")]
    public class GateLine : PaymentMethod
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
            get { return UrlStatus.NotificationUrl | UrlStatus.FailUrl | UrlStatus.ReturnUrl; }
        }
        public string Site { get; set; }
        public string Password { get; set; }
        public bool TestMode { get; set; }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {GateLineTemplate.Site, Site},
                               {GateLineTemplate.Password, Password},
                               {GateLineTemplate.TestMode, TestMode.ToString()}
                           };
            }
            set
            {
                Site = value.ElementOrDefault(GateLineTemplate.Site);
                Password = value.ElementOrDefault(GateLineTemplate.Password);
                TestMode = value.ElementOrDefault(GateLineTemplate.TestMode).TryParseBool();
            }
        }

        public override void ProcessForm(Order order)
        {
            var values = new Dictionary<string, string>
                {
                    {"amount", order.Sum.ToString("F2").Replace(',', '.')},
                    {"description", GetOrderDescription(order.Number)},
                    {"site", Site},
                    //{"email", Email},
                    {"merchant_order_id", order.OrderID.ToString()}
                };
            values.Add("checksum", GetCheckSum(values));
            new PaymentFormHandler
            {
                FormName = "_xclick",
                Method = FormMethod.POST,
                Url = string.Format("https://{0}/pay", TestMode ? "simpleapi.sandbox.gateline.net:18610" : "simpleapi.gateline.net"),
                InputValues = values
            }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            var values = new Dictionary<string, string>
                {
                    {"amount", order.Sum.ToString("F2").Replace(',', '.')},
                    {"description", GetOrderDescription(order.Number)},
                    {"site", Site},
                    //{"email", Email},
                    {"merchant_order_id", order.OrderID.ToString()}
                };
            values.Add("checksum", GetCheckSum(values));
            return new PaymentFormHandler
            {
                FormName = "_xclick",
                Method = FormMethod.POST,
                Page = page,
                Url = string.Format("https://{0}/pay", TestMode ? "simpleapi.sandbox.gateline.net:18610" : "simpleapi.gateline.net"),
                InputValues = values
            }.ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            HttpRequest req = context.Request;

            if (req["operation"].IsNotEmpty() && req["operation"] == "test")
            {
                context.Response.Write("SUCCESS");
                context.Response.End();
                return string.Empty;
            }

            int orderID = 0;
            if (CheckFields(req) && int.TryParse(req["merchant_order_id"], out orderID))
            {
                Order order = OrderService.GetOrder(orderID);
                if (order != null)
                {
                    OrderService.PayOrder(orderID, true);
                    return NotificationMessahges.SuccessfullPayment(orderID.ToString());
                }
            }
            return NotificationMessahges.InvalidRequestData;
        }

        private string GetCheckSum(Dictionary<string, string> values)
        {
            var parameters = values.OrderBy(key => key.Key).Select(pair => string.Format("{0}={1}", pair.Key, pair.Value)).ToList();

            var input = parameters.AggregateString(';');
            var privateKey = Password;

            var encoding = new UTF8Encoding();

            var hmac = new HMACSHA1(encoding.GetBytes(privateKey));
            var hash = hmac.ComputeHash(encoding.GetBytes(input));

            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        private bool CheckFields(HttpRequest req)
        {
            // message - Описание результата
            // status - Статус операции
            // order_id* - ID ордера
            // merchant_order_id* - Идентификатор заказа в системе клиента
            // code** - Код ошибки
            // checksum - Контрольная сумма
            // * - поле может не передаваться, если установлен статус error
            // ** - поле передается только для статуса error

            if (new[] { "message", "status", "checksum" }.Any(val => string.IsNullOrEmpty(req[val])))
                return false;

            // status - Статус операции:
            // success - Операция проведена успешно
            // failed - Операция была инициирована, но не завершилась удачно по какой-либо причине.
            // error - Возникла проблема, которая не позволяет запустить проведение операции.
            if (req["status"] != "success")
                return false;

            var values = new Dictionary<string, string>
                {
                    {"message", req["message"]},
                    {"status", req["status"]}
                };
            if (!string.IsNullOrEmpty(req["order_id"]))
                values.Add("order_id", req["order_id"]);
            if (!string.IsNullOrEmpty(req["merchant_order_id"]))
                values.Add("merchant_order_id", req["merchant_order_id"]);
            if (!string.IsNullOrEmpty(req["code"]))
                values.Add("code", req["code"]);

            return GetCheckSum(values).ToLower() == req["checksum"].ToLower();
        }
    }
}