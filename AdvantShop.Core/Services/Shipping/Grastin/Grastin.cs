//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Shipping.Grastin;
using AdvantShop.Core.Services.Shipping.Grastin.Api;

namespace AdvantShop.Shipping.Grastin
{
    [ShippingKey("Grastin")]
    public class Grastin : BaseShippingWithWeight
    {
        private readonly string _widgetFromCity;
        private readonly bool _widgetFromCityHide;
        private readonly bool _widgetFromCityNoChange;
        private readonly string _widgetHidePartners;
        private readonly bool _widgetHideCost;
        private readonly bool _widgetHideDuration;
        private readonly string _widgetExtrachargeTypen;
        private readonly float _widgetExtracharge;
        private readonly int _widgetAddDuration;
        private readonly string _widgetHidePartnersJson;
        private readonly string _apiKey;
        private readonly string _orderPrefix;
        private readonly EnCourierService _typePaymentDelivery;
        private readonly EnCourierService _typePaymentPickup;
        private readonly EnTypeCalc _typeCalc;
        private readonly List<EnTypeDelivery> _activeDeliveryTypes;
        private readonly bool _insure;
        private readonly bool _statusesSync;
        private readonly string _moscowRegionId;
        private readonly string _saintPetersburgRegionId;
        private readonly string _nizhnyNovgorodRegionId;
        private readonly string _orelRegionId;
        private readonly string _boxberryRegionId;

        private readonly GrastinApiService _grastinApiService;
        private readonly List<EnTypeContract> _activeContracts;
        private readonly Dictionary<EnTypeContract, string> _grastinRegionIds;

        public Grastin(ShippingMethod method, PreOrder preOrder) : base(method, preOrder)
        {
            _widgetFromCity = method.Params.ElementOrDefault(GrastinTemplate.WidgetFromCity);
            _widgetFromCityHide = method.Params.ElementOrDefault(GrastinTemplate.WidgetFromCityHide).TryParseBool();
            _widgetFromCityNoChange = method.Params.ElementOrDefault(GrastinTemplate.WidgetFromCityNoChange).TryParseBool();
            _widgetHidePartners = method.Params.ElementOrDefault(GrastinTemplate.WidgetHidePartners);
            _widgetHideCost = method.Params.ElementOrDefault(GrastinTemplate.WidgetHideCost).TryParseBool();
            _widgetHideDuration = method.Params.ElementOrDefault(GrastinTemplate.WidgetHideDuration).TryParseBool();
            _widgetExtrachargeTypen = method.Params.ElementOrDefault(GrastinTemplate.WidgetExtrachargeTypen);
            _widgetExtracharge = method.Params.ElementOrDefault(GrastinTemplate.WidgetExtracharge).TryParseFloat();
            _widgetAddDuration = method.Params.ElementOrDefault(GrastinTemplate.WidgetAddDuration).TryParseInt();
            _widgetHidePartnersJson = method.Params.ElementOrDefault(GrastinTemplate.WidgetHidePartnersJson);
            _apiKey = method.Params.ElementOrDefault(GrastinTemplate.ApiKey);
            _orderPrefix = method.Params.ElementOrDefault(GrastinTemplate.OrderPrefix);
            _typePaymentDelivery = (EnCourierService)method.Params.ElementOrDefault(GrastinTemplate.TypePaymentDelivery).TryParseInt();
            _typePaymentPickup = (EnCourierService)method.Params.ElementOrDefault(GrastinTemplate.TypePaymentPickup).TryParseInt();
            _typeCalc = (EnTypeCalc)method.Params.ElementOrDefault(GrastinTemplate.TypeCalc).TryParseInt();
            _activeDeliveryTypes = (method.Params.ElementOrDefault(GrastinTemplate.ActiveDeliveryTypes) ?? string.Empty).Split(",").Select(x => x.TryParseInt()).Cast<EnTypeDelivery>().ToList();
            _insure = method.Params.ElementOrDefault(GrastinTemplate.Insure).TryParseBool();
            _statusesSync = method.Params.ElementOrDefault(GrastinTemplate.StatusesSync).TryParseBool();
            _moscowRegionId = method.Params.ElementOrDefault(GrastinTemplate.MoscowRegionId);
            _saintPetersburgRegionId = method.Params.ElementOrDefault(GrastinTemplate.SaintPetersburgRegionId);
            _nizhnyNovgorodRegionId = method.Params.ElementOrDefault(GrastinTemplate.NizhnyNovgorodRegionId);
            _orelRegionId = method.Params.ElementOrDefault(GrastinTemplate.OrelRegionId);
            _boxberryRegionId = method.Params.ElementOrDefault(GrastinTemplate.BoxberryRegionId);

            _activeContracts = new List<EnTypeContract>();
            if (!string.IsNullOrEmpty(_moscowRegionId))
                _activeContracts.Add(EnTypeContract.Moscow);
            if (!string.IsNullOrEmpty(_saintPetersburgRegionId))
                _activeContracts.Add(EnTypeContract.SaintPetersburg);
            if (!string.IsNullOrEmpty(_nizhnyNovgorodRegionId))
                _activeContracts.Add(EnTypeContract.NizhnyNovgorod);
            if (!string.IsNullOrEmpty(_orelRegionId))
                _activeContracts.Add(EnTypeContract.Orel);
            if (!string.IsNullOrEmpty(_boxberryRegionId))
                _activeContracts.Add(EnTypeContract.Boxberry);

            _grastinApiService = new GrastinApiService(_apiKey);
            _grastinRegionIds = new Dictionary<EnTypeContract, string>
            {
                {EnTypeContract.Moscow, _moscowRegionId },
                {EnTypeContract.SaintPetersburg, _saintPetersburgRegionId },
                {EnTypeContract.NizhnyNovgorod, _nizhnyNovgorodRegionId },
                {EnTypeContract.Orel, _orelRegionId },
            };
        }

