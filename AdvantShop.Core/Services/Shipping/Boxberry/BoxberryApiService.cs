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
using AdvantShop.Shipping.Boxberry;

namespace AdvantShop.Core.Services.Shipping.Boxberry
{
    public class BoxberryApiService
    {
        private readonly string _apiUrlJson = "http://api.boxberry.de/json.php";

        private enum _methods
        {
            ListCities,
            ListCitiesFull,
            ListPoints,
            ListPointsShort,
            ListZips,
            ZipCheck,
            ListStatuses,
            ListStatusesFull,
            ListServices,
            CourierListCities,
            DeliveryCosts,
            PointsForParcels,
            PointsByPostCode,
            PointsDescription,

            ParselCreate,
            ParselCheck,
            ParselList,
            ParselDel,
            ParselStory,
            ParselSend,
            ParselSendStory,
            CreateIntake,
            OrdersBalance,

            WidgetSettings
        }

        private Dictionary<_methods, string> _methodsNames = new Dictionary<_methods, string>
        {
            {_methods.ListCities, "ListCities" },
            {_methods.ListCitiesFull, "ListCitiesFull" },
            {_methods.ListPoints, "ListPoints" },
            {_methods.ListPointsShort, "ListPointsShort" },
            { _methods.ListZips, "ListZips" },
            {_methods.ZipCheck, "ZipCheck" },
            {_methods.ListStatuses, "ListStatuses" },
            {_methods.ListStatusesFull, "ListStatusesFull" },
            {_methods.ListServices, "ListServices" },
            {_methods.CourierListCities, "CourierListCities" },
            {_methods.DeliveryCosts, "DeliveryCosts" },
            {_methods.PointsForParcels, "PointsForParcels" },
            {_methods.PointsByPostCode, "PointsByPostCode" },
            {_methods.PointsDescription, "PointsDescription" },

            {_methods.ParselCreate, "ParselCreate" },
            {_methods.ParselDel, "ParselDel" },

            {_methods.WidgetSettings, "WidgetSettings" }            
        };

        private Dictionary<_methods, string> _cacheKeys = new Dictionary<_methods, string>
        {
            {_methods.ListCitiesFull, "BoxberryListCitiesFull" },
            {_methods.ListPoints, "BoxberryListPoints" },
            {_methods.CourierListCities, "BoxberryCourierListCities" },
            {_methods.ListZips, "BoxberryListZips" }
        };

        private readonly string _token;
        private readonly string _receptionPointCode;
        private readonly float _defaultWeight;
        private readonly float _defaultHeight;
        private readonly float _defaultWidth;
        private readonly float _defaultDepth;

        public BoxberryApiService()
        {
        }

        public BoxberryApiService(string token, string receptionPointCode, float defaultWeight, float defaultHeight, float defaultWidth, float defaultDepth)
        {
            _token = token;
            _receptionPointCode = receptionPointCode;
            _defaultWeight = defaultWeight;
            _defaultHeight = defaultHeight;
            _defaultWidth = defaultWidth;
            _defaultDepth = defaultDepth;

        }

        public List<BoxberryCity> GetListCities()
        {
            var cities = CacheManager.Get<List<BoxberryCity>>(_cacheKeys[_methods.ListCitiesFull]);

            if (cities == null)
            {
                var resultString = PostRequestGetString(
                    new KeyValuePair<string, string>("method", _methodsNames[_methods.ListCitiesFull]));
                if (!string.IsNullOrEmpty(resultString))
                {
                    cities = JsonConvert.DeserializeObject<List<BoxberryCity>>(resultString);
                }
                if (cities != null)
                {
                    CacheManager.Insert<List<BoxberryCity>>(_cacheKeys[_methods.ListCitiesFull], cities, 1440);
                }
            }
            return cities;
        }

        public List<BoxberryCityCourier> GetCourierListCities()
        {
            var cities = CacheManager.Get<List<BoxberryCityCourier>>(_cacheKeys[_methods.CourierListCities]);

            if (cities == null)
            {
                var resultString = PostRequestGetString(
                    new KeyValuePair<string, string>("method", _methodsNames[_methods.CourierListCities]));
                if (!string.IsNullOrEmpty(resultString))
                {
                    cities = JsonConvert.DeserializeObject<List<BoxberryCityCourier>>(resultString);
                }
                if (cities != null)
                {
                    CacheManager.Insert<List<BoxberryCityCourier>>(_cacheKeys[_methods.CourierListCities], cities, 1440);
                }
            }
            return cities;
        }

