//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using Newtonsoft.Json;

namespace AdvantShop.Payment
{
    [PaymentKey("Avangard")]
    public class Avangard : PaymentMethod
    {
        public override ProcessType ProcessType
        {
            get { return ProcessType.ServerRequest; }
        }

        public override NotificationType NotificationType
        {
            get { return NotificationType.ReturnUrl | NotificationType.Handler; }
        }

        public override UrlStatus ShowUrls
        {
            get { return UrlStatus.CancelUrl | UrlStatus.ReturnUrl | UrlStatus.NotificationUrl; }
        }

        public string ShopId { get; set; }
        public string ShopPassword { get; set; }
        public string AvSign { get; set; }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {AvangardTemplate.ShopId, ShopId},
                               {AvangardTemplate.ShopPassword, ShopPassword},
                               {AvangardTemplate.AvSign, AvSign}
                           };
            }
            set
            {
                ShopId = value.ElementOrDefault(AvangardTemplate.ShopId);
                ShopPassword = value.ElementOrDefault(AvangardTemplate.ShopPassword);
                AvSign = value.ElementOrDefault(AvangardTemplate.AvSign);
            }
        }

        public override string ProcessServerRequest(Order order)
        {
            var responseTicket = GetTicket(order);
            if (responseTicket.ResponseCode != 0)
            {
                LogToFile("заказ " + order.OrderID + " пришел пустой тикет, " + responseTicket.ResponseMessage);
                return string.Empty;
            }

            PaymentService.SaveOrderpaymentInfo(order.OrderID, this.PaymentMethodId, AvangardTemplate.Ticket, responseTicket.Ticket);
            PaymentService.SaveOrderpaymentInfo(order.OrderID, this.PaymentMethodId, AvangardTemplate.OkCode, responseTicket.OkCode);
            PaymentService.SaveOrderpaymentInfo(order.OrderID, this.PaymentMethodId, AvangardTemplate.FailureCode, responseTicket.FailureCode);

            LogToFile("заказ " + order.OrderID + " получили коды операций и тикет. тикет:" + responseTicket.Ticket + ", OkCode:" + responseTicket.OkCode + ", FailureCode:" + responseTicket.FailureCode);

            return string.Format("https://www.avangard.ru/iacq/pay?ticket={0}", responseTicket.Ticket);
        }

        public override string ProcessResponse(HttpContext context)
        {
            LogToFile("обрабатываем ответ от банка после оплаты");
            string failNotification =
                "<span style=\"color:red;font-size:14px;\">Оплата не проведена: отказ банка - эмитента карты. Ошибка в процессе оплаты, указаны неверные данные карты.</span>";
            HttpRequest req = context.Request;


            var param = Parameters;
            var shopId = param["shop_id"];
            var ticket = string.Empty;
            var orderNumber = string.Empty;
            var status = -1;
            var statusDesc = string.Empty;
            var signature = string.Empty;
            var av_sign = param["av_sign"];
            var amount = string.Empty;

            LogToFile("Параметры запроса get: " + GetStringFromRequest(req, true));
            LogToFile("Параметры запроса post: " + GetStringFromRequest(req, true));
            LogToFile("Параметры запроса - количество файлов: " + req.Files.Count);

            context.Response.StatusCode = 202;
            context.Response.Status = "202 Accepted";

            if (req["result_code"] != null)
            {
                var additionalInfo = PaymentService.GetOrderIdByPaymentIdAndCode(this.PaymentMethodId, req["result_code"]);
                if (additionalInfo == null)
                {
                    LogToFile("Не нашли код " + req["result_code"] + " для метода оплаты " + this.PaymentMethodId);
                    return failNotification;
                }

                if (additionalInfo.Name == AvangardTemplate.FailureCode)
                {
                    LogToFile("заказ " + additionalInfo.OrderId + " получили FailureCode от банка:" +
                              additionalInfo.Value);
                    return failNotification;
                }

                var order = OrderService.GetOrder(additionalInfo.OrderId);
                if (order != null)
                {
                    LogToFile("заказ " + additionalInfo.OrderId + " получили OkCode от банка:" + additionalInfo.Value +
                              " выставляем статус оплачено");
                    OrderService.PayOrder(order.OrderID, true);

                    return "<span style=\"color:green; font-size:14px; \">Оплата прошла успешно.</span>";
                }
                return failNotification;
            }
            else if (req["xml"] != null)
            {
                using (var reader = XmlReader.Create(req["xml"],
                                                  new XmlReaderSettings
                                                  {
                                                      DtdProcessing = DtdProcessing.Ignore,
                                                      IgnoreWhitespace = true
                                                  }))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "ticket" && reader.Read())
                        {
                            ticket = reader.Value.Trim();
                        }
                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "order_number" && reader.Read())
                        {
                            orderNumber = reader.Value.Trim();
                        }
                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "status_code" && reader.Read())
                        {
                            status = Convert.ToInt32(reader.Value.Trim());
                        }
                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "status_desc" && reader.Read())
                        {
                            statusDesc = reader.Value.Trim();
                        }
                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "amount" && reader.Read())
                        {
                            amount = reader.Value.Trim();
                        }
                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "signature" && reader.Read())
                        {
                            signature = reader.Value.Trim();
                        }
                    }
                }
                if (string.IsNullOrEmpty(statusDesc))
                {
                    statusDesc = GetStatus(status);
                }

                if (!string.IsNullOrEmpty(ticket))
                {
                    var additionalInfo = PaymentService.GetOrderIdByPaymentIdAndCode(this.PaymentMethodId, ticket);
                    if (additionalInfo == null)
                    {
                        LogToFile("Не нашли код " + ticket + " для метода оплаты " + this.PaymentMethodId);
                        return failNotification;
                    }
                    var order = OrderService.GetOrder(additionalInfo.OrderId);
                    if (order != null && VerifySugrature(signature, av_sign, shopId, orderNumber, amount))
                    {
                        if (status != 3)
                        {
                            LogToFile("заказ " + additionalInfo.OrderId + " получил статус '" + statusDesc + "' от банка:" + additionalInfo.Value +
                                      " статус оплачено не выставляем");

                            return failNotification;
                        }
                        else
                        {
                            LogToFile("заказ " + additionalInfo.OrderId + " получил статус '" + statusDesc + "' от банка:" + additionalInfo.Value +
                                      " выставляем статус оплачено");
                            OrderService.PayOrder(order.OrderID, true);

                            return "<span style=\"color:green; font-size:14px; \">Оплата прошла успешно.</span>";
                        }
                    }
                }
                return failNotification;
            }
            else if (req["signature"] != null && req["order_number"] != null && req["amount"] != null && req["ticket"] != null && req["status_code"] != null)
            {
                ticket = req["ticket"];
                status = Convert.ToInt32(req["status_code"]);
                orderNumber = req["order_number"];
                amount = req["amount"];
                signature = req["signature"];
                statusDesc = GetStatus(status);
                if (!string.IsNullOrEmpty(ticket))
                {
                    var additionalInfo = PaymentService.GetOrderIdByPaymentIdAndCode(this.PaymentMethodId, ticket);
                    if (additionalInfo == null)
                    {
                        LogToFile("Не нашли код " + ticket + " для метода оплаты " + this.PaymentMethodId);
                        return failNotification;
                    }
                    var order = OrderService.GetOrder(additionalInfo.OrderId);
                    if (order != null && VerifySugrature(signature, av_sign, shopId, orderNumber, amount))
                    {
                        if (status != 3)
                        {
                            LogToFile("заказ " + additionalInfo.OrderId + " получил статус '" + statusDesc + "' от банка:" + additionalInfo.Value +
                                      " статус оплачено не выставляем");

                            return failNotification;
                        }
                        else
                        {
                            LogToFile("заказ " + additionalInfo.OrderId + " получил статус '" + statusDesc + "' от банка:" + additionalInfo.Value +
                                      " выставляем статус оплачено");
                            OrderService.PayOrder(order.OrderID, true);

                            return "<span style=\"color:green; font-size:14px; \">Оплата прошла успешно.</span>";
                        }
                    }
                }
                return failNotification;
            }
            else
            {
                LogToFile("Из банка пришел неизвестный ответ");
                return failNotification;
            }
        }

        private string GetStringFromRequest(HttpRequest request, bool variantGet)
        {
            string result = null;

            if (variantGet)
            {
                var keys = request.QueryString.AllKeys;
                foreach (var item in keys)
                {
                    result += item + " : " + request.QueryString[item] + ", ";
                }
            }
            else
            {
                var keys = request.Form.AllKeys;
                foreach (var item in keys)
                {
                    result += item + " : " + request.Form[item] + ", ";
                }
            }

            return result;
        }

        private bool VerifySugrature(string signature, string av_sign, string shop_id, string order_number, string amount)
        {
            return signature == ((((av_sign).Md5() + (shop_id + order_number + amount).Md5()).ToUpper()).Md5()).ToUpper();
        }

        private string GetStatus(int stat)
        {
            var result = string.Empty;
            if (stat == 0)
            {
                result = "Заказ не найден";
            }
            else if (stat == 1)
            {
                result = "Обрабатывается";
            }
            else if (stat == 2)
            {
                result = "Отбракован";
            }
            else if (stat == 3)
            {
                result = "Исполнен";
            }
            else
            {
                result = "Неизвестный статус";
            }

            return result;
        }

        private AvangardResponse GetTicket(Order order)
        {
            var result = new AvangardResponse();

            string sum = (Math.Round(order.Sum) * 100).ToString();

            var requestXmlString =
                "<?xml version=\"1.0\" encoding=\"windows-1251\"?>" +
                "<NEW_ORDER>" +
                "<SHOP_ID>{0}</SHOP_ID>" +
                "<SHOP_PASSWD>{1}</SHOP_PASSWD>" +
                "<AMOUNT>{2}</AMOUNT>" +
                "<ORDER_NUMBER>{3}</ORDER_NUMBER>" +
                "<ORDER_DESCRIPTION>{4}</ORDER_DESCRIPTION>" +
                "<LANGUAGE>{5}</LANGUAGE>" +
                "<BACK_URL>{6}</BACK_URL>" +
                "<CLIENT_NAME>{7}</CLIENT_NAME>" +
                "<CLIENT_ADDRESS>{8}</CLIENT_ADDRESS>" +
                "<CLIENT_EMAIL>{9}</CLIENT_EMAIL>" +
                "<CLIENT_PHONE>{10}</CLIENT_PHONE>" +
                "<CLIENT_IP>{11}</CLIENT_IP>" +
                "</NEW_ORDER >";

            //System.Net.HttpWebRequest adds the header 'HTTP header "Expect: 100-Continue"' to every request unless you explicitly ask it not to by setting this static property to false:
            ServicePointManager.Expect100Continue = false;

            var postData = string.Format(requestXmlString,
               ShopId,
               ShopPassword,
               sum,
               order.OrderID.ToString(),
               Translit(GetOrderDescription(order.Number)),
               CultureInfo.CurrentCulture.TwoLetterISOLanguageName,
               this.SuccessUrl,
               Translit(order.OrderCustomer.FirstName + " " + order.OrderCustomer.LastName),
               Translit(order.OrderCustomer.Country + "," + order.OrderCustomer.City + "," + order.OrderCustomer.GetCustomerAddress()),
               order.OrderCustomer.Email,
               order.OrderCustomer.StandardPhone,
               order.OrderCustomer.CustomerIP
               );

            LogToFile("заказ " + order.OrderID.ToString() + ", отправляем данные " + postData);

            WebRequest request = WebRequest.Create("https://www.avangard.ru/iacq/h2h/reg?xml=" + HttpUtility.UrlEncode(postData));
            request.Method = "GET";

            using (var reader = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.GetEncoding("windows-1251")))
            {
                var responseFromServer = reader.ReadToEnd();

                using (var xmlReader = XmlReader.Create(new StringReader(responseFromServer)))
                {
                    while (xmlReader.Read())
                    {
                        if (xmlReader.NodeType == XmlNodeType.Element)
                        {
                            switch (xmlReader.Name)
                            {
                                case "id":
                                    int id;
                                    if (xmlReader.Read() && int.TryParse(xmlReader.Value, out id))
                                    {
                                        result.Id = id;
                                    }
                                    break;
                                case "ticket":
                                    if (xmlReader.Read())
                                    {
                                        result.Ticket = xmlReader.Value;
                                    }
                                    break;
                                case "ok_code":
                                    if (xmlReader.Read())
                                    {
                                        result.OkCode = xmlReader.Value;
                                    }
                                    break;
                                case "failure_code":
                                    if (xmlReader.Read())
                                    {
                                        result.FailureCode = xmlReader.Value;
                                    }
                                    break;
                                case "response_code":
                                    int responseCode;
                                    if (xmlReader.Read() && int.TryParse(xmlReader.Value, out responseCode))
                                    {
                                        result.ResponseCode = responseCode;
                                    }
                                    break;
                                case "response_message":
                                    if (xmlReader.Read())
                                    {
                                        result.ResponseMessage = xmlReader.Value;
                                    }
                                    break;
                            }
                        }
                    }
                }

                reader.Close();
            }

            return result;
        }

        private string Translit(string input)
        {
            var dictionary = new Dictionary<string, string>
                {
                    {"а","a"},
                    {"б","b"},
                    {"в","v"},
                    {"г","g"},
                    {"д","d"},
                    {"е","e"},
                    {"ё","jo"},
                    {"ж","zh"},
                    {"з","z"},
                    {"и","i"},
                    {"й","j"},
                    {"к","k"},
                    {"л","l"},
                    {"м","m"},
                    {"н","n"},
                    {"о","o"},
                    {"п","p"},
                    {"р","r"},
                    {"с","s"},
                    {"т","t"},
                    {"у","u"},
                    {"ф","f"},
                    {"х","h"},
                    {"ц","c"},
                    {"ч","ch"},
                    {"ш","sh"},
                    {"щ","shh"},
                    {"ъ",""},
                    {"ы","y"},
                    {"ь","'"},
                    {"э","je"},
                    {"ю","ju"},
                    {"я","ja"}
                };



            var output = string.Empty;
            input = input.ToLower();
            for (int i = 0; i < input.Length; i++)
            {
                output += dictionary.ContainsKey(input[i].ToString())
                              ? dictionary[input[i].ToString()].ToString()
                              : input[i].ToString();
            }
            return output;
        }

        private void LogToFile(string logMessage)
        {
            try
            {
                var fullFilePath = HttpContext.Current.Server.MapPath("~/App_Data/avangardLog.txt");

                if (!File.Exists(fullFilePath))
                {
                    using (File.Create(fullFilePath))
                    {
                    }
                }

                using (var streamWriter = new StreamWriter(fullFilePath, true))
                {
                    streamWriter.WriteLine(DateTime.Now.ToString() + " " + logMessage);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error("не удалось залогировать событие: " + logMessage + " Ошибка: " + ex);
            }
        }

        private string GetRequestData()
        {
            var result = "";

            try
            {
                var request = HttpContext.Current.Request;
                var method = request.HttpMethod;
                var url = request.Url.ToString();
                var headers = "";
                var body = "";

                foreach (string key in request.Headers.AllKeys)
                {
                    headers += string.Format("{0}: {1} ", key, request.Headers[key]);
                }

                StreamReader reader = new StreamReader(request.InputStream);
                try
                {
                    body = reader.ReadToEnd();
                }
                finally
                {
                    reader.BaseStream.Position = 0;
                }

                var postData = "";
                string[] keys = request.Form.AllKeys;
                for (int i = 0; i < keys.Length; i++)
                {
                    postData += string.Format("&{0}={1}", keys[i], request.Form[keys[i]]);
                }

                result = string.Format("Method: {0},\n Url: {1},\n Headers: {2},\n Body: {3},\n Post data: {4}.", method, url, headers, body, postData);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return result;
        }

        enum OrderStstus
        {
            NotFound = 0,
            Processing = 1,
            Discarded = 2,
            Payed = 3,
            PartialRefund = 4,
            Refund = 5
        }

    }
}