        #region Properties

        public string ApiKey
        {
            get { return _apiKey; }
        }

        public string OrderPrefix
        {
            get { return _orderPrefix; }
        }

        public string WidgetFromCity
        {
            get { return _widgetFromCity; }
        }

        public bool Insure
        {
            get { return _insure; }
        }

        public EnCourierService TypePaymentDelivery
        {
            get { return _typePaymentDelivery; }
        }

        public EnCourierService TypePaymentPickup
        {
            get { return _typePaymentPickup; }
        }

        public bool StatusesSync
        {
            get { return _statusesSync; }
        }

        private Dictionary<string, int?> _statusesReference;
        public Dictionary<string, int?> StatusesReference
        {
            get
            {
                if (_statusesReference == null)
                {
                    _statusesReference = new Dictionary<string, int?>
                    {
                        { "draft", _method.Params.ElementOrDefault(GrastinTemplate.StatusDraft).TryParseInt(true)},
                        { "new", _method.Params.ElementOrDefault(GrastinTemplate.StatusNew).TryParseInt(true)},
                        { "return", _method.Params.ElementOrDefault(GrastinTemplate.StatusReturn).TryParseInt(true)},
                        { "done", _method.Params.ElementOrDefault(GrastinTemplate.StatusDone).TryParseInt(true)},
                        { "shipping", _method.Params.ElementOrDefault(GrastinTemplate.StatusShipping).TryParseInt(true)},
                        { "received", _method.Params.ElementOrDefault(GrastinTemplate.StatusReceived).TryParseInt(true)},
                        { "canceled", _method.Params.ElementOrDefault(GrastinTemplate.StatusCanceled).TryParseInt(true)},
                        { "prepared for shipment", _method.Params.ElementOrDefault(GrastinTemplate.StatusPreparedForShipment).TryParseInt(true)},
                        { "problem", _method.Params.ElementOrDefault(GrastinTemplate.StatusProblem).TryParseInt(true)},
                        { "returned to customer", _method.Params.ElementOrDefault(GrastinTemplate.StatusReturnedToCustomer).TryParseInt(true)},
                        { "decommissioned", _method.Params.ElementOrDefault(GrastinTemplate.StatusDecommissioned).TryParseInt(true)},
                    };
                }
                return _statusesReference;
            }
        }

