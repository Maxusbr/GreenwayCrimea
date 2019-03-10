using System.Collections.Generic;

namespace AdvantShop.Catalog
{
    public class ProductLabel
    {
        public string LabelCode { get; set; }
        public List<int> ProductIds { get; set; } 
    }
}