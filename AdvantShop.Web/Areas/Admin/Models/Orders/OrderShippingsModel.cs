using System;
using System.Collections.Generic;
using AdvantShop.Shipping;

namespace AdvantShop.Web.Admin.Models.Orders
{
    public class OrderShippingsModel
    {
        public List<BaseShippingOption> Shippings { get; set; }
        public BaseShippingOption SelectShipping { get; set; }
        public BaseShippingOption CustomShipping { get; set; }
    }
}
