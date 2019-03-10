//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdvantShop.Core.Services.Shipping.Grastin
{
    [JsonConverter(typeof(GrastinEventWidgetDataConverter))]
    public class GrastinEventWidgetData
    {
        public EnDeliveryType DeliveryType { get; set; }

        public EnPartner Partner { get; set; }

        public string PickPointId { get; set; }

        public PickPoint PickPointData { get; set; }
        public string CityTo { get; set; }
        public string CityFrom { get; set; }
        public float? Cost { get; set; }
    }

    public class PickPoint
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string Timetable { get; set; }
    }

    [JsonConverter(typeof(GrastinEnumConverter))]
    public enum EnDeliveryType
    {
        None = 0,
        Courier = 1,
        PickPoint = 2,
    }

    [JsonConverter(typeof(GrastinEnumConverter))]
    public enum EnPartner
    {
        None = 0,
        Grastin = 1,
        Hermes = 2,
        RussianPost = 3,
        Boxberry = 4,
        DPD = 5,
    }

    #region help for patch 6.0.9/8

    /*
     * Конверторы для того чтобы отказаться в от атрибутов JsonProperty и
     * представления в строковыми значениями EnDeliveryType и EnPartner (перейти на числовые).
     * При этом так чтобы уже засериализованные значения в БД корректно десериализовались.
     * В 7.0.0 (6.5) будет уже без конверторов.
     */

    public class GrastinEventWidgetDataConverter : JsonConverter
    {
        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObj = serializer.Deserialize<JObject>(reader);

            if (jObj.Property("partnerId") != null || jObj.Property("currentId") != null || jObj.Property("pvzData") != null)
            {
                var renamedDict = Rename(jObj, new Dictionary<string, string> { { "partnerId", "Partner" }, { "currentId", "PickPointId" }, { "pvzData", "PickPointData" } });
                return renamedDict.ToObject<GrastinEventWidgetData>();
            }

            var obj = new GrastinEventWidgetData();
            serializer.Populate(jObj.CreateReader(), obj);
            return obj;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(GrastinEventWidgetData);
        }

        public static JToken Rename(JToken json, Dictionary<string, string> map)
        {
            return Rename(json, name => map.ContainsKey(name) ? map[name] : name);
        }

        public static JToken Rename(JToken json, Func<string, string> map)
        {
            JProperty prop = json as JProperty;
            if (prop != null)
            {
                return new JProperty(map(prop.Name), Rename(prop.Value, map));
            }

            JObject o = json as JObject;
            if (o != null)
            {
                var cont = o.Properties().Select(el => Rename(el, map));
                return new JObject(cont);
            }

            return json;
        }
    }

    public class GrastinEnumConverter : JsonConverter
    {
        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //writer.WriteValue(value);
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (CanConvert(objectType))
            {
                if (reader.TokenType == JsonToken.String)
                {
                    var value = reader.Value.ToString();
                    if (objectType == typeof(EnDeliveryType))
                    {
                        if (value.Equals("courier", StringComparison.OrdinalIgnoreCase))
                            return EnDeliveryType.Courier;
                        if (value.Equals("pvz", StringComparison.OrdinalIgnoreCase))
                            return EnDeliveryType.PickPoint;
                        return EnDeliveryType.None;
                    }
                    if (objectType == typeof(EnPartner))
                    {
                        if (value.Equals("grastin", StringComparison.OrdinalIgnoreCase))
                            return EnPartner.Grastin;
                        if (value.Equals("hermes", StringComparison.OrdinalIgnoreCase))
                            return EnPartner.Hermes;
                        if (value.Equals("post", StringComparison.OrdinalIgnoreCase))
                            return EnPartner.RussianPost;
                        if (value.Equals("boxberry", StringComparison.OrdinalIgnoreCase))
                            return EnPartner.Boxberry;
                        if (value.Equals("dpd", StringComparison.OrdinalIgnoreCase))
                            return EnPartner.DPD;
                        return EnPartner.None;
                    }
                }
                if (reader.TokenType == JsonToken.Integer)
                {
                    return Enum.ToObject(objectType, reader.Value);
                }
            }
            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(EnDeliveryType) || objectType == typeof(EnPartner);
        }
    }

    #endregion
}
