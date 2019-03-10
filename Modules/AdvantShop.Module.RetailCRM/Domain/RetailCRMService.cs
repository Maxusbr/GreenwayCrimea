using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using AdvantShop.ExportImport;
using AdvantShop.Helpers;
using AdvantShop.Modules.RetailCRM;
using AdvantShop.Modules.RetailCRM.Models;
using AdvantShop.Orders;
using AdvantShop.Shipping;
using Newtonsoft.Json;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Taxes;
using AdvantShop.Module.RetailCRMs.Domain.Models;

namespace AdvantShop.Modules
{
    public class RetailCRMService
    {
        public static string ModuleName
        {
            get { return "RetailCRM"; }
        }

        private const string RetailCRMUrl = "https://{0}/api/v3/{1}?apiKey={2}{3}";
        private const int Limit = 50;

        public static StringBuilder Message = new StringBuilder();


        private static T MakeRequest<T>(string url, string data = null, string method = "POST", string contentType = "application/x-www-form-urlencoded") where T : ResponseSimple, new()
        {
            var log = new RetailCRMLog();


            string requestUrl = string.Format(RetailCRMUrl,
                            ModuleSettingsProvider.GetSettingValue<string>("SubDomain", RetailCRMModule.ModuleStringId),
                            url,
                            ModuleSettingsProvider.GetSettingValue<string>("ApiKey", RetailCRMModule.ModuleStringId), method == "GET" ? "&" + data : string.Empty);

            log.Write("REQUEST url: " + requestUrl + " data: " + data);

            try
            {
                var request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.Method = method;
                request.ContentType = contentType;

                if (data.IsNotEmpty() && method == "POST")
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(data);
                    request.ContentLength = bytes.Length;

                    using (var requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                    }
                }

                var responseContent = "";
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                            using (var reader = new StreamReader(stream))
                            {
                                responseContent = reader.ReadToEnd()
                                    .Replace("\\/", "/").Replace("\"orders\":[]", "\"orders\":{}").Replace("\"properties\":[]", "\"properties\":{}").Replace("\"customFields\":[]", "\"customFields\":{}"); // Владимир: это ндец...
                            }
                    }
                }

                log.Write("RESPONSE " + responseContent + "\r\n");
                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            catch (WebException ex)
            {
                using (var eResponse = ex.Response)
                {
                    if (eResponse != null)
                    {
                        using (var eStream = eResponse.GetResponseStream())
                            if (eStream != null)
                                using (var reader = new StreamReader(eStream))
                                {
                                    var error = reader.ReadToEnd();
                                    try
                                    {
                                        var err = JsonConvert.DeserializeObject<T>(error);
                                        log.Write("ERROR " + err.errorMsg + " " + err.errors + "\r\n");
                                        return err;
                                    }
                                    catch (Exception ex2)
                                    {
                                        log.Write("ERROR " + ex2.Message + "\r\n");
                                        return new T()
                                        {
                                            errorMsg = ex2.Message,
                                            success = false
                                        };
                                    }
                                }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Write("ERROR " + ex.Message + "\r\n");
                Debug.Log.Error(ex);
                return new T()
                {
                    errorMsg = ex.Message,
                    success = false
                };
            }

            return new T()
            {
                errorMsg = "Ошибка при отправке запроса " + url,
                success = false
            };
        }
        
        public static bool PingCRM()
        {
            var responce = MakeRequest<ResponseSimple>("reference/sites", null, "GET");
            return responce != null && responce.success;
        }

        public static bool IsOrderExists(string orderid)
        {
            var responce = MakeRequest<ResponseOrder>("orders/" + orderid, "site=" + ModuleSettingsProvider.GetSettingValue<string>("Site", RetailCRMModule.ModuleStringId), "GET");
            return responce != null && responce.order != null && responce.order.externalId == orderid;
        }

        public static bool IsCustomerExists(Guid customerId)
        {
            var responce = MakeRequest<ResponseCustomer>("customers/" + customerId, "site=" + ModuleSettingsProvider.GetSettingValue<string>("Site", RetailCRMModule.ModuleStringId), "GET");
            return responce != null && responce.customer != null && responce.customer.externalId == customerId.ToString();
        }

        public static bool UploadOrderStatus(Order order, bool updateAdds, out string error)
        {

            if (!IsOrderExists(order.OrderID.ToString()))
            {
                error = string.Format("Заказ {0} не найден", order.OrderID);
                return false;
            }

            var status = GetStatus(order.OrderStatus.StatusID, order.OrderStatus.StatusName, out error);

            var retailOrder = new SerializedOrder()
            {
                createdAt = order.OrderDate,
                //number = order.OrderID.ToString(),
                status = status,
                externalId = order.OrderID.ToString(),
            };

            var responce =
                MakeRequest<ResponseSimple>(
                    string.Format("orders/{0}/edit", order.OrderID),
                    "site=" + ModuleSettingsProvider.GetSettingValue<string>("Site", RetailCRMModule.ModuleStringId) +
                    "&order=" + JsonConvert.SerializeObject(retailOrder, Formatting.None, new JsonSerializerSettings()
                    {
                        DateFormatString = "yyyy-MM-dd HH:mm:ss",
                        NullValueHandling = NullValueHandling.Ignore
                    }));

            error = responce.errorMsg.IsNotEmpty() ? responce.errorMsg + " data = " + JsonConvert.SerializeObject(retailOrder, Formatting.None, new JsonSerializerSettings()
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                NullValueHandling = NullValueHandling.Ignore
            }) : string.Empty;
            return responce.success;
        }

