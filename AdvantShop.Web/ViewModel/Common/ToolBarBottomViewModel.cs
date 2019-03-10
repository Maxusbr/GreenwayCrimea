using AdvantShop.Configuration;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Trial;
using AdvantShop.ViewModel.Catalog;
using System;

namespace AdvantShop.ViewModel.Common
{
    public class ToolBarBottomViewModel
    {
        public ToolBarBottomViewModel()
        {
            DisplayCart = SettingsDesign.ShoppingCartVisibility;
            DisplayCompare = SettingsCatalog.EnableCompareProducts;
            DisplayRecentlyView = SettingsDesign.RecentlyViewVisibility;
            DisplayWishList = SettingsDesign.WishListVisibility;
            DisplayInplace = CustomerContext.CurrentCustomer.IsAdmin || CustomerContext.CurrentCustomer.IsModerator;
            CompareCount = ShoppingCartService.CurrentCompare.Count;
            WishlistCount = ShoppingCartService.CurrentWishlist.Count;
            CartCount = ShoppingCartService.CurrentShoppingCart.TotalItems;
            
            foreach (var module in AttachedModules.GetModules<IShoppingCart>())
            {
                var moduleObject = (IShoppingCart)Activator.CreateInstance(module, null);
                ShowConfirmButton &= moduleObject.ShowConfirmButtons;
            }
        }

        public bool DisplayRecentlyView { get; set; }
        public bool DisplayCompare { get; set; }
        public bool DisplayWishList { get; set; }
        public bool DisplayInplace { get; set; }
        public bool DisplayCart { get; set; }
        public bool ShowConfirmButton { get; set; }
        public float CompareCount { get; set; }
        public float WishlistCount { get; set; }
        public float CartCount { get; set; }

        public bool isCart { get; set; }
    }
}