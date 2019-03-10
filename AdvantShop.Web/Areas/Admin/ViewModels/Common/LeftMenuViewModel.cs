using AdvantShop.Web.Admin.Models.Menus;
using System.Collections.Generic;
using System;

namespace AdvantShop.Web.Admin.ViewModels.Common
{
    public class LeftMenuViewModel
    {
        public LeftMenuViewModel()
        {
            MenuItems = new List<AdminMenuModel>();
            DisplayCatalog = true;
            DisplayCustomers = true;
            DisplayOrders = true;
            DisplayCrm = true;            
        }

        public List<AdminMenuModel> MenuItems { get; set; }

        public Guid CustomerId { get; set; }
        public string AvatarSrc { get; set; }
        public string NoAvatarSrc { get; set; }

        public bool DisplayCatalog { get; set; }
        public bool DisplayCustomers { get; set; }
        public bool DisplayOrders { get; set; }
        public bool DisplayCrm { get; set; }
        public bool DisplayCms { get; set; }

        public bool ShowAddMenu
        {
            get
            {
                return DisplayCatalog || DisplayCustomers || DisplayOrders || DisplayCrm || DisplayCms;
            }
        }

    }
}
