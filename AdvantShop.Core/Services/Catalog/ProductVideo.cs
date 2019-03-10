//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Catalog
{
    [Serializable]
    public class ProductVideo
    {
        public int ProductVideoId { get; set; }

        public int ProductId { get; set; }

        public string Name { get; set; }

        public string PlayerCode { get; set; }

        public string Description { get; set; }

        public int VideoSortOrder { get; set; }
    }
}