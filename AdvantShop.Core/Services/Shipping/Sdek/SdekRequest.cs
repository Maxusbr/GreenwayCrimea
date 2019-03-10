//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdvantShop.Shipping.Sdek
{

    [Serializable]
    public class SdekGoods
    {
        [JsonProperty(PropertyName = "weight")]
        public string Weight { get; set; }

        [JsonProperty(PropertyName = "length")]
        public string Length { get; set; }

        [JsonProperty(PropertyName = "width")]
        public string Width { get; set; }

        [JsonProperty(PropertyName = "height")]
        public string Height { get; set; }
        
        public override int GetHashCode()
        {
            return Width.GetHashCode() ^ Length.GetHashCode() ^ Height.GetHashCode();
        }
    }

    [Serializable]
    public class SdekResponse
    {
        [JsonProperty(PropertyName = "result")]
        public SdekAnswerResult Result { get; set; }

        [JsonProperty(PropertyName = "error")]
        public List<SdekAnswerError> Error { get; set; }
    }

    [Serializable]
    public class SdekAnswerError
    {
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }

    [Serializable]
    public class SdekAnswerResult
    {
        [JsonProperty(PropertyName = "price")]
        public float Price { get; set; }

        [JsonProperty(PropertyName = "deliveryPeriodMin")]
        public int DeliveryPeriodMin { get; set; }

        [JsonProperty(PropertyName = "deliveryPeriodMax")]
        public int DeliveryPeriodMax { get; set; }

        [JsonProperty(PropertyName = "deliveryDateMin")]
        public string DeliveryDateMin { get; set; }

        [JsonProperty(PropertyName = "deliveryDateMax")]
        public string DeliveryDateMax { get; set; }

        [JsonProperty(PropertyName = "tariffId")]
        public int TariffId { get; set; }

        [JsonProperty(PropertyName = "cashOnDelivery")]
        public float CashOnDelivery { get; set; }

        [JsonProperty(PropertyName = "priceByCurrency")]
        public float PriceByCurrency { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }
    }


    public class SdekTariff
    {
        public int TariffId { get; set; }
        public string Name { get; set; }
        public string Mode { get; set; }
        public bool Active { get; set; }
        public int? WeightLimitation { get; set; }
        public string Description { get; set; }
        public bool MergeToOnePlace { get; set; }
    }
}