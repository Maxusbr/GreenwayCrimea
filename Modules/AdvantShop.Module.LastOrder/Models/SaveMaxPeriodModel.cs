using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.LastOrder.Models
{
    public enum SaveMaxPeriodType
    {
        Hours,
        Days
    }

    public class SaveMaxPeriodModel
    {
        public int Period { get; set; }
        public SaveMaxPeriodType PeriodType { get; set; }
    }
}
