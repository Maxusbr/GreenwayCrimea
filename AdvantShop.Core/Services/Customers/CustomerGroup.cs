//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Customers
{
    [Serializable]
    public class CustomerGroup
    {
        public int CustomerGroupId { get; set; }
        public string GroupName { get; set; }
        public float GroupDiscount { get; set; }
        public float MinimumOrderPrice { get; set; }
    }
}