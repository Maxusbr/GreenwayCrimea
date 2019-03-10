using AdvantShop.Module.SimaLand.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.SimaLand.Service
{
    public class PriceService
    {
        // i tak soidet
        public static float SetPrice(float price)
        {
            var priceRanges = PSLModuleSettings.PriceRange;

            if (priceRanges.Count == 0)
            {
                return price;
            }

            foreach (var range in priceRanges)
            {
                if (price >= range.MinPrice && price <= range.MaxPrice)
                {
                    return (price * (range.Markup/100) + price);
                }
            }

            return price;
        }
    }
}
