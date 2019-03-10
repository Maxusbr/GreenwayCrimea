using AdvantShop.CMS;
using System.Collections.Generic;

namespace AdvantShop.Web.Admin.Models.Menus
{
    public class MenusTree
    {
        public string Id { get; set; }

        public EMenuType MenuType { get; set; }

        public int? SelectedId { get; set; }

        public bool ShowRoot { get; set; }
        public bool ShowActions { get; set; }

        public int? ExcludeId { get; set; }
    }

    public class AdminMenuTreeViewItem
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public string name { get; set; }
        public Dictionary<string, string> li_attr { get; set; }
        public bool children { get; set; }
        public AdminMenuTreeViewItemState state { get; set; }
    }

    public class AdminMenuTreeViewItemState
    {
        public bool opened { get; set; }
        public bool selected { get; set; }
    }
}
