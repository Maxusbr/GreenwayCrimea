using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Shipping.EmsPost
{
    [ShippingKey("EmsPost")]
    public class EmsPost : BaseShipping
    {
        #region Fields

        private readonly float _defaultWeight;
        private readonly float _maxWeight;
        private readonly float _extraPrice;
        private readonly string _cityFrom;
        private readonly int _discountType;
        #endregion

        public EmsPost(ShippingMethod method, PreOrder preOrder)
            : base(method, preOrder)
        {
            _defaultWeight = _method.Params.ElementOrDefault(EmsPostTemplate.DefaultWeight).TryParseFloat();
            _maxWeight = _method.Params.ElementOrDefault(EmsPostTemplate.MaxWeight).TryParseFloat();
            _extraPrice = _method.Params.ElementOrDefault(EmsPostTemplate.ExtraPrice).TryParseFloat();
            _cityFrom = _method.Params.ElementOrDefault(EmsPostTemplate.CityFrom);
            _discountType = _method.Params.ElementOrDefault(EmsPostTemplate.DiscountType).TryParseInt();
        }

        protected override IEnumerable<BaseShippingOption> CalcOptions()
        {
            var options = new List<BaseShippingOption>();

            if (!EmsPostService.CheckService())
            {
                return options;
            }

            var weight = _preOrder.Items.Sum(item => item.Weight * item.Amount);
            if (weight == 0)
                weight = _defaultWeight;

            if (_maxWeight != 0 && weight >= _maxWeight)
                return options;

            var emsPrice = EmsPostService.GetEmsPriceByCity(_cityFrom, _preOrder.CityDest, weight);
            if (emsPrice == null && _preOrder.RegionDest.IsNotEmpty())
            {
                emsPrice = EmsPostService.GetEmsPriceByRegion(_cityFrom, _preOrder.RegionDest, weight);
                if (emsPrice == null && _preOrder.CountryDest.IsNotEmpty())
                {
                    emsPrice = EmsPostService.GetEmsPriceByCountry(_cityFrom, _preOrder.CountryDest, weight);
                }
            }

            if (emsPrice == null)
                return options;
            
            options.Add(new BaseShippingOption(_method)
            {
                Rate = emsPrice.price + (_discountType == 0 ? _extraPrice : emsPrice.price * _extraPrice / 100),
                DeliveryTime =
                    emsPrice.term != null
                        ? (emsPrice.term["min"] == emsPrice.term["max"]
                            ? emsPrice.term["min"] == "0" ? string.Empty : string.Format("{0} дн.", emsPrice.term["min"])
                            : string.Format("{0} - {1} дн.", emsPrice.term["min"], emsPrice.term["max"]))
                        : string.Empty
            });
            return options;
        }
    }
}