        private void DeactivateContract(EnTypeContract typeContract)
        {
            // Отключаем метод, чтобы эконопить кол-во запросов (ограничение 10к в день),
            // для большой нагрузки это очень мало.

            _activeContracts.Remove(typeContract);

            var nameParam = string.Empty;

            if (typeContract == EnTypeContract.Moscow)
                nameParam = GrastinTemplate.MoscowRegionId;

            if (typeContract == EnTypeContract.SaintPetersburg)
                nameParam = GrastinTemplate.SaintPetersburgRegionId;

            if (typeContract == EnTypeContract.NizhnyNovgorod)
                nameParam = GrastinTemplate.NizhnyNovgorodRegionId;

            if (typeContract == EnTypeContract.Orel)
                nameParam = GrastinTemplate.OrelRegionId;

            if (typeContract == EnTypeContract.Boxberry)
                nameParam = GrastinTemplate.BoxberryRegionId;

            if (!string.IsNullOrEmpty(nameParam))
                ShippingMethodService.UpdateShippingParams(_method.ShippingMethodId, new Dictionary<string, string>()
                {
                    {
                        nameParam,
                        string.Empty
                    }
                });
        }

        #endregion

        protected override IEnumerable<BaseShippingOption> CalcOptions()
        {
            var shippingOptions = new List<BaseShippingOption>();

            int weight = (int)_preOrder.Items.Sum(item => item.Weight * 1000 * item.Amount);

            if (_typeCalc == EnTypeCalc.Widget)
            {

                // при весе больше 25 виджет падает
                if (weight < 25000)
                {
                    shippingOptions.Add(new GrastinWidgetOption(_method)
                    {
                        WidgetConfigData = GetConfig(),
                    });
                }
            }
            else if (_typeCalc == EnTypeCalc.Api)
            {
                var orderCost = _preOrder.Items.Sum(item => item.Price * item.Amount) - _preOrder.TotalDiscount;

                if (!string.IsNullOrWhiteSpace(_preOrder.CityDest))
                {
                    var isMoscow = _preOrder.CityDest.Equals("москва", StringComparison.OrdinalIgnoreCase) ||
                                   (!string.IsNullOrWhiteSpace(_preOrder.RegionDest) &&
                                    (_preOrder.RegionDest.Equals("москва", StringComparison.OrdinalIgnoreCase) ||
                                     _preOrder.RegionDest.Equals("московская область", StringComparison.OrdinalIgnoreCase)));

                    var isSaintPetersburg =
                        _preOrder.CityDest.Equals("санкт-петербург", StringComparison.OrdinalIgnoreCase) ||
                        (!string.IsNullOrWhiteSpace(_preOrder.RegionDest) &&
                         (_preOrder.RegionDest.Equals("санкт-петербург", StringComparison.OrdinalIgnoreCase)));

                    var isNizhnyNovgorod = _preOrder.CityDest.Equals("нижний новгород", StringComparison.OrdinalIgnoreCase);
                    var isOrel = _preOrder.CityDest.Equals("орёл", StringComparison.OrdinalIgnoreCase) || _preOrder.CityDest.Equals("орел", StringComparison.OrdinalIgnoreCase);

                    if (isMoscow || isSaintPetersburg  || isNizhnyNovgorod || isOrel)
                    {
                        EnTypeContract typeContract =
                            isMoscow
                                ? EnTypeContract.Moscow
                                : isSaintPetersburg
                                    ? EnTypeContract.SaintPetersburg
                                    : isNizhnyNovgorod
                                        ? EnTypeContract.NizhnyNovgorod
                                        : EnTypeContract.Orel;

                        if (_activeContracts.Contains(typeContract))
                        {
                            if (_activeDeliveryTypes.Contains(EnTypeDelivery.Pickpoint))
                                shippingOptions.AddRange(GetShippingOptionsWithPoints(typeContract, orderCost, weight, _preOrder.CityDest, _insure));

                            if (_activeDeliveryTypes.Contains(EnTypeDelivery.Courier))
                                shippingOptions.AddRange(GetShippingOptions(typeContract, orderCost, weight, _preOrder.CityDest, _preOrder.ZipDest, _insure));
                        }
                    }

                    if (_activeContracts.Contains(EnTypeContract.Boxberry))
                    {
                        if (_activeDeliveryTypes.Contains(EnTypeDelivery.Pickpoint))
                            shippingOptions.AddRange(GetShippingOptionsWithPoints(EnTypeContract.Boxberry, orderCost, weight, _preOrder.CityDest, _insure));

                        if (_activeDeliveryTypes.Contains(EnTypeDelivery.Courier))
                            shippingOptions.AddRange(GetShippingOptions(EnTypeContract.Boxberry, orderCost, weight, _preOrder.CityDest, _preOrder.ZipDest, _insure));
                    }
                }
            }

            return shippingOptions;
        }

