using System.Collections.Generic;

namespace AdvantShop.Module.ProductTabs.Models
{
    public class AdminProductTabsModel
    {
        public List<AdminProductTabModel> Tabs { get; set; }
    }

    public class AdminProductTabModel
    {
        public int ProductId { get; set; }
        public int SortOrder { get; set; }
        public bool Active { get; set; }

        public int TabTitleId { get; set; }
        public string Title { get; set; }

        public int TabBodyId { get; set; }
        public string Body { get; set; }
    }
}
