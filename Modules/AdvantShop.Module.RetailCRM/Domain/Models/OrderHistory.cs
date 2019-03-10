using System;
using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdvantShop.Modules.RetailCRM.Models
{
    public class OrderHistoryResponse : ResponseSimple
    {
        [JsonConverter(typeof(OrderHistoryConverter))]
        public List<SerializedOrder> orders { get; set; }
    }

    class OrderHistoryConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Site[]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // deserialize as object
            try
            {
                var orders = serializer.Deserialize<JObject>(reader);
                var result = new List<SerializedOrder>();

                // create an array out of the properties
                foreach (JProperty property in orders.Properties())
                {
                    var order = property.Value.ToObject<SerializedOrder>();
                    order.id = property.Name.TryParseInt();
                    result.Add(order);
                }
                return result;
            }
            catch (Exception ex)
            {
                AdvantShop.Diagnostics.Debug.Log.Error(ex);
                return null;
            }
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

}