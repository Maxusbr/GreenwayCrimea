using Newtonsoft.Json;

namespace AdvantShop.Shipping.NovaPoshta
{
    public class NovaPoshtaRequest<T>
    {
        [JsonProperty(PropertyName = "apiKey")]
        public string ApiKey { get; set; }
        [JsonProperty(PropertyName = "modelName")]
        public string ModelName { get; set; }
        [JsonProperty(PropertyName = "calledMethod")]
        public string CalledMethod { get; set; }
        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }
        [JsonProperty(PropertyName = "methodProperties")]
        public T MethodProperties { get; set; }
    }
}