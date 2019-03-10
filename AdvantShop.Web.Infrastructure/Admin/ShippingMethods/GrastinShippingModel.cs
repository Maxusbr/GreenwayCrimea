using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Shipping;
using AdvantShop.Core.Services.Shipping.Grastin.Api;
using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.Shipping.Grastin;

namespace AdvantShop.Web.Infrastructure.Admin.ShippingMethods
{
    public class GrastinShippingModel : ShippingMethodAdminModel, IValidatableObject
    {
        public string WidgetFromCity
        {
            get { return Params.ElementOrDefault(GrastinTemplate.WidgetFromCity); }
            set { Params.TryAddValue(GrastinTemplate.WidgetFromCity, value.DefaultOrEmpty()); }
        }

        public List<SelectListItem> ListCities
        {
            get
            {
                var cities = new List<SelectListItem>()
                {
                    new SelectListItem() {Text = "Москва", Value = "Москва"},
                    new SelectListItem() {Text = "Санкт-Петербург", Value = "Санкт-Петербург"},
                };

                var city = cities.Find(x => x.Value == WidgetFromCity);
                if (city != null)
                    city.Selected = true;

                return cities;
            }
        }

        public bool WidgetFromCityHide
        {
            get { return Params.ElementOrDefault(GrastinTemplate.WidgetFromCityHide).TryParseBool(); }
            set { Params.TryAddValue(GrastinTemplate.WidgetFromCityHide, value.ToString()); }
        }

        public bool WidgetFromCityNoChange
        {
            get { return Params.ElementOrDefault(GrastinTemplate.WidgetFromCityNoChange).TryParseBool(); }
            set { Params.TryAddValue(GrastinTemplate.WidgetFromCityNoChange, value.ToString()); }
        }

        public string[] WidgetHidePartners
        {
            get { return (Params.ElementOrDefault(GrastinTemplate.WidgetHidePartners) ?? string.Empty).Split(","); }
            set { Params.TryAddValue(GrastinTemplate.WidgetHidePartners, value != null ? string.Join(",", value) : string.Empty); }
        }

        public List<SelectListItem> ListPartners
        {
            get
            {
                var partners = new List<SelectListItem>()
                {
                    new SelectListItem() {Text = "Grastin", Value = "grastin"},
                    new SelectListItem() {Text = "Boxberry", Value = "boxberry"},
                    new SelectListItem() {Text = "Hermes", Value = "hermes"},
                    new SelectListItem() {Text = "DPD", Value = "dpd"},
                    new SelectListItem() {Text = "Почта России", Value = "post"},
                };

                partners.Where(x => WidgetHidePartners.Contains(x.Value)).ForEach(x => x.Selected = true);

                return partners;
            }
        }

        public bool WidgetHideCost
        {
            get { return Params.ElementOrDefault(GrastinTemplate.WidgetHideCost).TryParseBool(); }
            set { Params.TryAddValue(GrastinTemplate.WidgetHideCost, value.ToString()); }
        }

        public bool WidgetHideDuration
        {
            get { return Params.ElementOrDefault(GrastinTemplate.WidgetHideDuration).TryParseBool(); }
            set { Params.TryAddValue(GrastinTemplate.WidgetHideDuration, value.ToString()); }
        }

        public string WidgetExtrachargeTypen
        {
            get { return Params.ElementOrDefault(GrastinTemplate.WidgetExtrachargeTypen); }
            set { Params.TryAddValue(GrastinTemplate.WidgetExtrachargeTypen, value.DefaultOrEmpty()); }
        }

        public List<SelectListItem> ExtrachargeTypes
        {
            get
            {
                var types = new List<SelectListItem>()
                {
                    new SelectListItem() {Text = "%", Value = "%"},
                    new SelectListItem() {Text = "Руб", Value = "Руб"},
                };

                var type = types.Find(x => x.Value == WidgetExtrachargeTypen);
                if (type != null)
                    type.Selected = true;

                return types;
            }
        }

        public float WidgetExtracharge
        {
            get { return Params.ElementOrDefault(GrastinTemplate.WidgetExtracharge).TryParseFloat(); }
            set { Params.TryAddValue(GrastinTemplate.WidgetExtracharge, value.ToString()); }
        }

        public int WidgetAddDuration
        {
            get { return Params.ElementOrDefault(GrastinTemplate.WidgetAddDuration).TryParseInt(); }
            set { Params.TryAddValue(GrastinTemplate.WidgetAddDuration, value.ToString()); }
        }

