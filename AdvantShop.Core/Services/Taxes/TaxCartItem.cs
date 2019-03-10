using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Core.Services.Taxes
{
    public class TaxCartItem
    {
        public float Price { get; set; }
        public float Amount { get; set; }
        public int? TaxId { get; set; }
    }
}
