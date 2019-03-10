using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.SimaLand.Models
{
    public class SimalandOffer
    {
        public int id { get; set; }
        public string title { get; set; }
        public DateTime expiration_date { get; set; }
        public DateTime start_date { get; set; }
        public string description { get; set; }
        public int is_disabled { get; set; }
        public string slug { get; set; }
    }
}
