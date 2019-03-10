using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Core.Services.Shipping.YandexDelivery;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Repository;
using Newtonsoft.Json;

namespace AdvantShop.Shipping.ShippingYandexDelivery
{
    [ShippingKey("YandexDelivery")]
    public class YandexDelivery : BaseShippingWithCargo //BaseShippingWithCargoAndCache
    {
        private readonly string _clientId;
        private readonly string _cityFrom;
        private readonly string _senderId;
        private readonly string _warehousId;
        private readonly string _requisiteId;
        private readonly string _secretKeyDelivery;
        private readonly string _secretKeyCreateOrder;
        private readonly string _widgetCode;
        private readonly bool _showAssessedValue;

        private const string Url = "https://delivery.yandex.ru/api/last";

        public YandexDelivery(ShippingMethod method, PreOrder preOrder)
            : base(method, preOrder)
        {
            _cityFrom = _method.Params.ElementOrDefault(YandexDeliveryTemplate.CityFrom);
            _clientId = _method.Params.ElementOrDefault(YandexDeliveryTemplate.ClientId);
            _senderId = _method.Params.ElementOrDefault(YandexDeliveryTemplate.SenderId);
            _warehousId = _method.Params.ElementOrDefault(YandexDeliveryTemplate.WarehouseId);
            _requisiteId = _method.Params.ElementOrDefault(YandexDeliveryTemplate.RequisiteId);

            _secretKeyDelivery = _method.Params.ElementOrDefault(YandexDeliveryTemplate.SecretKeyDelivery);
            _secretKeyCreateOrder = _method.Params.ElementOrDefault(YandexDeliveryTemplate.SecretKeyCreateOrder);

            _showAssessedValue = _method.Params.ElementOrDefault(YandexDeliveryTemplate.ShowAssessedValue).TryParseBool();

            _widgetCode = method.Params.ElementOrDefault(YandexDeliveryTemplate.WidgetCode);
        }


        private Dictionary<string, object> GetParam()
        {   
            var items = _preOrder.Items.Select(item => new Measure { XYZ = new[] { item.Height / 10, item.Width / 10, item.Length / 10 }, Amount = item.Amount }).ToList();
            var dimensions = MeasureHelper.GetDimensions(items);

            var length = (int)dimensions[0];
            var width = (int)dimensions[1];
            var height = (int)dimensions[2];
            var totalPrice = _preOrder.Items.Sum(item => item.Price * item.Amount);
            var weight = _preOrder.Items.Sum(item => item.Weight * item.Amount);
            if (weight == 0)
                weight = _defaultWeight;

            var data = new Dictionary<string, object>()
            {
                {"client_id", _clientId},
                {"sender_id", _senderId},
                {"city_from", _cityFrom},
                {"city_to", _preOrder.CityDest},
                {"height", height.ToString("F2", CultureInfo.InvariantCulture)},
                {"width", width.ToString("F2", CultureInfo.InvariantCulture)},
                {"length", length.ToString("F2", CultureInfo.InvariantCulture)},
                {"weight", weight.ToString("F2", CultureInfo.InvariantCulture)},
                {"total_cost", totalPrice.ToString("F2",CultureInfo.InvariantCulture)},
                {"order_cost", totalPrice.ToString("F2",CultureInfo.InvariantCulture)}
            };

            if (_showAssessedValue)
                data.Add("assessed_value", totalPrice.ToString("F2", CultureInfo.InvariantCulture));

            if (!string.IsNullOrWhiteSpace(_preOrder.ZipDest))
                data.Add("index_city", _preOrder.ZipDest);

            data.Add("secret_key", YandexDeliveryService.GetSign(data, _secretKeyDelivery));
            return data;
        }

