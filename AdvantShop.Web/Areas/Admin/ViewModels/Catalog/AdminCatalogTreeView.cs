using System.Collections.Generic;

namespace AdvantShop.Web.Admin.ViewModels.Catalog
{
    public class AdminCatalogTreeView
    {
        public int CategoryId { get; set; }
        public int CategoryIdSelected { get; set; }
    }

    public class AdminCatalogTreeViewItem
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public string name { get; set; }
        public bool children { get; set; }
        public object state { get; set; }
        public Dictionary<string, string> li_attr {get; set;}
    }

    public class AdminCatalogTreeViewItemState
    {
        public bool opened { get; set; }
        public bool selected { get; set; }
    }
}