        public string WidgetHidePartnersJson
        {
            get { return Params.ElementOrDefault(GrastinTemplate.WidgetHidePartnersJson); }
            set { Params.TryAddValue(GrastinTemplate.WidgetHidePartnersJson, value.DefaultOrEmpty()); }
        }

        public string ApiKey
        {
            get { return Params.ElementOrDefault(GrastinTemplate.ApiKey); }
            set { Params.TryAddValue(GrastinTemplate.ApiKey, value.DefaultOrEmpty()); }
        }

        public string OrderPrefix
        {
            get { return Params.ElementOrDefault(GrastinTemplate.OrderPrefix); }
            set { Params.TryAddValue(GrastinTemplate.OrderPrefix, value.DefaultOrEmpty()); }
        }

        public string TypePaymentDelivery
        {
            get { return Params.ElementOrDefault(GrastinTemplate.TypePaymentDelivery); }
            set { Params.TryAddValue(GrastinTemplate.TypePaymentDelivery, value.DefaultOrEmpty()); }
        }

        public List<SelectListItem> TypesPaymentDelivery
        {
            get
            {
                return (new[] { EnCourierService.DeliveryWithPayment, EnCourierService.DeliveryWithCashServices, EnCourierService.DeliverWithCreditCard, EnCourierService.GreatDeliveryWithPayment, EnCourierService.GreatDeliveryWithCashServices })
                    .Select(x => new SelectListItem()
                    {
                        Text = x.Localize(),
                        Value = ((int)x).ToString()
                    })
                    .ToList();
            }
        }

        public string TypePaymentPickup
        {
            get { return Params.ElementOrDefault(GrastinTemplate.TypePaymentPickup); }
            set { Params.TryAddValue(GrastinTemplate.TypePaymentPickup, value.DefaultOrEmpty()); }
        }

        public List<SelectListItem> TypesPaymentPickup
        {
            get
            {
                return (new[] { EnCourierService.PickupWithPayment, EnCourierService.PickupWithCashServices, EnCourierService.PickupWithCreditCard })
                    .Select(x => new SelectListItem()
                    {
                        Text = x.Localize(),
                        Value = ((int)x).ToString()
                    })
                    .ToList();
            }
        }

        public string TypeCalc
        {
            get { return Params.ElementOrDefault(GrastinTemplate.TypeCalc, ((int)EnTypeCalc.Api).ToString()); }
            set { Params.TryAddValue(GrastinTemplate.TypeCalc, value.DefaultOrEmpty()); }
        }

        public List<SelectListItem> ListTypesCalc
        {
            get
            {
                return Enum.GetValues(typeof (EnTypeCalc))
                    .Cast<EnTypeCalc>()
                    .Select(x => new SelectListItem()
                    {
                        Text = x.Localize(),
                        Value = ((int) x).ToString()
                    })
                    .ToList();
            }
        }

        public bool Insure
        {
            get { return Params.ElementOrDefault(GrastinTemplate.Insure).TryParseBool(); }
            set { Params.TryAddValue(GrastinTemplate.Insure, value.ToString()); }
        }

        public string MoscowRegionId
        {
            get { return Params.ElementOrDefault(GrastinTemplate.MoscowRegionId); }
            set { Params.TryAddValue(GrastinTemplate.MoscowRegionId, value.DefaultOrEmpty()); }
        }

        public string SaintPetersburgRegionId
        {
            get { return Params.ElementOrDefault(GrastinTemplate.SaintPetersburgRegionId); }
            set { Params.TryAddValue(GrastinTemplate.SaintPetersburgRegionId, value.DefaultOrEmpty()); }
        }

        public string NizhnyNovgorodRegionId
        {
            get { return Params.ElementOrDefault(GrastinTemplate.NizhnyNovgorodRegionId); }
            set { Params.TryAddValue(GrastinTemplate.NizhnyNovgorodRegionId, value.DefaultOrEmpty()); }
        }

        public string OrelRegionId
        {
            get { return Params.ElementOrDefault(GrastinTemplate.OrelRegionId); }
            set { Params.TryAddValue(GrastinTemplate.OrelRegionId, value.DefaultOrEmpty()); }
        }

        public string BoxberryRegionId
        {
            get { return Params.ElementOrDefault(GrastinTemplate.BoxberryRegionId); }
            set { Params.TryAddValue(GrastinTemplate.BoxberryRegionId, value.DefaultOrEmpty()); }
        }

