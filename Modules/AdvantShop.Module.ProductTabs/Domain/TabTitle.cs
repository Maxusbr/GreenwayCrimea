//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Module.ProductTabs.Domain
{
    public class TabTitle
    {
        public int TabTitleId { get; set; }

        public string Title { get; set; }

        public int SortOrder { get; set; }

        public bool Active { get; set; }
    }
}