        protected override IEnumerable<BaseShippingOption> CalcOptions()
        {
            var shippingOptions = new List<BaseShippingOption>();

            var postData = GetParam();
            var postDataString = postData.Keys.Aggregate("", (current, t) => current + string.Format("&{0}={1}", t, HttpUtility.UrlEncode(postData[t].ToString())));

            var responseData = YandexDeliveryService.MakeRequest(Url + "/searchDeliveryList", postDataString);

            if (!string.IsNullOrEmpty(responseData))
            {
				try
				{
					responseData = responseData.Replace("settings\":[]", "settings\":{}");
					var dto = JsonConvert.DeserializeObject<YandexDeliveryListDto>(responseData);
					if (dto.Status == "ok")
					{
						var onlypicks = dto.Data.Where(x => x.Type == "PICKUP" && x.PickupPoints != null && x.PickupPoints.Count > 0).ToList();
						if (onlypicks.Any())
						{
							var dimensions =
								_preOrder.Items
											.Select(item => new[] { item.Height / 10, item.Width / 10, item.Length / 10, item.Amount })
											.Aggregate("", (current, item) => current + ((current.IsNotEmpty() ? "," : "") + string.Format("[{0}, {1}, {2}, {3}]", item[0].ToString("F2", CultureInfo.InvariantCulture), item[1].ToString("F2", CultureInfo.InvariantCulture), item[2].ToString("F2", CultureInfo.InvariantCulture), item[3].ToString("F2", CultureInfo.InvariantCulture))));

							var amount = _preOrder.Items.Sum(x => x.Amount).ToString("F2", CultureInfo.InvariantCulture);

                            // костыль для модуля покупка на маркете, каждая доставка самовывоза - отдельно
                            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Url.ToString().Contains("/api/"))
                            {
                                foreach (var pointDelivery in dto.Data.Where(x => x.Type == "PICKUP" && x.PickupPoints != null && x.PickupPoints.Count == 0))
                                {
                                    shippingOptions.Add(new YandexDeliveryPickupPointOption(_method, new List<YandexDeliveryListItem>() { pointDelivery })
                                    {
                                        Name = pointDelivery.Delivery != null && pointDelivery.Delivery.name.IsNotEmpty() ? "Доставка в пункт выдачи " + pointDelivery.Delivery.name : "Доставка в пункт выдачи (" + pointDelivery.TariffName + ")",
                                        Weight = (string)postData["weight"],
                                        Cost = (string)postData["total_cost"],
                                        Dimensions = "[" + dimensions + "]",
                                        Amount = amount,
                                        Rate = pointDelivery.CostWithRules
                                    });
                                }
                            }
                            // обычная логика, группируем доствки самовывоза
                            else
                            {
                                shippingOptions.Add(new YandexDeliveryPickupPointOption(_method, dto.Data)
                                {
                                    WidgetCodeYa = _widgetCode,
                                    ShowAssessedValue = _showAssessedValue,
                                    Weight = (string)postData["weight"],
                                    Cost = (string)postData["total_cost"],
                                    Dimensions = "[" + dimensions + "]",
                                    Amount = amount,
                                    Rate = onlypicks.Min(x => x.CostWithRules),
                                });
                            }
						}

                        var notpicks = dto.Data.Where(x => x.Type != "PICKUP" && (x.PickupPoints == null || x.PickupPoints.Count == 0)).ToList();
                        if (notpicks.Any())
						{
							shippingOptions.AddRange(notpicks.Select(item =>
													item.Settings != null && item.Settings.ToYDWarehouse == "1"
													? new YandexDeliveryPickupWarehouseOption(_method, item)
													: new YandexDeliveryOption(_method, item)));
						}
					}
				}
				catch (Exception ex)
				{
					Debug.Log.Error(responseData, ex);
				}
            }

            return shippingOptions;
        }