        public List<SelectListItem> ListRegionIds
        {
            get
            {
                var listRegionIds = new List<SelectListItem>()
                {
                    new SelectListItem() {Text = "нет", Value = ""}
                };

                var apiKey = Params.ElementOrDefault(GrastinTemplate.ApiKey);
                if (!string.IsNullOrEmpty(apiKey))
                {
                    var grastinApiService = new GrastinApiService(apiKey);
                    var contracts = grastinApiService.GetContracts();
                    if (contracts != null)
                    {
                        listRegionIds.AddRange(contracts.Select(x => new SelectListItem() {Text = x.Name, Value = x.IdRegion}));
                    }
                }

                return listRegionIds;
            }
        }

        public string[] ActiveDeliveryTypes
        {
            get { return (Params.ElementOrDefault(GrastinTemplate.ActiveDeliveryTypes) ?? string.Empty).Split(","); }
            set { Params.TryAddValue(GrastinTemplate.ActiveDeliveryTypes, value != null ? string.Join(",", value) : string.Empty); }
        }

        public List<SelectListItem> ListDeliveryTypes
        {
            get
            {
                var partners = Enum.GetValues(typeof(EnTypeDelivery))
                    .Cast<EnTypeDelivery>()
                    .Select(x => new SelectListItem()
                    {
                        Text = x.Localize(),
                        Value = ((int)x).ToString()
                    })
                    .ToList();

                partners.Where(x => ActiveDeliveryTypes.Contains(x.Value)).ForEach(x => x.Selected = true);

                return partners;
            }
        }

        public string DefaultWeight
        {
            get { return Params.ElementOrDefault(GrastinTemplate.DefaultWeight) ?? "1"; }
            set { Params.TryAddValue(GrastinTemplate.DefaultWeight, value.TryParseFloat().ToString()); }
        }

        public bool EnabledCod
        {
            get { return Params.ElementOrDefault(GrastinTemplate.EnabledCOD).TryParseBool(); }
            set { Params.TryAddValue(GrastinTemplate.EnabledCOD, value.ToString()); }
        }

        public bool EnabledPickPoint
        {
            get { return Params.ElementOrDefault(GrastinTemplate.EnabledPickPoint).TryParseBool(); }
            set { Params.TryAddValue(GrastinTemplate.EnabledPickPoint, value.ToString()); }
        }

        public bool StatusesSync
        {
            get { return Params.ElementOrDefault(GrastinTemplate.StatusesSync).TryParseBool(); }
            set { Params.TryAddValue(GrastinTemplate.StatusesSync, value.ToString()); }
        }

        public string StatusDraft
        {
            get { return Params.ElementOrDefault(GrastinTemplate.StatusDraft); }
            set { Params.TryAddValue(GrastinTemplate.StatusDraft, value.DefaultOrEmpty()); }
        }

        public string StatusNew

        {
            get { return Params.ElementOrDefault(GrastinTemplate.StatusNew); }
            set { Params.TryAddValue(GrastinTemplate.StatusNew, value.DefaultOrEmpty()); }
        }

        public string StatusReturn

        {
            get { return Params.ElementOrDefault(GrastinTemplate.StatusReturn); }
            set { Params.TryAddValue(GrastinTemplate.StatusReturn, value.DefaultOrEmpty()); }
        }

        public string StatusDone

        {
            get { return Params.ElementOrDefault(GrastinTemplate.StatusDone); }
            set { Params.TryAddValue(GrastinTemplate.StatusDone, value.DefaultOrEmpty()); }
        }

        public string StatusShipping

        {
            get { return Params.ElementOrDefault(GrastinTemplate.StatusShipping); }
            set { Params.TryAddValue(GrastinTemplate.StatusShipping, value.DefaultOrEmpty()); }
        }

        public string StatusReceived

        {
            get { return Params.ElementOrDefault(GrastinTemplate.StatusReceived); }
            set { Params.TryAddValue(GrastinTemplate.StatusReceived, value.DefaultOrEmpty()); }
        }

        public string StatusCanceled

        {
            get { return Params.ElementOrDefault(GrastinTemplate.StatusCanceled); }
            set { Params.TryAddValue(GrastinTemplate.StatusCanceled, value.DefaultOrEmpty()); }
        }

        public string StatusPreparedForShipment

