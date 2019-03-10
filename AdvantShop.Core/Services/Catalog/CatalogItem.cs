//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop
{

    public enum CatalogItemType
    {
        Category = 0,
        Product = 1
    }

    [Serializable]
    public class CatalogItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductArtNo { get; set; }
        public CatalogItemType Type { get; set; }
        public int ChildCount { get; set; }
    }
}
