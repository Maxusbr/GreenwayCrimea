//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Module.ProductTabs.Domain
{
    public class TabBody
    {
        public int TabBodyId { get; set; }

        public int TabTitleId { get; set; }

        public int ProductId { get; set; }

        public string Body { get; set; }
    }
}