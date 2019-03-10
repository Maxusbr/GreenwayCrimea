
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AdvantShop.ExportImport;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Repository.Currencies;
using Newtonsoft.Json;

namespace AdvantShop.Web.Admin.Models.ExportFeeds
{
    public class ExportFeedSettingsYandexModel : IValidatableObject
    {
        public ExportFeedSettingsYandexModel(ExportFeedYandexOptions exportFeedYandexOptions)
        {
            Currency = exportFeedYandexOptions.Currency;
            RemoveHtml = exportFeedYandexOptions.RemoveHtml;
            Delivery = exportFeedYandexOptions.Delivery;
            Pickup = exportFeedYandexOptions.Pickup;
            DeliveryCost = exportFeedYandexOptions.DeliveryCost;
           //GlobalDeliveryCost = exportFeedYandexOptions.GlobalDeliveryCost;
            //LocalDeliveryOption = exportFeedYandexOptions.LocalDeliveryOption;
            ExportProductProperties = exportFeedYandexOptions.ExportProductProperties;
            ExportProductDiscount = exportFeedYandexOptions.ExportProductDiscount;
            SalesNotes = exportFeedYandexOptions.SalesNotes;
            ShopName = exportFeedYandexOptions.ShopName;
            CompanyName = exportFeedYandexOptions.CompanyName;
            ColorSizeToName = exportFeedYandexOptions.ColorSizeToName;
            ProductDescriptionType = exportFeedYandexOptions.ProductDescriptionType;
            OfferIdType = exportFeedYandexOptions.OfferIdType;
            ExportNotAvailable = exportFeedYandexOptions.ExportNotAvailable;
            AllowPreOrderProducts = exportFeedYandexOptions.AllowPreOrderProducts ?? true;
            //Available = exportFeedYandexOptions.Available;
            ExportPurchasePrice = exportFeedYandexOptions.ExportPurchasePrice;
            ExportRelatedProducts = exportFeedYandexOptions.ExportRelatedProducts;
            Store = exportFeedYandexOptions.Store;
            ExportBarCode = exportFeedYandexOptions.ExportBarCode;
            ExportAllPhotos = exportFeedYandexOptions.ExportAllPhotos;
            TypeExportYandex = exportFeedYandexOptions.TypeExportYandex;


            LocalDeliveryOption = new ExportFeedYandexDeliveryCostOption();
            try
            {
                LocalDeliveryOption =
                    JsonConvert.DeserializeObject<ExportFeedYandexDeliveryCostOption>(exportFeedYandexOptions.LocalDeliveryOption);
            }
            catch (Exception)
            {
            }
            finally
            {
                if (LocalDeliveryOption == null)
                    LocalDeliveryOption = new ExportFeedYandexDeliveryCostOption();
            }

            GlobalDeliveryCost = new List<ExportFeedYandexDeliveryCostOption>();
            try
            {
                if (!string.IsNullOrWhiteSpace(exportFeedYandexOptions.GlobalDeliveryCost))
                {
                    GlobalDeliveryCost =
                        JsonConvert.DeserializeObject<List<ExportFeedYandexDeliveryCostOption>>(exportFeedYandexOptions.GlobalDeliveryCost);

                    GlobalDeliveryCostJson = exportFeedYandexOptions.GlobalDeliveryCost;
                }
            }
            finally
            {
                if (GlobalDeliveryCost == null)
                    GlobalDeliveryCost = new List<ExportFeedYandexDeliveryCostOption>();
            }

            Bid = exportFeedYandexOptions.Bid ?? 0;
        }

        public int ExportFeedId { get; set; }

        public string Currency { get; set; }
        public bool RemoveHtml { get; set; }
        public bool Delivery { get; set; }
        public bool Pickup { get; set; }
        public ExportFeedYandexDeliveryCost DeliveryCost { get; set; }
        //public string GlobalDeliveryCost { get; set; }
        //public string LocalDeliveryOption { get; set; }
        public bool ExportProductProperties { get; set; }
        public bool ExportProductDiscount { get; set; }
        public string SalesNotes { get; set; }
        public string ShopName { get; set; }
        public string CompanyName { get; set; }
        public bool ColorSizeToName { get; set; }
        public string ProductDescriptionType { get; set; }
        public string OfferIdType { get; set; }
        public bool ExportNotAvailable { get; set; }
        public bool AllowPreOrderProducts { get; set; }        
        public bool Available { get; set; }
        public bool ExportPurchasePrice { get; set; }
        public bool ExportRelatedProducts { get; set; }
        public bool ExportAllPhotos { get; set; }
        public bool Store { get; set; }
        public bool ExportBarCode { get; set; }

        public float Bid { get; set; }
        public bool TypeExportYandex { get; set; }

        public ExportFeedYandexDeliveryCostOption LocalDeliveryOption { get; set; }

        public List<ExportFeedYandexDeliveryCostOption> GlobalDeliveryCost { get; set; }

        public string GlobalDeliveryCostJson { get; set; }

        public Dictionary<ExportFeedYandexDeliveryCost, string> DeliveryCostList
        {
            get
            {
                var deliveryCostList = new Dictionary<ExportFeedYandexDeliveryCost, string>();
                foreach (ExportFeedYandexDeliveryCost deliveryCost in Enum.GetValues(typeof(ExportFeedYandexDeliveryCost)))
                {
                    deliveryCostList.Add(deliveryCost, deliveryCost.Localize());
                }
                return deliveryCostList;
            }
        }

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

        public Dictionary<string, string> DeliveryCostTypes
        {
            get
            {
                return new Dictionary<string, string> {
                    {ExportFeedYandexDeliveryCost.None.ToString(),ExportFeedYandexDeliveryCost.None.Localize()},
                    {ExportFeedYandexDeliveryCost.LocalDeliveryCost.ToString(),ExportFeedYandexDeliveryCost.LocalDeliveryCost.Localize() },
                    {ExportFeedYandexDeliveryCost.GlobalDeliveryCost.ToString(),ExportFeedYandexDeliveryCost.GlobalDeliveryCost.Localize() }
                };
            }
        }



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(ShopName))
            {
                yield return new ValidationResult(LocalizationService.GetResource("Admin.Category.AdminCategoryModel.Error.Name"), new[] { "ShopName" });
            }
            if (string.IsNullOrEmpty(CompanyName))
            {
                yield return new ValidationResult(LocalizationService.GetResource("Admin.Category.AdminCategoryModel.Error.Name"), new[] { "CompanyName" });
            }
        }
    }
}
