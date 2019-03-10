using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdvantShop.Modules.RetailCRM.Models
{
    public class SitesResponse : ResponseSimple
    {
        [JsonConverter(typeof(RetailDictionaryConverter<Site>))]
        public List<Site> sites { get; set; }
    }

    public class Site :BaseRetailDictionary
    {
        public string name { get; set; }
        public string url { get; set; }
        public string code { get; set; }
    }
}