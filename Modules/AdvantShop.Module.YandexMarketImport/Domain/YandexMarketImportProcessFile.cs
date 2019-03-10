//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Repository.Currencies;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.SEO;
using AdvantShop.Statistic;
using AdvantShop.Diagnostics;
using AdvantShop.Core.SQL;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Saas;

namespace AdvantShop.Module.YandexMarketImport.Domain
{
    public class YandexMarketImportProcessFile
    {
        public enum MappingType
        {
            None = 0,
            Attribute = 1,
            Tag = 2,
            Param = 3
        }

        public enum UpdateType
        {
            Full = 0,
            Amount = 1,
            Price = 2,
            AmountAndPrice = 3
        }

        private struct NewOffers
        {
            public string offerArtNo;
            public int ProductId;
        }

        /// <summary>
        /// выполнение в потоке
        /// </summary>
        public static void ProcessYmlInJob()
        {
            if (!YandexMarketImport.AutoUpdateActive)
            {
                return;
            }

            var fullPath = SettingsGeneral.AbsolutePath + "modules\\YandexMarketImport\\temp\\products.yml";
            var folderPath = SettingsGeneral.AbsolutePath + "modules\\YandexMarketImport\\temp";
            var fullStatisticLogPath = folderPath + "\\statisticLog.txt";

            var fileUrl = YandexMarketImport.FileUrlPath;

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            if (!File.Exists(fullStatisticLogPath))
            {
                using (File.Create(fullStatisticLogPath)) { }
            }

            using (var streamWriter = new StreamWriter(fullStatisticLogPath, true, Encoding.UTF8))
            {
                streamWriter.WriteLine("Начало импорта " + DateTime.Now);
            }

            try
            {
                if (!string.IsNullOrEmpty(fileUrl))
                {
                    new WebClient().DownloadFile(fileUrl, fullPath);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }


            if (!File.Exists(fullPath))
            {
                using (var streamWriter = new StreamWriter(fullStatisticLogPath, true, Encoding.UTF8))
                {
                    streamWriter.WriteLine("Файл не найден " + fullPath);
                    streamWriter.WriteLine("Файл url " + fileUrl);
                    streamWriter.WriteLine("Конец импорта ");
                }

                return;
            }

            var categories = new List<YandexMarketImportCategory>(0);
            var currencies = new Dictionary<string, float>();


            //CommonStatistic.TotalRow = GetRowsCount(fullPath);
            CommonStatistic.TotalRow = GetOfferRowsCount(fullPath);


            var startAt = DateTime.Now;

            if (YandexMarketImport.AmountNulling)
            {
                ProductService.ClearAmountAllProducts();
            }

            if (YandexMarketImport.DeactivateProducts)
            {
                ProductService.DisableAllProducts();
            }

            using (var reader = XmlReader.Create(fullPath,
                                              new XmlReaderSettings
                                              {
                                                  DtdProcessing = DtdProcessing.Ignore,
                                                  IgnoreWhitespace = true
                                              }))
            {
                while (reader.Read())
                {
                    if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "currencies")
                    {
                        using (var streamWriter = new StreamWriter(fullStatisticLogPath, true, Encoding.UTF8))
                        {
                            streamWriter.WriteLine("Обработка валют");
                        }
                        ProcessCurrencies(reader, currencies);
                    }

                    else if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "categories")
                    {
                        using (var streamWriter = new StreamWriter(fullStatisticLogPath, true, Encoding.UTF8))
                        {
                            streamWriter.WriteLine("Обработка категорий");
                        }
                        ProcessCategories(reader, categories);
                    }
                    else if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "offers")
                    {
                        using (var streamWriter = new StreamWriter(fullStatisticLogPath, true, Encoding.UTF8))
                        {
                            streamWriter.WriteLine("Обработка продуктов");
                        }
                        ProcessOffer(reader, categories, currencies, true);
                    }
                }

                CategoryService.SetCategoryHierarchicallyEnabled(0);
                CategoryService.RecalculateProductsCountManual();
                ProductService.PreCalcProductParamsMass();


