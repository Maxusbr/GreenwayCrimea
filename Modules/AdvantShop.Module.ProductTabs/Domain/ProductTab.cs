//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Core.Modules.Interfaces;

namespace AdvantShop.Module.ProductTabs.Domain
{
    public class ProductTab : ITab
    {
        public int TabTitleId { get; set; }

        public int TabBodyId { get; set; }
        public bool Active { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string TabGroup { get; set; }

        public int ProductId { get; set; }

        public int SortOrder { get; set; }
    }
}