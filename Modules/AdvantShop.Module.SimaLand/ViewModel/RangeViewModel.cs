using AdvantShop.Module.SimaLand.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.SimaLand.ViewModel
{
    public class RangeViewModel
    {
        public List<MarkupPriceRange> ranges { get; set; }
        public bool Added { get; set; }
        public int Edited { get; set; }
    }
}
