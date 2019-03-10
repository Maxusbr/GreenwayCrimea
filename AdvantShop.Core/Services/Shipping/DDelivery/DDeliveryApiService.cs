using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;


using Newtonsoft.Json;
using AdvantShop.Core.Caching;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Repository;
using AdvantShop.Shipping.DDelivery;
using AdvantShop.Catalog;
using AdvantShop.Shipping;

namespace AdvantShop.Core.Services.Shipping.DDelivery
{
    public class DDeliveryApiService
    {
        private readonly string _apiUrlJson = "https://ddelivery.ru/api/";

        private enum _methods
        {
            Calculator,
            DeliveryType,
            Cities,
            DeliveryPoints,
            DeliveryCompany,
            CreateOrder,
            OrderStatus,
            OrderInfo,
            OrderCansel
        }

        private Dictionary<_methods, string> _methodsUrlPaths = new Dictionary<_methods, string>
        {
            {_methods.Calculator, "calculator.json" },
            {_methods.DeliveryType, "delivery-type.json" },
            {_methods.Cities, "list/city.json" },
            {_methods.DeliveryPoints, "list/delivery-point.json" },
            {_methods.DeliveryCompany, "list/delivery-company.json" },

            {_methods.CreateOrder, "order/create.json" },
            {_methods.OrderStatus, "order/status.json" },
            {_methods.OrderInfo, "order/info.json" },
            {_methods.OrderCansel, "order/cancel.json" }
        };

        private Dictionary<_methods, string> _cacheKeys = new Dictionary<_methods, string>
        {
            {_methods.DeliveryType, "ListDeliveryTypes" },
            {_methods.DeliveryCompany, "ListDeliveryCompanies" }  
            //{_methods.Cities, "DDeliveryCities" },
            //{_methods.DeliveryPoints, "DeliveryPoints" }            
        };

        private readonly string _apiKey;
        private readonly string _shopId;
        private readonly float _defaultWeight;
        private readonly float _defaultHeight;
        private readonly float _defaultWidth;
        private readonly float _defaultDepth;
        private readonly string _receptionCompanyId;
        private readonly bool _сreateDraftOrder;
        private readonly bool _useWidget;

        private readonly DDeliveryService _dDeliveryService;

        public DDeliveryApiService()
        {
        }

        public DDeliveryApiService(string apiKey, string shopId, string receptionCompanyId, bool сreateDraftOrder, bool useWidget, float defaultWeight, float defaultHeight, float defaultWidth, float defaultDepth)
        {
            _apiKey = apiKey;
            _shopId = shopId;
            _receptionCompanyId = receptionCompanyId;
            _сreateDraftOrder = сreateDraftOrder;
            _useWidget = useWidget;
            _defaultWeight = defaultWeight;
            _defaultHeight = defaultHeight;
            _defaultWidth = defaultWidth;
            _defaultDepth = defaultDepth;

            _dDeliveryService = new DDeliveryService();
        }

        public DDeliveryObjectResponse<DDeliveryObjectCalculatorAnswer> CalculateDelivery(int cityToId, float[] dimensions, float weight)
        {
            var calculateDelivery = new DDeliveryObjectResponse<DDeliveryObjectCalculatorAnswer>();

            var resultString = GetRequestGetString(
                _methodsUrlPaths[_methods.Calculator],
                new KeyValuePair<string, string>("city_to", cityToId.ToString()),
                new KeyValuePair<string, string>("side1", (dimensions[0]).ToString().Replace(".",",")),
                new KeyValuePair<string, string>("side2", (dimensions[1]).ToString().Replace(".", ",")),
                new KeyValuePair<string, string>("side3", (dimensions[2]).ToString().Replace(".", ",")),
                new KeyValuePair<string, string>("weight", weight.ToString())
                );
            if (!string.IsNullOrEmpty(resultString))
            {
                calculateDelivery = JsonConvert.DeserializeObject<DDeliveryObjectResponse<DDeliveryObjectCalculatorAnswer>>(resultString);
            }

            return calculateDelivery;
        }