        #region Without points

        private List<GrastinOption> GetShippingOptions(EnTypeContract typeContract, float orderCost, int weight, string cityDest, string zipDest, bool insure)
        {
            var list = new List<GrastinOption>();

            if (_grastinRegionIds.ContainsKey(typeContract))
            {
                list.AddRange(GetGrastinOptions(typeContract, orderCost, weight, cityDest, insure));
            }
            else if (typeContract == EnTypeContract.Boxberry)
            {
                list.AddRange(GetBoxberryOptions(orderCost, weight, cityDest, zipDest, insure));
            }

            return list;
        }

        private List<GrastinOption> GetBoxberryOptions(float orderCost, int weight, string cityDest, string zipDest, bool insure)
        {
            var list = new List<GrastinOption>();

            if (!string.IsNullOrEmpty(_boxberryRegionId))
            {
                var postCodes = _grastinApiService.GetBoxberryPostCode(cityDest);

                if (postCodes != null && postCodes.Count > 0)
                {
                    var selectedPostCode = postCodes.FirstOrDefault(x => zipDest != null && x.Name.StartsWith(zipDest)) ?? postCodes[0];

                    var deliveryCost = CalcShipingCost(_boxberryRegionId, orderCost, orderCost, weight, false, null, selectedPostCode.Id);

                    if (deliveryCost != null && deliveryCost[0].Status == "Ok" && deliveryCost[0].ShippingCost > 0)
                    {
                        var rate = GetDeliverySum(deliveryCost, insure);
                        list.Add(new GrastinOption(_method)
                        {
                            Name = _method.Name + " (Курьерская доставка Boxberry)",
                            Rate = rate,
                            BasePrice = rate,
                            PriceCash = GetDeliverySum(deliveryCost, insure, true),
                            PickpointAdditionalData = new GrastinEventWidgetData
                            {
                                DeliveryType = EnDeliveryType.Courier,
                                CityFrom = _widgetFromCity,
                                CityTo = cityDest,
                                Cost = rate,
                                Partner = EnPartner.Boxberry,
                                PickPointId = string.Empty
                            },
                        });
                    }
                    else if (deliveryCost != null &&
                             deliveryCost[0].Error == "Contract for the delivery region not found")
                    {
                        DeactivateContract(EnTypeContract.Boxberry);
                    }
                }
            }

            return list;
        }

        private List<GrastinOption> GetGrastinOptions(EnTypeContract typeContract, float orderCost, int weight, string cityDest, bool insure)
        {
            var list = new List<GrastinOption>();

            var deliveryCost = CalcShipingCost(_grastinRegionIds[typeContract], orderCost, orderCost, weight, false);

            if (deliveryCost != null && deliveryCost[0].Status == "Ok" && deliveryCost[0].ShippingCost > 0)
            {
                var rate = GetDeliverySum(deliveryCost, insure);
                list.Add(new GrastinOption(_method)
                {
                    Name = _method.Name + " (Курьерская доставка Грастин)",
                    Rate = rate,
                    BasePrice = rate,
                    PriceCash = GetDeliverySum(deliveryCost, insure, true),
                    PickpointAdditionalData = new GrastinEventWidgetData
                    {
                        DeliveryType = EnDeliveryType.Courier,
                        CityFrom = _widgetFromCity,
                        CityTo = cityDest,
                        Cost = rate,
                        Partner = EnPartner.Grastin,
                        PickPointId = string.Empty
                    },
                });

            }
            else if (deliveryCost != null && deliveryCost[0].Error == "Contract for the delivery region not found")
                DeactivateContract(typeContract);

            return list;
        }

