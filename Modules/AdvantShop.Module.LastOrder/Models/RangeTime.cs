using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.LastOrder.Models
{

    public enum RangeTimeType
    {
        Minutes,
        Hours,
        Days
    }

    public class RangeTime
    {
        public int from { get; set; }
        public int to { get; set; }
        public RangeTimeType rType { get; set; }
    }
}
