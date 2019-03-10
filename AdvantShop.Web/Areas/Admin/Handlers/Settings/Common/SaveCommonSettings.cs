using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;
using AdvantShop.Repository;
using AdvantShop.Trial;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.Common
{
    public class SaveCommonSettings
    {
        private CommonSettingsModel _model;

        public SaveCommonSettings(CommonSettingsModel model)
        {
            _model = model;
        }

        public void Execute()
        {
            // trim('/') ?!
            if (_model.StoreUrl.Substring(_model.StoreUrl.Length - 1).Equals("/"))
            {
                _model.StoreUrl = _model.StoreUrl.Substring(0, _model.StoreUrl.Length - 1);
            }

            TrackEvents();

            if (!TrialService.IsTrialEnabled)
            {
                SettingsMain.SiteUrl = _model.StoreUrl.StartsWith("http://") || _model.StoreUrl.StartsWith("https://")
                                            ? _model.StoreUrl
                                            : "http://" + _model.StoreUrl;
            }

            SettingsMain.ShopName = _model.StoreName.DefaultOrEmpty();

            //SettingsMain.LogoImageAlt = _model.LogoImgAlt.DefaultOrEmpty();

            var countries = CountryService.GetAllCountries();
            SettingsMain.SellerCountryId = countries.Any(x => x.CountryId == _model.CountryId)
                                                ? _model.CountryId
                                                : (countries.Count > 0 ? countries.FirstOrDefault().CountryId : 0);

            var regions = RegionService.GetRegions(_model.CountryId);
            SettingsMain.SellerRegionId = regions.Any(x => x.RegionId == _model.RegionId) ? _model.RegionId : 0;

            SettingsMain.City = _model.City.DefaultOrEmpty();

            SettingsMain.Phone = _model.Phone.DefaultOrEmpty();
            //SettingsMain.MobilePhone = _model.MobilePhone.DefaultOrEmpty();
            

            // Sales plan settings
            _model.SalesPlan = _model.SalesPlan >= 0 ? _model.SalesPlan : 0;
            _model.ProfitPlan = _model.ProfitPlan >= 0 ? _model.ProfitPlan : 0;

            OrderStatisticsService.SetProfitPlan(_model.SalesPlan, _model.ProfitPlan);

        }

        private void TrackEvents()
        {
            bool haveChanges = false;
            if (SettingsMain.ShopName != _model.StoreName)
            {
                TrialService.TrackEvent(TrialEvents.ChangeShopName, "");
                haveChanges = true;
            }

            if (SettingsMain.Phone != _model.Phone)
            {
                TrialService.TrackEvent(TrialEvents.ChangePhoneNumber, "");
                haveChanges = true;
            }

            if (SettingsMain.SiteUrl != _model.StoreUrl)
            {
                TrialService.TrackEvent(TrialEvents.ChangeDomain, "");
                haveChanges = true;
            }

            haveChanges |= SettingsMain.SellerCountryId != _model.CountryId;
            haveChanges |= SettingsMain.SellerRegionId != _model.RegionId;
            haveChanges |= SettingsMain.City != _model.City;
            if (haveChanges)
                TrialService.TrackEvent(ETrackEvent.Trial_ChangeAboutData);
        }
    }
}
