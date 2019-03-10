//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Linq;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Diagnostics;
using System;
using System.Collections.Generic;
using AdvantShop.Repository;

namespace AdvantShop.Shipping.NovaPoshta
{

    [ShippingKey("NovaPoshta")]
    public class NovaPoshta : BaseShippingWithCargoAndCache
    {
        private readonly string _apiKey;
        private readonly enNovaPoshtaDeliveryType _deliveryType;

        private readonly string _cityFrom;
        private readonly string _cityTo;

        private readonly float _rate;
        private readonly float _totalPrice;
        private readonly float _totalWeight;


        public NovaPoshta(ShippingMethod method, PreOrder preOrder)
            : base(method, preOrder)
        {
            _apiKey = _method.Params.ElementOrDefault(NovaPoshtaTemplate.APIKey);
            _cityFrom = _method.Params.ElementOrDefault(NovaPoshtaTemplate.CityFrom);
            _deliveryType = (enNovaPoshtaDeliveryType)_method.Params.ElementOrDefault(NovaPoshtaTemplate.DeliveryType).TryParseInt((int)enNovaPoshtaDeliveryType.WarehouseDoors);

            _rate = _method.Params.ElementOrDefault(NovaPoshtaTemplate.Rate).TryParseFloat();
            _totalPrice = _preOrder.Items.Sum(item => item.Price * item.Amount);

            _cityTo = _preOrder.CityDest;
            _totalWeight = _preOrder.Items.Sum(item => item.Weight);
            if (_totalWeight == 0)
                _totalWeight = _defaultWeight;
        }

        protected override IEnumerable<BaseShippingOption> CalcOptions()
        {
            var option = GetShippingOption();
            if (option == null)
                return new List<NovaPoshtaOptions>();

            return new List<NovaPoshtaOptions>() { option };
        }

        private NovaPoshtaOptions GetShippingOption()
        {
            var senderCity = NovaPoshtaService.GetCity(_apiKey, _cityFrom, "");
            var recipientCity = NovaPoshtaService.GetCity(_apiKey, _cityTo, "");
            var delivery = NovaPoshtaService.GetDocumentDeliveryDate(_apiKey, senderCity.Ref, recipientCity.Ref, _deliveryType, DateTime.Now);
            var price = NovaPoshtaService.GetDocumentPrice(_apiKey, senderCity.Ref, recipientCity.Ref, _deliveryType, _totalWeight, _rate != 0 ? _totalPrice / _rate : _totalPrice);
            var days = (delivery - DateTime.Today).Days;

            var option = new NovaPoshtaOptions(_method)
            {
                DeliveryTime = days + " " + Strings.Numerals(days, "нет", "день", "дня", "дней"),
                Rate = price * _rate
            };

            return option;
        }

        protected override int GetHashForCache()
        {
            //var items = _preOrder.Items.Select(item => new[] { item.Height, item.Width, item.Length }).ToList();
            var items = _preOrder.Items.Select(item => new Measure { XYZ = new[] { item.Height, item.Width, item.Length }, Amount = item.Amount }).ToList();

            var dimensions = MeasureHelper.GetDimensions(items);

            var length = dimensions[0];
            var width = dimensions[1];
            var height = dimensions[2];
            string postData = string.Format("auth{0},senderCity{1}recipientCity{2}mass{3}height{4}width{5}depth{6}",
                                            _apiKey, _cityFrom, _preOrder.CityDest, _preOrder.Items.Sum(item => item.Weight * item.Amount).ToString("F3"),
                                            height.ToString("F2"), width.ToString("F2"), length.ToString("F2"));
            var hash = postData.GetHashCode();
            return _method.ShippingMethodId ^ hash;
        }
    }
}