using AdvantShop.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdvantShop.Module.ShippingPaymentPage.Models
{
    public class ShippingPaymentModel
    {
        public IpZone Zone { get; set; }
        public string TextBlock { get; set; }
        public string TextBlockBottom { get; set; }
    }
}