        public List<BoxberryObjectPoint> GetListPoints(string cityCode, bool prepaid = true)
        {
            var points = CacheManager.Get<List<BoxberryObjectPoint>>(_cacheKeys[_methods.ListPoints] + cityCode);
            if (points == null)
            {
                var resultString = PostRequestGetString(
                    new KeyValuePair<string, string>("method", _methodsNames[_methods.ListPoints]),
                    new KeyValuePair<string, string>("CityCode", cityCode),
                    new KeyValuePair<string, string>("prepaid", prepaid ? "1" : "0"));
                if (!string.IsNullOrEmpty(resultString))
                {
                    points = JsonConvert.DeserializeObject<List<BoxberryObjectPoint>>(resultString);
                }
                if (points != null && points.Count > 0 && string.IsNullOrEmpty(points[0].Error))
                {
                    CacheManager.Insert<List<BoxberryObjectPoint>>(_cacheKeys[_methods.ListPoints] + cityCode, points, 60);
                }
            }
            return points;
        }

        public BoxberryDeliveryCost GetDeliveryCosts(string target, float weight, float ordersum, float deliverysum, float height, float width, float depth, string zip, float paySum)
        {
            BoxberryDeliveryCost deliveryCost = null;

            var resultString = PostRequestGetString(
                new KeyValuePair<string, string>("method", _methodsNames[_methods.DeliveryCosts]),
                new KeyValuePair<string, string>("target", target),
                new KeyValuePair<string, string>("weight", weight.ToString()),
                new KeyValuePair<string, string>("ordersum", ordersum.ToString()),
                new KeyValuePair<string, string>("deliverysum", deliverysum.ToString()),
                new KeyValuePair<string, string>("targetstart", string.IsNullOrEmpty(_receptionPointCode) ? "0" : _receptionPointCode),
                new KeyValuePair<string, string>("height", height.ToString()),
                new KeyValuePair<string, string>("width", width.ToString()),
                new KeyValuePair<string, string>("width", depth.ToString()),
                new KeyValuePair<string, string>("zip", zip.ToString()),
                new KeyValuePair<string, string>("paysum", paySum.ToString()),
                new KeyValuePair<string, string>("sucrh", "1"));

            if (!string.IsNullOrEmpty(resultString))
            {
                deliveryCost = JsonConvert.DeserializeObject<BoxberryDeliveryCost>(resultString);
            }

            return deliveryCost;
        }

        public List<BoxberryZip> GetListZips()
        {
            var zips = CacheManager.Get<List<BoxberryZip>>(_cacheKeys[_methods.ListZips]);
            if (zips == null)
            {
                var resultString = PostRequestGetString(
                new KeyValuePair<string, string>("method", _methodsNames[_methods.ListZips]));

                if (!string.IsNullOrEmpty(resultString))
                {
                    zips = JsonConvert.DeserializeObject<List<BoxberryZip>>(resultString);
                }
                if (zips != null)
                {
                    CacheManager.Insert<List<BoxberryZip>>(_cacheKeys[_methods.ListZips], zips, 1440);
                }
            }
            return zips;
        }

        public bool ZipCheck(string zipCode)
        {
            var result = false;

            var resultString = PostRequestGetString(
                new KeyValuePair<string, string>("method", _methodsNames[_methods.ZipCheck]),
                new KeyValuePair<string, string>("Zip", zipCode));

            if (!string.IsNullOrEmpty(resultString))
            {
                var zipCheckAnswerObject = JsonConvert.DeserializeObject<List<BoxberryZipCheck>>(resultString);
                result = zipCheckAnswerObject != null ? zipCheckAnswerObject[0].ExpressDelivery == "1" : false;

            }
            return result;
        }

        public BoxberryWaitingOrdersAnswer GetWaitingOrders()
        {
            var result = new BoxberryWaitingOrdersAnswer();
            var resultString = PostRequestGetString(
                new KeyValuePair<string, string>("method", _methodsNames[_methods.ParselList]));

            if (!string.IsNullOrEmpty(resultString))
            {
                result = JsonConvert.DeserializeObject<BoxberryWaitingOrdersAnswer>(resultString);
            }

            return result;
        }

        public BoxberryObjectOptions GetBoxberryOptions()
        {
            var result = new BoxberryObjectOptions();
            var resultString = PostRequestGetString(
               new KeyValuePair<string, string>("method", _methodsNames[_methods.WidgetSettings]));
            if (!string.IsNullOrEmpty(resultString))
            {
                result = JsonConvert.DeserializeObject<BoxberryObjectOptions>(resultString);
            }

            return result;
        }

        #region Parsel