        public List<DDeliveryObjectPoint> GetPoints(int cityId)
        {
            var points = new List<DDeliveryObjectPoint>();

            var resultString = GetRequestGetString(
                _methodsUrlPaths[_methods.DeliveryPoints],
                new KeyValuePair<string, string>("city_id", cityId.ToString()));
            if (!string.IsNullOrEmpty(resultString))
            {
                points = JsonConvert.DeserializeObject<List<DDeliveryObjectPoint>>(resultString);
            }

            return points;
        }

        public DDeliveryObjectCity GetCity(string cityName)
        {
            DDeliveryObjectCity result = null;
            //CacheManager.Get<List<DDeliveryCity>>(_cacheKeys[_methods.Cities]);
            //if (result == null)
            //{
            var resultString = GetRequestGetString(_methodsUrlPaths[_methods.Cities], new KeyValuePair<string, string>("name", cityName));
            if (!string.IsNullOrEmpty(resultString))
            {
                var resultList = JsonConvert.DeserializeObject<List<DDeliveryObjectCity>>(resultString);
                if (resultList != null && resultList.Count > 0)
                {
                    result = resultList[0];
                }
            }
            //if (result != null)
            //{
            //    CacheManager.Insert<List<DDeliveryCity>>(_cacheKeys[_methods.Cities], result, 1440);
            //}
            //}

            return result;
        }

        public Dictionary<int, string> GetDeliveryTypes()
        {
            var result = CacheManager.Get<Dictionary<int, string>>(_cacheKeys[_methods.DeliveryType]);
            if (result == null)
            {
                var resultString = GetRequestGetString(_methodsUrlPaths[_methods.DeliveryType]);
                if (!string.IsNullOrEmpty(resultString))
                {
                    result = JsonConvert.DeserializeObject<Dictionary<int, string>>(resultString);
                }
                if (result != null)
                {
                    CacheManager.Insert<Dictionary<int, string>>(_cacheKeys[_methods.DeliveryType], result, 1440);
                }
            }

            return result;
        }

        public List<DDeliveryObjectCompany> GetDeliveryCompanies()
        {
            var result = CacheManager.Get<List<DDeliveryObjectCompany>>(_cacheKeys[_methods.DeliveryCompany]);
            if (result == null)
            {
                var resultString = GetRequestGetString(_methodsUrlPaths[_methods.DeliveryCompany]);
                if (!string.IsNullOrEmpty(resultString))
                {
                    result = JsonConvert.DeserializeObject<List<DDeliveryObjectCompany>>(resultString);
                }
                if (result != null)
                {
                    CacheManager.Insert<List<DDeliveryObjectCompany>>(_cacheKeys[_methods.DeliveryCompany], result, 1440);
                }
            }

            return result;
        }

        #region Process Order

        public DDeliveryObjectResponse<DDeliveryObjectNewOrder> CreateOrder(Order order, float[] dimensions)
        {
            DDeliveryOption dDeliveryOption = null;
            DDeliveryPoint dDeliveryPointOption = null;
            int deliveryTypeId = -1;
            int deliveryCompanyId = -1;

            if (order.OrderPickPoint.AdditionalData != null && order.OrderPickPoint.AdditionalData.Contains("ModelType"))
            {
                dDeliveryOption = JsonConvert.DeserializeObject<DDeliveryOption>(order.OrderPickPoint.AdditionalData);
                if (dDeliveryOption != null)
                {
                    deliveryTypeId = dDeliveryOption.DeliveryTypeId;
                    deliveryCompanyId = dDeliveryOption.DeliveryCompanyId;
                }
            }
            else
            {
                dDeliveryPointOption = JsonConvert.DeserializeObject<DDeliveryPoint>(order.OrderPickPoint.AdditionalData);
                if (dDeliveryPointOption != null)
                {
                    deliveryTypeId = dDeliveryPointOption.DeliveryTypeId;
                    deliveryCompanyId = dDeliveryPointOption.DeliveryCompanyId;
                }
            }

            if (dDeliveryOption == null && dDeliveryPointOption == null)
            {
                return new DDeliveryObjectResponse<DDeliveryObjectNewOrder>
                {
                    Status = "error",
                    Message = "Ошибка создания черновика заказа DDelivery, нет данных о пункте назначения"
                };
            }

            var orderItems = OrderService.RecalculateItemsPriceIncludingAllDiscounts(order.OrderItems, order.ShippingCost, order.Sum);


            var totalWeight = orderItems.Sum(item => (item.Weight == 0 ? _defaultWeight : item.Weight) * item.Amount);
            var cityDest = GetCity(order.OrderCustomer.City);

            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("type", deliveryTypeId.ToString()),
                new KeyValuePair<string, string>("order_id_from_shop", order.OrderID.ToString()),
                new KeyValuePair<string, string>("delivery_company_id", deliveryCompanyId.ToString()),//Компания доставки
                new KeyValuePair<string, string>("pickup_company_id", _receptionCompanyId),//Компания забора 
            };

