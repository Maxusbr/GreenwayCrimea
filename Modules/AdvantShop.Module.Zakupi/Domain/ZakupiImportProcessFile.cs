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
using AdvantShop.Core.Modules;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.SEO;
using AdvantShop.Statistic;
using AdvantShop.Diagnostics;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Module.ZakupiImport.Domain
{
    public class ZakupiImportProcessFile
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

        public static void ProcessPartialYmlInJob()
        {
            if (!ModulesRepository.IsActiveModule(ZakupiImport.ModuleID))
            {
                return;
            }

            var fullStatisticogPath = SettingsGeneral.AbsolutePath + "modules\\ZakupiImport\\temp\\statisticLog.txt";
            var fullPath = SettingsGeneral.AbsolutePath + "modules\\ZakupiImport\\temp\\zakupiPartialImport.xml";

            var fileUrl = ZakupiImport.FileUrlPath + (ZakupiImport.FileUrlPath.Contains("?") ? "&" : "?") + "content=off";

            using (var streamWriter = new StreamWriter(fullStatisticogPath, true, Encoding.UTF8))
            {
                streamWriter.WriteLine("Начало импорта остатков " + DateTime.Now);
            }

            if (!string.IsNullOrEmpty(fileUrl))
            {
                new WebClient().DownloadFile(fileUrl, fullPath);
            }

            if (!File.Exists(fullPath))
            {
                using (var streamWriter = new StreamWriter(fullStatisticogPath, true, Encoding.UTF8))
                {
                    streamWriter.WriteLine("Файл не найден " + fullPath);
                    streamWriter.WriteLine("Файл url " + fileUrl);
                    streamWriter.WriteLine("Конец импорта ");
                }

                return;
            }

            var categories = new List<ZakupiImportCategory>(0);
            var currencies = new Dictionary<string, float>();

            CommonStatistic.TotalRow = GetRowsCount(fullPath);

            using (var reader = XmlReader.Create(fullPath,
                                              new XmlReaderSettings
                                              {
                                                  DtdProcessing = DtdProcessing.Ignore,
                                                  IgnoreWhitespace = true
                                              }))
            {
                while (reader.Read())
                {
                    if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "offers")
                    {
                        using (var streamWriter = new StreamWriter(fullStatisticogPath, true, Encoding.UTF8))
                        {
                            streamWriter.WriteLine("Обработка продуктов");
                        }
                        ProcessOffer(reader, categories, currencies, true);
                    }
                }

                CategoryService.SetCategoryHierarchicallyEnabled(0);
                CategoryService.RecalculateProductsCountManual();
                ProductService.PreCalcProductParamsMass();


                using (var streamWriter = new StreamWriter(fullStatisticogPath, true))
                {
                    streamWriter.WriteLine("Конец импорта " + DateTime.Now);
                }
            }
        }

        /// <summary>
        /// выполнение в потоке
        /// </summary>
        public static void ProcessYmlInJob()
        {
            if (!ModulesRepository.IsActiveModule(ZakupiImport.ModuleID))
            {
                return;
            }

            var fullStatisticogPath = SettingsGeneral.AbsolutePath + "modules\\ZakupiImport\\temp\\statisticLog.txt";

            var fullPath = SettingsGeneral.AbsolutePath + "modules\\ZakupiImport\\temp\\zakupiImport.xml";

            var fileUrl = ZakupiImport.FileUrlPath;

            using (var streamWriter = new StreamWriter(fullStatisticogPath, true, Encoding.UTF8))
            {
                streamWriter.WriteLine("Начало импорта " + DateTime.Now);
            }

            if (!string.IsNullOrEmpty(fileUrl))
            {
                new WebClient().DownloadFile(fileUrl, fullPath);
            }

            if (!File.Exists(fullPath))
            {
                using (var streamWriter = new StreamWriter(fullStatisticogPath, true, Encoding.UTF8))
                {
                    streamWriter.WriteLine("Файл не найден " + fullPath);
                    streamWriter.WriteLine("Файл url " + fileUrl);
                    streamWriter.WriteLine("Конец импорта ");
                }

                return;
            }

            var categories = new List<ZakupiImportCategory>(0);
            var currencies = new Dictionary<string, float>();

            CommonStatistic.TotalRow = GetRowsCount(fullPath);

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
                        ProcessOffer(reader, categories, currencies, true);
                    }
                }

                using (var streamWriter = new StreamWriter(fullStatisticogPath, true))
                {
                    streamWriter.WriteLine("Реиндексация " + DateTime.Now);
                }

                CategoryService.SetCategoryHierarchicallyEnabled(0);
                CategoryService.RecalculateProductsCountManual();
                ProductService.PreCalcProductParamsMass();


                using (var streamWriter = new StreamWriter(fullStatisticogPath, true))
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
            var fullStatisticogPath = SettingsGeneral.AbsolutePath + "modules\\ZakupiImport\\temp\\statisticLog.txt";

            var categories = new List<ZakupiImportCategory>(0);
            var currencies = new Dictionary<string, float>();

            CommonStatistic.TotalRow = GetRowsCount(fullPath);

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

                using (var streamWriter = new StreamWriter(fullStatisticogPath, true))
                {
                    streamWriter.WriteLine("Реиндексация " + DateTime.Now);
                }

                CategoryService.SetCategoryHierarchicallyEnabled(0);
                CategoryService.RecalculateProductsCountManual();
                ProductService.PreCalcProductParamsMass();

                using (var streamWriter = new StreamWriter(fullStatisticogPath, true))
                {
                    streamWriter.WriteLine("Конец импорта " + DateTime.Now);
                }
            }

            CommonStatistic.IsRun = false;
        }

        public static void ProcessPartialYml(string fullPath)
        {
            var fullStatisticogPath = SettingsGeneral.AbsolutePath + "modules\\ZakupiImport\\temp\\statisticLog.txt";

            var currencies = new Dictionary<string, float>();

            CommonStatistic.TotalRow = GetOfferRowsCount(fullPath);

            using (var reader = XmlReader.Create(fullPath,
                                              new XmlReaderSettings
                                              {
                                                  DtdProcessing = DtdProcessing.Ignore,
                                                  IgnoreWhitespace = true
                                              }))
            {
                using (var streamWriter = new StreamWriter(fullStatisticogPath, true, Encoding.UTF8))
                {
                    streamWriter.WriteLine("Начало импорта остатков " + DateTime.Now + " файл:" + fullPath);
                }

                CommonStatistic.IsRun = true;

                while (reader.Read() && CommonStatistic.IsRun)
                {
                    if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "offers")
                    {
                        using (var streamWriter = new StreamWriter(fullStatisticogPath, true, Encoding.UTF8))
                        {
                            streamWriter.WriteLine("Обработка продуктов");
                        }
                        ProcessOfferPartial(reader, currencies);
                    }

                    if (CommonStatistic.TotalRow == CommonStatistic.RowPosition)
                    {
                        break;
                    }
                }

                using (var streamWriter = new StreamWriter(fullStatisticogPath, true))
                {
                    streamWriter.WriteLine("Реиндексация " + DateTime.Now);
                }

                CategoryService.SetCategoryHierarchicallyEnabled(0);
                CategoryService.RecalculateProductsCountManual();
                ProductService.PreCalcProductParamsMass();

                using (var streamWriter = new StreamWriter(fullStatisticogPath, true))
                {
                    streamWriter.WriteLine("Конец импорта " + DateTime.Now);
                }
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
        private static void ProcessOffer(XmlReader reader, List<ZakupiImportCategory> categories, Dictionary<string, float> currencies, bool inJob = false)
        {
            while (reader.Read() && reader.Name != "offers" && (CommonStatistic.IsRun || inJob))
            {
                if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "offer")
                {
                    var isVendorModel = !string.IsNullOrEmpty(reader.GetAttribute("type")) && string.Equals(reader.GetAttribute("type"), "vendor.model");

                    var product = new Product
                    {
                        ArtNo = reader.GetAttribute("id"),
                        Enabled = reader.GetAttribute("available") == null || Convert.ToBoolean(reader.GetAttribute("available")),
                        Multiplicity = 1,
                        HasMultiOffer = true,
                        Weight = -1
                    };

                    var productCetegoryId = 0;
                    var pictureUrls = new List<string>();
                    var productProperties = new List<ProductProperty>();
                    var productUrl = string.Empty;
                    float offerCurrencyRate = 1;

                    var offer = new Offer
                    {
                        ArtNo = reader.GetAttribute("id"),
                        Amount = reader.GetAttribute("available") == null || Convert.ToBoolean(reader.GetAttribute("available"))
                            ? 1
                            : 0,
                        Main = true
                    };

                    while (reader.Read() && reader.Name != "offer")
                    {
                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "url" && reader.Read())
                        {
                            productUrl = reader.Value;
                        }
                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "price" && reader.Read())
                        {
                            offer.BasePrice = Convert.ToSingle(reader.Value, CultureInfo.InvariantCulture);
                        }

                        /*получение artno из тэга*/
                        if (reader.NodeType != XmlNodeType.EndElement && string.Equals(reader.Name.ToLower(), "vendorcode") && reader.Read())
                        {
                            offer.ArtNo = reader.Value;
                            product.ArtNo = reader.Value;
                        }

                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name.ToLower() == "currencyid" && reader.Read())
                        {
                            if (currencies.ContainsKey(reader.Value))
                            {
                                offerCurrencyRate = currencies[reader.Value];
                            }
                        }
                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name.ToLower() == "categoryid" && reader.Read())
                        {
                            if (categories.Any(item => string.Equals(item.YmlCategoryId, reader.Value)))
                            {
                                productCetegoryId = categories.First(item => string.Equals(item.YmlCategoryId, reader.Value)).AdvCategoryId;
                            }
                        }
                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "picture" && reader.Read())
                        {
                            pictureUrls.Add(reader.Value);
                        }
                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "vendor" && reader.Read())
                        {
                            var brandName = reader.Value;
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
                                if (isVendorModel)
                                {
                                    product.Name = reader.Value;
                                }
                        }
                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "description")
                        {
                            if (!reader.IsEmptyElement && reader.Read())
                            {
                                product.Description = reader.Value;
                            }
                        }
                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "descriptionShort")
                        {
                            if (!reader.IsEmptyElement && reader.Read())
                            {
                                product.BriefDescription = reader.Value;
                            }
                        }
                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "sales_notes")
                        {
                            if (!reader.IsEmptyElement && reader.Read())
                                product.SalesNote = reader.Value;
                        }
                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "name")
                        {
                            if (!reader.IsEmptyElement && reader.Read())
                                if (!isVendorModel)
                                {
                                    product.Name = reader.Value;
                                }
                        }
                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "adult")
                        {
                            if (!reader.IsEmptyElement && reader.Read())
                            {
                                var adult = false;
                                if (bool.TryParse(reader.Value, out adult))
                                {
                                    product.Adult = adult;
                                }
                            }
                        }
                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "weight" && reader.Read())
                        {
                            float weight = 0;
                            if (float.TryParse(reader.Value, out weight))
                            {
                                product.Weight = weight;
                            }
                        }
                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "dimensions" && reader.Read())
                        {
                            var dimensions = reader.Value.Split(new[] { '/' });

                            var length = 0f;
                            var width = 0f;
                            var height = 0f;

                            if (dimensions.Length == 3)
                            {
                                if (float.TryParse(dimensions[0], out length) &&
                                    float.TryParse(dimensions[1], out width) &&
                                    float.TryParse(dimensions[2], out height))
                                {
                                    product.Length = length;
                                    product.Width = width;
                                    product.Height = height;
                                }
                            }
                        }
                        if (reader.NodeType != XmlNodeType.EndElement && !reader.IsEmptyElement && reader.Name == "param")
                        {
                            // если парамерт имеет unit, то его добавлять в скобках 
                            var unit = !string.IsNullOrEmpty(reader.GetAttribute("unit")) ? " (" + reader.GetAttribute("unit") + ")" : string.Empty;
                            var paramName = reader.GetAttribute("name") + unit;

                            if (!string.IsNullOrEmpty(paramName) && reader.Read())
                            {
                                var paramValue = reader.Value;
                                if (!string.IsNullOrEmpty(paramValue))
                                {
                                    ProcessParam(productProperties, offer, product, paramName, paramValue);
                                }
                            }
                        }
                    }

                    AddUpdateProduct(product, offer, productCetegoryId, pictureUrls, productProperties, productUrl, offerCurrencyRate);

                    CommonStatistic.RowPosition++;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="currencies"></param>
        /// <param name="inJob"></param>
        private static void ProcessOfferPartial(XmlReader reader, Dictionary<string, float> currencies, bool inJob = false)
        {
            while (reader.Read() && reader.Name != "offers" && (CommonStatistic.IsRun || inJob))
            {
                if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "offer")
                {
                    var product = new Product
                    {
                        ArtNo = reader.GetAttribute("id"),
                        Enabled = reader.GetAttribute("available") == null || Convert.ToBoolean(reader.GetAttribute("available")),
                        Multiplicity = 1,
                        HasMultiOffer = true
                    };

                    float offerCurrencyRate = 1;

                    var offer = new Offer
                    {
                        ArtNo = reader.GetAttribute("id"),
                        Amount = reader.GetAttribute("available") == null || Convert.ToBoolean(reader.GetAttribute("available"))
                            ? 1
                            : 0,
                        Main = true
                    };

                    while (reader.Read() && reader.Name != "offer")
                    {

                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "price" && reader.Read())
                        {
                            offer.BasePrice = Convert.ToSingle(reader.Value, CultureInfo.InvariantCulture);
                        }

                        /*получение artno из тэга*/
                        if (reader.NodeType != XmlNodeType.EndElement && string.Equals(reader.Name.ToLower(), "vendorcode") && reader.Read())
                        {
                            offer.ArtNo = reader.Value;
                            product.ArtNo = reader.Value;
                        }

                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name.ToLower() == "currencyid" && reader.Read())
                        {
                            if (currencies.ContainsKey(reader.Value))
                            {
                                offerCurrencyRate = currencies[reader.Value];
                            }
                        }
                    }

                    offer.BasePrice = offer.BasePrice * (CurrencyService.CurrentCurrency.Rate / offerCurrencyRate);

                    var existOffer = OfferService.GetOffer(offer.ArtNo);
                    if (existOffer != null)
                    {
                        existOffer.Amount = offer.Amount;
                        existOffer.BasePrice = offer.BasePrice;
                        OfferService.UpdateOffer(existOffer);
                        CommonStatistic.WriteLog("Товар обновлен " + product.ArtNo);
                        CommonStatistic.TotalUpdateRow++;
                    }
                    else
                    {
                        CommonStatistic.WriteLog("Товар не найден " + product.ArtNo);
                        CommonStatistic.TotalErrorRow++;
                    }

                    //AddUpdateProduct(product, offer, productCetegoryId, pictureUrls, productProperties, productUrl, offerCurrencyRate);

                    CommonStatistic.RowPosition++;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="categories"></param>
        private static void ProcessCategories(XmlReader reader, List<ZakupiImportCategory> categories)
        {
            while (reader.Read() && reader.Name != "categories")
            {
                if (reader.IsStartElement() && reader.Name == "category")
                {
                    var ymlCategoryId = reader.GetAttribute("id");
                    var ymlParentCategoryId = reader.GetAttribute("parentId");

                    if (reader.Read())
                    {
                        categories.Add(new ZakupiImportCategory
                        {
                            YmlCategoryId = string.IsNullOrEmpty(ymlCategoryId) ? "0" : ymlCategoryId,
                            YmlParentCategoryId = string.IsNullOrEmpty(ymlParentCategoryId) ? "0" : ymlParentCategoryId,
                            YmlCategoryName = reader.Value.Trim()
                        });
                    }
                }
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
                    CommonStatistic.WriteLog("Категория обновлена " + category.YmlCategoryName);
                    CommonStatistic.TotalUpdateRow++;
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
                    }, false, false);
                    CommonStatistic.WriteLog("Категория добавлена " + category.YmlCategoryName);
                    CommonStatistic.TotalAddRow++;
                }
                CommonStatistic.RowPosition++;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="ymlCategory"></param>
        /// <returns></returns>
        private static int AddUpdateCategory(List<ZakupiImportCategory> categories, ZakupiImportCategory ymlCategory)
        {
            var categoryParentId = 0;
            var parentYmlCategory = categories.FirstOrDefault(item => string.Equals(item.YmlCategoryId, ymlCategory.YmlParentCategoryId));
            if (parentYmlCategory != null && parentYmlCategory.AdvCategoryId != 0)
            {
                categoryParentId = parentYmlCategory.AdvCategoryId;
            }
            else if (parentYmlCategory != null)
            {
                categoryParentId = AddUpdateCategory(categories, parentYmlCategory);
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
                }, false, false);
            }
            return ymlCategory.AdvCategoryId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="currencies"></param>
        protected static void ProcessCurrencies(XmlReader reader, Dictionary<string, float> currencies)
        {
            while (reader.Read() && reader.Name != "currencies")
            {
                if (reader.IsStartElement() && reader.Name == "currency")
                {
                    float rate = 0;
                    if (Single.TryParse(reader.GetAttribute("rate"), out rate))
                    {
                        currencies.Add(reader.GetAttribute("id"), Convert.ToSingle(reader.GetAttribute("rate"), CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        CommonStatistic.WriteLog("Неверный формат курса валюты " + reader.GetAttribute("id"));
                        CommonStatistic.TotalErrorRow++;
                    }
                }
            }
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
                    if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "feature" && !reader.IsEmptyElement)
                    {
                        if (reader.Read())
                        {
                            var property = PropertyService.GetPropertyByName(reader.Value);
                            if (property == null)
                            {
                                PropertyService.AddProperty(new Property
                                {
                                    Name = reader.Value,
                                    UseInDetails = true,
                                    UseInFilter = true
                                });
                            }
                            //Features.Add(property);
                        }
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productProperties"></param>
        /// <param name="offer"></param>
        /// <param name="product"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private static void ProcessParam(List<ProductProperty> productProperties, Offer offer, Product product, string name, string value)
        {
            var productProperty = new ProductProperty
            {
                Name = name,
                Value = value
            };

            var weight = 0f;

            if (productProperty.Name.ToLower().Contains("вес упаковки") && float.TryParse(productProperty.Value, out weight))
            {
                product.Weight = productProperty.Name.Contains("(г)") ? weight / 1000 : weight;
            }
            else if (productProperty.Name.ToLower().Contains("вес") && product.Weight != 0 && float.TryParse(productProperty.Value, out weight))
            {
                product.Weight = productProperty.Name.Contains("(г)") ? weight / 1000 : weight;
            }
            else
            {
                productProperties.Add(productProperty);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productProperties"></param>
        /// <returns></returns>
        private static void ProcessProductDimensions(Product product, List<ProductProperty> productProperties)
        {
            if (productProperties.Any(item => item.Name.Contains("Длина упаковки"))
                && productProperties.Any(item => item.Name.Contains("Ширина упаковки"))
                && productProperties.Any(item => item.Name.Contains("Высота упаковки")))
            {
                var length = 0f;
                var width = 0f;
                var height = 0f;

                if (float.TryParse(productProperties.FirstOrDefault(item => item.Name.Contains("Длина упаковки")).Value, out length) &&
                    float.TryParse(productProperties.FirstOrDefault(item => item.Name.Contains("Ширина упаковки")).Value, out width) &&
                    float.TryParse(productProperties.FirstOrDefault(item => item.Name.Contains("Высота упаковки")).Value, out height))
                {
                    product.Length = length;
                    product.Width = width;
                    product.Height = height;
                }
            }
            else if (
                productProperties.Any(item => item.Name.Contains("Глубина"))
                && productProperties.Any(item => item.Name.Contains("Ширина"))
                && productProperties.Any(item => item.Name.Contains("Высота")))
            {

                var length = 0f;
                var width = 0f;
                var height = 0f;

                if (float.TryParse(productProperties.FirstOrDefault(item => item.Name.Contains("Глубина")).Value, out length) &&
                    float.TryParse(productProperties.FirstOrDefault(item => item.Name.Contains("Ширина")).Value, out width) &&
                    float.TryParse(productProperties.FirstOrDefault(item => item.Name.Contains("Высота")).Value, out height))
                {
                    product.Length = length;
                    product.Width = width;
                    product.Height = height;
                }
            }
            else if (productProperties.Any(item => item.Name.Contains("Размеры (ШхГхВ)")))
            {
                var length = 0f;
                var width = 0f;
                var height = 0f;

                var dimensions = productProperties.FirstOrDefault(item => item.Name.Contains("Размеры (ШхГхВ)")).Value.Split(new[] { "х" }, StringSplitOptions.RemoveEmptyEntries);
                if (dimensions.Length == 3)
                {
                    if (float.TryParse(dimensions[0], out length) &&
                        float.TryParse(dimensions[1], out width) &&
                        float.TryParse(dimensions[2], out height))
                    {
                        product.Length = length;
                        product.Width = width;
                        product.Height = height;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productProperties"></param>
        private static void AddUpdateProductProperties(int productId, List<ProductProperty> productProperties)
        {
            foreach (var productProperty in productProperties)
            {
                //TODO need optimization 
                //PropertyService.AddProductProperty(product.ProductId, productProperty.Name, productProperty.Value, 0, 0, true, true);

                var property = PropertyService.GetPropertyByName(productProperty.Name);

                if (property == null)
                {
                    property = new Property()
                    {
                        Name = productProperty.Name,
                        Type = 1,
                        UseInDetails = true,
                        UseInFilter = true
                    };

                    property.PropertyId = PropertyService.AddProperty(property);
                }

                var propertyValue = PropertyService.GetPropertyValueByName(property.PropertyId, productProperty.Value);

                if (propertyValue == null)
                {
                    propertyValue = new PropertyValue()
                    {
                        Value = productProperty.Value,
                        PropertyId = property.PropertyId
                    };

                    propertyValue.PropertyValueId = PropertyService.AddPropertyValue(propertyValue);
                }

                if (PropertyService.GetPropertyValuesByProductId(productId).All(p => p.PropertyValueId != propertyValue.PropertyValueId))
                {
                    PropertyService.AddProductProperyValue(propertyValue.PropertyValueId, productId);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="pictureUrls"></param>
        /// <param name="colorId"></param>
        private static void AddUpdateProductPhotos(int productId, List<string> pictureUrls, int? colorId)
        {
            foreach (var pictureUrl in pictureUrls)
            {
                if (string.IsNullOrEmpty(pictureUrl))
                    continue;

                var photoname = pictureUrl.Md5() + "." + (pictureUrl.Split('?').FirstOrDefault().Split('.').LastOrDefault() ?? "jpg");

                if (!PhotoService.IsProductHaveThisPhotoByName(productId, photoname))
                {
                    if (!FileHelpers.DownloadRemoteImageFile(pictureUrl, FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoname)))
                        continue;

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
        /// <param name="offer"></param>
        /// <param name="productCetegoryId"></param>
        /// <param name="pictureUrls"></param>
        /// <param name="productProperties"></param>
        /// <param name="productUrl"></param>
        /// <param name="offerCurrencyRate"></param>
        private static void AddUpdateProduct(Product product, Offer offer, int productCetegoryId, List<string> pictureUrls, List<ProductProperty> productProperties, string productUrl, float offerCurrencyRate)
        {
            try
            {
                if (!string.IsNullOrEmpty(product.Name))
                {
                    product.UrlPath = UrlService.GetAvailableValidUrl(0, ParamType.Product, product.Name);

                    offer.BasePrice = offer.BasePrice * (CurrencyService.CurrentCurrency.Rate / offerCurrencyRate);

                    product.Offers.Add(offer);


                    ProcessProductDimensions(product, productProperties);

                    if (product.Weight == -1)
                    {
                        product.Weight = 0;
                    }

                    var existProduct = ProductService.GetProduct(product.ArtNo);

                    if (existProduct != null)
                    {
                        offer.ProductId = existProduct.ProductId;

                        //обновление офера
                        var existOffer = OfferService.GetOffer(offer.ArtNo);
                        if (existProduct.Offers.Any(o => o.Main && o.ArtNo != offer.ArtNo))
                        {
                            offer.Main = false;
                            if (existOffer != null)
                            {
                                existOffer.Main = false;
                            }
                        }

                        if (existOffer != null)
                        {
                            existOffer.Amount = offer.Amount;
                            existOffer.ColorID = offer.ColorID;
                            existOffer.SizeID = offer.SizeID;

                            existOffer.BasePrice = offer.BasePrice;
                            existOffer.SupplyPrice = offer.SupplyPrice;

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
                            existProduct.Offers.Add(offer);
                            OfferService.AddOffer(offer);
                        }

                        if (ZakupiImport.UpdateName)
                        {
                            existProduct.Name = product.Name;
                        }
                        if (ZakupiImport.UpdateDescription)
                        {
                            existProduct.Description = product.Description;
                            existProduct.BriefDescription = product.BriefDescription;
                        }

                        existProduct.Enabled = product.Enabled;

                        existProduct.Width = product.Width;
                        existProduct.Length = product.Length;
                        existProduct.Height = product.Height;

                        if (product.BrandId != 0)
                        {
                            existProduct.BrandId = product.BrandId;
                        }
                        if (product.Weight != 0)
                        {
                            existProduct.Weight = product.Weight;
                        }
                        product.ModifiedBy = ZakupiImport.ModuleID;
                        ProductService.UpdateProduct(existProduct, false);


                        product.ProductId = existProduct.ProductId;
                        CommonStatistic.TotalUpdateRow++;
                        CommonStatistic.WriteLog("Товар обновлен " + product.ArtNo);
                    }
                    else
                    {
                        product.ModifiedBy = ZakupiImport.ModuleID;
                        var currency = CurrencyService.GetCurrencyByIso3("RUB");
                        product.CurrencyID = currency != null ? currency.CurrencyId : CurrencyService.CurrentCurrency.CurrencyId;
                        product.ProductId = ProductService.AddProduct(product, true);
                        CommonStatistic.TotalAddRow++;
                        CommonStatistic.WriteLog("Товар добавлен " + product.ArtNo);
                    }
                }

                if (product.ProductId == 0)
                {
                    CommonStatistic.WriteLog("Не удалось добавить товар: " + product.ArtNo);
                    CommonStatistic.TotalErrorRow++;
                    CommonStatistic.RowPosition++;
                    return;
                }

                if (productCetegoryId != 0)
                {
                    ProductService.AddProductLink(product.ProductId, productCetegoryId, 0, false);
                }

                if (ZakupiImport.UpdateParams)
                {
                    AddUpdateProductProperties(product.ProductId, productProperties);
                }

                if (ZakupiImport.UpdatePhotos)
                {
                    AddUpdateProductPhotos(product.ProductId, pictureUrls, offer.ColorID);
                }
            }
            catch (Exception ex)
            {
                CommonStatistic.WriteLog("Ошибка при добавлении товара: " + product.ArtNo + " , ошибка: " + ex.Message);
                CommonStatistic.TotalErrorRow++;
                Debug.LogError(ex, false);
            }
        }
    }
}