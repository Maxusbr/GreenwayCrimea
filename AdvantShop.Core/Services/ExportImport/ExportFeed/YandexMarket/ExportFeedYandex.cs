//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Repository.Currencies;
using AdvantShop.Statistic;
using System.Web;
using Newtonsoft.Json;
using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.ExportImport
{
    [ExportFeedKey("YandexMarket")]
    public class ExportFeedYandex : BaseExportFeed
    {
        private readonly List<Currency> _currencies;
        private readonly ExportFeedYandexOptions _yandexOptions;

        private readonly int _productsCount;
        private readonly int _categoriesCount;
        private readonly IEnumerable<ExportFeedCategories> _categories;
        private readonly IEnumerable<ExportFeedProductModel> _products;


        //private readonly ExportFeedYandexDeliveryCostOption _localDeliveryOption;

        public ExportFeedYandex()
        { }

        public ExportFeedYandex(IEnumerable<ExportFeedCategories> categories, IEnumerable<ExportFeedProductModel> products, ExportFeedSettings options, int categoriesCount, int productsCount)
            : base(categories, products, options, categoriesCount, productsCount)
        {
            _categories = categories;
            _products = products;
            _currencies = CurrencyService.GetAllCurrencies();
            _yandexOptions = (ExportFeedYandexOptions)options;

            _productsCount = productsCount;
            _categoriesCount = categoriesCount;
            //_localDeliveryOption = GetLocalDeliveryOption(_yandexOptions.LocalDeliveryOption);
        }

        private List<ProductDiscount> _productDiscountModels = null;

        public static List<string> AvailableCurrencies
        {
            get { return new List<string> { "RUB", "RUR", "USD", "BYR", "KZT", "EUR", "UAH" }; }
        }

        public static List<string> AvailableFileExtentions
        {
            get { return new List<string> { "xml", "yml" }; }
        }

        [Obsolete("This method is obsolete. Use Export method", false)]
        public override void Build()
        {
            try
            {
                var discountModule = AttachedModules.GetModules<IDiscount>().FirstOrDefault();
                if (discountModule != null)
                {
                    var classInstance = (IDiscount)Activator.CreateInstance(discountModule);
                    _productDiscountModels = classInstance.GetProductDiscountsList();
                }

                var fileName = _yandexOptions.FileName + "." + _yandexOptions.FileExtention;
                var filePath = SettingsGeneral.AbsolutePath + "/" + fileName;
                var directory = filePath.Substring(0, filePath.LastIndexOf('/'));


                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                FileHelpers.DeleteFile(filePath);

                using (var outputFile = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    var settings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true };
                    using (var writer = XmlWriter.Create(outputFile, settings))
                    {


                        writer.WriteStartDocument();
                        writer.WriteDocType("yml_catalog", null, "shops.dtd", null);
                        writer.WriteStartElement("yml_catalog");
                        writer.WriteAttributeString("date", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                        writer.WriteStartElement("shop");

                        writer.WriteStartElement("name");
                        writer.WriteRaw(_yandexOptions.ShopName.Replace("#STORE_NAME#", SettingsMain.ShopName).XmlEncode());
                        writer.WriteEndElement();

                        writer.WriteStartElement("company");
                        writer.WriteRaw(_yandexOptions.CompanyName.Replace("#STORE_NAME#", SettingsMain.ShopName).XmlEncode());
                        writer.WriteEndElement();

                        writer.WriteStartElement("url");
                        writer.WriteRaw(SettingsMain.SiteUrl);
                        writer.WriteEndElement();

                        writer.WriteStartElement("currencies");
                        ProcessCurrency(_currencies, _yandexOptions.Currency, writer);
                        writer.WriteEndElement();

                        CommonStatistic.TotalRow = _categoriesCount + _productsCount;

                        writer.WriteStartElement("categories");
                        foreach (ExportFeedCategories categoryRow in _categories)
                        {
                            ProcessCategoryRow(categoryRow, writer);
                            CommonStatistic.RowPosition++;
                        }
                        writer.WriteEndElement();

                        if ((_yandexOptions.DeliveryCost == ExportFeedYandexDeliveryCost.GlobalDeliveryCost ||
                             _yandexOptions.DeliveryCost == ExportFeedYandexDeliveryCost.LocalDeliveryCost) &&
                            !string.IsNullOrWhiteSpace(_yandexOptions.GlobalDeliveryCost))
                        {
                            try
                            {
                                var deliveryOptions =
                                    JsonConvert.DeserializeObject<List<ExportFeedYandexDeliveryCostOption>>(_yandexOptions.GlobalDeliveryCost);

                                writer.WriteStartElement("delivery-options");

                                foreach (var deliveryOption in deliveryOptions)
                                {
                                    writer.WriteStartElement("option");
                                    writer.WriteAttributeString("cost", deliveryOption.Cost);
                                    writer.WriteAttributeString("days", deliveryOption.Days);
                                    writer.WriteAttributeString("order-before", deliveryOption.OrderBefore);
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                            }
                            catch (Exception ex)
                            {
                                Debug.Log.Error(ex);
                            }
                        }

                        writer.WriteStartElement("offers");
                        var currency = _yandexOptions.Currency.IsNotEmpty() ? _currencies.FirstOrDefault(x => x.Iso3 == _yandexOptions.Currency) : _currencies.FirstOrDefault();
                        foreach (ExportFeedYandexProduct offerRow in _products)
                        {
                            ProcessProductRow(offerRow, writer, _yandexOptions, currency);
                            CommonStatistic.RowPosition++;
                        }
                        writer.WriteEndElement();

                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        writer.WriteEndDocument();

                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        private static void ProcessCurrency(List<Currency> currencies, string currency, XmlWriter writer)
        {
            if (currencies == null) return;
            var defaultCurrency = currencies.FirstOrDefault(item => item.Iso3 == currency);
            if (defaultCurrency == null) return;
            ProcessCurrencyRow(new Currency
            {
                CurrencyId = defaultCurrency.CurrencyId,
                Rate = 1,
                Iso3 = defaultCurrency.Iso3
            }, writer);

            foreach (var curRow in currencies.Where(item => item.Iso3 != currency))
            {
                curRow.Rate = Convert.ToSingle(curRow.Rate / defaultCurrency.Rate, CultureInfo.InvariantCulture);
                ProcessCurrencyRow(curRow, writer);
            }
        }

        private static void ProcessCurrencyRow(Currency currency, XmlWriter writer)
        {
            writer.WriteStartElement("currency");
            writer.WriteAttributeString("id", currency.Iso3);

            var nfi = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            nfi.NumberDecimalSeparator = ".";
            writer.WriteAttributeString("rate", Math.Round(currency.Rate, 2).ToString(nfi));
            writer.WriteEndElement();
        }

        private static void ProcessCategoryRow(ExportFeedCategories row, XmlWriter writer)
        {
            writer.WriteStartElement("category");
            writer.WriteAttributeString("id", row.Id.ToString(CultureInfo.InvariantCulture));
            if (row.ParentCategory != 0)
            {
                writer.WriteAttributeString("parentId", row.ParentCategory.ToString(CultureInfo.InvariantCulture));
            }
            writer.WriteRaw(row.Name.XmlEncode().RemoveInvalidXmlChars());
            writer.WriteEndElement();
        }

        private void ProcessProductRow(ExportFeedYandexProduct row, XmlWriter writer, ExportFeedSettings commonSettings, Currency currency)
        {
            var advancedSettings = ExportFeedSettingsProvider.ConvertAdvancedSettings<ExportFeedYandexOptions>(commonSettings.AdvancedSettings);
            var showVendorModel = !advancedSettings.TypeExportYandex && !string.IsNullOrEmpty(row.BrandName);

            writer.WriteStartElement("offer");

            switch (advancedSettings.OfferIdType)
            {
                case "id":
                    writer.WriteAttributeString("id", row.OfferId.ToString(CultureInfo.InvariantCulture));
                    break;
                case "artno":
                    writer.WriteAttributeString("id", row.OfferArtNo.ToString(CultureInfo.InvariantCulture));
                    break;
                default:
                    writer.WriteAttributeString("id", row.OfferId.ToString(CultureInfo.InvariantCulture));
                    break;
            }

            if(row.Amount > 0 || row.AllowPreorder && (advancedSettings.AllowPreOrderProducts ?? true)) 
                writer.WriteAttributeString("available", (row.Amount > 0 ? true : false).ToString().ToLower());

            writer.WriteAttributeString("group_id", row.ProductId.ToString(CultureInfo.InvariantCulture));

            if (showVendorModel)
            {
                writer.WriteAttributeString("type", "vendor.model");
            }

            if (advancedSettings.Bid != null && advancedSettings.Bid.Value > 0)
            {
                writer.WriteAttributeString("bid", advancedSettings.Bid.Value.ToString(CultureInfo.InvariantCulture));
            }
            if (row.Cbid > 0)
            {
                writer.WriteAttributeString("cbid", row.Cbid.ToString());
            }
            if (row.Fee > 0)
            {
                writer.WriteAttributeString("fee", row.Fee.ToString());
            }

            if (row.Cpa)
            {
                writer.WriteStartElement("cpa");
                writer.WriteRaw("0");
                writer.WriteEndElement();
            }

            writer.WriteStartElement("url");
            writer.WriteRaw(CreateLink(row, commonSettings.AdditionalUrlTags));
            writer.WriteEndElement();


            float discount = 0;
            if (_productDiscountModels != null)
            {
                var prodDiscount = _productDiscountModels.Find(d => d.ProductId == row.ProductId);
                if (prodDiscount != null)
                {
                    discount = prodDiscount.Discount;
                }
            }

            var priceDiscount = discount > 0 && discount > row.Discount ? new Discount(discount, 0) : new Discount(row.Discount, row.DiscountAmount);
            // TODO: Check discount!
            var newPrice =
              PriceService.GetFinalPrice((row.Price + (row.Price * commonSettings.PriceMargin / 100)), priceDiscount, row.CurrencyValue, currency);

            writer.WriteStartElement("price");
            var nfi = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            nfi.NumberDecimalSeparator = ".";
            writer.WriteRaw(newPrice.ToString(nfi));
            writer.WriteEndElement();

            if (advancedSettings.ExportPurchasePrice)
            {
                writer.WriteStartElement("purchase_price");
                writer.WriteRaw((Convert.ToInt32(row.SupplyPrice)).ToString(nfi));
                writer.WriteEndElement();
            }

            writer.WriteStartElement("currencyId");
            writer.WriteRaw(advancedSettings.Currency);
            writer.WriteEndElement();


            writer.WriteStartElement("categoryId");
            writer.WriteRaw(row.ParentCategory.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();

            if (!string.IsNullOrWhiteSpace(row.YandexMarketCategory))
            {
                writer.WriteStartElement("market_category");
                writer.WriteRaw(row.YandexMarketCategory.ToString(CultureInfo.InvariantCulture));
                writer.WriteEndElement();
            }


            if (!string.IsNullOrEmpty(row.Photos))
            {
                if (advancedSettings.ExportAllPhotos)
                {
                    var temp = row.Photos.Split(',').Take(10);
                    foreach (var item in temp.Where(item => !string.IsNullOrWhiteSpace(item)))
                    {
                        writer.WriteStartElement("picture");
                        writer.WriteRaw(GetImageProductPath(item));
                        writer.WriteEndElement();
                    }
                }
                else
                {
                    writer.WriteStartElement("picture");
                    writer.WriteRaw(GetImageProductPath(row.Photos.Split(',')[0]));
                    writer.WriteEndElement();
                }
            }

            writer.WriteStartElement("store");
            writer.WriteRaw(advancedSettings.Store.ToString().ToLower());
            writer.WriteEndElement();

            if (advancedSettings.Pickup)
            {
                writer.WriteStartElement("pickup");
                writer.WriteRaw("true");
                writer.WriteEndElement();
            }

            long barcode;
            if (advancedSettings.ExportBarCode && !string.IsNullOrEmpty(row.BarCode) && long.TryParse(row.BarCode, out barcode) &&
                (row.BarCode.Length == 8 || row.BarCode.Length == 12 || row.BarCode.Length == 13))
            {
                writer.WriteStartElement("barcode");
                writer.WriteRaw(barcode.ToString().ToLower());
                writer.WriteEndElement();
            }

            writer.WriteStartElement("delivery");
            writer.WriteRaw(advancedSettings.Delivery.ToString().ToLower());
            writer.WriteEndElement();

            if (showVendorModel)
            {
                if (!string.IsNullOrEmpty(row.YandexTypePrefix))
                {
                    writer.WriteStartElement("typePrefix");
                    writer.WriteRaw(row.YandexTypePrefix);
                    writer.WriteEndElement();
                }

                writer.WriteStartElement("vendor");
                writer.WriteRaw(row.BrandName.XmlEncode().RemoveInvalidXmlChars());
                writer.WriteEndElement();

                writer.WriteStartElement("country_of_origin");
                writer.WriteRaw(row.BrandCountry.XmlEncode().RemoveInvalidXmlChars());
                writer.WriteEndElement();
                writer.WriteStartElement("model");
                writer.WriteRaw(GetProductName(row, advancedSettings.ColorSizeToName, true));
                writer.WriteEndElement();

            }
            else
            {
                writer.WriteStartElement("name");
                writer.WriteRaw(GetProductName(row, advancedSettings.ColorSizeToName, false));
                writer.WriteEndElement();
            }

            // на 6 марта 2018г работает только в категориях Шины, Диски
            //if (row.MinAmount > 0)
            //{
            //    writer.WriteStartElement("min-quantity");
            //    writer.WriteRaw(row.MinAmount.ToString());
            //    writer.WriteEndElement();
            //}

            //if (row.Multiplicity > 0)
            //{
            //    writer.WriteStartElement("step-quantity");
            //    writer.WriteRaw(row.Multiplicity.ToString());
            //    writer.WriteEndElement();
            //}

            writer.WriteStartElement("description");
            if (!string.IsNullOrEmpty(advancedSettings.ProductDescriptionType) && !string.Equals(advancedSettings.ProductDescriptionType, "none"))
            {
                string desc = SQLDataHelper.GetString(advancedSettings.ProductDescriptionType == "full" ? row.Description : row.BriefDescription);

                if (advancedSettings.RemoveHtml)
                {
                    desc = StringHelper.RemoveHTML(desc);
                }

                writer.WriteRaw(desc.XmlEncode().RemoveInvalidXmlChars());
            }
            writer.WriteEndElement();


            writer.WriteStartElement("vendorCode");
            writer.WriteRaw(row.ArtNo.XmlEncode().RemoveInvalidXmlChars());
            writer.WriteEndElement();



            if (!string.IsNullOrWhiteSpace(row.SalesNote))
            {
                writer.WriteStartElement("sales_notes");
                writer.WriteRaw(row.SalesNote.XmlEncode().RemoveInvalidXmlChars());
                writer.WriteEndElement();
            }
            else if (!string.IsNullOrWhiteSpace(advancedSettings.SalesNotes))
            {
                writer.WriteStartElement("sales_notes");
                writer.WriteRaw(advancedSettings.SalesNotes.XmlEncode().RemoveInvalidXmlChars());
                writer.WriteEndElement();
            }

            var oldPrice = PriceService.GetFinalPrice(
              row.Price + (row.Price / 100 * commonSettings.PriceMargin),
              new Discount(), row.CurrencyValue, currency);
            if (oldPrice - newPrice > 1 && advancedSettings.ExportProductDiscount)
            {
                writer.WriteStartElement("oldprice");
                writer.WriteRaw(oldPrice.ToString(nfi));
                writer.WriteEndElement();
            }

            var localDeliveryOption = GetLocalDeliveryOption(advancedSettings.LocalDeliveryOption);
            if (advancedSettings.DeliveryCost == ExportFeedYandexDeliveryCost.LocalDeliveryCost && row.ShippingPrice >= 0 && localDeliveryOption != null)
            {
                writer.WriteStartElement("delivery-options");
                writer.WriteStartElement("option");
                writer.WriteAttributeString("cost", Math.Round(row.ShippingPrice.Value).ToString(nfi));
                writer.WriteAttributeString("days", !string.IsNullOrEmpty(localDeliveryOption.Days) ? localDeliveryOption.Days : "1-3");

                if (!string.IsNullOrEmpty(localDeliveryOption.OrderBefore))
                {
                    writer.WriteAttributeString("order-before", localDeliveryOption.OrderBefore);
                }

                writer.WriteEndElement();
                writer.WriteEndElement();
            }


            if (row.ManufacturerWarranty)
            {
                writer.WriteStartElement("manufacturer_warranty");
                writer.WriteRaw(row.ManufacturerWarranty.ToString().ToLower());
                writer.WriteEndElement();
            }

            if (row.Adult)
            {
                writer.WriteStartElement("adult");
                writer.WriteRaw(row.Adult.ToString().ToLower());
                writer.WriteEndElement();
            }

            if (row.Weight > 0)
            {
                writer.WriteStartElement("weight");
                writer.WriteRaw(row.Weight.ToString("F2").Replace(",", ".").ToLower());
                writer.WriteEndElement();
            }

            if (advancedSettings.ExportRelatedProducts)
            {
                var recProducts = ProductService.GetRelatedProducts(row.OfferId, RelatedType.Related);
                var result = string.Empty;
                for (int index = 0; index < recProducts.Count && index < 30; ++index)
                {
                    result += (index > 0 ? "," : string.Empty) + (advancedSettings.OfferIdType == "id" ? row.OfferId.ToString(CultureInfo.InvariantCulture) : row.OfferArtNo.ToString(CultureInfo.InvariantCulture));
                }

                if (!string.IsNullOrEmpty(result))
                {
                    writer.WriteStartElement("rec");
                    writer.WriteRaw(result);
                    writer.WriteEndElement();
                }
            }

            if (!string.IsNullOrWhiteSpace(row.ColorName))
            {
                writer.WriteStartElement("param");
                writer.WriteAttributeString("name", SettingsCatalog.ColorsHeader.XmlEncode().RemoveInvalidXmlChars());
                writer.WriteRaw(row.ColorName.XmlEncode().RemoveInvalidXmlChars());
                writer.WriteEndElement();
            }

            if (!string.IsNullOrWhiteSpace(row.SizeName))
            {
                writer.WriteStartElement("param");
                writer.WriteAttributeString("name", SettingsCatalog.SizesHeader.XmlEncode().RemoveInvalidXmlChars());
                if (!string.IsNullOrEmpty(row.YandexSizeUnit))
                {
                    writer.WriteAttributeString("unit", row.YandexSizeUnit);
                }
                writer.WriteRaw(row.SizeName.XmlEncode().RemoveInvalidXmlChars());
                writer.WriteEndElement();
            }

            if (advancedSettings.ExportProductProperties)
            {
                foreach (var prop in PropertyService.GetPropertyValuesByProductId(row.ProductId))
                {
                    if (prop.Property.Name.IsNotEmpty() && prop.Value.IsNotEmpty())
                    {
                        writer.WriteStartElement("param");
                        writer.WriteAttributeString("name", prop.Property.Name.XmlEncode().RemoveInvalidXmlChars());
                        if (prop.Property.Unit != null)
                        {
                            writer.WriteAttributeString("unit", prop.Property.Unit.XmlEncode().RemoveInvalidXmlChars());
                        }
                        writer.WriteRaw(prop.Value.XmlEncode().RemoveInvalidXmlChars());
                        writer.WriteEndElement();
                    }
                }
            }

            writer.WriteEndElement();
        }

        private string CreateLink(ExportFeedYandexProduct row, string additionalUrlTags)
        {
            var sufix = string.Empty;
            if (!row.Main)
            {
                if (row.ColorId != 0)
                {
                    sufix = "color=" + row.ColorId;
                }
                if (row.SizeId != 0)
                {
                    if (string.IsNullOrEmpty(sufix))
                        sufix = "size=" + row.SizeId;
                    else
                        sufix += "&size=" + row.SizeId;
                }
            }

            var urlTags = GetAdditionalUrlTags(row, additionalUrlTags);
            if (!string.IsNullOrEmpty(urlTags))
            {
                sufix += (!string.IsNullOrEmpty(sufix) ? "&" + urlTags : urlTags);
            }
            return HttpUtility.HtmlEncode(SettingsMain.SiteUrl.TrimEnd('/') + "/" + UrlService.GetLink(ParamType.Product, row.UrlPath, row.ProductId, sufix));
        }

        private string GetProductName(ExportFeedYandexProduct row, bool colorSizeToName, bool vendorModel)
        {
            var result = string.Empty;
            if (vendorModel && !row.YandexModel.IsNullOrEmpty())
            {
                result = row.YandexModel;
            }
            else
            {
                result = row.YandexName.IsNotEmpty() ? row.YandexName.XmlEncode().RemoveInvalidXmlChars() : row.Name.XmlEncode().RemoveInvalidXmlChars();
            }
            if (colorSizeToName)
            {
                result +=
                    (!string.IsNullOrWhiteSpace(row.SizeName) ? " " + row.SizeName.XmlEncode().RemoveInvalidXmlChars() : string.Empty) +
                    (!string.IsNullOrWhiteSpace(row.ColorName) ? " " + row.ColorName.XmlEncode().RemoveInvalidXmlChars() : string.Empty);
            }
            return result;
        }

        private ExportFeedYandexDeliveryCostOption GetLocalDeliveryOption(string localDeliveryOptionString)
        {
            var localDeliveryOption = new ExportFeedYandexDeliveryCostOption();

            try
            {
                localDeliveryOption =
                    JsonConvert.DeserializeObject<ExportFeedYandexDeliveryCostOption>(localDeliveryOptionString);
            }
            catch (Exception)
            {
            }
            finally
            {
                if (localDeliveryOption == null)
                    localDeliveryOption = new ExportFeedYandexDeliveryCostOption();
            }
            return localDeliveryOption;
        }



        public override string Export(int exportFeedId)
        {
            var commonSettings = ExportFeedSettingsProvider.GetSettings(exportFeedId);
            var advancedSettings = JsonConvert.DeserializeObject<ExportFeedYandexOptions>(commonSettings.AdvancedSettings);

            return Export(
                ExportFeedYandexService.GetCategories(exportFeedId),
                ExportFeedYandexService.GetProducts(exportFeedId, commonSettings, advancedSettings),
                commonSettings,
                ExportFeedYandexService.GetCategoriesCount(exportFeedId),
                ExportFeedYandexService.GetProductsCount(exportFeedId, commonSettings, advancedSettings));
        }

        public override string Export(IEnumerable<ExportFeedCategories> categories, IEnumerable<ExportFeedProductModel> products, ExportFeedSettings options, int categoriesCount, int productsCount)
        {
            try
            {
                var advancedSettings = JsonConvert.DeserializeObject<ExportFeedYandexOptions>(options.AdvancedSettings);

                var discountModule = AttachedModules.GetModules<IDiscount>().FirstOrDefault();
                if (discountModule != null)
                {
                    var classInstance = (IDiscount)Activator.CreateInstance(discountModule);
                    _productDiscountModels = classInstance.GetProductDiscountsList();
                }

                var currencies = CurrencyService.GetAllCurrencies().Where(item => AvailableCurrencies.Contains(item.Iso3)).ToList();

                var exportFile = new FileInfo(options.FileFullPath);
                if (!string.IsNullOrEmpty(exportFile.Directory.FullName))
                {
                    FileHelpers.CreateDirectory(exportFile.Directory.FullName);
                }
                FileHelpers.DeleteFile(exportFile.FullName);

                CommonStatistic.FileName = "../" + options.FileFullName;

                using (var outputFile = new FileStream(exportFile.FullName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    var settings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true };
                    using (var writer = XmlWriter.Create(outputFile, settings))
                    {
                        writer.WriteStartDocument();
                        writer.WriteDocType("yml_catalog", null, "shops.dtd", null);
                        writer.WriteStartElement("yml_catalog");
                        writer.WriteAttributeString("date", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                        writer.WriteStartElement("shop");

                        writer.WriteStartElement("name");
                        writer.WriteRaw(advancedSettings.ShopName.Replace("#STORE_NAME#", SettingsMain.ShopName).XmlEncode());
                        writer.WriteEndElement();

                        writer.WriteStartElement("company");
                        writer.WriteRaw(advancedSettings.CompanyName.Replace("#STORE_NAME#", SettingsMain.ShopName).XmlEncode());
                        writer.WriteEndElement();

                        writer.WriteStartElement("url");
                        writer.WriteRaw(SettingsMain.SiteUrl);
                        writer.WriteEndElement();

                        writer.WriteStartElement("currencies");
                        ProcessCurrency(currencies, advancedSettings.Currency, writer);
                        writer.WriteEndElement();

                        CommonStatistic.TotalRow = categoriesCount + productsCount;

                        writer.WriteStartElement("categories");
                        foreach (ExportFeedCategories categoryRow in categories)
                        {
                            ProcessCategoryRow(categoryRow, writer);
                            CommonStatistic.RowPosition++;
                        }
                        writer.WriteEndElement();

                        if ((advancedSettings.DeliveryCost == ExportFeedYandexDeliveryCost.GlobalDeliveryCost ||
                             advancedSettings.DeliveryCost == ExportFeedYandexDeliveryCost.LocalDeliveryCost) &&
                            !string.IsNullOrWhiteSpace(advancedSettings.GlobalDeliveryCost))
                        {
                            try
                            {
                                var deliveryOptions =
                                    JsonConvert.DeserializeObject<List<ExportFeedYandexDeliveryCostOption>>(advancedSettings.GlobalDeliveryCost);

                                writer.WriteStartElement("delivery-options");

                                foreach (var deliveryOption in deliveryOptions)
                                {
                                    writer.WriteStartElement("option");
                                    writer.WriteAttributeString("cost", deliveryOption.Cost);
                                    writer.WriteAttributeString("days", deliveryOption.Days);
                                    writer.WriteAttributeString("order-before", deliveryOption.OrderBefore);
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                            }
                            catch (Exception ex)
                            {
                                Debug.Log.Error(ex);
                            }
                        }

                        writer.WriteStartElement("offers");
                        var currency = CurrencyService.GetCurrencyByIso3(advancedSettings.Currency);
                        foreach (ExportFeedYandexProduct offerRow in products)
                        {
                            ProcessProductRow(offerRow, writer, options, currency);
                            CommonStatistic.RowPosition++;
                        }
                        writer.WriteEndElement();

                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        writer.WriteEndDocument();

                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonStatistic.TotalErrorRow++;
                Debug.Log.Error(ex);
            }

            CommonStatistic.IsRun = false;
            return options.FileFullName;
        }

        public override void SetDefaultSettings(int exportFeedId)
        {
            ExportFeedSettingsProvider.SetSettings(exportFeedId, new ExportFeedSettings
            {
                PriceMargin = 0,
                FileName = File.Exists(SettingsGeneral.AbsolutePath + "/export/yamarket.xml") ? "export/yamarket" + exportFeedId : "export/yamarket",
                FileExtention = AvailableFileExtentions[0],

                AdditionalUrlTags = string.Empty,

                Interval = 1,
                IntervalType = Core.Scheduler.TimeIntervalType.Days,
                JobStartTime = new DateTime(2017, 1, 1, 1, 0, 0),
                Active = false,

                AdvancedSettings = JsonConvert.SerializeObject(new ExportFeedYandexOptions
                {
                    CompanyName = "#STORE_NAME#",
                    ShopName = "#STORE_NAME#",
                    ProductDescriptionType = "short",
                    Currency = ExportFeedYandex.AvailableCurrencies[0],
                    GlobalDeliveryCost = "[]",
                    LocalDeliveryOption = "{\"Cost\":null,\"Days\":\"\",\"OrderBefore\":\"\"}",
                    RemoveHtml = true,
                    //AllowPreOrderProducts = true
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
            var advancedSettings = JsonConvert.DeserializeObject<ExportFeedYandexOptions>(commonSettings.AdvancedSettings);

            return ExportFeedYandexService.GetProductsCount(exportFeedId, commonSettings, advancedSettings);
        }

        public override int GetCategoriesCount(int exportFeedId)
        {
            return ExportFeedYandexService.GetCategoriesCount(exportFeedId);
        }

        public override List<string> GetAvailableFileExtentions()
        {
            return new List<string> { "xml", "yml" };
        }
    }
}