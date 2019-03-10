using System.Collections.Generic;
using AdvantShop.Core.Services.CMS;
using AdvantShop.Customers;

namespace AdvantShop.Areas.Mobile.Models.Sidebar
{
    public class SidebarMobileViewModel
    {
        public string StoreName { get; set; }

        public Customer Customer { get; set; }

        public string CurrentCity { get; set; }
        public bool DisplayCity { get; set; }

        public List<MenuItemModel> Menu { get; set; }
    }
}