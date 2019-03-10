//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Collections.Generic;

using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Shipping.DDelivery;
using AdvantShop.Repository;
using AdvantShop.Orders;

using Newtonsoft.Json;

namespace AdvantShop.Shipping.DDelivery
{
    [ShippingKey("DDelivery")]
    public class DDelivery : BaseShippingWithCargoAndCache
    {
        private readonly string _apiKey;
        private readonly string _shopId;
        private readonly string _receptionCompanyId;
        private readonly bool _сreateDraftOrder;
        private readonly bool _useWidget;

        private readonly DDeliveryApiService _dDeliveryApiService;

        public DDelivery(ShippingMethod method, PreOrder preOrder)
            : base(method, preOrder)
        {
            _apiKey = _method.Params.ElementOrDefault(DDeliveryTemplate.ApiKey);
            _shopId = _method.Params.ElementOrDefault(DDeliveryTemplate.ShopId);
            _receptionCompanyId = _method.Params.ElementOrDefault(DDeliveryTemplate.ReceptionCompanyId);
            _сreateDraftOrder = _method.Params.ElementOrDefault(DDeliveryTemplate.CreateDraftOrder).TryParseBool();
            _useWidget = _method.Params.ElementOrDefault(DDeliveryTemplate.UseWidget).TryParseBool();

            _dDeliveryApiService = new DDeliveryApiService(_apiKey, _shopId, _receptionCompanyId, _сreateDraftOrder, _useWidget, _defaultWeight, _defaultHeight, _defaultWidth, _defaultLength);
        }

        protected override IEnumerable<BaseShippingOption> CalcOptions()
        {
            var result = new List<BaseShippingOption>();

            var city = _dDeliveryApiService.GetCity(_preOrder.CityDest);

            var items = _preOrder.Items.Select(item => new Measure
            {
                XYZ = new[] {
                    item.Height == 0 ?_defaultHeight / 10 : item.Height / 10,
                    item.Width == 0 ? _defaultWidth / 10: item.Width / 10,
                    item.Length == 0 ? _defaultLength / 10 : item.Length / 10 },
                Amount = item.Amount
            }).ToList();
            var totalDimensions = MeasureHelper.GetDimensions(items);
            var totalWeight = _preOrder.Items.Sum(item => (item.Weight == 0 ? _defaultWeight : item.Weight) * item.Amount);

            var data = _dDeliveryApiService.CalculateDelivery(city.Id, totalDimensions, totalWeight);

            if (_useWidget)
            {
                var widgetOption = GetDDeliveryWidgetOption();
                if (widgetOption != null)
                {
                    if (data.Data!= null && data.Data.Pickup.Points!= null && data.Data.Pickup.Points.Any())
                    {
                        widgetOption.Rate = data.Data.Pickup.Points[0].PriceDelivery;                        
                    }
                    result.Add(widgetOption);
                }
            }
            else
            {
                var pointOptions = GetDeliveryOptionsWithPoints(data);
                if (pointOptions != null && pointOptions.Count > 0)
                {
                    result.AddRange(pointOptions);
                }
            }
                       
            var courierOptions = GetDeliveryOptionsCourier(data);
            if (courierOptions != null && courierOptions.Count > 0)
            {
                result.AddRange(courierOptions);
            }

            var postOptions = GetDeliveryOptionsPost(data);
            if (postOptions != null && postOptions.Count > 0)
            {
                result.AddRange(postOptions);
            }

            return result;
        }

        #region Get shipping options

        private List<DDeliveryPointOption> GetDeliveryOptionsWithPoints(DDeliveryObjectResponse<DDeliveryObjectCalculatorAnswer> deliveryData)
        {
            var options = new List<DDeliveryPointOption>();

            foreach (var deliveryCompany in deliveryData.Data.Pickup.Delivery)
            {
                var points = new List<DDeliveryPoint>();
                foreach (var ddeliveryPoint in deliveryData.Data.Pickup.Points.Where(item => item.DeliveryCompanyId == deliveryCompany.DeliveryCompanyId))
                {
                    points.Add(new DDeliveryPoint
                    {
                        Id = ddeliveryPoint.Id,
                        Address = ddeliveryPoint.Adress,
                        Code = ddeliveryPoint.Id.ToString(),
                        Description = ddeliveryPoint.DescriptionIn + "<br/>" + ddeliveryPoint.DescriptionOut,
                        Rate = ddeliveryPoint.PriceDelivery,
                        DeliveryDate = ddeliveryPoint.DeliveryDate,
                        DeliveryCompanyId = ddeliveryPoint.DeliveryCompanyId,
                        DeliveryTypeId = (int)EDeliveryType.Pickup
                    });
                }

                var option = new DDeliveryPointOption(_method)
                {
                    Name = deliveryCompany.DeliveryCompanyName,
                    DeliveryTime = deliveryCompany.DeliveryDays + "дн.",
                    Rate = deliveryCompany.TotalPrice,
                    DisplayIndex = false,
                    HideAddressBlock = false,
                    DisplayCustomFields = false,
                    ShippingPoints = points.OrderBy(item => item.Address).ToList(),
                    DeliveryTypeId = (int)EDeliveryType.Pickup,
                    IconName = ShippingIcons.GetShippingIcon(_method.ShippingType, _method.IconFileName.PhotoName, deliveryCompany.DeliveryCompanyName)
                };

                options.Add(option);
            }

            return options;
        }