        #endregion

        #region With points

        private List<GrastinPointOption> GetShippingOptionsWithPoints(EnTypeContract typeContract, float orderCost, int weight, string cityDest, bool insure)
        {
            var list = new List<GrastinPointOption>();

            var preorderOption = _preOrder.ShippingOption != null &&
                _preOrder.ShippingOption.ShippingType == ((ShippingKeyAttribute)typeof (Grastin).GetCustomAttributes(typeof (ShippingKeyAttribute), false).First()).Value &&
                _preOrder.ShippingOption.GetType() == typeof (GrastinPointOption)
                    ? ((GrastinPointOption) _preOrder.ShippingOption)
                    : new GrastinPointOption();

            if (_grastinRegionIds.ContainsKey(typeContract))
            {
                list.AddRange(GetGrastinOptionsWithPoints(typeContract, orderCost, weight, cityDest, preorderOption, insure));
            }
            else if (typeContract == EnTypeContract.Boxberry)
            {
                list.AddRange(GetBoxberryOptionsWithPoints(orderCost, weight, cityDest, preorderOption, insure));
            }

            return list;
        }

        private List<GrastinPointOption> GetBoxberryOptionsWithPoints(float orderCost, int weight, string cityDest, GrastinPointOption preorderOption, bool insure)
        {
            var list = new List<GrastinPointOption>();

            if (!string.IsNullOrEmpty(_boxberryRegionId))
            {
                var points =
                    _grastinApiService.GetBoxberrySelfPickup(cityDest)
                        .OrderBy(x => x.Name)
                        .ToList();

                if (points != null && points.Count > 0)
                {
                    var selectedPoint = preorderOption != null && preorderOption.SelectedPoint != null
                        ? points.FirstOrDefault(x => preorderOption.SelectedPoint.Code == x.Id) ?? points[0]
                        : points[0];

                    var deliveryCost = CalcShipingCost(_boxberryRegionId, orderCost, orderCost, weight, true, selectedPoint.Id);

                    if (deliveryCost != null && deliveryCost[0].Status == "Ok" && deliveryCost[0].ShippingCost > 0)
                    {
                        list.Add(GetBoxberryOptionWithPoints(cityDest, deliveryCost, points, selectedPoint, insure));
                    }
                    else if (deliveryCost != null &&
                             deliveryCost[0].Error == "Contract for the delivery region not found")
                    {
                        DeactivateContract(EnTypeContract.Boxberry);
                    }
                }
            }

            return list;
        }

        private GrastinPointOption GetBoxberryOptionWithPoints(string cityDest, List<CostResponse> deliveryCost, List<SelfpickupBoxberry> points, SelfpickupBoxberry selectedPoint, bool insure)
        {
            var rate = GetDeliverySum(deliveryCost, insure);

            var shippingPoints = new List<GrastinPoint>();
            var selectedGrastinPoint = new GrastinPoint();

            foreach (var point in points)
            {
                var grastinPoint = new GrastinPoint()
                {
                    Id = point.Id.GetHashCode(),
                    Code = point.Id,
                    Address = point.Name,
                    Description = point.DrivingDescription,
                    Scheldule = point.Schedule,
                    DeliveryTime =
                        !string.IsNullOrWhiteSpace(point.DeliveryPeriod) ? string.Format("{0} д.", point.DeliveryPeriod) : null
                };

                if (selectedPoint == point)
                    selectedGrastinPoint = grastinPoint;

                shippingPoints.Add(grastinPoint);
            }

            var shippingOption = new GrastinPointOption(_method)
            {
                Name = _method.Name + " (Самовывоз Boxberry)",
                Rate = rate,
                BasePrice = rate,
                PriceCash = GetDeliverySum(deliveryCost, insure, true),
                DeliveryTime = selectedGrastinPoint.DeliveryTime,
                PickpointAdditionalData = new GrastinEventWidgetData
                {
                    DeliveryType = EnDeliveryType.PickPoint,
                    CityFrom = _widgetFromCity,
                    CityTo = cityDest,
                    Cost = rate,
                    Partner = EnPartner.Boxberry,
                    PickPointId = selectedGrastinPoint.Code
                },
                ShippingPoints = shippingPoints,
                SelectedPoint = selectedGrastinPoint
            };

            return shippingOption;
        }

