
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AdvantShop.Modules.RetailCRM.Models
{

    public class StatusResponse : ResponseSimple
    {
        [JsonConverter(typeof(RetailDictionaryConverter<Status>))]
        public List<Status> statuses { get; set; }
    }

    public class Status : BaseRetailDictionary
    {
        public string name { get; set; }
        //public string code { get; set; }
        public int ordering { get; set; }
        public string group { get; set; }
    }
}