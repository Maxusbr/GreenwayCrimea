//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Helpers;
using AdvantShop.Statistic;

using Newtonsoft.Json;
using AdvantShop.Repository.Currencies;
using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.ExportImport
{
    [ExportFeedKey("GoogleMerchentCenter")]
    public class ExportFeedGoogleMerchantCenter : BaseExportFeed
    {
        const string GoogleBaseNamespace = "http://base.google.com/ns/1.0";

        private List<ProductDiscount> _productDiscountModels = null;

        private readonly IEnumerable<ExportFeedProductModel> _products;
        private readonly ExportFeedGoogleMerchantCenterOptions _googleBaseOptions;
        private readonly int _productsCount;
        private readonly int _categoriesCount;
        private Currency _currency;

        public ExportFeedGoogleMerchantCenter()
        {

        }

        public ExportFeedGoogleMerchantCenter(IEnumerable<ExportFeedCategories> categories, IEnumerable<ExportFeedProductModel> products, ExportFeedSettings options, int categoriesCount, int productsCount)
            : base(categories, products, options, categoriesCount, productsCount)
        {
            _products = products;
            _googleBaseOptions = (ExportFeedGoogleMerchantCenterOptions)options;
            _productsCount = productsCount;
            _categoriesCount = categoriesCount;
            _currency = CurrencyService.GetCurrencyByIso3(_googleBaseOptions.Currency);
        }

        public static List<string> AvailableFileExtentions
        {
            get { return new List<string> { "xml" }; }
        }

        [ObsoleteAttribute("This method is obsolete. Use Export method", false)]
        public override void Build()
        {
            var discountModule = AttachedModules.GetModules<IDiscount>().FirstOrDefault();
            if (discountModule != null)
            {
                var classInstance = (IDiscount)Activator.CreateInstance(discountModule);
                _productDiscountModels = classInstance.GetProductDiscountsList();
            }

            var fileName = _googleBaseOptions.FileName + "." + _googleBaseOptions.FileExtention;
            var filePath = SettingsGeneral.AbsolutePath + "/" + fileName;
            var directory = filePath.Substring(0, filePath.LastIndexOf('/'));


            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            FileHelpers.DeleteFile(filePath);

            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            {
                var settings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true };
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    // source http://www.google.com/support/merchants/bin/answer.py?answer=188494&expand=GB
                    writer.WriteStartDocument();

                    writer.WriteStartElement("rss");
                    writer.WriteAttributeString("version", "2.0");
                    writer.WriteAttributeString("xmlns", "g", null, GoogleBaseNamespace);
                    writer.WriteStartElement("channel");
                    writer.WriteElementString("title", _googleBaseOptions.DatafeedTitle.Replace("#STORE_NAME#", SettingsMain.ShopName));
                    writer.WriteElementString("link", SettingsMain.SiteUrl);
                    writer.WriteElementString("description", _googleBaseOptions.DatafeedDescription.Replace("#STORE_NAME#", SettingsMain.ShopName));

                    CommonStatistic.TotalRow = _productsCount;
                    foreach (ExportFeedGoogleMerchantCenterProduct productRow in _products)
                    {
                        ProcessProductRow(productRow, writer, _googleBaseOptions);
                        CommonStatistic.RowPosition++;
                    }

                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
        }

        private void ProcessProductRow(ExportFeedGoogleMerchantCenterProduct row, XmlWriter writer, ExportFeedSettings commonOptions)
        {
            var advancedSettings = JsonConvert.DeserializeObject<ExportFeedGoogleMerchantCenterOptions>(commonOptions.AdvancedSettings);

            writer.WriteStartElement("item");

            #region Основные сведения о товарах

            switch (advancedSettings.OfferIdType)
            {
                case "id":
                    writer.WriteElementString("g", "id", GoogleBaseNamespace, row.OfferId.ToString(CultureInfo.InvariantCulture));
                    break;
                case "artno":
                    writer.WriteElementString("g", "id", GoogleBaseNamespace, row.OfferArtNo.ToString(CultureInfo.InvariantCulture));
                    break;
                default:
                    writer.WriteElementString("g", "id", GoogleBaseNamespace, row.OfferId.ToString(CultureInfo.InvariantCulture));
                    break;
            }

            //title [title]
            writer.WriteStartElement("title");
            var title = row.Name +
                (advancedSettings.ColorSizeToName && !row.SizeName.IsNullOrEmpty() ? " " + row.SizeName : string.Empty) +
                (advancedSettings.ColorSizeToName && !row.ColorName.IsNullOrEmpty() ? " " + row.ColorName : string.Empty);
            //title should be not longer than 150 characters
            if (title.Length > 150)
                title = title.Substring(0, 150);
            writer.WriteCData(title);
            writer.WriteEndElement();

            //description

            if (!string.Equals(advancedSettings.ProductDescriptionType, "none"))
            {
                var desc = advancedSettings.ProductDescriptionType == "full" ? row.Description : row.BriefDescription;
                if (advancedSettings.RemoveHtml)
                    desc = StringHelper.RemoveHTML(desc);

                if (desc.IsNotEmpty())
                {
                    writer.WriteStartElement("g", "description", GoogleBaseNamespace);
                    writer.WriteCData(desc);
                    writer.WriteEndElement();
                }
            }
            else
            {
                writer.WriteStartElement("g", "description", GoogleBaseNamespace);
                writer.WriteEndElement();
            }

            //google_product_category http://www.google.com/support/merchants/bin/answer.py?answer=160081
            var googleProductCategory = row.GoogleProductCategory;
            if (string.IsNullOrEmpty(googleProductCategory))
                googleProductCategory = advancedSettings.GoogleProductCategory;
            if (!googleProductCategory.IsNullOrEmpty())
            {
                writer.WriteStartElement("g", "google_product_category", GoogleBaseNamespace);
                writer.WriteCData(googleProductCategory);
                writer.WriteEndElement();
            }

            //product_type
            var localPath = string.Empty;
            var cats =
                CategoryService.GetParentCategories(row.ParentCategory)
                    .Reverse()
                    .Select(cat => new { Name = cat.Name, Url = UrlService.GetLink(ParamType.Category, cat.UrlPath, cat.ID) })
                    .ToList();

            for (var i = 0; i < cats.Count; i++)
            {
                var cat = cats[i];
                localPath = localPath + cat.Name;
                if (i == cats.Count - 1) continue;
                localPath = localPath + " > ";
            }
            writer.WriteStartElement("g", "product_type", GoogleBaseNamespace);
            writer.WriteCData(localPath);
            writer.WriteEndElement();

            if (row.Adult)
                writer.WriteElementString("g", "adult", GoogleBaseNamespace, row.Adult.ToString());


            //link
            var urlTags = GetAdditionalUrlTags(row, commonOptions.AdditionalUrlTags);
            writer.WriteElementString("link", SettingsMain.SiteUrl.TrimEnd('/') + "/" + UrlService.GetLink(ParamType.Product, row.UrlPath, row.ProductId, (!string.IsNullOrEmpty(urlTags) ? urlTags : string.Empty)));

            //image link
            if (!string.IsNullOrEmpty(row.Photos))
            {
                var temp = row.Photos.Split(',');
                for (var i = 0; i < temp.Length && i < 11; i++)
                    writer.WriteElementString("g", i == 0 ? "image_link" : "additional_image_link", GoogleBaseNamespace, GetImageProductPath(temp[i]));
            }

            //condition 
            writer.WriteElementString("g", "condition", GoogleBaseNamespace, "new");
            #endregion

            #region наличие и цена
            //availability
            string availability = "in stock";
            if (row.Amount == 0)
                if (advancedSettings.AllowPreOrderProducts && row.AllowPreorder) availability = "preorder";
                else availability = "out_of_stock";

            writer.WriteElementString("g", "availability", GoogleBaseNamespace, availability);


            float discount = 0;
            if (_productDiscountModels != null)
            {
                var prodDiscount = _productDiscountModels.Find(d => d.ProductId == row.ProductId);
                if (prodDiscount != null)
                {
                    discount = prodDiscount.Discount;
                }
            }
            
            var discountPrice = discount > 0 && discount > row.Discount ? new Discount(discount, 0) : new Discount(row.Discount, row.DiscountAmount);

            var price =
                Math.Round(PriceService.GetFinalPrice(row.Price + (row.Price / 100 * commonOptions.PriceMargin), discountPrice, row.CurrencyValue, _currency));

            writer.WriteElementString("g", "price", GoogleBaseNamespace, price.ToString() + " " + advancedSettings.Currency);



            #endregion

            #region Уникальные идентификаторы товаров
            //GTIN 
            var gtin = row.Gtin;
            if (!string.IsNullOrEmpty(gtin))
            {
                writer.WriteStartElement("g", "gtin", GoogleBaseNamespace);
                writer.WriteCData(gtin);
                writer.WriteFullEndElement(); // g:gtin
            }

            //brand 
            if (!string.IsNullOrEmpty(row.BrandName))
            {
                writer.WriteStartElement("g", "brand", GoogleBaseNamespace);
                writer.WriteCData(row.BrandName);
                writer.WriteFullEndElement(); // g:brand
            }

            //mpn [mpn]
            if (!string.IsNullOrEmpty(row.ArtNo))
            {
                writer.WriteStartElement("g", "mpn", GoogleBaseNamespace);
                writer.WriteCData(row.ArtNo);
                writer.WriteFullEndElement(); // g:mpn
            }
            #endregion

            #region Варианты товара
            if (row.ColorName.IsNotEmpty() || row.SizeName.IsNotEmpty())
            {
                //item_group_id
                writer.WriteElementString("g", "item_group_id", GoogleBaseNamespace, row.ProductId.ToString());
                //color
                if (row.ColorName.IsNotEmpty())
                {
                    writer.WriteElementString("g", "color", GoogleBaseNamespace, row.ColorName);
                }
                //size
                if (row.SizeName.IsNotEmpty())
                {
                    writer.WriteElementString("g", "size", GoogleBaseNamespace, row.SizeName);
                }
            }

            #endregion

            #region Tax & Shipping
            #endregion

            writer.WriteElementString("g", "expiration_date", GoogleBaseNamespace, DateTime.Now.AddDays(28).ToString("yyyy-MM-dd"));
            writer.WriteEndElement();
        }


        public override string Export(int exportFeedId)
        {
            var commonSettings = ExportFeedSettingsProvider.GetSettings(exportFeedId);
            var advancedSettings = JsonConvert.DeserializeObject<ExportFeedGoogleMerchantCenterOptions>(commonSettings.AdvancedSettings);

            return Export(
                ExportFeedGoogleMerchantCenterService.GetCategories(exportFeedId),
                ExportFeedGoogleMerchantCenterService.GetProducts(exportFeedId, commonSettings, advancedSettings),
                commonSettings,
                ExportFeedGoogleMerchantCenterService.GetCategoriesCount(exportFeedId),
                ExportFeedGoogleMerchantCenterService.GetProductsCount(exportFeedId, commonSettings, advancedSettings));
        }

        public override string Export(IEnumerable<ExportFeedCategories> categories, IEnumerable<ExportFeedProductModel> products, ExportFeedSettings options, int categoriesCount, int productsCount)
        {
            var advancedSettings = JsonConvert.DeserializeObject<ExportFeedGoogleMerchantCenterOptions>(options.AdvancedSettings);

            _currency = CurrencyService.GetCurrencyByIso3(advancedSettings.Currency);
            var discountModule = AttachedModules.GetModules<IDiscount>().FirstOrDefault();
            if (discountModule != null)
            {
                var classInstance = (IDiscount)Activator.CreateInstance(discountModule);
                _productDiscountModels = classInstance.GetProductDiscountsList();
            }

            var exportFile = new FileInfo(options.FileFullPath);
            if (!string.IsNullOrEmpty(exportFile.Directory.FullName))
            {
                FileHelpers.CreateDirectory(exportFile.Directory.FullName);
            }
            FileHelpers.DeleteFile(exportFile.FullName);

            CommonStatistic.FileName = "../" + options.FileFullName;

            using (var stream = new FileStream(exportFile.FullName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            {
                var settings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true };
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    // source http://www.google.com/support/merchants/bin/answer.py?answer=188494&expand=GB
                    writer.WriteStartDocument();

                    writer.WriteStartElement("rss");
                    writer.WriteAttributeString("version", "2.0");
                    writer.WriteAttributeString("xmlns", "g", null, GoogleBaseNamespace);
                    writer.WriteStartElement("channel");
                    writer.WriteElementString("title", advancedSettings.DatafeedTitle.Replace("#STORE_NAME#", SettingsMain.ShopName));
                    writer.WriteElementString("link", SettingsMain.SiteUrl);
                    writer.WriteElementString("description", advancedSettings.DatafeedDescription.Replace("#STORE_NAME#", SettingsMain.ShopName));

                    CommonStatistic.TotalRow = productsCount;
                    foreach (ExportFeedGoogleMerchantCenterProduct productRow in products)
                    {
                        ProcessProductRow(productRow, writer, options);
                        CommonStatistic.RowPosition++;
                    }

                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            CommonStatistic.IsRun = false;
            return options.FileFullName;
        }

        public override void SetDefaultSettings(int exportFeedId)
        {
            ExportFeedSettingsProvider.SetSettings(exportFeedId, new ExportFeedSettings
            {
                Interval = 1,
                IntervalType = Core.Scheduler.TimeIntervalType.Hours,
                Active = false,

                PriceMargin = 0,
                FileName = System.IO.File.Exists(Configuration.SettingsGeneral.AbsolutePath + "/export/google.xml") ? "export/google" + exportFeedId : "export/google",
                FileExtention = AvailableFileExtentions[0],

                AdditionalUrlTags = string.Empty,

                AdvancedSettings = JsonConvert.SerializeObject(new ExportFeedGoogleMerchantCenterOptions
                {
                    ProductDescriptionType = "short",
                    DatafeedTitle = "#STORE_NAME#",
                    DatafeedDescription = "#STORE_NAME#",
                    Currency = CurrencyService.CurrentCurrency.Iso3,
                    RemoveHtml = true
                })
            });
        }

        public override List<string> GetAvailableVariables()
        {
            return new List<string> { "#STORE_NAME#", "#STORE_URL#", "#PRODUCT_NAME#", "#PRODUCT_ID#", "#PRODUCT_ARTNO#" };
        }

        public override List<ExportFeedCategories> GetCategories(int exportFeedId)
        {
            throw new NotImplementedException();
        }

        public override List<ExportFeedProductModel> GetProducts(int exportFeedId)
        {
            throw new NotImplementedException();
        }

        public override int GetProductsCount(int exportFeedId)
        {
            var commonSettings = ExportFeedSettingsProvider.GetSettings(exportFeedId);
            var advancedSettings = JsonConvert.DeserializeObject<ExportFeedGoogleMerchantCenterOptions>(commonSettings.AdvancedSettings);

            return ExportFeedGoogleMerchantCenterService.GetProductsCount(exportFeedId, commonSettings, advancedSettings);
        }

        public override int GetCategoriesCount(int exportFeedId)
        {
            return ExportFeedGoogleMerchantCenterService.GetCategoriesCount(exportFeedId);
        }

        public override List<string> GetAvailableFileExtentions()
        {
            return new List<string> { "xml" };
        }
    }
}