        private List<GrastinPointOption> GetGrastinOptionsWithPoints(EnTypeContract typeContract, float orderCost, int weight, string cityDest, GrastinPointOption preorderOption, bool insure)
        {
            var list = new List<GrastinPointOption>();

            var deliveryCost = CalcShipingCost(_grastinRegionIds[typeContract], orderCost, orderCost, weight, true);

            if (deliveryCost != null && deliveryCost[0].Status == "Ok" && deliveryCost[0].ShippingCost > 0)
            {
                var points =
                    _grastinApiService.GetGrastinSelfPickups()
                        .Where(x =>
                            !string.IsNullOrWhiteSpace(x.City) &&
                            x.City.Equals(cityDest, StringComparison.OrdinalIgnoreCase))
                        .OrderBy(x => x.Name)
                        .ToList();

                if (points.Count > 0)
                    list.Add(GetGrastinOptionWithPoints(cityDest, preorderOption, deliveryCost, points, insure));

            }
            else if (deliveryCost != null && deliveryCost[0].Error == "Contract for the delivery region not found")
                DeactivateContract(typeContract);

            return list;
        }

        private GrastinPointOption GetGrastinOptionWithPoints(string cityDest, GrastinPointOption preorderOption, List<CostResponse> deliveryCost, List<Selfpickup> points, bool insure)
        {
            var rate = GetDeliverySum(deliveryCost, insure);

            var shippingPoints = new List<GrastinPoint>();
            var selectedGrastinPoint = new GrastinPoint();

            var selectedPoint = preorderOption != null && preorderOption.SelectedPoint != null
                ? points.FirstOrDefault(x => preorderOption.SelectedPoint.Code == x.Id) ?? points[0]
                : points[0];

            foreach (var point in points)
            {
                var grastinPoint = new GrastinPoint()
                {
                    Id = point.Id.GetHashCode(),
                    Code = point.Id,
                    Address = point.Name,
                    Description = point.DrivingDescription,
                    Phone = point.Phone,
                    Scheldule = point.TimeTable,
                    LinkDriving = point.LinkDrivingDescription
                };

                if (selectedPoint == point)
                    selectedGrastinPoint = grastinPoint;

                shippingPoints.Add(grastinPoint);
            }

            var shippingOption = new GrastinPointOption(_method)
            {
                Name = _method.Name + " (Самовывоз Грастин)",
                Rate = rate,
                BasePrice = rate,
                PriceCash = GetDeliverySum(deliveryCost, insure, true),
                PickpointAdditionalData = new GrastinEventWidgetData
                {
                    DeliveryType = EnDeliveryType.PickPoint,
                    CityFrom = _widgetFromCity,
                    CityTo = cityDest,
                    Cost = rate,
                    Partner = EnPartner.Grastin,
                    PickPointId = selectedGrastinPoint.Code
                },
                ShippingPoints = shippingPoints,
                SelectedPoint = selectedGrastinPoint
            };
            return shippingOption;
        }

        #endregion

        #region Help methods