        public static bool UploadOrderPaid(Order order, bool paid, out string error)
        {
            var retailOrder = new SerializedOrder()
            {
                externalId = order.OrderID.ToString(),
                paymentStatus = paid ? "paid" : "not-paid",
            };

            var responce =
                MakeRequest<ResponseSimple>(string.Format("orders/{0}/edit", order.OrderID),
                    "site=" + ModuleSettingsProvider.GetSettingValue<string>("Site", RetailCRMModule.ModuleStringId) +
                    "&order=" + JsonConvert.SerializeObject(retailOrder, Formatting.None, new JsonSerializerSettings()
                    {
                        DateFormatString = "yyyy-MM-dd HH:mm:ss",
                        NullValueHandling = NullValueHandling.Ignore
                    }));

            error = responce.errorMsg.IsNotEmpty() ? responce.errorMsg + " data = " + JsonConvert.SerializeObject(retailOrder, Formatting.None, new JsonSerializerSettings()
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                NullValueHandling = NullValueHandling.Ignore
            }) : string.Empty;
            return responce.success;

        }


        public static bool UploadOrder(Order order, bool updateAdds, out string error)
        {
            if (order.OrderCustomer == null)
            {
                error = "Не найден покупатель для заказа " + order.OrderID;
                return false;
            }

            var internalShipping = GetInternalShipping(order.ShippingMethodId, order.ArchivedShippingName);
            var status = GetStatus(order.OrderStatus.StatusID, order.OrderStatus.StatusName, out error);
            if (updateAdds)
            {
                if (order.PaymentMethodId != 0)
                {
                    if (!UpdatePaymentType(order.PaymentMethodId, order.PaymentMethodName, out error))
                        return false;
                }

                if (internalShipping != null)
                {
                    if (!UpdateDeliveryType(order.ShippingMethodId, order.ShippingMethodName, new List<string>() { "adv-" + order.PaymentMethodId }, out error))
                        return false;

                    if (internalShipping.HasServices)
                    {
                        if (!UpdateDeliveryService(order.ShippingMethodId, order.ArchivedShippingName, out error))
                            return false;
                    }
                }

                //if (!UpdateOrderType("Физическое лицо", "eshop-individual", out error))
                //   return false;

                if (!UpdateOrderMethod("Через корзину", "shopping-cart", out error))
                    return false;



                if (!UpdatePaymentStatus(order.Payed, out error))
                    return false;
            }
            if (!UploadOrderCustomer(order, out error))
                return false;


            var productSum = order.OrderItems.Sum(item => item.Amount * item.Price);
            var discount = -(order.Sum - order.ShippingCost - productSum);

            var roistat = GetRoistat(order.OrderID);

            Dictionary<string, string> customFields = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(roistat))
            {
                customFields.Add("roistat", roistat);
            }

            if (order.Coupon != null) {
                customFields.Add("coupon", order.Coupon.Code + ", " + (order.Coupon.Type == CouponType.Fixed ? PriceFormatService.FormatPrice(order.Coupon.Value) : order.Coupon.Value + "%"));
            }
                    
            var retailOrder = new SerializedOrder()
            {
                createdAt = order.OrderDate,
                customerComment = RetailEncode(order.CustomerComment),
                delivery = order.ShippingMethodId != 0 ? new SerializedOrderDelivery()
                {
                    address = order.OrderCustomer != null ? new CustomerAddress()
                    {
                        city = RetailEncode(order.OrderCustomer.City),
                        country = RetailEncode(order.OrderCustomer.Country),
                        index = RetailEncode(order.OrderCustomer.Zip),
                        region = RetailEncode(order.OrderCustomer.Region),
                        
                        //text = RetailEncode(order.OrderCustomer.GetCustomerAddress()),
                        street = order.OrderCustomer.Street,
                        building = order.OrderCustomer.House,
                        flat = order.OrderCustomer.Apartment,
                        house = order.OrderCustomer.Structure,
                        block = order.OrderCustomer.Entrance.TryParseInt(),
                        floor = order.OrderCustomer.Floor.TryParseInt(),

                        notes = RetailEncode(
                            new string[]
                            {
                                order.OrderCustomer.CustomField1, order.OrderCustomer.CustomField2,
                                order.OrderCustomer.CustomField3
                            }.Where(s => s.IsNotEmpty()).AggregateString(", ")),
                    } : null,
                    code = internalShipping != null ? internalShipping.Code :  null,
                    cost = order.ShippingCost,
                    //data = new List<object>(), // "deliverydata", //TODO Данные для интеграционных типов доставки
                    service = internalShipping != null && !internalShipping.IsIntegratedShipping && internalShipping.HasServices ? new SerializedDeliveryService()
                    {
                        code = "adv-" + Slugify(StringHelper.Translit(order.ArchivedShippingName)),
                        deliveryType = "adv-" + order.ShippingMethodId,
                        name = order.ArchivedShippingName
                    } : null,
                } : null,
                discount = (float?)Math.Round(discount, 2),
                discountPercent = 0,
                email = order.OrderCustomer.Email,
                externalId = order.OrderID.ToString(),
                number = order.Number,
                customerId = order.OrderCustomer.CustomerID.ToString(),
                firstName = RetailEncode(order.OrderCustomer.FirstName),
                lastName = RetailEncode(order.OrderCustomer.LastName),
                patronymic = RetailEncode(order.OrderCustomer.Patronymic.IsNotEmpty() ? order.OrderCustomer.Patronymic : order.OrderCustomer.FirstName.Split(" ").Count() == 3 ? order.OrderCustomer.FirstName.Split(" ").Last() : null),
                phone = RetailEncode(order.OrderCustomer.Phone),
                managerComment = RetailEncode(order.AdminOrderComment)
                                    + (order.ArchivedShippingName != null ? RetailEncode(" Доставка: " + order.ArchivedShippingName) : string.Empty)
                                    + (order.OrderPickPoint != null ? RetailEncode(" Пункт выдачи: " + order.OrderPickPoint.PickPointAddress) : string.Empty)
                                    + RetailEncode(order.OrderCustomer.FirstName.IsNotEmpty() && !order.OrderCustomer.FirstName.Contains(order.OrderCustomer.FirstName) ? " Контакт для доставки: " + order.OrderCustomer.FirstName : ""),
                orderMethod = order.OrderSource != null && order.OrderSource.Type == Core.Services.Orders.OrderType.LandingPage ? "landing-page" : "shopping-cart",
                //orderType = "eshop-individual",
                paymentStatus = order.Payed ? "paid" : "not-paid",
                paymentType = order.PaymentMethodId != 0 ? "adv-" + order.PaymentMethodId : null,
                status = status,
                items = order.OrderItems.Select(item => new SerializedOrderProduct()
                {
                    productId = order.OrderItems.Count(x => x.ArtNo == item.ArtNo) > 1 && item.Price == 0 ? (item.ArtNo + item.Name).GetHashCode().ToString() : item.ArtNo,
                    initialPrice = item.Price,
                    productName = RetailEncode(item.Name),
                    purchasePrice = item.SupplyPrice,
                    quantity = item.Amount,
                    xmlId = order.OrderItems.Count(x => x.ArtNo == item.ArtNo) > 1 && item.Price == 0 ? (item.ArtNo + item.Name).GetHashCode().ToString() : item.ArtNo,
                    properties = item.Weight != 0 ? new List<RetailCRM.Models.Property> { new RetailCRM.Models.Property { code = "weight", name = "weight", value = item.Weight.ToString() } } : null
                }).ToList(),

                customFields = customFields.Any() ? customFields : null
            };

            var responce = new ResponseSimple();
            try
            {
                responce = MakeRequest<ResponseSimple>(
                        IsOrderExists(order.OrderID.ToString()) ? string.Format("orders/{0}/edit", order.OrderID) : "orders/create",
                        "site=" + ModuleSettingsProvider.GetSettingValue<string>("Site", RetailCRMModule.ModuleStringId) +
                        "&order=" + JsonConvert.SerializeObject(retailOrder, Formatting.None, new JsonSerializerSettings()
                        {
                            DateFormatString = "yyyy-MM-dd HH:mm:ss",
                            NullValueHandling = NullValueHandling.Ignore
                        }));

                error = responce.errorMsg.IsNotEmpty() ? responce.errorMsg + " data = " + JsonConvert.SerializeObject(retailOrder, Formatting.None, new JsonSerializerSettings()
                {
                    DateFormatString = "yyyy-MM-dd HH:mm:ss",
                    NullValueHandling = NullValueHandling.Ignore
                }) : string.Empty;
            }
            catch(Exception ex)
            {
                Debug.LogError(ex);
                error = ex.Message;
            }
            return responce.success;

        }

        private static InternalShipping GetInternalShipping(int shippingid, string archivedShippingName)
        {
            InternalShipping shipping = new InternalShipping() { AdvId = "adv-" + shippingid };
            var searchName = archivedShippingName.ToLower();


            var shippingMethod = ShippingMethodService.GetShippingMethod(shippingid);

            if (shippingMethod == null)
                return null;

            if (shippingMethod != null)
            {
                switch (shippingMethod.ShippingType)
                {
                    case "Cdek":
                    case "Sdek":
                        shipping.Code = shipping.IntegrationCode = "sdek";
                        shipping.IsIntegratedShipping = true;
                        break;
                    case "CheckoutRu":
                        shipping.Code = shipping.IntegrationCode = "checkout";
                        shipping.IsIntegratedShipping = true;
                        break;
                    case "ShippingByEmsPost":
                        shipping.Code = shipping.IntegrationCode = "ems";
                        shipping.IsIntegratedShipping = true;
                        break;
                    case "ShippingNovaPoshta":
                        shipping.Code = shipping.IntegrationCode = "newpost";
                        shipping.IsIntegratedShipping = true;
                        break;

                    case "Edost":
                        shipping.Code = "";
                        shipping.IsIntegratedShipping = false;
                        shipping.HasServices = true;
                        break;

                    default:
                        shipping.Code = null;
                        shipping.IsIntegratedShipping = false;
                        break;
                }
            }

            //if (shipping.IsIntegratedShipping)
            //    return shipping;

            if (searchName.Contains("ems почта россии"))
            {
                shipping.Code = shipping.IntegrationCode = "ems";
            }
            else if (searchName.Contains("почта россии"))
            {
                shipping.Code = "russian-post";
                shipping.IntegrationCode = "russianpost";
            }
            else if (searchName.Contains("сдэк"))
            {
                shipping.Code = shipping.IntegrationCode = "sdek";
            }
            else if (searchName.Contains("спср"))
            {
                shipping.Code = shipping.IntegrationCode = "spsr";
            }
            else if (searchName.Contains("dpd"))
            {
                shipping.Code = shipping.IntegrationCode = "dpd";
            }
            else if (searchName.Contains("новая почта") || searchName.Contains("нова пошта"))
            {
                shipping.Code = shipping.IntegrationCode = "newpost";

            }
            else
            {
                shipping.Code = "adv-" + shippingid;
            }


            string error = "";
            var crmShippings = GetDeliveries(out error);

            if (crmShippings != null && crmShippings.Any(ship => ship.code == shipping.AdvId && (ship.integrationCode == shipping.Code || ship.integrationCode == shipping.IntegrationCode)))
            {
                shipping.Code = shipping.AdvId;
                shipping.IsIntegratedShipping = true;
                shipping.HasServices = false;
            }
            else if (crmShippings != null && crmShippings.Any(ship => ship.integrationCode == shipping.Code || ship.integrationCode == shipping.IntegrationCode))
            {
                shipping.IsIntegratedShipping = true;
                shipping.HasServices = false;
            }
            else
            {
                shipping.Code = "adv-" + shippingid;
                shipping.IsIntegratedShipping = false;
            }

            return shipping;
        }



        public static bool UploadOrders(out string error)
        {
            var orders = OrderService.GetAllOrders();
            bool valid = true;

            try
            {

                Message.Append("<br/> Отправка статусов");
                foreach (var status in orders.Select(o => new KeyValuePair<int, string>(o.OrderStatusId, o.OrderStatus.StatusName)).Distinct())
                {
                    GetStatus(status.Key, status.Value, out error);
                    Thread.Sleep(500);
                }

                Message.Append("<br/> Отправка методов оплаты");
                foreach (var payment in orders.Select(o => new KeyValuePair<int, string>(o.PaymentMethodId, o.PaymentMethodName)).Distinct())
                {
                    if (!UpdatePaymentType(payment.Key, payment.Value, out error))
                        return false;
                    Thread.Sleep(500);
                }

                var paymentsCodes = orders.Select(o => "adv-" + o.PaymentMethodId).Distinct().ToList();

                Message.Append("<br/> Отправка методов доставки");
                foreach (var shipping in orders.Select(o => new KeyValuePair<int, string>(o.ShippingMethodId, o.ShippingMethodName)).Distinct())
                {
                    if (!UpdateDeliveryType(shipping.Key, shipping.Value, paymentsCodes, out error))
                        return false;
                    Thread.Sleep(500);
                }

                foreach (var shipping in orders.Select(o => new KeyValuePair<int, string>(o.ShippingMethodId, o.ArchivedShippingName)).Distinct())
                {
                    var internalShipping = GetInternalShipping(shipping.Key, shipping.Value);
                    if (internalShipping != null && internalShipping.HasServices)
                    {
                        if (!UpdateDeliveryService(shipping.Key, shipping.Value, out error))
                            return false;
                        Thread.Sleep(500);
                    }
                    if(internalShipping == null)
                    {
                        error = "Не верный метод доставки: Id " + shipping.Key + " \"" + shipping.Value + "\", в заказах: " + 
                            string.Join(",", orders.Where(x => x.ShippingMethodId == shipping.Key).Select(o => o.Number));
                        throw new Exception(error);
                     }
                }

                //if (!UpdateOrderType("Физическое лицо", "eshop-individual", out error))
                //   return false;

                if (!UpdateOrderMethod("Через корзину", "shopping-cart", out error))
                    return false;

                if (!UpdatePaymentStatus(true, out error))
                    return false;

                if (!UpdatePaymentStatus(false, out error))
                    return false;


                Message.Append("<br/> Отправка заказов");
                int i = 0;
                foreach (var order in orders)
                {
                    valid &= UploadOrder(order, false, out error);
                    error += error;
                    Message.Append("<br/>" + error + " заказ №" + order.OrderID);
                    Message.Append("<br/> отправлено " + ++i + " заказ(ов)");
                    Thread.Sleep(500);
                }
            }
            catch(Exception ex)
            {
                error = ex.Message;
                Debug.LogError(ex);
                valid = false;
            }

            return valid;
        }

        public static bool UploadOrderCustomer(Order order, out string error)
        {
            if (order.OrderCustomer == null)
            {
                error = "Покупатель не найден для заказа " + order.OrderID;
                return false;
            }

            var retailCustomer = new SerializedCustomer()
            {
                createdAt = order.OrderDate,
                address = order.OrderCustomer != null ? new CustomerAddress()
                {
                    city = RetailEncode(order.OrderCustomer.City),
                    country = RetailEncode(order.OrderCustomer.Country),
                    index = RetailEncode(order.OrderCustomer.Zip),
                    region = RetailEncode(order.OrderCustomer.Region),
                    //text = RetailEncode(order.OrderCustomer.GetCustomerAddress()),
                    
                    notes = RetailEncode(new string[]
                    {
                        order.OrderCustomer.CustomField1, order.OrderCustomer.CustomField2,
                        order.OrderCustomer.CustomField3
                    }.Where(s => s.IsNotEmpty()).AggregateString(", ")),
                } : null,
                email = RetailEncode(order.OrderCustomer.Email),
                externalId = order.OrderCustomer.CustomerID.ToString(),
                firstName = RetailEncode(order.OrderCustomer.FirstName),
                lastName = RetailEncode(order.OrderCustomer.LastName),
                patronymic = RetailEncode(order.OrderCustomer.Patronymic),
                phones = new List<CustomerPhone>()
                {
                    new CustomerPhone()
                    {
                        number = RetailEncode(order.OrderCustomer.Phone)
                    }
                }
            };

            var responce =
                MakeRequest<ResponseSimple>(
                    IsCustomerExists(order.OrderCustomer.CustomerID)
                        ? string.Format("customers/{0}/edit", order.OrderCustomer.CustomerID)
                        : "customers/create",
                    "site=" + ModuleSettingsProvider.GetSettingValue<string>("Site", RetailCRMModule.ModuleStringId) +
                    "&customer=" +
                    JsonConvert.SerializeObject(retailCustomer, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            DateFormatString = "yyyy-MM-dd HH:mm:ss",
                            NullValueHandling = NullValueHandling.Ignore
                        }));

            error = responce.errorMsg.IsNotEmpty() ? responce.errorMsg + " data = " + JsonConvert.SerializeObject(retailCustomer, Formatting.None,
                new JsonSerializerSettings()
                {
                    DateFormatString = "yyyy-MM-dd HH:mm:ss",
                    NullValueHandling = NullValueHandling.Ignore
                }) : string.Empty;

            return responce.success;
        }

        public static bool UpdateDeliveryType(int shippingmethodId, string shippingMethodName, List<string> payments, out string error)
        {
            var my = new DeliveryType()
            {
                name = shippingMethodName.IsNotEmpty() ? RetailEncode(shippingMethodName) : "Не выбран тип доставки",
                code = "adv-" + shippingmethodId,
                defaultCost = 0,
                //paymentTypes = payments
            };

            var responce = MakeRequest<ResponseSimple>(string.Format("reference/delivery-types/{0}/edit", my.code), "deliveryType=" + JsonConvert.SerializeObject(my, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
            error = responce.errorMsg;
            return responce.success;

        }

        public static bool UpdateDeliveryService(int shippingmethodId, string shippingName, out string error)
        {
            if (shippingName.IsNullOrEmpty())
            {
                error = "";
                return true;
            }

            var my = new DeliveryService()
            {
                name = shippingName.IsNotEmpty() ? RetailEncode(shippingName) : "Не выбран сервис доставки",
                code = "adv-" + Slugify(StringHelper.Translit(shippingName)) + "-adv-" + shippingmethodId,
                deliveryType = "adv-" + shippingmethodId,
            };

            var responce = MakeRequest<ResponseSimple>(string.Format("reference/delivery-services/{0}/edit", my.code), "deliveryService=" + JsonConvert.SerializeObject(my));
            error = responce.errorMsg;
            return responce.success;
        }

        public static bool UpdateOrderType(string ordertype, string code, out string error)
        {
            var my = new RetailCRM.Models.OrderType()
            {
                name = RetailEncode(ordertype),
                code = code, //"adv-" + StringHelper.Translit(ordertype),
                defaultForApi = true
            };

            var responce = MakeRequest<ResponseSimple>(string.Format("reference/order-types/{0}/edit", my.code), "orderType=" + JsonConvert.SerializeObject(my));
            error = responce.errorMsg;
            return responce.success;

        }

        public static bool UpdateOrderMethod(string ordermethod, string code, out string error)
        {
            var my = new OrderMethod()
            {
                name = RetailEncode(ordermethod),
                code = code, //"adv-" + StringHelper.Translit(ordermethod),
                defaultForApi = true,
            };

            var responce = MakeRequest<ResponseSimple>(string.Format("reference/order-methods/{0}/edit", my.code), "orderMethod=" + JsonConvert.SerializeObject(my));
            error = responce.errorMsg;
            return responce.success;

        }

        public static bool UpdatePaymentStatus(bool paid, out string error)
        {
            var my = new PaymentStatus()
            {
                name = RetailEncode(paid ? "Оплачен" : "Не оплачен"),
                code = paid ? "paid" : "not-paid",
                paymentComplete = paid
            };

            var responce = MakeRequest<ResponseSimple>(string.Format("reference/payment-statuses/{0}/edit", my.code), "paymentStatus=" + JsonConvert.SerializeObject(my));
            error = responce.errorMsg;
            return responce.success;

        }

        public static bool UpdatePaymentType(int paymentMethodId, string paymentMethod, out string error)
        {
            var my = new PaymentType()
            {
                name = paymentMethod.IsNotEmpty() ? RetailEncode(paymentMethod) : "Не выбран способ оплаты",
                code = "adv-" + paymentMethodId,
                defaultForApi = true,
            };

            var responce = MakeRequest<ResponseSimple>(string.Format("reference/payment-types/{0}/edit", my.code), "paymentType=" + JsonConvert.SerializeObject(my));
            error = responce.errorMsg;
            return responce.success;
        }

        public static List<Site> GetSites(out string error)
        {
            var responce = MakeRequest<SitesResponse>("reference/sites", null, "GET");
            error = responce.errorMsg;
            return responce.sites;
        }

        public static List<PaymentType> GetPayments(out string error)
        {
            var responce = MakeRequest<PaymentTypeResponse>("reference/payment-types", null, "GET");
            error = responce.errorMsg;
            return responce.paymentTypes;
        }

        public static List<DeliveryType> GetDeliveries(out string error)
        {
            var responce = MakeRequest<DeliveryTypeResponse>("reference/delivery-types", null, "GET");
            error = responce.errorMsg;
            return responce.deliveryTypes;
        }



        public static List<Status> GetStatuses(out string error)
        {
            var responce = MakeRequest<StatusResponse>("reference/statuses", null, "GET");
            error = responce.errorMsg;
            return responce.statuses;
        }

        public static List<StatusGroup> GetGroups(out string error)
        {
            var responce = MakeRequest<GetStatusgroups>("reference/status-groups", null, "GET");
            error = responce.errorMsg;
            return responce.statusGroups;
        }

        public static string GetStatus(int statusID, string statusName, out string error)
        {
            var statuses = JsonConvert.DeserializeObject<Dictionary<int, string>>(ModuleSettingsProvider.GetSettingValue<string>("Statuses", RetailCRMModule.ModuleStringId));
            if (statuses != null && statuses.ContainsKey(statusID))
            {
                error = "";
                return statuses[statusID];
            }
            else
            {
                var my = new Status()
                {
                    name = RetailEncode(statusName),
                    code = "adv-" + statusID,
                    ordering = 0,
                };

                my.group = ModuleSettingsProvider.GetSettingValue<string>("DefaultStatusCategory", RetailCRMModule.ModuleStringId);
                var responce = MakeRequest<ResponseSimple>(string.Format("reference/statuses/{0}/edit", my.code), "status=" + JsonConvert.SerializeObject(my));
                error = responce.errorMsg;

                if (responce.success)
                {
                    if (statuses == null) statuses = new Dictionary<int, string>();
                    statuses.Add(statusID, my.code);
                    ModuleSettingsProvider.SetSettingValue("Statuses", JsonConvert.SerializeObject(statuses), RetailCRMModule.ModuleStringId);
                }

                return my.code;
            }
        }

        public static bool GetOrderHistory(out string error, DateTime? from = null)
        {
            DateTime startDate = from ??
                                     DateTime.ParseExact(ModuleSettingsProvider.GetSettingValue<string>("SyncDate", RetailCRMModule.ModuleStringId) ??
                                     new DateTime(2000, 1, 1).ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            DateTime endDate = DateTime.Now;
            bool res = true;
            error = "";
            List<SerializedOrder> orders;

            try
            {
                int offset = 0;
                do
                {
                    var responce = MakeRequest<OrderHistoryResponse>(string.Format("orders/history"),
                                    string.Format("startDate={0}&endDate={1}&skipMyChanges=true&offset={2}", startDate.ToString("yyyy-MM-dd HH:mm:ss"), endDate.ToString("yyyy-MM-dd HH:mm:ss"), offset), "GET");
                    orders = responce.orders;
                    error += responce.errorMsg;

                    if (responce.success && orders != null)
                    {
                        ProcessOrders(orders);
                    }

                    offset += Limit;
                } while (orders != null && orders.Count > 0);

            }
            catch (Exception ex) {
                Debug.Log.Error(ex);
            }
            ModuleSettingsProvider.SetSettingValue("SyncDate", endDate.ToString("yyyy-MM-dd HH:mm:ss"), RetailCRMModule.ModuleStringId);
            return res;
        }
        

        private static void ProcessOrders(IEnumerable<SerializedOrder> crmOrders)
        {
            string error = "";

            var statuses = new Dictionary<int, string>();
            var str = ModuleSettingsProvider.GetSettingValue<string>("Statuses", RetailCRMModule.ModuleStringId);
            if (str != null)
            {
                statuses = JsonConvert.DeserializeObject<Dictionary<int, string>>(str);
            }

            var payments = GetPayments(out error);


            foreach (var crmOrder in crmOrders)
            {
                if (crmOrder == null || crmOrder.externalId.IsNullOrEmpty())
                    continue;

                var baseOrder = OrderService.GetOrder(crmOrder.externalId.Replace("A", "").TryParseInt());
                if (baseOrder == null)
                    continue;

                if (crmOrder.paymentStatus != null && crmOrder.paymentStatus == "paid" && !baseOrder.Payed)
                {
                    OrderService.PayOrder(baseOrder.OrderID, true, false);
                }

                if (crmOrder.paymentStatus != null && crmOrder.paymentStatus == "not-paid" && baseOrder.Payed)
                {
                    OrderService.PayOrder(baseOrder.OrderID, false, false);
                }


                if (crmOrder.customerComment != null)
                {
                    baseOrder.CustomerComment = crmOrder.customerComment;
                }

                if (crmOrder.delivery != null)
                {
                    if (crmOrder.delivery.code != null)
                    {
                        var shippingId = crmOrder.delivery.code.Replace("adv-", "").TryParseInt();

                        var shipping = ShippingMethodService.GetShippingMethod(shippingId);
                        if (shipping != null)
                        {
                            baseOrder.ShippingMethodId = shipping.ShippingMethodId;
                            baseOrder.ArchivedShippingName = shipping.Name;
                        }
                    }

                    if (crmOrder.delivery.service != null && crmOrder.delivery.service.name != null)
                        baseOrder.ArchivedShippingName = crmOrder.delivery.service.name;

                    if (crmOrder.delivery.cost != 0)
                        baseOrder.ShippingCost = crmOrder.delivery.cost;


                    if (crmOrder.delivery.address != null)
                    {
                        if (crmOrder.delivery.address.city != null)
                            baseOrder.OrderCustomer.City = crmOrder.delivery.address.city;

                        if (crmOrder.delivery.address.country != null)
                            baseOrder.OrderCustomer.Country = crmOrder.delivery.address.country;

                        if (crmOrder.delivery.address.region != null)
                            baseOrder.OrderCustomer.Region = crmOrder.delivery.address.region;

                        if (crmOrder.delivery.address.text != null)
                        {
                            baseOrder.OrderCustomer.Street = crmOrder.delivery.address.text;
                            baseOrder.OrderCustomer.Apartment = "";
                            baseOrder.OrderCustomer.Entrance = "";
                            baseOrder.OrderCustomer.House = "";
                            baseOrder.OrderCustomer.Structure = "";
                            baseOrder.OrderCustomer.Floor = "";
                        }
                        else
                        {
                            baseOrder.OrderCustomer.Street = !string.IsNullOrEmpty(crmOrder.delivery.address.street) ? crmOrder.delivery.address.street : string.Empty;
                            baseOrder.OrderCustomer.Street += !string.IsNullOrEmpty(crmOrder.delivery.address.metro) ? ", ст. м. " + crmOrder.delivery.address.metro : string.Empty;

                            if (!string.IsNullOrEmpty(crmOrder.delivery.address.building))
                                baseOrder.OrderCustomer.House =  crmOrder.delivery.address.building;

                            if (!string.IsNullOrEmpty(crmOrder.delivery.address.flat))
                                baseOrder.OrderCustomer.Apartment +=  crmOrder.delivery.address.flat;

                            if (!string.IsNullOrEmpty(crmOrder.delivery.address.house))
                                baseOrder.OrderCustomer.Structure +=  crmOrder.delivery.address.house;

                            if (crmOrder.delivery.address.block > 0)
                                baseOrder.OrderCustomer.Entrance += crmOrder.delivery.address.block;

                            if (crmOrder.delivery.address.floor > 0)
                                baseOrder.OrderCustomer.Floor += crmOrder.delivery.address.floor;
                        }

                        if (crmOrder.delivery.address.index != null)
                            baseOrder.OrderCustomer.Zip = crmOrder.delivery.address.index;
                    }
                }

                var baseOrderCustomer = baseOrder.OrderCustomer.DeepClone();

                if (crmOrder.email != null)
                {
                    baseOrderCustomer.Email = crmOrder.email;
                }

                if (crmOrder.firstName != null)
                {
                    baseOrderCustomer.FirstName = crmOrder.firstName;
                }

                if (crmOrder.lastName != null)
                {
                    baseOrderCustomer.LastName = crmOrder.lastName;
                }

                if (crmOrder.managerComment != null)
                {
                    baseOrder.AdminOrderComment = crmOrder.managerComment;
                }

                if (crmOrder.phone != null)
                {
                    baseOrderCustomer.Phone = crmOrder.phone;
                    baseOrderCustomer.StandardPhone = StringHelper.ConvertToStandardPhone(crmOrder.phone);
                }

                if (crmOrder.paymentType != null)
                {
                    var payment = payments.Find(p => p.code == crmOrder.paymentType);
                    if (payment != null && payment.code != null)
                    {
                        baseOrder.ArchivedPaymentName = payment.name;
                        baseOrder.PaymentMethodId = payment.code.Replace("adv-", "").TryParseInt();
                    }
                }

                if (crmOrder.customerComment != null)
                {
                    baseOrder.CustomerComment = crmOrder.customerComment;
                }

                // items

                var oldItems = baseOrder.OrderItems.DeepClone();

                foreach (var crmItem in crmOrder.items.Where(item => !item.deleted))
                {
                    var baseItem = baseOrder.OrderItems.FirstOrDefault(item => item.ArtNo == crmItem.offer.externalId);
                    if (baseItem != null)
                    {
                        baseItem.Amount = crmItem.quantity;
                        baseItem.Price = crmItem.initialPrice;
                        baseItem.SupplyPrice = crmItem.purchasePrice;
                    }
                    else
                    {
                        Catalog.Offer offer = null;

                        if (crmItem.offer.externalId != null && (offer = OfferService.GetOffer(crmItem.offer.externalId)) != null)
                        {
                            var item = new OrderItem()
                            {
                                Amount = crmItem.quantity,
                                Price = crmItem.initialPrice,
                                SupplyPrice = crmItem.purchasePrice,
                                ArtNo = crmItem.offer.externalId ?? string.Empty,
                                Color = offer.Color != null ? offer.Color.ColorName : string.Empty,
                                Size = offer.Size != null ? offer.Size.SizeName : string.Empty,
                                Name = offer.Product.Name ?? string.Empty,
                                ProductID = offer.ProductId,
                                Weight = offer.Product.Weight,
                            };

                            var tax = offer.Product.TaxId != null ? TaxService.GetTax(offer.Product.TaxId.Value) : null;
                            if (tax != null)
                            {
                                item.TaxId = tax.TaxId;
                                item.TaxName = tax.Name;
                                item.TaxRate = tax.Rate;
                                item.TaxShowInPrice = tax.ShowInPrice;
                                item.TaxType = tax.TaxType;
                            }

                            baseOrder.OrderItems.Add(item);
                        }
                        else
                        {
                            baseOrder.OrderItems.Add(new OrderItem()
                            {
                                Amount = crmItem.quantity,
                                Price = crmItem.initialPrice,
                                SupplyPrice = crmItem.purchasePrice,
                                ArtNo = crmItem.offer.externalId ?? string.Empty,
                                Color = string.Empty,
                                Size = string.Empty,
                                Name = crmItem.offer.name ?? string.Empty,
                                ProductID = null,
                            });

                        }
                    }
                }

                foreach (var crmItem in crmOrder.items.Where(item => item.deleted))
                {
                    baseOrder.OrderItems.RemoveAll(baseItem => (crmItem.offer != null && crmItem.offer.externalId == baseItem.ArtNo) || crmItem.id == baseItem.ArtNo);
                }

                if (crmOrder.discountPercent.HasValue)
                {
                    baseOrder.OrderDiscount = crmOrder.discountPercent.Value;
                }

                //    productId = item.ArtNo,
                //    initialPrice = item.Price,
                //    productName = RetailEncode(item.Name),
                //    purchasePrice = item.SupplyPrice,
                //    quantity = item.Amount,
                //    xmlId = item.ArtNo

                //itmes

                try
                {
                    OrderService.UpdateOrderMain(baseOrder, false, new OrderChangedBy("RetailCRM"), false);
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                    error += " OrderId: " + baseOrder.OrderID + " Error: " + ex.StackTrace;
                }

                try
                {
                    OrderService.UpdateOrderCustomer(baseOrderCustomer, new OrderChangedBy("RetailCRM"), false);
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                    error += " OrderId: " + baseOrder.OrderID + " Error: " + ex.StackTrace;
                }

                try
                {
                    var prevStatus = baseOrder.OrderStatusId;
                    if (crmOrder.status != null)
                    {
                        var statusPair = statuses.FirstOrDefault(s => s.Value == crmOrder.status);
                        if (statusPair.Key != 0)
                        {
                            baseOrder.OrderStatusId = statusPair.Key;
                        }
                        else
                        {
                            var crmStatuses = GetStatuses(out error);
                            var crmStatus = crmStatuses.FirstOrDefault(s => s.code == crmOrder.status);
                            if (crmStatus != null)
                            {
                                baseOrder.OrderStatusId = OrderStatusService.AddOrderStatus(new OrderStatus() {StatusName = crmStatus.name});
                                statuses.Add(baseOrder.OrderStatusId, crmStatus.code);
                                ModuleSettingsProvider.SetSettingValue("Statuses", JsonConvert.SerializeObject(statuses), RetailCRMModule.ModuleStringId);
                            }
                        }

                        if (prevStatus != baseOrder.OrderStatusId)
                        {
                            OrderStatusService.ChangeOrderStatus(baseOrder.OrderID, baseOrder.OrderStatusId, "Обновление статуса из retailCRM", false);
                        }
                    }

                    if (baseOrder.OrderItems != null && oldItems != null)
                    {
                        OrderService.AddUpdateOrderItems(baseOrder.OrderItems, oldItems, baseOrder.OrderID, new OrderChangedBy("RetailCRM"), false, false);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                    error += " OrderId: " + baseOrder.OrderID + " Error: " + ex.StackTrace;
                }
            }

            if (!string.IsNullOrEmpty(error))
            {
                var log = new RetailCRMLog();
                log.Write(error);
            }
        }

        private static string RetailEncode(string input)
        {
            if (input.IsNullOrEmpty())
                return string.Empty;

            return HttpUtility.UrlEncode(input)
                .Replace("%5c", "\\\\")
                .Replace("%22", "\\\"")
                .Replace("%2f", "/")
                .Replace("%08", "\b")
                .Replace("%0c", "\f")
                .Replace("%0a", "\n")
                .Replace("%0d", "\r")
                .Replace("%09", "\t"); // вот такая вот магия. %22 не пропускает, надо передавать как \"
        }

        public static string Slugify(string phrase)
        {
            string str = phrase.ToLower();
            str = System.Text.RegularExpressions.Regex.Replace(str, @"[^a-z0-9\s-]", ""); // Remove all non valid chars          
            str = System.Text.RegularExpressions.Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space  
            str = System.Text.RegularExpressions.Regex.Replace(str, @"\s", "-"); // //Replace spaces by dashes
            return str;
        }

        public static IEnumerable<ExportFeedProductModel> GetProducts(ExportFeedSettings settings)
        {
            return SQLDataAccess.ExecuteReadIEnumerable<RetailCrmExportFeedProduct>(
                @"DECLARE @defaultCurrencyRatio FLOAT;
                SELECT @defaultCurrencyRatio = CurrencyValue FROM [Catalog].[Currency] WHERE CurrencyIso3 = @selectedCurrency;
                SELECT [Product].[BarCode],[Product].[Enabled],[Product].[ProductID],[Product].[Discount],[Product].[DiscountAmount],AllowPreOrder,Amount,[ProductCategories].[CategoryId] AS [ParentCategory],([Offer].[Price] / @defaultCurrencyRatio) AS Price,ShippingPrice,[Product].[Name],[Product].[UrlPath],[Product].[Description],[Product].[BriefDescription],[Product].SalesNote,OfferId,[Offer].ArtNo,[Offer].Main,[Offer].ColorID,ColorName,[Offer].SizeID,SizeName,BrandName,GoogleProductCategory,YandexMarketCategory,Gtin,Adult,CurrencyValue,[Settings].PhotoToString(Offer.ColorID, Product.ProductId) AS Photos,ManufacturerWarranty,[Weight],[Product].[Enabled],[Offer].SupplyPrice,
                height, length, width 
                FROM [Catalog].[Product]
                INNER JOIN [Catalog].[Offer] ON [Offer].[ProductID] = [Product].[ProductID]
                INNER JOIN [Catalog].[ProductCategories] ON [ProductCategories].[ProductID] = [Product].[ProductID]
                LEFT JOIN [Catalog].[Color] ON [Color].ColorID = [Offer].ColorID
                LEFT JOIN [Catalog].[Size] ON [Size].SizeID = [Offer].SizeID
                LEFT JOIN [Catalog].Brand ON Brand.BrandID = [Product].BrandID
                INNER JOIN [Catalog].Currency ON Currency.CurrencyID = [Product].CurrencyID
                WHERE (
				SELECT TOP (1) [ProductCategories].[CategoryId]
				FROM [Catalog].[ProductCategories]
				INNER JOIN [Catalog].[Category] ON [Category].[CategoryId] = [ProductCategories].[CategoryId]
				WHERE [ProductID] = [Product].[ProductID]
					AND [Enabled] = 1
					order by [Main] desc
				) = [ProductCategories].[CategoryId]
			AND (Offer.Price > 0 OR @exportNotAvailable = 1)
			AND (
				Offer.Amount > 0
				OR Product.AllowPreOrder = 1
				OR @exportNotAvailable = 1
				)
			AND CategoryEnabled = 1
			AND (Product.Enabled = 1 OR @exportNotAvailable = 1)",
                CommandType.Text,
                reader => new RetailCrmExportFeedProduct
                {
                    ProductId = SQLDataHelper.GetInt(reader, "ProductID"),
                    OfferId = SQLDataHelper.GetInt(reader, "OfferId"),
                    ArtNo = SQLDataHelper.GetString(reader, "ArtNo"),
                    Amount = SQLDataHelper.GetInt(reader, "Amount"),
                    UrlPath = SQLDataHelper.GetString(reader, "UrlPath"),
                    Price = SQLDataHelper.GetFloat(reader, "Price"),
                    ShippingPrice = SQLDataHelper.GetFloat(reader, "ShippingPrice"),
                    Discount = SQLDataHelper.GetFloat(reader, "Discount"),
                    DiscountAmount = SQLDataHelper.GetFloat(reader, "DiscountAmount"),
                    ParentCategory = SQLDataHelper.GetInt(reader, "ParentCategory"),
                    Name = SQLDataHelper.GetString(reader, "Name"),
                    Description = SQLDataHelper.GetString(reader, "Description"),
                    BriefDescription = SQLDataHelper.GetString(reader, "BriefDescription"),
                    Photos = SQLDataHelper.GetString(reader, "Photos"),
                    SalesNote = SQLDataHelper.GetString(reader, "SalesNote"),
                    ColorId = SQLDataHelper.GetInt(reader, "ColorId"),
                    ColorName = SQLDataHelper.GetString(reader, "ColorName"),
                    SizeId = SQLDataHelper.GetInt(reader, "SizeId"),
                    SizeName = SQLDataHelper.GetString(reader, "SizeName"),
                    BrandName = SQLDataHelper.GetString(reader, "BrandName"),
                    Main = SQLDataHelper.GetBoolean(reader, "Main"),
                    YandexMarketCategory = SQLDataHelper.GetString(reader, "YandexMarketCategory"),
                    Adult = SQLDataHelper.GetBoolean(reader, "Adult"),
                    CurrencyValue = SQLDataHelper.GetFloat(reader, "CurrencyValue"),
                    ManufacturerWarranty = SQLDataHelper.GetBoolean(reader, "ManufacturerWarranty"),

                    Enabled = SQLDataHelper.GetBoolean(reader, "Enabled"),
                    Weight = SQLDataHelper.GetFloat(reader, "Weight"),
                    SupplyPrice = SQLDataHelper.GetFloat(reader, "SupplyPrice"),

                    Height = SQLDataHelper.GetFloat(reader, "Height"),
                    Width = SQLDataHelper.GetFloat(reader, "Width"),
                    Length = SQLDataHelper.GetFloat(reader, "Length"),
                    BarCode = SQLDataHelper.GetString(reader, "BarCode")

                },
                new SqlParameter("@selectedCurrency", ((ExportFeedYandexOptions) settings).Currency),
                new SqlParameter("@exportNotAvailable", ((ExportFeedYandexOptions) settings).ExportNotAvailable)
                //new SqlParameter("@exportNotActive", settings.ExportNotActiveProducts),
                //new SqlParameter("@exportNotAmount", settings.ExportNotAmountProducts)
                );
        }

        public static IEnumerable<ExportFeedCategories> GetCategories()
        {
            return SQLDataAccess.ExecuteReadIEnumerable<ExportFeedCategories>("Select * from [Catalog].Category",
                                                                               CommandType.Text,
                                                                               reader => new ExportFeedCategories
                                                                               {
                                                                                   Id = SQLDataHelper.GetInt(reader, "CategoryID"),
                                                                                   ParentCategory = SQLDataHelper.GetInt(reader, "ParentCategory"),
                                                                                   Name = SQLDataHelper.GetString(reader, "Name")
                                                                               });
        }


        public static void InstallModule()
        {
            if (!ModulesRepository.IsExistsModuleTable("Module", "RetailCRMOrder"))
            {
                ModulesRepository.ModuleExecuteNonQuery(@"CREATE TABLE [Module].[RetailCrmOrder](
	                    [OrderID] [int] NOT NULL,
	                    [Roistat] [nvarchar](250) NOT NULL,
                     CONSTRAINT [PK_RetailCRMOrder] PRIMARY KEY CLUSTERED 
                    (
	                    [OrderID] ASC
                    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                    ) ON [PRIMARY]", CommandType.Text);

                ModulesRepository.ModuleExecuteNonQuery(@"ALTER TABLE [Module].[RetailCrmOrder]  WITH CHECK ADD  CONSTRAINT [FK_RetailCrmOrder_Order] FOREIGN KEY([OrderID])
                    REFERENCES [Order].[Order] ([OrderID])
                    ON UPDATE CASCADE
                    ON DELETE CASCADE", CommandType.Text);

            }
        }

        public static void SaveRoistat(int orderID)
        {
            if (!ModulesRepository.IsExistsModuleTable("Module", "RetailCRMOrder"))
            {
                InstallModule();
            }

            var cookie = HttpContext.Current.Request.Cookies["roistat_visit"];
            var roistat_visit = "";
            if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
            {
                roistat_visit = cookie.Value;

                try
                {
                    ModulesRepository.ModuleExecuteNonQuery("Insert into [Module].[RetailCrmOrder] (OrderID, Roistat) values(@OrderID, @Roistat)", CommandType.Text,
                        new SqlParameter("@OrderID", orderID), new SqlParameter("@Roistat", roistat_visit));
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                }

            }
        }

        public static string GetRoistat(int orderID)
        {
            try
            {
                return ModulesRepository.ModuleExecuteScalar<string>("select Roistat from [Module].[RetailCrmOrder] where OrderID  = @OrderID", CommandType.Text,
                    new SqlParameter("@OrderID", orderID));
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return null;
            }
        }
    }
}