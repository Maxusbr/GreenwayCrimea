//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Linq;
using System.Collections.Generic;

using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Shipping.Boxberry;
using AdvantShop.Orders;
using AdvantShop.Repository;
using System;

namespace AdvantShop.Shipping.Boxberry
{
    [ShippingKey("Boxberry")]
    public class Boxberry : BaseShippingWithCargo
    {
        private readonly string _token;
        private readonly string _integrationToken;
        private readonly string _receptionPointCode;
        private readonly bool _calculateCourier;

        private readonly BoxberryApiService _boxberryApiService;

        public Boxberry(ShippingMethod method, PreOrder preOrder)
            : base(method, preOrder)
        {
            _token = _method.Params.ElementOrDefault(BoxberryTemplate.Token);
            _integrationToken = _method.Params.ElementOrDefault(BoxberryTemplate.IntegrationToken);
            _receptionPointCode = _method.Params.ElementOrDefault(BoxberryTemplate.ReceptionPointCode);
            _calculateCourier = Convert.ToBoolean(_method.Params.ElementOrDefault(BoxberryTemplate.CalculateCourier));
            _boxberryApiService = new BoxberryApiService(_token, _receptionPointCode, _defaultWeight, _defaultHeight, _defaultWidth, _defaultLength);
        }

        protected override IEnumerable<BaseShippingOption> CalcOptions()
        {
            var cities = _boxberryApiService.GetListCities();

            var orderPrice = _preOrder.Items.Sum(item => item.Price * item.Amount) - _preOrder.TotalDiscount;
            var orderWeight = _preOrder.Items.Sum(item => item.Weight == 0 ? _defaultWeight * 1000 * item.Amount : item.Weight * 1000 * item.Amount);

            var boxberryOptions = _boxberryApiService.GetBoxberryOptions();
            var hideDeliveryTime = false;
            if (boxberryOptions != null && boxberryOptions.Result != null && boxberryOptions.Result.Settings3 != null)
            {
                hideDeliveryTime = boxberryOptions.Result.Settings3.HideDeliveryDay == 1;
            }

            var result = new List<BaseShippingOption>();

            if (!string.IsNullOrEmpty(_preOrder.CityDest))
            {
                var boxberryCity = cities.FirstOrDefault(item => item.Name == _preOrder.CityDest.Replace("ё", "е") && _preOrder.RegionDest.Contains(item.Region));

                if (boxberryCity == null)
                {
                    boxberryCity = cities.FirstOrDefault(item => item.Name == _preOrder.CityDest.Replace("ё", "е"));
                }

                if (boxberryCity != null)
                {
                    var optionWidget = GetShippingWidgentOption(boxberryCity, orderWeight, orderPrice, hideDeliveryTime);
                    if (optionWidget != null)
                    {
                        result.Add(optionWidget);
                    }

                    //var optionPoint = GetShippingOptionWithPoints(boxberryCity, orderWeight, orderPrice,hideDeliveryTime);
                    //if (optionPoint != null)
                    //{
                    //    result.Add(optionPoint);
                    //}
                }
            }
            if (_calculateCourier)
            {
                var optionZip = GetShippingOptionZip(orderWeight, orderPrice, hideDeliveryTime);
                if (optionZip != null)
                {
                    result.Add(optionZip);
                }
            }

            return result;
        }