        {
            get { return Params.ElementOrDefault(GrastinTemplate.StatusPreparedForShipment); }
            set { Params.TryAddValue(GrastinTemplate.StatusPreparedForShipment, value.DefaultOrEmpty()); }
        }

        public string StatusProblem

        {
            get { return Params.ElementOrDefault(GrastinTemplate.StatusProblem); }
            set { Params.TryAddValue(GrastinTemplate.StatusProblem, value.DefaultOrEmpty()); }
        }

        public string StatusReturnedToCustomer

        {
            get { return Params.ElementOrDefault(GrastinTemplate.StatusReturnedToCustomer); }
            set { Params.TryAddValue(GrastinTemplate.StatusReturnedToCustomer, value.DefaultOrEmpty()); }
        }

        public string StatusDecommissioned

        {
            get { return Params.ElementOrDefault(GrastinTemplate.StatusDecommissioned); }
            set { Params.TryAddValue(GrastinTemplate.StatusDecommissioned, value.DefaultOrEmpty()); }
        }

        public List<SelectListItem> ListStatuses
        {
            get
            {
                var statuses = OrderStatusService.GetOrderStatuses()
                    .Select(x => new SelectListItem() {Text = x.StatusName, Value = x.StatusID.ToString()}).ToList();

                statuses.Insert(0, new SelectListItem() { Text = "", Value = "" });

                return statuses;
            }
        }

        public int ShipIdCod
        {
            get { return Params.ElementOrDefault(GrastinTemplate.ShipIdCOD, "0").TryParseInt(); }
            set
            {
                var paymentMethodId = value;

                if (EnabledCod)
                {
                    var payment = PaymentService.GetPaymentMethod(paymentMethodId);
                    if (payment == null)
                    {
                        var methodId = ShippingMethodId;

                        if (methodId == 0 && HttpContext.Current != null && HttpContext.Current.Request["shippingmethodid"] != null)
                            methodId = HttpContext.Current.Request["shippingmethodid"].TryParseInt();

                        var payMethod = new CashOnDelivery
                        {
                            Name = "Оплата курьеру (Грастин)",
                            Enabled = true,
                            ShippingMethodId = methodId
                        };
                        var paymentId = PaymentService.AddPaymentMethod(payMethod);

                        Params.TryAddValue(GrastinTemplate.ShipIdCOD, paymentId.ToString());
                    }
                }
                else
                {
                    if (paymentMethodId != 0)
                        PaymentService.DeletePaymentMethod(paymentMethodId);

                    Params.TryAddValue(GrastinTemplate.ShipIdCOD, "");
                }
            }
        }

        public int ShipIdPickPoint
        {
            get { return Params.ElementOrDefault(GrastinTemplate.ShipIdPickPoint).TryParseInt(); }
            set
            {
                var paymentMethodId = value;

                if (EnabledPickPoint)
                {
                    var payment = PaymentService.GetPaymentMethod(paymentMethodId);
                    if (payment == null)
                    {
                        var methodId = ShippingMethodId;

                        if (methodId == 0 && HttpContext.Current != null && HttpContext.Current.Request["shippingmethodid"] != null)
                            methodId = HttpContext.Current.Request["shippingmethodid"].TryParseInt();

                        var payMethod = new PickPoint
                        {
                            Name = "Оплата при получении заказа в пункте выдачи (Грастин)",
                            Enabled = true,
                            ShippingMethodId = methodId
                        };
                        var paymentId = PaymentService.AddPaymentMethod(payMethod);

                        Params.TryAddValue(GrastinTemplate.ShipIdPickPoint, paymentId.ToString());
                    }
                }
                else
                {
                    if (paymentMethodId != 0)
                        PaymentService.DeletePaymentMethod(paymentMethodId);

                    Params.TryAddValue(GrastinTemplate.ShipIdPickPoint, "");
                }
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                yield return new ValidationResult("Укажите API ключ", new[] { "ApiKey" });
            }
            if (string.IsNullOrWhiteSpace(OrderPrefix))
            {
                yield return new ValidationResult("Укажите префикс к заказу", new[] { "OrderPrefix" });
            }

            if (((EnTypeCalc) TypeCalc.TryParseInt()) == EnTypeCalc.Api)
            {
                var activeDeliveryTypes = ActiveDeliveryTypes;
                if (activeDeliveryTypes == null || activeDeliveryTypes.Length == 0)
                {
                    yield return new ValidationResult("Укажите методы доставки", new[] { "ActiveDeliveryTypes" });
                }
                
            }
        }
    }
}
