//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using Newtonsoft.Json;

namespace AdvantShop.Catalog
{
    public enum OptionPriceType
    {
        [Localize("Core.Catalog.OptionPriceType.Fixed")]
        Fixed = 0,

        [Localize("Core.Catalog.OptionPriceType.Percent")]
        Percent = 1
    }

    public enum OptionField
    {
        Title = 1,
        PriceBc = 2,
        SortOrder = 4
    }

    [Serializable]
    public class OptionItem : IDentable
    {

        public int OptionId { get; set; }
        public string Title { get; set; }

        [JsonIgnore]
        public float CurrencyRate { get; set; }

        [JsonIgnore]
        public float BasePrice { get; set; }

        private float? _roundedPrice;

        [JsonIgnore]
        public float RoundedPrice
        {
            get
            {
                if (PriceType == OptionPriceType.Fixed)
                {
                    return _roundedPrice ??
                           (float) (_roundedPrice = PriceService.RoundPrice(BasePrice, null, CurrencyRate));
                }
                else
                {
                    return BasePrice;
                }
            }
        }

        [JsonIgnore]
        public OptionPriceType PriceType { get; set; }

        [JsonIgnore]
        public int SortOrder { get; set; }

        private int _nullFields;

        public void SetFieldToNull(OptionField field)
        {
            _nullFields = _nullFields | (int) field;
        }

        public bool IsNull(OptionField field)
        {
            return (_nullFields & (int) field) > 0;
        }

        public string OptionText
        {
            get
            {

                var result = Title.Trim();

                if (RoundedPrice != 0)
                {
                    if (PriceType == OptionPriceType.Fixed)
                    {
                        result = Title + (RoundedPrice > 0 ? " +" : RoundedPrice < 0 ? " -" : "") + PriceFormatService.FormatPrice(Math.Abs(RoundedPrice));
                    }
                    else if (PriceType == OptionPriceType.Percent)
                    {
                        result = Title + (RoundedPrice > 0 ? " +" : RoundedPrice < 0 ? " -" : "") + Math.Abs(RoundedPrice).ToString("#,0.##") + "%";
                    }
                }

                return result;
            }
        }

        public int ID
        {
            get { return OptionId; }
        }
    }
}