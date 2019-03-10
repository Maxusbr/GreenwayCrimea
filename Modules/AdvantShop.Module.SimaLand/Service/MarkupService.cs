using AdvantShop.Module.SimaLand.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.SimaLand.Service
{
    public class MarkupService
    {
        public static void AddMarkup(MarkupPriceRange range)
        {
            var ranges = PSLModuleSettings.PriceRange;
            range.Id = PSLModuleSettings.PriceRange.Count + 1;
            ranges.Add(range);
            PSLModuleSettings.PriceRange = ranges;
        }
    }
}
