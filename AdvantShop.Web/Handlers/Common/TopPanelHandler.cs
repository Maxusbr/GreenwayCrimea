using System.Globalization;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Repository;
using AdvantShop.Repository.Currencies;
using AdvantShop.ViewModel.Home;
using AdvantShop.Design;
using System.Linq;

namespace AdvantShop.Handlers.Common
{
    public class TopPanelHandler
    {
        public TopPanelViewModel Get()
        {
            var curentCustomer = CustomerContext.CurrentCustomer;
            var model = new TopPanelViewModel()
            {
                IsShowDesignConstructor = curentCustomer.CustomerRole == Role.Administrator || Demo.IsDemoEnabled,
                IsRegistered = curentCustomer.RegistredUser,
                IsShowAdminLink =
                    curentCustomer.CustomerRole == Role.Administrator || curentCustomer.CustomerRole == Role.Moderator,
                IsDemoEnabled = Demo.IsDemoEnabled,
                IsShowCurrency = SettingsCatalog.AllowToChangeCurrency,
                IsShowCity = SettingsDesign.DisplayCityInTopPanel,
                IsShowWishList = SettingsDesign.WishListVisibility && !SettingsDesign.DisplayCityInTopPanel,
                CurrentCity = IpZoneContext.CurrentZone.City
            };

            if (model.IsShowCurrency)
            {
                var currentCurrency = CurrencyService.CurrentCurrency;
                model.CurrentCurrency = CurrencyService.CurrentCurrency;
                foreach (var currency in CurrencyService.GetAllCurrencies(true))
                {
                    model.Currencies.Add(new SelectListItem()
                    {
                        Text = currency.Name,
                        Value = currency.Iso3,
                        Selected = currency.Iso3 == currentCurrency.Iso3
                    });
                }
            }

            if (model.IsShowWishList)
            {
                var wishCount = ShoppingCartService.CurrentWishlist.Count;
                model.WishCount =
                    string.Format("{0} {1}",
                        wishCount == 0 ? "" : wishCount.ToString(CultureInfo.InvariantCulture),
                        Strings.Numerals(wishCount,
                            LocalizationService.GetResource("Common.TopPanel.WishList0"),
                            LocalizationService.GetResource("Common.TopPanel.WishList1"),
                            LocalizationService.GetResource("Common.TopPanel.WishList2"),
                            LocalizationService.GetResource("Common.TopPanel.WishList5")));
            }


            model.IsTemplatePreview = curentCustomer.IsAdmin && SettingsDesign.PreviewTemplate != null;

            if (model.IsTemplatePreview)
            {
                model.TemplatePreviewName = SettingsDesign.PreviewTemplate;
                var previewTemplate = TemplateService.GetTemplates().Items.First(tpl => tpl.StringId == SettingsDesign.PreviewTemplate);
                model.HasTemplate = previewTemplate.Active;
                model.BuyTemplateLink = previewTemplate.DetailsLink;
            }

            return model;
        }

    }
}