        /*
         *boxberry.open(‘callback_function’,‘api_token’,‘custom_city’,’target_start’,’ordersum’,’weight’,’paysum’,’height’,’width’,’depth’)
         */
        private BoxberryWidgetOption GetShippingWidgentOption(BoxberryCity boxberryCity, float totalWeight, float totalPrice, bool hideDeliveryTime)
        {
            var points = _boxberryApiService.GetListPoints(boxberryCity.Code).ToList();
            if (points == null || points.Count == 0 ||
                (points != null && points.Count > 0 && !string.IsNullOrEmpty(points[0].Error)))
            {
                return null;
            }

            var items = _preOrder.Items.Select(item => new Measure
            {
                XYZ = new[] {
                    item.Height == 0 ?_defaultHeight / 10 : item.Height / 10,
                    item.Width == 0 ? _defaultWidth /10: item.Width / 10,
                    item.Length == 0 ? _defaultLength / 10 : item.Length / 10 },
                Amount = item.Amount
            }).ToList();
            var dimensions = MeasureHelper.GetDimensions(items);

            var deliveryCost = _boxberryApiService.GetDeliveryCosts(
                  points[0].Code,
                  totalWeight,
                  totalPrice,
                  0,
                  dimensions[0],
                  dimensions[1],
                  dimensions[2],
                  "",
                  totalPrice);

            return new BoxberryWidgetOption(_method)
            {
                Name = _method.Name + " (постоматы и пункты выдачи)",
                WidgetConfigData = new Dictionary<string, object> {
                    { "api_token", _integrationToken },
                    { "custom_city", boxberryCity.Name },
                    { "targetstart ", _receptionPointCode },
                    { "ordersum", totalPrice },
                    { "weight", totalWeight },
                    { "paysum", totalPrice },
                    { "height", dimensions[0] },
                    { "width", dimensions[1] },
                    { "depth", dimensions[2] }
                },
                DeliveryTime = hideDeliveryTime ? string.Empty : deliveryCost.DeliveryPeriod + "дн.",
                Rate = deliveryCost.Price,
                HideAddressBlock = true,
                DisplayIndex = false,
                DisplayCustomFields = false
            };
        }

        private BoxberryPointOption GetShippingOptionWithPoints(BoxberryCity boxberryCity, float totalWeight, float totalPrice, bool hideDeliveryTime)
        {
            var preorderOption = _preOrder.ShippingOption != null && _preOrder.ShippingOption.ShippingType == "Boxberry" && _preOrder.ShippingOption.GetType() == typeof(BoxberryPointOption)
                ? ((BoxberryPointOption)_preOrder.ShippingOption)
                : new BoxberryPointOption();
            var shippingPoints = new List<BoxberryPoint>();

            var selectedPoint = new BoxberryPoint();

            var items = _preOrder.Items.Select(item => new Measure
            {
                XYZ = new[] {
                    item.Height == 0 ?_defaultHeight / 10 : item.Height / 10,
                    item.Width == 0 ? _defaultWidth / 10: item.Width / 10,
                    item.Length == 0 ? _defaultLength / 10 : item.Length / 10 },
                Amount = item.Amount
            }).ToList();
            var dimensions = MeasureHelper.GetDimensions(items);

            var index = 0;
            foreach (var point in _boxberryApiService.GetListPoints(boxberryCity.Code).OrderBy(x => x.Name))
            {
                var boxberryPoint = new BoxberryPoint
                {
                    Id = point.Code.GetHashCode(),
                    Code = point.Code,
                    Address = point.Address,
                    Description = point.TripDescription,
                    DeliveryPeriod = point.DeliveryPeriod,
                    OnlyPrepaidOrders = point.OnlyPrepaidOrders == "Yes"
                };

                if ((preorderOption != null && preorderOption.SelectedPoint != null && preorderOption.SelectedPoint.Code == point.Code) ||
                    index == 0)
                {
                    var deliveryCost = _boxberryApiService.GetDeliveryCosts(
                        point.Code,
                        totalWeight,
                        totalPrice,
                        0, dimensions[0], dimensions[1], dimensions[2],
                        "",
                        0);

                    var deliveryCostCod = _boxberryApiService.GetDeliveryCosts(
                        point.Code,
                        totalWeight,
                        totalPrice,
                        0, dimensions[0], dimensions[1], dimensions[2],
                        "",
                        totalPrice);

                    if (deliveryCost != null)
                    {
                        boxberryPoint.BasePrice = deliveryCost.Price;
                        boxberryPoint.PriceCash = deliveryCostCod.Price;
                        boxberryPoint.DeliveryPeriod = deliveryCost.DeliveryPeriod.ToString();
                        selectedPoint = boxberryPoint;
                    }
                }

                shippingPoints.Add(boxberryPoint);
                index++;
            }

            var shippingOption = new BoxberryPointOption(_method)
            {
                Rate = selectedPoint.BasePrice,
                DeliveryTime = hideDeliveryTime ? string.Empty : selectedPoint.DeliveryPeriod + "дн.",
                DisplayCustomFields = false,
                ShippingPoints = shippingPoints,
                SelectedPoint = selectedPoint,
                DisplayIndex = true,
                HideAddressBlock = false
            };

            return shippingOption;
        }

