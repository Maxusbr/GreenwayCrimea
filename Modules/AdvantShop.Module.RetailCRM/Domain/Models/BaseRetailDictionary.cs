using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdvantShop.Modules.RetailCRM.Models
{
    public class BaseRetailDictionary
    {
        public string code { get; set; }
    }

    class RetailDictionaryConverter<T> : JsonConverter where T : BaseRetailDictionary
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Site[]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // deserialize as object
            var sites = serializer.Deserialize<JObject>(reader);
            var result = new List<T>();

            // create an array out of the properties
            foreach (JProperty property in sites.Properties())
            {
                var site = property.Value.ToObject<T>();
                site.code = property.Name;
                result.Add(site);
            }

            return result;
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);
            t.WriteTo(writer);
        }
    }

}