        public BoxberryOrderTrackNumber ParselCreate(Order order)
        {
            var boxberryCustomer = new BoxberryOrderCustomer
            {
                Fio =
                            (string.IsNullOrEmpty(order.OrderCustomer.FirstName) ? string.Empty : order.OrderCustomer.FirstName + " ") +
                            (string.IsNullOrEmpty(order.OrderCustomer.Patronymic) ? string.Empty : order.OrderCustomer.Patronymic + " ") +
                            (string.IsNullOrEmpty(order.OrderCustomer.LastName) ? string.Empty : order.OrderCustomer.LastName + " "),
                Email = order.OrderCustomer.Email,
                Phone = order.OrderCustomer.Phone,
                Address = order.OrderCustomer.Country + "," +
                            order.OrderCustomer.Region + "," +
                            order.OrderCustomer.City + "," +
                            order.OrderCustomer.Street + "," +
                            order.OrderCustomer.House + "," +
                            order.OrderCustomer.Apartment
            };

            var boxberryOrderItems = new List<BoxberryOrderItem>();

            var orderItems = OrderService.RecalculateItemsPriceIncludingAllDiscounts(order.OrderItems, order.ShippingCost, order.Sum);
            var totalWeight = orderItems.Sum(item => item.Weight == 0 ? _defaultWeight * 1000 * item.Amount : item.Weight * 1000 * item.Amount);

            foreach (var orderItem in orderItems)
            {
                boxberryOrderItems.Add(new BoxberryOrderItem
                {
                    Id = orderItem.ArtNo,
                    Name = orderItem.Name,
                    Nds = "0",
                    Price = orderItem.Price,
                    Quantity = orderItem.Amount,
                    UnitName = orderItem.Unit
                });
            }

            var courierDeliveryInfo = new BoxberryOrderShippingInfo { };

            BoxberryOption boxberryOption = null;
            BoxberryObjectPoint boxberryPoint = null;
            if (order.OrderPickPoint.AdditionalData != null && order.OrderPickPoint.AdditionalData.Contains("ModelType"))
            {
                boxberryOption = JsonConvert.DeserializeObject<BoxberryOption>(order.OrderPickPoint.AdditionalData);
            }
            else
            {
                boxberryPoint = JsonConvert.DeserializeObject<BoxberryObjectPoint>(order.OrderPickPoint.AdditionalData);
            }

            var data = new BoxberryOrder
            {
                OrderId = order.OrderID.ToString(),
                TrackCode = order.TrackNumber,
                Price = order.Sum.ToString(),
                PaymentSum = order.Payed ? "0" : order.Sum.ToString(),
                DeliverySum = order.ShippingCost.ToString(),
                DeliveryType = boxberryOption != null ? "2" : "1",
                Customer = boxberryCustomer,
                Items = boxberryOrderItems,
                Weights = new BoxberryOrderWeight { Weight = totalWeight.ToString() }
            };

            if (boxberryOption != null)
            {
                data.Kurdost = new BoxberryOrderShippingInfo
                {
                    Address =
                        order.OrderCustomer.Street + "," +
                        order.OrderCustomer.House + "," +
                        order.OrderCustomer.Apartment,
                    City = order.OrderCustomer.City,
                    Index = order.OrderCustomer.Zip,
                    Comment = order.CustomerComment
                };
            }

            data.Shop = new BoxberryOrderShopInfo
            {
                CodeDestination = boxberryPoint != null ? boxberryPoint.Code : "",
                CodeDeparture = _receptionPointCode
            };


            var resultString = PostRequestGetString(_methods.ParselCreate, "sdata=" + JsonConvert.SerializeObject(data));

            if (!string.IsNullOrEmpty(resultString))
            {
                return JsonConvert.DeserializeObject<BoxberryOrderTrackNumber>(resultString);
            }

            return null;
        }

        public BoxberryOrderDeleteAnswer ParselDelete(string trackNumber)
        {
            var resultString = PostRequestGetString(
                new KeyValuePair<string, string>("method", _methodsNames[_methods.ParselDel]),
                new KeyValuePair<string, string>("ImId", trackNumber));

            if (!string.IsNullOrEmpty(resultString))
            {
                return JsonConvert.DeserializeObject<BoxberryOrderDeleteAnswer>(resultString);
            }
            return null;
        }

        public List<BoxberryParcelPoint> GetListPointsForParcels()
        {
            var result = new List<BoxberryParcelPoint>();

            var resultString = PostRequestGetString(
             new KeyValuePair<string, string>("method", _methodsNames[_methods.PointsForParcels]));

            if (!string.IsNullOrEmpty(resultString))
            {
                result = JsonConvert.DeserializeObject<List<BoxberryParcelPoint>>(resultString);
            }
            return result.OrderBy(point => point.City).ThenBy(pont => pont.Name).ToList();
        }

        #endregion

        #region Post request

        private string PostRequestGetString(params KeyValuePair<string, string>[] urlParams)
        {
            try
            {
                var url = _apiUrlJson + "?platform=advantshop&token=" + _token;

                for (int index = 0; index < urlParams.Count(); index++)
                {
                    url += string.Format("&{0}={1}", urlParams[index].Key, urlParams[index].Value);
                }

                var request = WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = 3000;


                request.ContentType = "application/www-x-form-urlencoded";

                using (var response = request.GetResponse())
                {
                    return (new StreamReader(response.GetResponseStream())).ReadToEnd();
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private string PostRequestGetString(_methods method, string data)
        {
            try
            {
                var url = _apiUrlJson;

                var request = WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = 3000;

                byte[] byteArray = Encoding.UTF8.GetBytes("platform=advantshop&token=" + _token + "&method=" + method.ToString() + "&" + data);

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
            }
            return string.Empty;
        }

        #endregion Post request
    }
}
