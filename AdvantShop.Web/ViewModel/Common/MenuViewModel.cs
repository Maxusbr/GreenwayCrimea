using System.Collections.Generic;
using AdvantShop.Core.Services.CMS;
using AdvantShop.Configuration;

namespace AdvantShop.ViewModel.Common
{
    public partial class MenuViewModel
    {
        public MenuViewModel()
        {
            MenuItems = new List<MenuItemModel>();
        }

        public List<MenuItemModel> MenuItems { get; set; }

        public int SelectedItemId { get; set; }

        public bool IsExpanded { get; set; }

        public bool InLayout { get; set; }

        public SettingsDesign.eMenuStyle ViewMode { get; set; }

        public bool DisplayProductsCount { get; set; }

        public bool IsСlickability
        {
            get
            {
                return ViewMode == SettingsDesign.eMenuStyle.Accordion || ViewMode == SettingsDesign.eMenuStyle.Treeview;
            }
        }

        public int CountColsProductsInRow { get; set; }
    }
}