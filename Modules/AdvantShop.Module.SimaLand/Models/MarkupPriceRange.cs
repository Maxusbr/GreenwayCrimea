using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.SimaLand.Models
{
    public class MarkupPriceRange
    {
        public int Id { get; set; }
        public float MinPrice { get; set; }
        public float MaxPrice { get; set; }
        public float Markup { get; set; }
    }
}