        private List<DDeliveryOption> GetDeliveryOptionsCourier(DDeliveryObjectResponse<DDeliveryObjectCalculatorAnswer> deliveryData)
        {
            var options = new List<DDeliveryOption>();

            foreach (var deliveryCompany in deliveryData.Data.Courier.Delivery)
            {
                var shippingOption = new DDeliveryOption(_method)
                {
                    Name = deliveryCompany.DeliveryCompanyName + " (курьер)",
                    Rate = deliveryCompany.TotalPrice,
                    DeliveryTime = deliveryCompany.DeliveryDays + "дн.",
                    DisplayIndex = true,
                    DisplayCustomFields = false,
                    HideAddressBlock = false,
                    DeliveryCompanyId = deliveryCompany.DeliveryCompanyId,
                    DeliveryTypeId = (int)EDeliveryType.Courier,
                    IconName = ShippingIcons.GetShippingIcon(_method.ShippingType, _method.IconFileName.PhotoName, deliveryCompany.DeliveryCompanyName)
                };
                options.Add(shippingOption);
            }

            return options;
        }

        private List<DDeliveryOption> GetDeliveryOptionsPost(DDeliveryObjectResponse<DDeliveryObjectCalculatorAnswer> deliveryData)
        {
            var options = new List<DDeliveryOption>();

            foreach (var deliveryCompany in deliveryData.Data.Post.Delivery)
            {
                var shippingOption = new DDeliveryOption(_method)
                {
                    Name = deliveryCompany.DeliveryCompanyName + " (почта)",
                    Rate = deliveryCompany.TotalPrice,
                    DeliveryTime = deliveryCompany.DeliveryDays + "дн.",
                    DisplayIndex = true,
                    DisplayCustomFields = false,
                    HideAddressBlock = false,
                    DeliveryTypeId = (int)EDeliveryType.Post,
                    DeliveryCompanyId = deliveryCompany.DeliveryCompanyId,
                    IconName = ShippingIcons.GetShippingIcon(_method.ShippingType, _method.IconFileName.PhotoName, deliveryCompany.DeliveryCompanyName)
                };
                options.Add(shippingOption);
            }

            return options;
        }

        private DDeliveryWidgetOption GetDDeliveryWidgetOption()
        {
            var products = new List<DDeliveryObjectProduct>();
            foreach (var orderItem in _preOrder.Items)
            {
                products.Add(new DDeliveryObjectProduct
                {
                    Id = orderItem.Id.ToString(),
                    Sku = orderItem.Id.ToString(),
                    Name = orderItem.Name,
                    Price = orderItem.Price,
                    Weight = orderItem.Weight == 0 ? _defaultWeight : orderItem.Weight,
                    Width = orderItem.Width == 0 ? _defaultWidth / 10 : orderItem.Width / 10,
                    Height = orderItem.Height == 0 ? _defaultHeight / 10 : orderItem.Height / 10,
                    Length = orderItem.Length == 0 ? _defaultLength / 10 : orderItem.Length / 10,
                    Quantity = Convert.ToInt32(orderItem.Amount)
                });
            }

            return new DDeliveryWidgetOption(_method)
            {
                Name = _method.Name,
                WidgetConfigData = new DDeliveryObjectWidgetConfig
                {
                    Products = products,
                    Id = _shopId,
                    //Width = 500,
                    //Height = 550,
                    //Env = "DDeliveryWidget.ENV_PROD"
                }
            };
        }

        #endregion

        #region Api methods

        public DDeliveryObjectResponse<DDeliveryObjectNewOrder> CreateOrder(Order order)
        {
            var items = _preOrder.Items.Select(item => new Measure
            {
                XYZ = new[] {
                    item.Height == 0 ?_defaultHeight /10 : item.Height / 10,
                    item.Width == 0 ? _defaultWidth / 10: item.Width / 10,
                    item.Length == 0 ? _defaultLength / 10 : item.Length / 10 },
                Amount = item.Amount
            }).ToList();
            var totalDimensions = MeasureHelper.GetDimensions(items);

            return _dDeliveryApiService.CreateOrder(order, totalDimensions);
        }

        public DDeliveryObjectResponse<DDeliveryObjectOrderInfo> GetOrderInfo(string ddeliveryOrderId)
        {
            return _dDeliveryApiService.GetOrderInfo(ddeliveryOrderId);
        }

        public DDeliveryObjectResponse<object> CanselOrder(string ddeliveryOrderId)
        {
            return _dDeliveryApiService.CanselOrder(ddeliveryOrderId);
        }

        #endregion

        protected override int GetHashForCache()
        {
            var totalPrice = _preOrder.Items.Sum(item => item.Price * item.Amount);
            var str =
                string.Format(
                    "checkout/calculation?ClientId={0}&CityDest={1}&RegionDest={2}&totalSum={3}&assessedSum={4}&totalWeight={5}&itemsCount={6}&paymentMethod=cash",
                    _apiKey,
                    _preOrder.CityDest,
                    _preOrder.RegionDest,
                    Math.Ceiling(totalPrice),
                    Math.Ceiling(totalPrice),
                    _preOrder.Items.Sum(item => item.Weight * item.Amount).ToString().Replace(",", "."),
                    Math.Ceiling(_preOrder.Items.Sum(x => x.Amount)));
            var hash = _method.ShippingMethodId ^ str.GetHashCode();
            return hash;
        }
    }
}