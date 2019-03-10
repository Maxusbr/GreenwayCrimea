using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Configuration.Settings;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Localization;
using AdvantShop.Saas;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.ApiSettings
{
    public class LoadSaveApiSettingsHandler
    {
        private readonly APISettingsModel _model;
        private readonly bool _isRu = Culture.Language == Culture.SupportLanguage.Russian ||
                              Culture.Language == Culture.SupportLanguage.Ukrainian;

        public LoadSaveApiSettingsHandler(APISettingsModel model)
        {
            _model = model;
        }

        public APISettingsModel Load()
        {
            var model = new APISettingsModel();

            var siteUrl = SettingsMain.SiteUrl.TrimEnd('/');
            var key = SettingsApi.ApiKey;

            model.Key = key;
            model.IsRus = _isRu;
            model._1CEnabled = Settings1C.Enabled;
            model._1CDisableProductsDecremention = Settings1C.DisableProductsDecremention;
            model._1CUpdateStatuses = Settings1C.UpdateStatuses;

            model.ExportOrdersType = Settings1C.OnlyUseIn1COrders;
            model.ExportOrders = new List<SelectListItem>();
            model.ExportOrders.Add(new SelectListItem() { Text = LocalizationService.GetResource("Admin.Settings.API.ExportOrdersType.SelectValue0"), Value = "True", Selected = model.ExportOrdersType });
            model.ExportOrders.Add(new SelectListItem() { Text = LocalizationService.GetResource("Admin.Settings.API.ExportOrdersType.SelectValue1"), Value = "False", Selected = model.ExportOrdersType });

            model._1CUpdateProducts = Settings1C.UpdateProducts;
            model.UpdateProducts = new List<SelectListItem>();
            model.UpdateProducts.Add(new SelectListItem() { Text = LocalizationService.GetResource("Admin.Settings.API.1CUpdateProducts.SelectValue0"), Value = "True", Selected = model._1CUpdateProducts });
            model.UpdateProducts.Add(new SelectListItem() { Text = LocalizationService.GetResource("Admin.Settings.API.1CUpdateProducts.SelectValue1"), Value = "False", Selected = model._1CUpdateProducts });

            model._1CSendProducts = Settings1C.SendAllProducts;
            model.SendProducts = new List<SelectListItem>();
            model.SendProducts.Add(new SelectListItem() { Text = LocalizationService.GetResource("Admin.Settings.API.1CSendProducts.SelectValue0"), Value = "True", Selected = model._1CSendProducts });
            model.SendProducts.Add(new SelectListItem() { Text = LocalizationService.GetResource("Admin.Settings.API.1CSendProducts.SelectValue1"), Value = "False", Selected = model._1CSendProducts });

            model.ImportPhotosUrl = siteUrl + "/api/1c/importphotos?apikey=" + key;
            model.ImportProductsUrl = siteUrl + "/api/1c/importproducts?apikey=" + key;
            model.ExportProductsUrl = siteUrl + "/api/1c/exportproducts?apikey=" + key;
            model.DeletedProducts = siteUrl + "/api/1c/deletedproducts?apikey=" + key;
            model.ExportOrdersUrl = siteUrl + "/api/1c/exportorders?apikey=" + key;
            model.ChangeOrderStatusUrl = siteUrl + "/api/1c/changeorderstatus?apikey=" + key;
            model.DeletedOrdersUrl = siteUrl + "/api/1c/deletedorders?apikey=" + key;

            model.LeadAddUrl = siteUrl + "/api/leads/add?apikey=" + key;
            model.VkUrl = siteUrl + "/api/vk?apikey=" + key;

            model.ShowOneC = !SaasDataService.IsSaasEnabled || SaasDataService.CurrentSaasData.Have1C;

            return model;
        }

        public void Save()
        {
            SettingsApi.ApiKey = _model.Key;
            Settings1C.Enabled = _model._1CEnabled;
            Settings1C.DisableProductsDecremention = _model._1CDisableProductsDecremention;
            Settings1C.UpdateStatuses = _model._1CUpdateStatuses;
            Settings1C.OnlyUseIn1COrders = _model.ExportOrdersType;
            Settings1C.UpdateProducts = _model._1CUpdateProducts;
            Settings1C.SendAllProducts = _model._1CSendProducts;
        }
    }
}
