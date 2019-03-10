
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdvantShop.Modules.RetailCRM.Models
{

    public class DeliveryTypeResponse : ResponseSimple
    {
        [JsonConverter(typeof(RetailDictionaryConverter<DeliveryType>))]
        public List<DeliveryType> deliveryTypes { get; set; }
    }


    public class DeliveryType : BaseRetailDictionary
    {
        public string name { get; set; }
        public float defaultCost { get; set; }
        public string integrationCode { get; set; }
        public List<string> paymentTypes { get; set; }

    }
}