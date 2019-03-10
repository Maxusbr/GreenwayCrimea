using System;
using System.Collections.Generic;

namespace AdvantShop.Shipping.NovaPoshta
{
    public class NovaResponseCity
    {
        public string Description { get; set; }
        public string DescriptionRu { get; set; }
        public Guid Ref { get; set; }
        public bool Delivery1 { get; set; }
        public bool Delivery2 { get; set; }
        public bool Delivery3 { get; set; }
        public bool Delivery4 { get; set; }
        public bool Delivery5 { get; set; }
        public bool Delivery6 { get; set; }
        public bool Delivery7 { get; set; }
        public Guid Area { get; set; }
        public List<Guid> Conglomerates { get; set; }
    }
}