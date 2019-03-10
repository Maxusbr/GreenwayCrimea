using System.Linq;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.FilePath;
using AdvantShop.Orders;
using AdvantShop.Repository;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.Common
{
    public class GetCommonSettings
    {
        public CommonSettingsModel Execute()
        {
            // common settings
            var model = new CommonSettingsModel()
            {
                StoreUrl = SettingsMain.SiteUrl,
                StoreName = SettingsMain.ShopName,

                LogoImgSrc =
                    !string.IsNullOrEmpty(SettingsMain.LogoImageName)
                        ? FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.LogoImageName, true)
                        : "../images/nophoto_small.jpg",

                FaviconSrc =
                    !string.IsNullOrEmpty(SettingsMain.FaviconImageName)
                        ? FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.FaviconImageName, true)
                        : "../images/nophoto_small.jpg",
            };

            var countryId = SettingsMain.SellerCountryId;
            model.CountryId = countryId;
            model.Countries = CountryService.GetAllCountries().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.CountryId.ToString(),
                Selected = x.CountryId == countryId
            }).ToList();


            var regionId = SettingsMain.SellerRegionId;
            model.RegionId = regionId;
            model.Regions = RegionService.GetRegions(countryId).Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.RegionId.ToString(),
                Selected = x.RegionId == regionId
            }).ToList();
            model.HasRegions = model.Regions.Count > 0;

            model.City = SettingsMain.City;
            model.Phone = SettingsMain.Phone;
            //model.MobilePhone = SettingsMain.MobilePhone;
            

            // sales plan settings
            model.SalesPlan = OrderStatisticsService.SalesPlan;
            model.ProfitPlan = OrderStatisticsService.ProfitPlan;

            model.IsTrial = Trial.TrialService.IsTrialEnabled;

            return model;
        }
    }
}
