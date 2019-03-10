using System.Collections.Generic;

namespace AdvantShop.Modules.RetailCRM.Models
{
    public class ResponseSimple
    {
        public bool success { get; set; }
        public Pagination pagination { get; set; }
        public string errorMsg { get; set; }
        public object errors { get; set; }
        public int id { get; set; }

    }
}