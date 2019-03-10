using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AdvantShop.Core.Services.Shipping.Boxberry
{
    [Serializable]
    public class BoxberryApiResponse
    {
        [JsonProperty(PropertyName = "err")]
        public string Error { get; set; }

        //public T Object { get; set; }

        //public List<T> ListObjects { get; set; }
    }

    [Serializable]
    public class BoxberryOrderDeleteAnswer : BoxberryApiResponse
    {
        [JsonProperty(PropertyName = "Text")]
        public string Result { get; set; }
    }

    [Serializable]
    public class BoxberryWaitingOrdersAnswer : BoxberryApiResponse
    {
        [JsonProperty(PropertyName = "ImIds")]
        public string ImIds { get; set; }
    }
}