                using (var streamWriter = new StreamWriter(fullStatisticLogPath, true))
                {
                    streamWriter.WriteLine("Конец импорта " + DateTime.Now);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullPath"></param>
        public static void ProcessYml(string fullPath)
        {
            var fullStatisticogPath = SettingsGeneral.AbsolutePath + "modules\\YandexMarketImport\\temp\\statisticLog.txt";

            var categories = new List<YandexMarketImportCategory>(0);
            var currencies = new Dictionary<string, float>();

            //CommonStatistic.TotalRow = GetRowsCount(fullPath);
            CommonStatistic.TotalRow = GetOfferRowsCount(fullPath);

            var startAt = DateTime.Now;
            if (YandexMarketImport.AmountNulling)
            {
                ProductService.ClearAmountAllProducts();
            }

            if (YandexMarketImport.DeactivateProducts)
            {
                ProductService.DisableAllProducts();
            }

            using (var reader = XmlReader.Create(fullPath,
                                              new XmlReaderSettings
                                              {
                                                  DtdProcessing = DtdProcessing.Ignore,
                                                  IgnoreWhitespace = true
                                              }))
            {
                using (var streamWriter = new StreamWriter(fullStatisticogPath, true, Encoding.UTF8))
                {
                    streamWriter.WriteLine("Начало импорта " + DateTime.Now + " файл:" + fullPath);
                }

                CommonStatistic.IsRun = true;
                while (reader.Read() && CommonStatistic.IsRun)
                {
                    if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "currencies")
                    {
                        using (var streamWriter = new StreamWriter(fullStatisticogPath, true, Encoding.UTF8))
                        {
                            streamWriter.WriteLine("Обработка валют");
                        }
                        ProcessCurrencies(reader, currencies);
                    }

                    else if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "categories")
                    {
                        using (var streamWriter = new StreamWriter(fullStatisticogPath, true, Encoding.UTF8))
                        {
                            streamWriter.WriteLine("Обработка категорий");
                        }
                        ProcessCategories(reader, categories);
                    }
                    else if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "offers")
                    {
                        using (var streamWriter = new StreamWriter(fullStatisticogPath, true, Encoding.UTF8))
                        {
                            streamWriter.WriteLine("Обработка продуктов");
                        }
                        ProcessOffer(reader, categories, currencies);
                    }
                    if (CommonStatistic.TotalRow == CommonStatistic.RowPosition)
                    {
                        break;
                    }
                }

                CategoryService.SetCategoryHierarchicallyEnabled(0);
                CategoryService.RecalculateProductsCountManual();
                ProductService.PreCalcProductParamsMassInBackground();

                using (var streamWriter = new StreamWriter(fullStatisticogPath, true))
                {
                    streamWriter.WriteLine("Конец импорта " + DateTime.Now);
                }