            if (dDeliveryPointOption != null)
            {
                parameters.Add(new KeyValuePair<string, string>("delivery_point_id", dDeliveryPointOption.Code.ToString()));//ПВЗ(пункт самовывоза)
                parameters.Add(new KeyValuePair<string, string>("old_delivery_point_id", dDeliveryPointOption.Code.ToString()));
                
            }

            if (dDeliveryOption != null)
            {
                parameters.Add(new KeyValuePair<string, string>("to_city_id", cityDest != null ? cityDest.Id.ToString() : "-1"));
                parameters.Add(new KeyValuePair<string, string>("to_street", order.OrderCustomer.Street));
                parameters.Add(new KeyValuePair<string, string>("to_house", order.OrderCustomer.House));
                parameters.Add(new KeyValuePair<string, string>("to_flat", order.OrderCustomer.Apartment));
                parameters.Add(new KeyValuePair<string, string>("to_postal_code", order.OrderCustomer.Zip));
            }
            if (order.OrderCustomer != null)
            {
                parameters.Add(new KeyValuePair<string, string>("to_name", order.OrderCustomer.FirstName + " " + order.OrderCustomer.LastName));
                parameters.Add(new KeyValuePair<string, string>("to_phone", order.OrderCustomer.Phone));
                parameters.Add(new KeyValuePair<string, string>("to_email", order.OrderCustomer.Email));
                parameters.Add(new KeyValuePair<string, string>("to_add_phone", ""));
            }

            parameters.Add(new KeyValuePair<string, string>("price_declared", order.Sum.ToString()));

            if (dimensions != null && dimensions.Count() == 3)
            {
                parameters.Add(new KeyValuePair<string, string>("side1", Convert.ToInt32(dimensions[0]).ToString()));
                parameters.Add(new KeyValuePair<string, string>("side2", Convert.ToInt32(dimensions[1]).ToString()));
                parameters.Add(new KeyValuePair<string, string>("side3", Convert.ToInt32(dimensions[2]).ToString()));
            }

            parameters.Add(new KeyValuePair<string, string>("weight", totalWeight.ToString("f2").Replace(",",".")));
            parameters.Add(new KeyValuePair<string, string>("item_count", Convert.ToInt32(orderItems.Sum(item => item.Amount)).ToString()));
            parameters.Add(new KeyValuePair<string, string>("client_price", order.Payed ? order.Sum.ToString() : "0"));
            parameters.Add(new KeyValuePair<string, string>("comment", order.CustomerComment));
     
            foreach (var orderItem in orderItems.Select(item => new
            {
                name = item.Name,
                vendor_code = item.ArtNo,
                barcode = item.ArtNo,
                nds = 0,
                price_declared = Convert.ToInt32(item.Price),
                count = Convert.ToInt32(item.Amount)
            }).ToList())
            {
                parameters.Add(new KeyValuePair<string, string>("products[]", JsonConvert.SerializeObject(orderItem)));
               
            }

            //parameters.Add(new KeyValuePair<string, string>("products[]", JsonConvert.SerializeObject(orderItems.Select(item => new
            //{
            //    name = item.Name,
            //    vendor_code = item.ArtNo,
            //    barcode = item.ArtNo,
            //    nds = 0,
            //    price_declared = Convert.ToInt32(item.Price),
            //    count = Convert.ToInt32(item.Amount)
            //}).ToList())));

            parameters.Add(new KeyValuePair<string, string>("services", ""));
            parameters.Add(new KeyValuePair<string, string>("npp_option", "false"));
            parameters.Add(new KeyValuePair<string, string>("is_confirm", (!_сreateDraftOrder).ToString().ToLower()));

