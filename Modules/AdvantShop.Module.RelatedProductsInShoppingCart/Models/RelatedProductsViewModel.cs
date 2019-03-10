using System.Collections.Generic;

namespace AdvantShop.Module.RelatedProductsInShoppingCart.Models
{
    public class RelatedProductsViewModel
    {
        public List<int> ProductIds { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string TextBefore { get; set; }
        public string TextAfter { get; set; }
    }
}
