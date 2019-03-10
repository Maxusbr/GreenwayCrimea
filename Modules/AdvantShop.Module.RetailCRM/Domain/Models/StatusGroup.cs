
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdvantShop.Modules.RetailCRM.Models
{

    public class GetStatusgroups : ResponseSimple
    {
        [JsonConverter(typeof(StatusGroupConverter))]
        public List<StatusGroup> statusGroups { get; set; }
    }

    public class StatusGroup
    {
        public string name { get; set; }
        public string code { get; set; }
        public int ordering { get; set; }
        public bool process { get; set; }
        //public List<Status> statuses { get; set; }
    }


    class StatusGroupConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Site[]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // deserialize as object
            var groups = serializer.Deserialize<JObject>(reader);
            var result = new List<StatusGroup>();

            // create an array out of the properties
            foreach (JProperty property in groups.Properties())
            {
                var group = property.Value.ToObject<StatusGroup>();
                group.code = property.Name;
                result.Add(group);
            }

            return result;
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

}