            var resultString = PostRequestGetString(_methodsUrlPaths[_methods.CreateOrder], parameters.ToArray());

            if (!string.IsNullOrEmpty(resultString))
            {
                return JsonConvert.DeserializeObject<DDeliveryObjectResponse<DDeliveryObjectNewOrder>>(resultString);
            }

            return new DDeliveryObjectResponse<DDeliveryObjectNewOrder>
            {
                Status = "error",
                Message = "Ошибка создания черновика заказа DDelivery"
            };
        }

        //public string GetOrderStatus(string ddeliveryOrderId)
        //{

        //    var resultString = GetRequestGetString(
        //        _methodsUrlPaths[_methods.OrderStatus],
        //        new KeyValuePair<string, string>("order_ids", orderId));
        //    if (!string.IsNullOrEmpty(resultString))
        //    {
        //        return resultString;
        //        //return JsonConvert.DeserializeObject<Dictionary<int, string>>(resultString);
        //    }


        //    return string.Empty;
        //}

        public DDeliveryObjectResponse<DDeliveryObjectOrderInfo> GetOrderInfo(string ddeliveryOrderId)
        {
            var resultString = GetRequestGetString(
                _methodsUrlPaths[_methods.OrderInfo],
                new KeyValuePair<string, string>("order_id", ddeliveryOrderId));
            if (!string.IsNullOrEmpty(resultString))
            {
                return JsonConvert.DeserializeObject<DDeliveryObjectResponse<DDeliveryObjectOrderInfo>>(resultString);
            }

            return new DDeliveryObjectResponse<DDeliveryObjectOrderInfo>
            {
                Status = "error",
                Message = "Ошибка получения информации о заказе в системе DDelivery " + ddeliveryOrderId
            };
        }

        public DDeliveryObjectResponse<object> CanselOrder(string ddeliveryOrderId)
        {
            var resultString = PostRequestGetString(
              _methodsUrlPaths[_methods.OrderCansel],
              new KeyValuePair<string, string>("order_ids[]", ddeliveryOrderId));
            if (!string.IsNullOrEmpty(resultString))
            {
                return JsonConvert.DeserializeObject<DDeliveryObjectResponse<object>>(resultString);
            }

            return new DDeliveryObjectResponse<object>
            {
                Status = "error",
                Message = "Ошибка отмены заказа в системе DDelivery " + ddeliveryOrderId
            };
        }


        #endregion

        #region Post request

        private string GetRequestGetString(string methodUrlPath, params KeyValuePair<string, string>[] urlParams)
        {
            try
            {
                var url = _apiUrlJson + _apiKey + "/" + methodUrlPath;

                for (int index = 0; index < urlParams.Count(); index++)
                {
                    if (!string.IsNullOrEmpty(urlParams[index].Key))
                    {
                        url += string.Format((index == 0 ? "?" : "&") + "{0}={1}", urlParams[index].Key, urlParams[index].Value);
                    }
                }

                var request = WebRequest.Create(url);
                request.Method = "GET";
                request.Timeout = 30000;


                request.ContentType = "application/json";

                using (var response = request.GetResponse())
                {
                    return (new StreamReader(response.GetResponseStream())).ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            return string.Empty;
        }


        private string PostRequestGetString(string methodUrlPath, params KeyValuePair<string, string>[] urlParams)
        {
            try
            {
                var url = _apiUrlJson + _apiKey + "/" + methodUrlPath;
                var dataString = string.Empty;
                for (int index = 0; index < urlParams.Count(); index++)
                {
                    if (!string.IsNullOrEmpty(urlParams[index].Key))
                    {
                        dataString += string.Format((index == 0 ? "" : "&") + "{0}={1}", HttpUtility.UrlEncode(urlParams[index].Key), HttpUtility.UrlEncode(urlParams[index].Value));
                    }
                }

                var request = WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = 30000;

                byte[] byteArray = Encoding.UTF8.GetBytes(dataString);

                request.ContentType = "application/x-www-form-urlencoded";
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                }
                using (var response = request.GetResponse())
                {
                    return (new StreamReader(response.GetResponseStream())).ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return string.Empty;
            }
        }

        #endregion Post request
    }
}