        private List<CostResponse> CalcShipingCost(string regionId, float orderSum, float assessedCost, int weight, bool isSelfPickup, string boxberryPointId = null, string boxberryZipId = null)
        {
            var deliveryCost = _grastinApiService.CalcShipingCost(new CalcShipingCostContainer()
            {
                Orders = new List<CalcShipingCostOrder>()
                {
                    new CalcShipingCostOrder()
                    {
                        Number = "123",
                        RegionId = regionId,
                        Weight = weight,
                        OrderSum = orderSum,
                        AssessedCost = assessedCost,
                        IsSelfPickup = isSelfPickup,
                        PickupId = boxberryPointId,
                        PostcodeId = boxberryZipId
                    }
                }
            });
            return deliveryCost;
        }

        private static float GetDeliverySum(List<CostResponse> deliveryCost, bool withInsurance, bool cachOnDelivery = false)
        {
            var rate =
                deliveryCost[0].ShippingCost +
                deliveryCost[0].ShippingCostDistance +
                (cachOnDelivery ? deliveryCost[0].Commission : 0f) +
                (withInsurance ? deliveryCost[0].SafetyStock : 0f) +
                deliveryCost[0].AdditionalShippingCosts +
                deliveryCost[0].OrderProcessing;
            return rate;
        }

        private Dictionary<string, string> GetConfig()
        {
            var _widgetConfigData = new Dictionary<string, string>();

            _widgetConfigData.Add("data-no-weight", "1");

            if (!string.IsNullOrEmpty(_widgetFromCity))
                _widgetConfigData.Add("data-from-city", _widgetFromCity);

            if (_widgetFromCityHide)
                _widgetConfigData.Add("data-from-hide", "1");

            if (_widgetFromCityNoChange)
                _widgetConfigData.Add("data-from-single", "1");

            if (!string.IsNullOrEmpty(_widgetHidePartners))
                _widgetConfigData.Add("data-no-partners", _widgetHidePartners);

            if (_widgetHideCost)
                _widgetConfigData.Add("data-no-cost", "1");

            if (_widgetHideDuration)
                _widgetConfigData.Add("data-no-duration", "1");

            if (_widgetExtracharge > 0f)
                _widgetConfigData.Add("data-add-cost", string.Format("{0}{1}", _widgetExtracharge.ToString(CultureInfo.InvariantCulture), _widgetExtrachargeTypen));

            if (_widgetAddDuration > 0f)
                _widgetConfigData.Add("data-add-duration", _widgetAddDuration.ToString());

            var weight = _preOrder.Items.Sum(item => item.Weight * item.Amount);
            if (weight > 0f)
                _widgetConfigData.Add("data-weight-base", Math.Ceiling(weight).ToString(CultureInfo.InvariantCulture));

            if (!string.IsNullOrEmpty(_widgetHidePartnersJson))
                _widgetConfigData.Add("data-no-partners-obj", Uri.EscapeDataString(_widgetHidePartnersJson));

            return _widgetConfigData;
        }

        #endregion
    }

    public enum EnTypeCalc
    {
        /// <summary>
        /// Через Api
        /// </summary>
        [Localize("Через Api")]
        Api = 0,

        /// <summary>
        /// Через Api
        /// </summary>
        [Localize("Виджет")]
        Widget = 1,
    }

    public enum EnTypeContract
    {
        /// <summary>
        /// Грастин Москва
        /// </summary>
        [Localize("Грастин Москва")]
        Moscow = 0,

        /// <summary>
        /// Грастин Санкт-Петербург
        /// </summary>
        [Localize("Грастин Санкт-Петербург")]
        SaintPetersburg = 1,

        /// <summary>
        /// Грастин Нижний Новгород
        /// </summary>
        [Localize("Грастин Нижний Новгород")]
        NizhnyNovgorod = 2,

        /// <summary>
        /// Грастин Орёл
        /// </summary>
        [Localize("Грастин Орёл")]
        Orel = 3,

        /// <summary>
        /// Boxberry
        /// </summary>
        [Localize("Boxberry")]
        Boxberry = 30,

    }

    public enum EnTypeDelivery
    {
        /// <summary>
        /// Самовывоз
        /// </summary>
        [Localize("Самовывоз")]
        Pickpoint = 0,

        /// <summary>
        /// Курьер
        /// </summary>
        [Localize("Курьер")]
        Courier = 1,
    }
}