                Trial.TrialService.TrackEvent(Trial.ETrackEvent.Trial_ImportYML);
            }
            CommonStatistic.IsRun = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="categories"></param>
        /// <param name="currencies"></param>
        /// <param name="inJob"></param>
        private static void ProcessOffer(XmlReader reader, List<YandexMarketImportCategory> categories, Dictionary<string, float> currencies, bool inJob = false)
        {
            var amountMappingType = YandexMarketImport.AmountMappingType;
            var amountMappingTypeField = YandexMarketImport.AmountMappingTypeField;

            var artnoMappingType = YandexMarketImport.ArtnoMappingType;
            var artnoMappingTypeField = YandexMarketImport.ArtnoMappingTypeField;

            var artnoProductMappingType = YandexMarketImport.ArtnoProductMappingType;
            var artnoProductMappingTypeField = YandexMarketImport.ArtnoProductMappingTypeField;
            var nameFromTagName = YandexMarketImport.TagForNameProduct;

            var updateType = YandexMarketImport.UpdateType;

            var allowPreorder = YandexMarketImport.AllowPreorder;
            var extraCharge = YandexMarketImport.ExtraCharge;

            var shopCurrencies = Repository.Currencies.CurrencyService.GetAllCurrencies();
            var offersIsAdded = new List<NewOffers>();

            while (reader.Read() && reader.Name != "offers" && (CommonStatistic.IsRun || inJob))
            {
                if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "offer")
                {
                    var product = new Product
                    {
                        ArtNo = reader.GetAttribute("id") == null ? null : reader.GetAttribute("id").Trim(),
                        Enabled = true,
                        Multiplicity = 1,
                        HasMultiOffer = true,
                        ModifiedBy = "yandexmarket",
                        AllowPreOrder = allowPreorder
                    };

                    if (string.Equals(artnoProductMappingType, MappingType.Attribute.ToString()) && !string.IsNullOrEmpty(reader.GetAttribute("group_id")))
                    {
                        product.ArtNo = reader.GetAttribute("group_id").Trim();
                    }

                    var available = reader.GetAttribute("available") != null ? reader.GetAttribute("available").Trim().TryParseBool(true) ?? true : true;
                    var nameTag = string.Empty;
                    var productCetegoryId = 0;
                    var pictureUrls = new List<string>();
                    var productProperties = new List<ProductProperty>();
                    var productUrl = string.Empty;
                    float offerCurrencyRate = 1;
                    var colorId = 0;
                    var colorRgb = string.Empty;

                    var offer = new Offer { ArtNo = reader.GetAttribute("id") == null ? null : reader.GetAttribute("id").Trim(), Amount = available ? 1 : 0, Main = true };

                    while (reader.Read() && reader.Name != "offer")
                    {
                            if (reader.IsEmptyElement)
                            {
                                continue;
                            }
                            if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "url" && reader.Read())
                            {
                                productUrl = reader.Value.Trim();
                            }
                            if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "price")
                            {
                                if (!reader.IsEmptyElement && reader.Read())
                                {
                                    var basePrice = reader.Value.Trim().TryParseFloat();
                                    offer.BasePrice = basePrice + basePrice * extraCharge / 100;
                                }
                            }

                            if (reader.NodeType != XmlNodeType.EndElement &&
                                (reader.Name == "quantity" || reader.Name == "amount") && reader.Read())
                            {
                                offer.Amount = reader.Value.Replace(">", "").Trim().TryParseFloat();
                            }

                            /*получение amount из тэга*/
                            if (string.Equals(amountMappingType, MappingType.Tag.ToString()) &&
                                reader.NodeType != XmlNodeType.EndElement &&
                                string.Equals(reader.Name, amountMappingTypeField) && reader.Read())
                            {
                                float amount = 1;
                                if (float.TryParse(reader.Value.Replace(">", "").Replace(".",",").Trim(), out amount))
                                {
                                    offer.Amount = amount;
                                }
                            }


                            // костыль если artno оффера и товара в одном теге
                            if (string.Equals(artnoMappingType, MappingType.Tag.ToString()) &&
                                reader.NodeType != XmlNodeType.EndElement &&
                                string.Equals(reader.Name, artnoMappingTypeField)
                                ||
                                string.Equals(artnoProductMappingType, MappingType.Tag.ToString()) &&
                                reader.NodeType != XmlNodeType.EndElement &&
                                string.Equals(reader.Name, artnoProductMappingTypeField))
                            {
                                var name = reader.Name.Trim();
                                if (reader.Read())
                                {
                                    var value = reader.Value.Trim();

                                    if (string.Equals(artnoMappingType, MappingType.Tag.ToString()) &&
                                        string.Equals(name, artnoMappingTypeField))
                                    {
                                        offer.ArtNo = value;
                                    }

                                    if (string.Equals(artnoProductMappingType, MappingType.Tag.ToString()) &&
                                        string.Equals(name, artnoProductMappingTypeField))
                                    {
                                        product.ArtNo = value;
                                    }
                                }
                            }
                            /*получение artno из тэга*/
                            if (string.Equals(artnoMappingType, MappingType.Tag.ToString()) &&
                                reader.NodeType != XmlNodeType.EndElement &&
                                string.Equals(reader.Name, artnoMappingTypeField) && reader.Read())
                            {
                                offer.ArtNo = reader.Value.Trim();
                            }

                            /*получение artno продукта из тэга*/
                            if (string.Equals(artnoProductMappingType, MappingType.Tag.ToString()) &&
                                reader.NodeType != XmlNodeType.EndElement &&
                                string.Equals(reader.Name, artnoProductMappingTypeField) && reader.Read())
                            {
                                product.ArtNo = reader.Value.Trim();
                            }

                            if (reader.NodeType != XmlNodeType.EndElement && reader.Name.ToLower() == "currencyid" &&
                                reader.Read())
                            {
                                if (currencies.ContainsKey(reader.Value.Trim()))
                                {
                                    offerCurrencyRate = currencies[reader.Value.Trim()];

                                    var shopCurrency =
                                        shopCurrencies.FirstOrDefault(
                                            item =>
                                                string.Equals(item.Iso3.ToString(),
                                                    reader.Value.Replace("RUR", "RUB").Trim()));
                                    if (shopCurrency != null)
                                    {
                                        product.CurrencyID = shopCurrency.CurrencyId;
                                    }
                                }
                            }
                            if (reader.NodeType != XmlNodeType.EndElement && reader.Name.ToLower() == "categoryid" &&
                                reader.Read())
                            {
                                if (categories.Any(item => string.Equals(item.YmlCategoryId, reader.Value)))
                                {
                                    productCetegoryId =
                                        categories.First(item => string.Equals(item.YmlCategoryId, reader.Value.Trim()))
                                            .AdvCategoryId;
                                }
                            }
                            if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "picture" && reader.Read())
                            {
                                pictureUrls.Add(reader.Value.Trim());
                            }
                            if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "vendor" && reader.Read())
                            {
                                var brandName = reader.Value.Trim();
                                if (!string.IsNullOrEmpty(reader.Value))
                                {
                                    var brandId = BrandService.GetBrandIdByName(brandName);
                                    if (brandId == 0)
                                    {
                                        brandId = BrandService.AddBrand(new Brand
                                        {
                                            Name = brandName,
                                            Enabled = true,
                                            UrlPath = UrlService.GetAvailableValidUrl(0, ParamType.Brand, brandName)
                                        });
                                    }
                                    product.BrandId = brandId;
                                }
                            }
                            if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "model")
                            {
                                if (!reader.IsEmptyElement && reader.Read())
                                {
                                    product.Name = reader.Value.Trim();
                                }
                            }
                            if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "description")
                            {
                                if (!reader.IsEmptyElement && reader.Read())
                                {
                                    product.Description = reader.Value.Trim();
                                }
                            }
                            if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "sales_notes")
                            {
                                if (!reader.IsEmptyElement && reader.Read())
                                {
                                    product.SalesNote = reader.Value.Trim();
                                }
                            }
                            if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "name")
                            {
                                if (!reader.IsEmptyElement && reader.Read())
                                    if (string.IsNullOrEmpty(nameTag))
                                    {
                                        nameTag = reader.Value.Trim();
                                    }
                            }
                            if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "typePrefix" &&
                                reader.Read())
                            {
                                if (!reader.IsEmptyElement && reader.Read())
                                {
                                    product.YandexTypePrefix = reader.Value.Trim();
                                }
                            }
                            if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "adult" && reader.Read())
                            {
                                if (!reader.IsEmptyElement && reader.Read())
                                {
                                    var adult = false;
                                    if (bool.TryParse(reader.Value.Trim(), out adult))
                                    {
                                        product.Adult = adult;
                                    }
                                }
                            }
                            if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "weight" && reader.Read())
                            {
                                float weight = 0;
                                if (float.TryParse(reader.Value.Trim(), out weight))
                                {
                                    product.Weight = weight;
                                }
                            }
                            if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "dimensions" &&
                                reader.Read())
                            {
                                float[] dimensions =
                                    reader.Value.Trim().Split(new[] {'/'}).Select(x => x.TryParseFloat() * 10).ToArray();

                                if (dimensions.Count() > 0)
                                {
                                    product.Length = dimensions[0];
                                }
                                if (dimensions.Count() > 1)
                                {
                                    product.Width = dimensions[1];
                                }
                                if (dimensions.Count() > 2)
                                {
                                    product.Height = dimensions[2];
                                }
                            }
                            if (reader.NodeType != XmlNodeType.EndElement && !reader.IsEmptyElement &&
                                reader.Name == "param" && reader.HasAttributes)
                            {
                                var productProperty = reader.GetAttribute("name") != null ? new ProductProperty
                                {
                                    Name = reader.GetAttribute("name").Trim()
                                } : new ProductProperty();

                                if (reader.Read())
                                {
                                    productProperty.Value = reader.Value.Trim();
                                }

                                if (string.Equals(amountMappingType, MappingType.Param.ToString()) &&
                                    string.Equals(productProperty.Name.ToLower(), amountMappingTypeField.ToLower()))
                                {
                                    float amount = 1;
                                    if (float.TryParse(productProperty.Value, out amount))
                                    {
                                        offer.Amount = amount;
                                    }
                                }
                                else if (string.Equals(artnoMappingType, MappingType.Param.ToString()) &&
                                         string.Equals(productProperty.Name.ToLower(), artnoMappingTypeField.ToLower()))
                                {
                                    offer.ArtNo = productProperty.Value;

                                }
                                else if (string.Equals(artnoProductMappingType, MappingType.Param.ToString()) &&
                                         string.Equals(productProperty.Name.ToLower(),
                                             artnoProductMappingTypeField.ToLower()))
                                {
                                    product.ArtNo = productProperty.Value;
                                }
                                else if (string.Equals(productProperty.Name.ToLower(), "rgb"))
                                {
                                    if (colorId != 0)
                                    {
                                        var color = ColorService.GetColor(colorId);
                                        if (color != null)
                                        {
                                            color.ColorCode = "#" + productProperty.Value;
                                            ColorService.UpdateColor(color);
                                        }
                                    }
                                    else
                                    {
                                        colorRgb = "#" + productProperty.Value;
                                    }
                                }
                                else if (productProperty.Name.ToLower() == SettingsCatalog.ColorsHeader.ToLower() ||
                                         productProperty.Name.ToLower() == "цвет")
                                {
                                    var color = ColorService.GetColor(productProperty.Value);
                                    if (color != null)
                                    {
                                        offer.ColorID = color.ColorId;
                                    }
                                    else
                                    {
                                        colorId = ColorService.AddColor(new Color
                                        {
                                            ColorName = productProperty.Value,
                                            ColorCode = string.IsNullOrEmpty(colorRgb) ? "#000000" : colorRgb,
                                            SortOrder = 0,
                                        });
                                        offer.ColorID = colorId;
                                    }
                                }
                                else if (productProperty.Name.ToLower() == SettingsCatalog.SizesHeader.ToLower() ||
                                         productProperty.Name.ToLower() == "размер")
                                {
                                    var size = SizeService.GetSize(productProperty.Value);
                                    if (size != null)
                                    {
                                        offer.SizeID = size.SizeId;
                                    }
                                    else
                                    {
                                        var sizeId = SizeService.AddSize(new Size
                                        {
                                            SizeName = productProperty.Value,
                                            SortOrder = 0
                                        });
                                        offer.SizeID = sizeId;
                                    }
                                }
                                else
                                {
                                    productProperties.Add(productProperty);
                                }
                            }
                    }
                    try
                    {
                        var newProduct = false;

                        if ((string.IsNullOrEmpty(product.Name) && !string.IsNullOrEmpty(nameTag)) ||
                            (!string.IsNullOrEmpty(nameTag) && nameFromTagName == "Name"))
                        {
                            product.Name = nameTag;
                        }

                        if (!string.IsNullOrEmpty(product.Name))
                        {
                            if (!(product.CurrencyID > 0))
                            {
                                if (currencies.Count > 1)
                                {
                                    var currency = currencies.First();
                                    product.CurrencyID = currency.Key.TryParseInt();
                                }
                                else
                                {
                                    product.CurrencyID = CurrencyService.CurrentCurrency.CurrencyId;
                                }
                            }

                            product.UrlPath = UrlService.GetAvailableValidUrl(0, ParamType.Product, product.Name);

                            //var defaulRate = YandexMarketImport.DefaultCurrencyIso;
                            //offer.BasePrice = offer.BasePrice * (defaulRate / offerCurrencyRate);

                            product.Offers.Add(offer);

                            var existProduct = ProductService.GetProduct(product.ArtNo);

                            if (existProduct != null)
                            {
                                //обновляем продукт в базе
                                UpdateExistProduct(product, offer, existProduct, amountMappingType, updateType);
                                product.ProductId = existProduct.ProductId;

                                CommonStatistic.TotalUpdateRow++;
                                CommonStatistic.WriteLog("продукт обновлен " + product.ArtNo);
                            }
                            else
                            {
                                if (SaasDataService.CurrentSaasData.ProductsCount <= ProductService.GetProductsCount() && SaasDataService.IsSaasEnabled)
                                {
                                    CommonStatistic.WriteLog("Количество загружаемого товара превышает резрешенное количество по тарифному плану");
                                }
                                else
                                {
                                    product.HasMultiOffer = product.Offers.Count(x => x.SizeID > 0 || x.ColorID > 0) > 0;
                                    newProduct = true;
                                    product.ProductId = ProductService.AddProduct(product, true);

                                    CommonStatistic.TotalAddRow++;
                                    CommonStatistic.WriteLog("продукт добавлен " + product.ArtNo);
                                }
                            }
                        }

                        if (product.ProductId == 0)
                        {
                            CommonStatistic.WriteLog("Не удалось добавить товар: " + product.ArtNo);//+ "-" + product.Name + "-" + product.Description);
                            CommonStatistic.TotalErrorRow++;
                            CommonStatistic.RowPosition++;
                            continue;
                        }
                        offersIsAdded.Add(new NewOffers { offerArtNo = offer.ArtNo, ProductId = product.ProductId });

                        //если не полное обновление
                        if (!string.Equals(updateType, UpdateType.Full.ToString()) && !newProduct)
                        {
                            CommonStatistic.RowPosition++;
                            continue;
                        }

                        //добавление в категорию
                        if (productCetegoryId != 0)
                        {
                            ProductService.AddProductLink(product.ProductId, productCetegoryId, 0, false);
                        }

                        //301 redirect
                        ProcessingRedirects(product, productUrl);

                        //обработка свойств
                        ProcessingProperties(product, productProperties);

                        //обработка изображений
                        ProcessingPhotos(product.ProductId, offer.ArtNo, offer.ColorID, pictureUrls);
                    }
                    catch (Exception ex)
                    {
                        CommonStatistic.WriteLog("ошибка при добавлении товара: " + product.ArtNo + " ,ошибка: " + ex.Message);
                        CommonStatistic.TotalErrorRow++;
                        Debug.LogError(ex);
                    }

                    CommonStatistic.RowPosition++;
                }
            }

            if (YandexMarketImport.DeleteOldPrices)
            {
                DeleteOldOffers(offersIsAdded);
            }
            //UpdateSingleOffer(offersIsAdded);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="categories"></param>
        private static void ProcessCategories(XmlReader reader, List<YandexMarketImportCategory> categories)
        {
            while (reader.Read() && reader.Name != "categories")
            {
                if (reader.IsStartElement() && reader.Name == "category")
                {
                    var ymlCategoryId = reader.GetAttribute("id");
                    var ymlParentCategoryId = reader.GetAttribute("parentId");

                    if (reader.Read())
                    {
                        categories.Add(new YandexMarketImportCategory
                        {
                            YmlCategoryId = string.IsNullOrEmpty(ymlCategoryId) ? "0" : ymlCategoryId.Trim(),
                            YmlParentCategoryId = string.IsNullOrEmpty(ymlParentCategoryId) ? "0" : ymlParentCategoryId.Trim(),
                            YmlCategoryName = reader.Value.Trim()
                        });
                    }
                }
            }

            if (!string.Equals(YandexMarketImport.UpdateType, UpdateType.Full.ToString()))
            {
                return;
            }

            foreach (var category in categories)
            {
                var categoryParentId = 0;

                var parentYmlCategory = categories.FirstOrDefault(item => string.Equals(item.YmlCategoryId, category.YmlParentCategoryId));
                if (parentYmlCategory != null && parentYmlCategory.AdvCategoryId != 0)
                {
                    categoryParentId = parentYmlCategory.AdvCategoryId;
                }
                else if (parentYmlCategory != null)
                {
                    categoryParentId = AddUpdateCategory(categories, parentYmlCategory);
                }

                var categoryId = CategoryService.GetChildCategoryIdByName(categoryParentId, category.YmlCategoryName);
                if (categoryId.HasValue)
                {
                    category.AdvCategoryId = categoryId.Value;
                    CommonStatistic.WriteLog("категория обновлена " + category.YmlCategoryName);
                    //CommonStatistic.TotalUpdateRow++;
                }
                else
                {
                    category.AdvCategoryId = CategoryService.AddCategory(new Category
                    {
                        Name = category.YmlCategoryName,
                        ParentCategoryId = categoryParentId,
                        UrlPath = UrlService.GetAvailableValidUrl(0, ParamType.Category, category.YmlCategoryName),
                        Enabled = true,
                        DisplayStyle = ECategoryDisplayStyle.Tile
                    }, true);
                    CommonStatistic.WriteLog("категория добавлена " + category.YmlCategoryName);
                    //CommonStatistic.TotalAddRow++;
                }
                //CommonStatistic.RowPosition++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="currencies"></param>
        private static void ProcessCurrencies(XmlReader reader, Dictionary<string, float> currencies)
        {
            while (reader.Read() && reader.Name != "currencies")
            {
                if (reader.IsStartElement() && reader.Name == "currency")
                {

                    if (reader.GetAttribute("rate") == null)
                    {
                        currencies.Add(reader.GetAttribute("id").Trim(), 1.0F);
                    }
                    else if (reader.GetAttribute("rate") != null && reader.GetAttribute("rate").Trim().TryParseFloat() > 0)
                    {
                        currencies.Add(reader.GetAttribute("id").Trim(), reader.GetAttribute("rate").Trim().TryParseFloat());
                    }
                    else
                    {
                        CommonStatistic.WriteLog("Неверный формат курса валюты " + reader.GetAttribute("id"));
                        CommonStatistic.TotalErrorRow++;
                    }
                }
            }
        }

        #region Help methods

        //private static void UpdateSingleOffer(List<NewOffers> listOffers)
        //{
        //    try
        //    {
        //        List<int> productIds = listOffers.Select(x => x.ProductId).Distinct().ToList();

        //        foreach (int item in productIds)
        //        {
        //            var product = ProductService.GetProduct(item);
        //            if (product.Offers.Count == 1)
        //            {
        //                product.Offers.First().ArtNo = product.ArtNo;
        //                ProductService.UpdateProduct(product, false);
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Debug.LogError(ex);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productFromFile"></param>
        /// <param name="offerFromFile"></param>
        /// <param name="existProduct"></param>
        /// <param name="amountMappingType"></param>
        /// <param name="updateType"></param>
        private static void UpdateExistProduct(Product productFromFile, Offer offerFromFile, Product existProduct, string amountMappingType, string updateType)
        {
            offerFromFile.ProductId = existProduct.ProductId;

            offerFromFile.ArtNo = offerFromFile.ArtNo.IsNullOrEmpty() ? productFromFile.ArtNo : offerFromFile.ArtNo;

            var existOffer = OfferService.GetOffer(offerFromFile.ArtNo);
            if (existProduct.Offers.Any(o => o.Main && o.ArtNo != offerFromFile.ArtNo))
            {
                offerFromFile.Main = false;
                if (existOffer != null)
                {
                    existOffer.Main = false;
                }
            }

            if (existOffer != null)
            {
                //обновляем количество если судя по типу мапинга он пришел
                if (!string.Equals(amountMappingType, MappingType.None.ToString()) && !string.Equals(updateType, UpdateType.Price.ToString()) || YandexMarketImport.AmountNulling)
                {
                    existOffer.Amount = offerFromFile.Amount;
                }
                //если цвет не пришел, оставляем старый
                if (offerFromFile.ColorID != null && string.Equals(updateType, UpdateType.Full.ToString()))
                {
                    existOffer.ColorID = offerFromFile.ColorID;
                }
                //если размер не пришел, оставляем старый
                if (offerFromFile.SizeID != null && string.Equals(updateType, UpdateType.Full.ToString()))
                {
                    existOffer.SizeID = offerFromFile.SizeID;
                }

                if (!string.Equals(updateType, UpdateType.Amount.ToString()))
                {
                    existOffer.BasePrice = offerFromFile.BasePrice;
                    existOffer.SupplyPrice = offerFromFile.SupplyPrice;
                }

                //на случай, если offer относится к другому продукту
                existOffer.ProductId = existProduct.ProductId;

                for (int i = 0; i < existProduct.Offers.Count; i++)
                {
                    if (existProduct.Offers[i].OfferId == existOffer.OfferId)
                    {
                        existProduct.Offers[i] = existOffer;
                    }
                }

                OfferService.UpdateOffer(existOffer);
            }
            else
            {
                existProduct.Offers.Add(offerFromFile);
                OfferService.AddOffer(offerFromFile);
            }

            if (!string.Equals(updateType, UpdateType.Amount.ToString()))
            {
                if (productFromFile.CurrencyID > 0)
                {
                    existProduct.CurrencyID = productFromFile.CurrencyID;
                }
            }

            if (string.Equals(updateType, UpdateType.Full.ToString()))
            {
                if (productFromFile.BrandId != 0)
                {
                    existProduct.BrandId = productFromFile.BrandId;
                }
                if (!string.IsNullOrEmpty(productFromFile.Name))
                {
                    existProduct.Name = productFromFile.Name;
                }
                if (productFromFile.Weight != 0)
                {
                    existProduct.Weight = productFromFile.Weight;
                }
                if (productFromFile.Length != 0)
                {
                    existProduct.Length = productFromFile.Length;
                }
                if (productFromFile.Width != 0)
                {
                    existProduct.Width = productFromFile.Width;
                }
                if (productFromFile.Height != 0)
                {
                    existProduct.Height = productFromFile.Height;
                }

                existProduct.AllowPreOrder = productFromFile.AllowPreOrder;
                existProduct.Description = productFromFile.Description;
                existProduct.ModifiedBy = productFromFile.ModifiedBy;
            }

            if (!productFromFile.YandexTypePrefix.IsNullOrEmpty())
            {
                existProduct.YandexTypePrefix = productFromFile.YandexTypePrefix.Trim();
            }

            if (YandexMarketImport.DeactivateProducts || string.Equals(updateType, UpdateType.Full.ToString()))
            {
                existProduct.Enabled = productFromFile.Enabled;
            }

            existProduct.HasMultiOffer = existProduct.Offers.Count(x => x.SizeID > 0 || x.ColorID > 0) > 0 || existProduct.Offers.Count > 1;

            ProductService.UpdateProduct(existProduct, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="productProperties"></param>
        private static void ProcessingProperties(Product product, List<ProductProperty> productProperties)
        {
            foreach (var productProperty in productProperties)
            {
                Property property = PropertyService.GetPropertyByName(productProperty.Name);

                if (property == null)
                {
                    property = new Property()
                    {
                        Name = productProperty.Name,
                        UseInDetails = true,
                        UseInFilter = true,
                        Type = 1
                    };

                    property.PropertyId = PropertyService.AddProperty(property);
                }

                PropertyValue propertyValue = null;


                propertyValue = PropertyService.GetPropertyValueByName(property.PropertyId, productProperty.Value);

                if (propertyValue == null)
                {
                    propertyValue = new PropertyValue()
                    {
                        Value = productProperty.Value,
                        PropertyId = property.PropertyId
                    };

                    propertyValue.PropertyValueId = PropertyService.AddPropertyValue(propertyValue);
                }

                if (PropertyService.GetPropertyValuesByProductId(product.ProductId).All(p => p.PropertyValueId != propertyValue.PropertyValueId))
                {
                    PropertyService.AddProductProperyValue(propertyValue.PropertyValueId, product.ProductId);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="colorId"></param>
        /// <param name="pictureUrls"></param>
        private static void ProcessingPhotos(int productId, string productArtNo, int? colorId, List<string> pictureUrls)
        {
            foreach (var pictureUrl in pictureUrls)
            {
                if (string.IsNullOrEmpty(pictureUrl))
                    continue;

                var photoname = pictureUrl.Md5() + "." + (pictureUrl.Split('.').LastOrDefault() ?? "jpg");

                if (!PhotoService.IsProductHaveThisPhotoByName(productId, photoname))
                {
                    if (!FileHelpers.DownloadRemoteImageFile(pictureUrl, FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoname)))
                    {
                        CommonStatistic.WriteLog("не найдено изображение для товара: " + productArtNo + " , путь к файлу: " + pictureUrl);
                        continue;
                    }

                    var fullfilename = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoname);
                    if (File.Exists(fullfilename))
                    {
                        ProductService.AddProductPhotoByProductId(productId, fullfilename, string.Empty, true, colorId);
                        File.Delete(fullfilename);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="oldProductUrl"></param>
        private static void ProcessingRedirects(Product product, string oldProductUrl)
        {
            if (YandexMarketImport.Process301Redirect && !string.IsNullOrWhiteSpace(oldProductUrl))
            {
                if (RedirectSeoService.GetRedirectsSeoByRedirectFrom(oldProductUrl) == null)
                {
                    RedirectSeoService.AddRedirectSeo(new RedirectSeo
                    {
                        RedirectFrom = oldProductUrl,
                        RedirectTo = UrlService.GetLink(ParamType.Product, product.UrlPath, product.ProductId),
                        ProductArtNo = product.ArtNo
                    });
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="ymlCategory"></param>
        /// <returns></returns>
        private static int AddUpdateCategory(List<YandexMarketImportCategory> categories, YandexMarketImportCategory ymlCategory)
        {
            var categoryParentId = 0;
            var parentYmlCategory = categories.FirstOrDefault(item => string.Equals(item.YmlCategoryId, ymlCategory.YmlParentCategoryId));
            if (parentYmlCategory != null)
            {
                if (parentYmlCategory.AdvCategoryId != 0)
                {
                    categoryParentId = parentYmlCategory.AdvCategoryId;
                }
                else if (parentYmlCategory.AdvCategoryId == 0)
                {
                    return 0;
                }
                else
                {
                    categoryParentId = AddUpdateCategory(categories, parentYmlCategory);
                }
            }

            var categoryId = CategoryService.GetChildCategoryIdByName(categoryParentId, ymlCategory.YmlCategoryName);
            if (categoryId.HasValue)
            {
                ymlCategory.AdvCategoryId = categoryId.Value;
            }
            else
            {
                ymlCategory.AdvCategoryId = CategoryService.AddCategory(new Category
                {
                    Name = ymlCategory.YmlCategoryName,
                    ParentCategoryId = categoryParentId,
                    UrlPath = UrlService.GetAvailableValidUrl(0, ParamType.Category, ymlCategory.YmlCategoryName),
                    Enabled = true,
                    DisplayStyle = ECategoryDisplayStyle.Tile
                }, true, true);
            }
            return ymlCategory.AdvCategoryId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        private static int GetRowsCount(string fullPath)
        {
            int count = 0;
            using (var reader = XmlReader.Create(fullPath, new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore, IgnoreWhitespace = false }))
            {
                while (reader.Read())
                {
                    if (reader.NodeType != XmlNodeType.EndElement && (reader.Name == "category" || reader.Name == "offer"))
                    {
                        ++count;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artNos"></param>
        private static void DeleteOldOffers(List<NewOffers> artNos)
        {
            var oldoffers = SQLDataAccess.ExecuteReadList<Offer>(
                "Select * From [Catalog].[Offer]",
                CommandType.Text,
                (reader) =>
                {
                    return new Offer
                    {
                        ArtNo = SQLDataHelper.GetString(reader, "ArtNo"),
                        BasePrice = SQLDataHelper.GetFloat(reader, "Price"),
                        Amount = SQLDataHelper.GetFloat(reader, "Amount"),
                        SupplyPrice = SQLDataHelper.GetFloat(reader, "SupplyPrice"),
                        ProductId = SQLDataHelper.GetInt(reader, "ProductID"),
                        OfferId = SQLDataHelper.GetInt(reader, "OfferID"),
                        ColorID = SQLDataHelper.GetNullableInt(reader, "ColorID"),
                        SizeID = SQLDataHelper.GetNullableInt(reader, "SizeID"),
                        Main = SQLDataHelper.GetBoolean(reader, "Main")
                    };
                });

            foreach (var offer in oldoffers.Where(i => !artNos.Any(item => item.offerArtNo == i.ArtNo) && artNos.Any(item => item.ProductId == i.ProductId)))
            {
                SQLDataAccess.ExecuteNonQuery(
                    "Delete From [Catalog].[Offer] Where ArtNo = @artno",
                    CommandType.Text,
                    new SqlParameter("@artno", offer.ArtNo));
            }
            CategoryService.ClearCategoryCache();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static int GetOfferRowsCount(string fullPath)
        {
            int count = 0;
            using (var reader = XmlReader.Create(fullPath, new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore, IgnoreWhitespace = false }))
            {
                while (reader.Read())
                {
                    if (reader.NodeType != XmlNodeType.EndElement && (reader.Name == "offer"))
                    {
                        ++count;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static int GetCategoryRowsCount(string fullPath)
        {
            int count = 0;
            using (var reader = XmlReader.Create(fullPath, new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore, IgnoreWhitespace = false }))
            {
                while (reader.Read() && !(reader.NodeType == XmlNodeType.EndElement && (reader.Name == "categories")))
                {
                    if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "category")
                    {
                        ++count;
                    }
                }
            }
            return count;
        }

        #endregion        
    }
}