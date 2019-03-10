using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.SimaLand.Models
{
    public class ResponseProduct
    {
        public List<SimalandProduct> items { get; set; }
        public Links _links { get; set; }
        public Meta _meta { get; set; }
    }
}
