using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdvantShop.Module.YaMetrika.Model
{
    public class EcommerceProductsModel
    {
        [JsonProperty(PropertyName = "products")]
        public List<EcommerceProduct> Products { get; set; }
    }

    public class EcommerceProduct
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }


        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }


        [JsonProperty(PropertyName = "price", NullValueHandling = NullValueHandling.Ignore)]
        public int Price { get; set; }


        [JsonProperty(PropertyName = "brand", NullValueHandling = NullValueHandling.Ignore)]
        public string Brand { get; set; }


        [JsonProperty(PropertyName = "category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }


		[JsonProperty(PropertyName = "quantity", NullValueHandling = NullValueHandling.Ignore)]
		public int Quantity { get; set; }


        [JsonProperty(PropertyName = "variant", NullValueHandling = NullValueHandling.Ignore)]
        public string Variant { get; set; }
    }

    public class EcommerceActionField
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "coupon", NullValueHandling = NullValueHandling.Ignore)]
        public string Coupon { get; set; }


        [JsonProperty(PropertyName = "goal_id", NullValueHandling = NullValueHandling.Ignore)]
        public string GoalId { get; set; }


        [JsonProperty(PropertyName = "list", NullValueHandling = NullValueHandling.Ignore)]
        public string List { get; set; }


        [JsonProperty(PropertyName = "revenue", NullValueHandling = NullValueHandling.Ignore)]
        public string Revenue { get; set; }


        [JsonProperty(PropertyName = "shipping", NullValueHandling = NullValueHandling.Ignore)]
        public int Shipping { get; set; }


        [JsonProperty(PropertyName = "tax", NullValueHandling = NullValueHandling.Ignore)]
        public string Tax { get; set; }
    }
}
