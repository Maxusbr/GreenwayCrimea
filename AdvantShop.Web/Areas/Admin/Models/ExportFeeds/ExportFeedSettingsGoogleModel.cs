
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AdvantShop.Core.Services.Localization;
using AdvantShop.ExportImport;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Web.Admin.Models.ExportFeeds
{
    public class ExportFeedSettingsGoogleModel : IValidatableObject
    {
        public ExportFeedSettingsGoogleModel(ExportFeedGoogleMerchantCenterOptions exportFeedGoogleMerchantCenterOptions)
        {
            DatafeedTitle = exportFeedGoogleMerchantCenterOptions.DatafeedTitle;
            DatafeedDescription = exportFeedGoogleMerchantCenterOptions.DatafeedDescription;
            Currency = exportFeedGoogleMerchantCenterOptions.Currency;
            RemoveHtml = exportFeedGoogleMerchantCenterOptions.RemoveHtml;
            GoogleProductCategory = exportFeedGoogleMerchantCenterOptions.GoogleProductCategory;
            ProductDescriptionType = exportFeedGoogleMerchantCenterOptions.ProductDescriptionType;
            OfferIdType = exportFeedGoogleMerchantCenterOptions.OfferIdType;
            ExportNotAvailable = exportFeedGoogleMerchantCenterOptions.ExportNotAvailable;
            AllowPreOrderProducts = exportFeedGoogleMerchantCenterOptions.AllowPreOrderProducts;
        }

        public string Currency { get; set; }
        public bool RemoveHtml { get; set; }
        public string DatafeedTitle { get; set; }
        public string DatafeedDescription { get; set; }
        public string GoogleProductCategory { get; set; }
        public string ProductDescriptionType { get; set; }
        public string OfferIdType { get; set; }
        public bool ExportNotAvailable { get; set; }
        public bool AllowPreOrderProducts { get; set; }

        public Dictionary<string, string> Currencies
        {
            get
            {
                var currencyList = new Dictionary<string, string>();
                foreach (var item in CurrencyService.GetAllCurrencies().Where(item => ExportFeedYandex.AvailableCurrencies.Contains(item.Iso3)).ToList())
                {
                    currencyList.Add(item.Iso3, item.Name);
                }
                return currencyList;
            }
        }

        public Dictionary<string, string> ProductDescriptionTypeList
        {
            get
            {
                return new Dictionary<string, string> {
                    { "short", LocalizationService.GetResource("Admin.ExportFeed.Settings.BriefDescription") },
                    { "full", LocalizationService.GetResource("Admin.ExportFeed.Settings.FullDescription") },
                    { "none", LocalizationService.GetResource("Admin.ExportFeed.Settings.DontUseDescription") }
                };
            }
        }

        public Dictionary<string, string> OfferTypes
        {
            get
            {
                return new Dictionary<string, string> {
                    { "id",  LocalizationService.GetResource("Admin.ExportFeed.Settings.OfferId") },
                    { "artno",  LocalizationService.GetResource("Admin.ExportFeed.Settings.OfferSku")}
                };
            }
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(DatafeedTitle))
            {
                yield return new ValidationResult(LocalizationService.GetResource("Admin.Category.AdminCategoryModel.Error.Name"), new[] { "DatafeedTitle" });
            }
            if (string.IsNullOrEmpty(DatafeedDescription))
            {
                yield return new ValidationResult(LocalizationService.GetResource("Admin.Category.AdminCategoryModel.Error.Name"), new[] { "DatafeedDescription" });
            }
        }
    }
}