        /// <summary>
        /// Создание заказа
        /// </summary>
        public bool CreateOrder(Order order)
        {
            DefaultIfNotSet();

            var items = _preOrder.Items.Select(item => new Measure { XYZ = new[] { item.Height / 10, item.Width / 10, item.Length / 10 }, Amount = item.Amount }).ToList();
            var dimensions = MeasureHelper.GetDimensions(items);

            var length = (int)dimensions[0];
            var width = (int)dimensions[1];
            var height = (int)dimensions[2];
            var weight = _preOrder.Items.Sum(item => item.Weight * item.Amount);
            if (weight == 0) 
                weight = _defaultWeight;

            // Если есть скидки, наценки и тд включаем их в стоимость товаров
            var extraPrice = order.Sum - order.ShippingCost - order.OrderItems.Sum(x => x.Amount*x.Price);
            if (extraPrice != 0)
            {
                var firstItem = order.OrderItems.FirstOrDefault();
                if (firstItem != null && firstItem.Price > extraPrice)
                {
                    firstItem.Price += extraPrice/firstItem.Amount;
                }
                else
                {
                    foreach (var item in order.OrderItems)
                    {
                        var percent = item.Price * item.Amount * 100 / extraPrice;
                        
                        item.Price += percent*extraPrice/(100*item.Amount);
                    }
                }
            }

            foreach (var item in order.OrderItems)
            {
                item.Price = (float)Math.Round(item.Price, 0);
            }

            YandexDeliveryAdditionalData additionalData = null;
            try
            {
                if (order.OrderPickPoint != null && order.OrderPickPoint.AdditionalData.IsNotEmpty())
                {
                    additionalData =
                        JsonConvert.DeserializeObject<YandexDeliveryAdditionalData>(order.OrderPickPoint.AdditionalData);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            
            var data = new Dictionary<string, object>()
            {
                {"order_date", order.OrderDate.ToUniversalTime()},

                {"order_weight", weight.ToString("F2").Replace(",", ".")},
                {"order_height", height.ToString("F2", CultureInfo.InvariantCulture)},
                {"order_width", width.ToString("F2", CultureInfo.InvariantCulture)},
                {"order_length", length.ToString("F2", CultureInfo.InvariantCulture)},

                {"order_amount_prepaid", order.Payed ?  order.Sum.ToString("F2", CultureInfo.InvariantCulture) : "0"},
                {"is_fully_prepaid", order.Payed ?  "1": "0"},
                {"order_delivery_cost", order.ShippingCost.ToString("F2", CultureInfo.InvariantCulture)},
                {"is_manual_delivery_cost", "1"},

                {"order_assessed_value", order.OrderItems.Sum(x => x.Amount*x.Price).ToString("F2", CultureInfo.InvariantCulture)},
                
                {"order_requisite", _requisiteId},
                {"order_warehouse", _warehousId},

                {"recipient[first_name]", order.OrderCustomer.FirstName},
                {"recipient[last_name]", order.OrderCustomer.LastName ?? ""},
                {"recipient[middle_name]", order.OrderCustomer.Patronymic ?? ""},
                {"recipient[phone]", order.OrderCustomer.Phone.Replace("+", "").Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "")},
                {"recipient[email]", order.OrderCustomer.Email},
                {"order_comment", order.CustomerComment},


                {"delivery[pickuppoint]", order.OrderPickPoint != null ? order.OrderPickPoint.PickPointId : null},

                {"delivery[tariff]", additionalData != null ? additionalData.tariffId : (order.OrderPickPoint != null ? order.OrderPickPoint.AdditionalData.TryParseInt() : 0)},
                
                {"deliverypoint[city]", order.OrderCustomer.City},
                {"deliverypoint[index]", order.OrderCustomer.Zip},
                {"deliverypoint[street]", order.OrderCustomer.GetCustomerAddress()},

                {"order_num", order.OrderID},
                
                {"client_id", _clientId},
                {"sender_id", _senderId},
            };

            if (additionalData != null)
            {
                data.Add("delivery[direction]", additionalData.direction);
                data.Add("delivery[delivery]", additionalData.delivery);
                //data.Add("delivery_price", additionalData.price);

                if (additionalData.to_ms_warehouse != null)
                    data.Add("delivery[to_yd_warehouse]", (int)additionalData.to_ms_warehouse);
            }

            var dict2 = new Dictionary<string, object>(data) {{"order_items", order.OrderItems.ToArray()}};
            var sign = YandexDeliveryService.GetSign(dict2, _secretKeyCreateOrder);


            var postDataString = String.Join("&", data.Where(x=> x.Value !=null).Select(x => HttpUtility.UrlEncode(x.Key) + "=" + HttpUtility.UrlEncode(x.Value.ToString()))) +
                                 "&" + YandexDeliveryService.GetOrderItems(order.OrderItems) +
                                 "&secret_key=" + sign;

            var resultData = YandexDeliveryService.MakeRequest(Url + "/createOrder", postDataString);
            
            if (string.IsNullOrEmpty(resultData) || !resultData.Contains("\"status\":\"ok\""))
            {
                Debug.Log.Error("Can't create order in yandex delivery: " + resultData);
                return false;
            }

            return true;
        }
    }
}