        private BoxberryOption GetShippingOptionZip(float totalWeight, float totalPrice, bool hideDeliveryTime)
        {
            if (string.IsNullOrEmpty(_preOrder.ZipDest))
            {
                return null;
            }

            if (!_boxberryApiService.ZipCheck(_preOrder.ZipDest))
            {
                return null;
            }

            var cities = _boxberryApiService.GetCourierListCities();
            if (!cities.Any(item => item.City.ToLower() == _preOrder.CityDest.ToLower()))
            {
                return null;
            }

            var items = _preOrder.Items.Select(item => new Measure
            {
                XYZ = new[] {
                    item.Height == 0 ?_defaultHeight / 10:item.Height / 10,
                    item.Width == 0 ? _defaultWidth / 10 : item.Width / 10,
                    item.Length == 0 ? _defaultLength / 10 : item.Length / 10 },
                Amount = item.Amount
            }).ToList();
            var dimensions = MeasureHelper.GetDimensions(items);

            var deliveryCost = _boxberryApiService.GetDeliveryCosts(
                     string.Empty,
                     totalWeight,
                     totalPrice,
                     0, dimensions[0], dimensions[1], dimensions[2],
                     _preOrder.ZipDest,
                     0);

            var deliveryCostCash = _boxberryApiService.GetDeliveryCosts(
                 string.Empty,
                 totalWeight,
                 totalPrice,
                 0, dimensions[0], dimensions[1], dimensions[2],
                 _preOrder.ZipDest,
                 totalPrice);

            var shippingOption = new BoxberryOption(_method)
            {
                Name = _method.Name + "(курьером)",
                Rate = deliveryCost.Price,
                BasePrice = deliveryCost.Price,
                PriceCash = deliveryCostCash.Price,
                DeliveryTime = hideDeliveryTime ? string.Empty : deliveryCost.DeliveryPeriod + "дн.",
                DisplayIndex = true,
                DisplayCustomFields = false,
                HideAddressBlock = false
            };
            return shippingOption;
        }

        #region ApiMethods

        public BoxberryOrderTrackNumber CreateOrUpdateOrder(Order order)
        {
            return _boxberryApiService.ParselCreate(order);
        }

        public BoxberryOrderDeleteAnswer DeleteOrder(string trackNumber)
        {
            return _boxberryApiService.ParselDelete(trackNumber);
        }

        //protected override int GetHashForCache()
        //{
        //    var totalPrice = _items.Sum(item => item.Price * item.Amount);
        //    var str =
        //        string.Format(
        //            "checkout/calculation?ClientId={0}&CityDest={1}&RegionDest={2}&ZipDest={3}&totalSum={4}&assessedSum={5}&totalWeight={6}&itemsCount={7}&paymentMethod=cash",
        //            _token,
        //            _preOrder.CityDest,
        //            _preOrder.RegionDest,
        //            _preOrder.ZipDest,
        //            _preOrder.
        //            Math.Ceiling(totalPrice),
        //            Math.Ceiling(totalPrice),
        //            _items.Sum(item => item.Weight * item.Amount).ToString().Replace(",", "."),
        //            Math.Ceiling(_items.Sum(x => x.Amount)));
        //    var hash = _method.ShippingMethodId ^ str.GetHashCode();
        //    return hash;
        //}

        #endregion

    }
}