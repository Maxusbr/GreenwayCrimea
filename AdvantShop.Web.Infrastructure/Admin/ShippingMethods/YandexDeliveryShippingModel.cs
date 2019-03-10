using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Shipping;
using AdvantShop.Diagnostics;
using AdvantShop.Shipping.ShippingYandexDelivery;
using Newtonsoft.Json;

namespace AdvantShop.Web.Infrastructure.Admin.ShippingMethods
{
    public class YandexDeliveryShippingAdminModel : ShippingMethodAdminModel, IValidatableObject
    {
        private bool _isActiveNow;
        public bool IsActive
        {
            get { return Params.ElementOrDefault(YandexDeliveryTemplate.IsActive).TryParseBool(); }
            set
            {
                _isActiveNow = true;
                Params.TryAddValue(YandexDeliveryTemplate.IsActive, value.ToString());
            }
        }

        private bool _isApiKeysAdded;

        public string ApiKeys
        {
            get { return Params.ElementOrDefault(YandexDeliveryTemplate.ApiKeys); }
            set
            {
                var apiKeys = value.DefaultOrEmpty();

                if (!string.IsNullOrWhiteSpace(apiKeys))
                {
                    try
                    {
                        var keys = JsonConvert.DeserializeObject<Dictionary<string, string>>(apiKeys);

                        SecretKeyDelivery = keys["searchDeliveryList"];
                        SecretKeyCreateOrder = keys["createOrder"];

                        _isApiKeysAdded = true;
                    }
                    catch (Exception ex)
                    {
                        Debug.Log.Error(ex);
                    }
                }

                Params.TryAddValue(YandexDeliveryTemplate.ApiKeys, apiKeys);
            }
        }

        //private bool _isApiDataAdded;
        public string ApiData
        {
            get { return Params.ElementOrDefault(YandexDeliveryTemplate.ApiData); }
            set
            {
                var apiData = value.DefaultOrEmpty();

                if (!string.IsNullOrWhiteSpace(apiData))
                {
                    try
                    {
                        var apiDataKeys = JsonConvert.DeserializeObject<YaDeliveryJsonConfigParams>(apiData);

                        ClientId = apiDataKeys.client.id;

                        var warehouse = apiDataKeys.warehouses.FirstOrDefault();
                        WarehouseId = warehouse != null ? warehouse.id : "";

                        var sender = apiDataKeys.senders.FirstOrDefault();
                        SenderId = sender != null ? sender.id : "";

                        var requisite = apiDataKeys.requisites.FirstOrDefault();
                        RequisiteId = requisite != null ? requisite.id : "";

                        CityFrom = "Москва";

                        //_isApiDataAdded = true;
                    }
                    catch (Exception ex)
                    {
                        Debug.Log.Error(ex);
                    }
                }

                Params.TryAddValue(YandexDeliveryTemplate.ApiData, apiData);
            }
        }



        public string SecretKeyDelivery
        {
            get { return Params.ElementOrDefault(YandexDeliveryTemplate.SecretKeyDelivery); }
            set { Params.TryAddValue(YandexDeliveryTemplate.SecretKeyDelivery, value.DefaultOrEmpty()); }
        }

        public string SecretKeyCreateOrder
        {
            get { return Params.ElementOrDefault(YandexDeliveryTemplate.SecretKeyCreateOrder); }
            set { Params.TryAddValue(YandexDeliveryTemplate.SecretKeyCreateOrder, value.DefaultOrEmpty()); }
        }



        public string WidgetCode
        {
            get { return Params.ElementOrDefault(YandexDeliveryTemplate.WidgetCode); }
            set
            {
                var widgetCode = value.DefaultOrEmpty();

                if (!string.IsNullOrWhiteSpace(widgetCode))
                {
                    // Save only url of widget like https://delivery.yandex.ru/widget/loader?resource_id=..&sid=..&key=..

                    var index = widgetCode.IndexOf("https://delivery.yandex.ru/widget/");
                    if (index != -1)
                        widgetCode = widgetCode.Substring(index).Replace("\"></script>", "");
                }

                Params.TryAddValue(YandexDeliveryTemplate.WidgetCode, widgetCode);
            }
        }

        public bool ShowAssessedValue
        {
            get { return Params.ElementOrDefault(YandexDeliveryTemplate.ShowAssessedValue).TryParseBool(); }
            set { Params.TryAddValue(YandexDeliveryTemplate.ShowAssessedValue, value.ToString()); }
        }

        public string ClientId
        {
            get { return Params.ElementOrDefault(YandexDeliveryTemplate.ClientId); }
            set { Params.TryAddValue(YandexDeliveryTemplate.ClientId, value.DefaultOrEmpty()); }
        }

        public string SenderId
        {
            get { return Params.ElementOrDefault(YandexDeliveryTemplate.SenderId); }
            set { Params.TryAddValue(YandexDeliveryTemplate.SenderId, value.DefaultOrEmpty()); }
        }

        public string WarehouseId
        {
            get { return Params.ElementOrDefault(YandexDeliveryTemplate.WarehouseId); }
            set { Params.TryAddValue(YandexDeliveryTemplate.WarehouseId, value.DefaultOrEmpty()); }
        }
        
        public string RequisiteId
        {
            get { return Params.ElementOrDefault(YandexDeliveryTemplate.RequisiteId); }
            set { Params.TryAddValue(YandexDeliveryTemplate.RequisiteId, value.DefaultOrEmpty()); }
        }

        public string CityFrom
        {
            get { return Params.ElementOrDefault(YandexDeliveryTemplate.CityFrom, "Москва"); }
            set { Params.TryAddValue(YandexDeliveryTemplate.CityFrom, value.DefaultOrEmpty()); }
        }
        
        
        public string DefaultWeight
        {
            get { return Params.ElementOrDefault(YandexDeliveryTemplate.DefaultWeight, "1"); }
            set { Params.TryAddValue(YandexDeliveryTemplate.DefaultWeight, value.TryParseFloat().ToString()); }
        }

        public string DefaultHeight
        {
            get { return Params.ElementOrDefault(YandexDeliveryTemplate.DefaultHeight, "100"); }
            set { Params.TryAddValue(YandexDeliveryTemplate.DefaultHeight, value.TryParseFloat().ToString()); }
        }

        public string DefaultWidth
        {
            get { return Params.ElementOrDefault(YandexDeliveryTemplate.DefaultWidth, "100"); }
            set { Params.TryAddValue(YandexDeliveryTemplate.DefaultWidth, value.TryParseFloat().ToString()); }
        }

        public string DefaultLength
        {
            get { return Params.ElementOrDefault(YandexDeliveryTemplate.DefaultLength, "100"); }
            set { Params.TryAddValue(YandexDeliveryTemplate.DefaultLength, value.TryParseFloat().ToString()); }
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (_isActiveNow)
            {
                if (string.IsNullOrWhiteSpace(ApiKeys) || string.IsNullOrWhiteSpace(ApiData))
                    yield return new ValidationResult("Введите ключи");

                if (!_isApiKeysAdded)
                    IsActive = false;
            }
        }
    }
}
