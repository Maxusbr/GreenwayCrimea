//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.SQL;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Repository.Currencies;
using AdvantShop.SEO;
using AdvantShop.Statistic;
using CsvHelper;

namespace AdvantShop.Module.SupplierOfHappiness.Domain
{
    public class SupplierOfHappinessProcessFile
    {

        public static bool UpdateIsProcessing;

        private static List<int> productIds = new List<int>();
        private static List<string> offerArtnos = new List<string>();

        private struct NewOffers
        {
            public string offerArtNo;
            public int ProductId;
        }

        private class ProductImg
        {
            public int ProductId { get; set; }
            public List<string> ImgUrl { get; set; }
        }

        //public enum PriceType
        //{
        //    RetailPrice = 6,
        //    BaseRetailPrice = 7,
        //    WholePrice = 8,
        //    BaseWholePrice = 9
        //}

        public static void ProcessFileInJob()
        {
            if (!ModuleSettingsProvider.GetSettingValue<bool>("AutoUpdateActiveFull", SupplierOfHappiness.ModuleID) || UpdateIsProcessing)
            {
                return;
            }

            var filePath = HostingEnvironment.MapPath(SupplierOfHappiness.FilePathFull);

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            new WebClient().DownloadFile(SupplierOfHappiness.UrlPathFull + "?param=" + Guid.NewGuid(), filePath);

            UpdateIsProcessing = true;

            ProcessFile(filePath, true);

            UpdateIsProcessing = false;
        }

        public static void ProcessFileQuickInJob()
        {
            if (!ModuleSettingsProvider.GetSettingValue<bool>("AutoUpdateActiveQuick",
                SupplierOfHappiness.ModuleID) || UpdateIsProcessing)
            {
                return;
            }

            var filePath = HostingEnvironment.MapPath(SupplierOfHappiness.FilePathQuick);

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            new WebClient().DownloadFile(SupplierOfHappiness.UrlPathQuick + "?param=" + Guid.NewGuid(), filePath);

            UpdateIsProcessing = true;

            ProcessFileQuick(filePath, true);

            UpdateIsProcessing = false;
        }

        /*
         * 0- prodID – наш внутренний идентификатор модели;
           1- aID - наш внутренний идентификатор данного товарного предложения с точностью до цвет/размер (SKU).
           2- Barcode - штрихкод;
           3- Vendor - производитель;
           4- VendorCode - артикул производителя;
           5- Name – название модели;
           6- RetailPrice – рекомендованная розничная цена c учетом скидки;
           7- BaseRetailPrice - базовая рекомендованная розничная цена;
           8- WholePrice – оптовая цена c учетом скидки;
           9- BaseWholePrice – базовая оптовая цена;
           10- Discount - размер скидки;
           11- InSale – свободный остаток. Если 0, - то нет в наличии;
           12- ShippingDate - дата и время возможной отгрузки с нашего склада;
           13- Description - описание;
           14- Brutto - вес в килограммах;
           15- Batteries - тип и количество батареек;
           16- Pack - тип упаковки;
           17- Material - материал;
           18- Lenght - длина в сантиметрах;
           19- Diameter - диаметр в сантиметрах;
           20- Collection - коллекция;
           21- Image – ссылки на фотографии разделенные пробелом;
           22- Category – корневая категория;
           23- SubCategory – подкатегория;
           24- Color - цвет;
           25- Size - размер;
           26- BestSeller - хит;
           27- New - новинка;
           28- Function - свойство, функция;
           29- addFunction - свойство, доп функция;
           30- Vibration - свойство, вибрация;
           31- Volume - свойство, объем;
           32- ModelYear - свойство, год выпуска модели;
         */
        public static void ProcessFile(string fullPath, bool inJob = false)
        {
            var log = new SupplierOfHappinessLog();
            log.Write(DateTime.Now.ToString("[dd.MM.yy HH:mm]") + " Обновление каталога");

            if (!File.Exists(fullPath))
            {
                if (!inJob)
                {
                    CommonStatistic.IsRun = false;
                }
                return;
            }

            log.TotalRow = GetRowsCount(fullPath);
            if (!inJob)
            {
                CommonStatistic.RowPosition = 1;
                CommonStatistic.TotalRow = log.TotalRow;
            }

            float extraCharge = ModuleSettingsProvider.GetSettingValue<float>("ExtraCharge", SupplierOfHappiness.ModuleID);

            var categories = SupplierOfHappinessRepository.GetCategories();

            var isFirstImport = !categories.Any();

            var stringAddCategories = string.Empty;

            var offerIsAdded = new List<NewOffers>();

            using (var csvReader = new CsvReader(new StreamReader(fullPath, Encoding.UTF8, true)))
            {
                csvReader.Configuration.Delimiter = ";";
                csvReader.Configuration.HasHeaderRecord = true;
                while (csvReader.Read() && (CommonStatistic.IsRun || inJob))
                {
                    try
                    {
                        if (csvReader.CurrentRecord.Length != 33)
                        {
                            log.TotalErrorRow++;

                            if (!inJob)
                            {
                                CommonStatistic.WriteLog("Не верная строка в файле, количество столбцов != 33");
                                CommonStatistic.RowPosition++;
                                CommonStatistic.TotalErrorRow++;
                            }
                            continue;
                        }

                        var sohCategory =
                            categories.FirstOrDefault(
                                item =>
                                    string.Equals(item.Category, csvReader.CurrentRecord[22].Trim()) &&
                                    string.Equals(item.SubCategory, csvReader.CurrentRecord[23].Trim()));

                        if (sohCategory != null && sohCategory.AdvCategoryId == null)
                        {
                            if (!inJob)
                            {
                                CommonStatistic.WriteLog("Продукт не обработан, нет привязанной категории для " + csvReader.CurrentRecord[22].Trim() + " -> " + csvReader.CurrentRecord[23].Trim());
                                CommonStatistic.TotalErrorRow++;
                            }
                            CommonStatistic.RowPosition++;
                            continue;
                        }

                        if (sohCategory == null)
                        {
                            sohCategory = new SupplierOfHappinessCategory
                            {
                                Category = csvReader.CurrentRecord[22].Trim(),
                                SubCategory = csvReader.CurrentRecord[23].Trim(),
                                AdvCategoryId = null
                            };

                            if (SupplierOfHappinessRepository.AddCategory(sohCategory))
                            {
                                categories.Add(sohCategory);

                                if (isFirstImport)
                                {
                                    sohCategory.AdvCategoryId =
                                        SupplierOfHappinessService.SetDefaultCategory(
                                            csvReader.CurrentRecord[22].Trim(),
                                            csvReader.CurrentRecord[23].Trim());
                                }
                                else
                                {
                                    stringAddCategories += csvReader.CurrentRecord[22].Trim() + "//" +
                                                           csvReader.CurrentRecord[23].Trim() + "<br/>";
                                    continue;
                                }
                            }
                        }


                        //product
                        var product = GetProduct(csvReader.CurrentRecord[0]);

                        product.ArtNo = csvReader.CurrentRecord[0];
                        product.Name = csvReader.CurrentRecord[5];
                        product.Description = csvReader.CurrentRecord[13];
                        product.BarCode = csvReader.CurrentRecord[2] ?? string.Empty;
                        product.Adult = true;
                        product.Enabled = true;
                        product.Multiplicity = 1;
                        product.BestSeller = !csvReader.CurrentRecord[26].IsNullOrEmpty() ? csvReader.CurrentRecord[26] == "1" ? true : false : product.BestSeller;
                        product.New = !csvReader.CurrentRecord[27].IsNullOrEmpty() ? csvReader.CurrentRecord[27] == "1" ? true : false : product.New;
                        product.Meta = new MetaInfo
                        {
                            H1 = string.Empty,
                            MetaDescription = string.Empty,
                            MetaKeywords = string.Empty,
                            Title = string.Empty,
                            Type = MetaType.Product
                        };

                        product.ModifiedBy = "SupplierOfHappines";
                        product.CurrencyID = CurrencyService.CurrentCurrency.CurrencyId;

                        product.UrlPath = UrlService.GetAvailableValidUrl(product.ProductId, ParamType.Product,
                            StringHelper.Translit(product.Name));


                        if (!string.IsNullOrEmpty(csvReader.CurrentRecord[3]))
                        {
                            var brandId = BrandService.GetBrandIdByName(csvReader.CurrentRecord[3]);
                            if (brandId == 0)
                            {
                                brandId = BrandService.AddBrand(new Brand
                                {
                                    Name = csvReader.CurrentRecord[3],
                                    Enabled = true,
                                    UrlPath =
                                        UrlService.GetAvailableValidUrl(0, ParamType.Brand,
                                            StringHelper.Translit(csvReader.CurrentRecord[3]))
                                });
                            }
                            product.BrandId = brandId;
                        }

                        var weight = 0f;
                        if (Single.TryParse(csvReader.CurrentRecord[14], NumberStyles.Float,
                            CultureInfo.InvariantCulture, out weight))
                        {
                            product.Weight = weight;
                        }
                        else if(product.Weight == 0)
                        {
                            product.Weight = 0.1f;
                        }

                        // offer
                        var offer = OfferService.GetOffer(csvReader.CurrentRecord[1]) ?? new Offer();
                        offer.ArtNo = csvReader.CurrentRecord[1];
                        offer.Main = false;

                        if (product.ProductId != 0)
                        {
                            offer.ProductId = product.ProductId;
                        }

                        var stringRetailPriceValue = string.Empty;
                        string stringDiscount = "0";
                        switch (
                            ModuleSettingsProvider.GetSettingValue<string>("RetailPriceType",
                                SupplierOfHappiness.ModuleID))
                        {
                            case "RetailPrice":
                                stringRetailPriceValue = csvReader.CurrentRecord[6];
                                break;
                            case "BaseRetailPrice":
                                stringRetailPriceValue = csvReader.CurrentRecord[7];
                                break;

                            case "BaseRetailPriceWithDiscount":
                                stringRetailPriceValue = csvReader.CurrentRecord[7];
                                stringDiscount = csvReader.CurrentRecord[10];
                                break;

                            case "WholePrice":
                                stringRetailPriceValue = csvReader.CurrentRecord[8];
                                break;

                            case "BaseWholePrice":
                                stringRetailPriceValue = csvReader.CurrentRecord[9];
                                break;

                            case "BaseWholePriceWithDiscount":
                                stringRetailPriceValue = csvReader.CurrentRecord[9];
                                stringDiscount = csvReader.CurrentRecord[10];
                                break;

                            default:
                                stringRetailPriceValue = csvReader.CurrentRecord[6];
                                break;
                        }

                        var stringWholePriceValue = string.Empty;
                        switch (
                            ModuleSettingsProvider.GetSettingValue<string>("WholePriceType",
                                SupplierOfHappiness.ModuleID))
                        {
                            case "WholePrice":
                                stringWholePriceValue = csvReader.CurrentRecord[8];
                                break;
                            case "BaseWholePrice":
                                stringWholePriceValue = csvReader.CurrentRecord[9];
                                break;
                            default:
                                stringWholePriceValue = csvReader.CurrentRecord[8];
                                break;
                        }

                        var price = 0f;
                        if (Single.TryParse(stringRetailPriceValue, NumberStyles.Float, CultureInfo.InvariantCulture,
                            out price))
                        {
                            offer.BasePrice = price + price * extraCharge / 100;
                            /*
                            if (extraCharge >= 0)
                            {
                                offer.BasePrice = price + price * extraCharge / 100;
                            }
                            else if (extraCharge < 0)
                            {
                                offer.BasePrice = price;
                                product.Discount = -1 * extraCharge;
                            }*/
                        }

                        if (ModuleSettingsProvider.GetSettingValue<bool>("UpdateDiscount", SupplierOfHappiness.ModuleID))
                        {
                            var discount = 0f;
                            if (Single.TryParse(stringDiscount, NumberStyles.Float, CultureInfo.InvariantCulture, out discount))
                            {
                                product.Discount = new Discount(discount, 0);
                            }
                        }

                        var supplyPrice = 0f;
                        if (Single.TryParse(stringWholePriceValue, NumberStyles.Float, CultureInfo.InvariantCulture,
                            out supplyPrice))
                        {
                            offer.SupplyPrice = supplyPrice;
                        }

                        var amount = 0;
                        if (int.TryParse(csvReader.CurrentRecord[11], out amount))
                        {
                            offer.Amount = amount;
                        }

                        if (!string.IsNullOrEmpty(csvReader.CurrentRecord[24].Trim()))
                        {
                            var color = ColorService.GetColor(csvReader.CurrentRecord[24]);
                            if (color != null)
                            {
                                offer.ColorID = color.ColorId;
                            }
                            else
                            {
                                offer.ColorID = ColorService.AddColor(new Color
                                {
                                    ColorName = csvReader.CurrentRecord[24],
                                    ColorCode = "#000000",
                                    SortOrder = 0,
                                });
                            }
                        }

                        if (!string.IsNullOrEmpty(csvReader.CurrentRecord[25].Trim()))
                        {
                            var size = SizeService.GetSize(csvReader.CurrentRecord[25]);
                            if (size != null)
                            {
                                offer.SizeID = size.SizeId;
                            }
                            else
                            {
                                offer.SizeID = SizeService.AddSize(new Size
                                {
                                    SizeName = csvReader.CurrentRecord[25],
                                    SortOrder = 0
                                });
                            }
                        }


                        if (product.ProductId != 0)
                        {
                            int index = -1;
                            if (product.Offers != null && (index = product.Offers.FindIndex(x => string.Equals(x.ArtNo, offer.ArtNo.Trim()))) > -1)
                            {
                                product.Offers[index] = offer;
                            }
                            else
                            {
                                product.Offers.Add(offer);
                            }

                            if (product.Offers.Count(x => x.Main) == 0)
                            {
                                product.Offers[0].Main = true;
                            }

                            product.HasMultiOffer = product.Offers.Count > 0 && product.Offers.Count(x => x.ColorID != null || x.SizeID != null) > 0 || product.Offers.Count > 1;

                            ProductService.UpdateProduct(product, false);

                            log.TotalUpdateRow++;
                            if (!inJob)
                            {
                                CommonStatistic.TotalUpdateRow++;
                            }

                            offerIsAdded.Add(new NewOffers { offerArtNo = offer.ArtNo, ProductId = product.ProductId });

                        }
                        else
                        {
                            if (offer.OfferId == 0)
                            {
                                product.Offers.Add(offer);
                            }

                            product.HasMultiOffer = product.Offers.Count > 0 && product.Offers.Count(x => x.ColorID != null || x.SizeID != null) > 0;

                            if (product.Offers.Count(x => x.Main) == 0 && product.Offers.Count > 0)
                            {
                                product.Offers[0].Main = true;
                            }

                            product.ProductId = ProductService.AddProduct(product, false);

                            if (product.ProductId == 0)
                            {
                                log.TotalErrorRow++;
                                if (!inJob)
                                {
                                    CommonStatistic.WriteLog("Не удалось добавить продукт " + product.ArtNo);
                                    CommonStatistic.RowPosition++;
                                    CommonStatistic.TotalErrorRow++;
                                }
                                continue;
                            }

                            offerIsAdded.Add(new NewOffers { offerArtNo = offer.ArtNo, ProductId = product.ProductId });

                            if (sohCategory.AdvCategoryId != null)
                            {
                                ProductService.AddProductLink(product.ProductId, (int)sohCategory.AdvCategoryId, 0, true);
                            }

                            log.TotalAddRow++;
                            if (!inJob)
                            {
                                CommonStatistic.TotalAddRow++;
                            }
                        }

                        productIds.Add(product.ProductId);
                        offerArtnos.Add(offer.ArtNo);
                        //properties
                        var productProperties = new List<ProductProperty>
                            {
                                new ProductProperty
                                {
                                    Name = "Артикул производителя",
                                    Value = csvReader.CurrentRecord[4],
                                    DisplayInBrief = true,
                                    DisplayInDetails = false,
                                    DisplayInFilter = false
                                },

                                new ProductProperty
                                {
                                    Name = "Тип и количество батареек",
                                    Value = csvReader.CurrentRecord[15]
                                },
                                new ProductProperty
                                {
                                    Name = "Тип упаковки",
                                    Value = csvReader.CurrentRecord[16]
                                },
                                new ProductProperty
                                {
                                    Name = "Материал",
                                    Value = csvReader.CurrentRecord[17]
                                },
                                new ProductProperty
                                {
                                    Name = "Длина в сантиметрах",
                                    Value = csvReader.CurrentRecord[18]
                                },
                                new ProductProperty
                                {
                                    Name = "Диаметр в сантиметрах",
                                    Value = csvReader.CurrentRecord[19]
                                },
                                new ProductProperty
                                {
                                    Name = "Коллекция",
                                    Value = csvReader.CurrentRecord[20]
                                }
                            };

                        if (!string.IsNullOrEmpty(csvReader.CurrentRecord[28]))
                        {
                            productProperties.Add(new ProductProperty { Name = "Функция", Value = csvReader.CurrentRecord[28] });
                        }
                        if (!string.IsNullOrEmpty(csvReader.CurrentRecord[29]))
                        {
                            productProperties.Add(new ProductProperty { Name = "Дополнительная функция", Value = csvReader.CurrentRecord[29] });
                        }
                        if (!string.IsNullOrEmpty(csvReader.CurrentRecord[30]))
                        {
                            productProperties.Add(new ProductProperty { Name = "Вибрация", Value = csvReader.CurrentRecord[30] == "1" ? "Да" : "Нет" });
                        }
                        if (!string.IsNullOrEmpty(csvReader.CurrentRecord[31]))
                        {
                            productProperties.Add(new ProductProperty { Name = "Объем", Value = csvReader.CurrentRecord[31] });
                        }
                        if (!string.IsNullOrEmpty(csvReader.CurrentRecord[32]))
                        {
                            productProperties.Add(new ProductProperty { Name = "Год выпуска модели", Value = csvReader.CurrentRecord[32] });
                        }

                        ProcessProperties(product, productProperties);

                        //photos
                        ProcessPhotos(product, csvReader.CurrentRecord[21].Split(new[] { ' ' }));

                        if (!inJob)
                        {
                            CommonStatistic.RowPosition++;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Write(DateTime.Now.ToString("[dd.MM.yy HH:mm]") + " Ошибка " + ex.Message + " " +
                                  ex.StackTrace);

                        if (!inJob)
                        {
                            CommonStatistic.WriteLog(DateTime.Now.ToString("[dd.MM.yy HH:mm]") + " Ошибка " + ex.Message + " " + ex.StackTrace);
                            CommonStatistic.RowPosition++;
                            CommonStatistic.TotalErrorRow++;
                        }
                    }
                }
            }

            Task.Factory.StartNew(() =>
            {
                if (ModuleSettingsProvider.GetSettingValue<bool>("DeactivateProducts", SupplierOfHappiness.ModuleID))
                {
                    DisableAllProductsNotInList(productIds);
                }

                if (ModuleSettingsProvider.GetSettingValue<bool>("DeactivateProductsNotInStock", SupplierOfHappiness.ModuleID))
                {
                    DisableAllProductsNotInStock();
                }

                if (ModuleSettingsProvider.GetSettingValue<bool>("AmountNulling", SupplierOfHappiness.ModuleID))
                {
                    ClearAmountAllProductsNotInList(offerArtnos);
                }


                CategoryService.SetCategoryHierarchicallyEnabled(0);
                CategoryService.RecalculateProductsCountManual();
                ProductService.PreCalcProductParamsMassInBackground();
                SQLDataAccess.ExecuteNonQuery("[Settings].[sp_Reindex]", CommandType.StoredProcedure);
            }).ContinueWith(x => Debug.Log.Error(x.Exception), TaskContinuationOptions.OnlyOnFaulted);

            log.WriteTotal();

            DeleteOldOffers(offerIsAdded);

            if (!inJob)
            {
                CommonStatistic.IsRun = false;
            }

            if (!string.IsNullOrEmpty(stringAddCategories))
            {
                ModulesService.SendModuleMail(
                    Guid.Empty,
                    "Модуль 'Поставщик счастья' - новая категория",
                    "Появились новые категории требующие привязки <br/>" + stringAddCategories,
                    SettingsMail.EmailForFeedback,
                    true);
            }
        }

        /*
           0- prodID – наш внутренний идентификатор модели;
           1 aID - наш внутренний идентификатор данного товарного предложения с точностью до цвет/размер (SKU).
           //1 Name – название модели;
           2 RetailPrice – рекомендованная розничная цена c учетом скидки;
           3 BaseRetailPrice - базовая рекомендованная розничная цена;
           4 WholePrice – оптовая цена c учетом скидки;
           5 BaseWholePrice – базовая оптовая цена;
           6 Discount - размер скидки;
           7 InSale – свободный остаток. Если 0, - то нет в наличии;
           8 ShippingDate - дата и время возможной отгрузки с нашего склада;
         */
        public static void ProcessFileQuick(string fullPath, bool inJob = false)
        {
            var log = new SupplierOfHappinessLog();
            log.Write(DateTime.Now.ToString("[dd.MM.yy HH:mm]") + " Обновление остатков");
            if (!File.Exists(fullPath))
            {
                if (!inJob)
                {
                    CommonStatistic.IsRun = false;
                }
                return;
            }

            log.TotalRow = GetRowsCount(fullPath);
            if (!inJob)
            {
                CommonStatistic.RowPosition = 1;
                CommonStatistic.TotalRow = log.TotalRow;
            }

            float extraCharge = ModuleSettingsProvider.GetSettingValue<float>("ExtraCharge", SupplierOfHappiness.ModuleID);

            using (var csvReader = new CsvReader(new StreamReader(fullPath, Encoding.UTF8, true)))
            {
                csvReader.Configuration.Delimiter = ";";
                csvReader.Configuration.HasHeaderRecord = true;
                while (csvReader.Read() && (CommonStatistic.IsRun || inJob))
                {
                    try
                    {
                        if (csvReader.CurrentRecord.Length != 9)
                        {
                            log.TotalErrorRow++;
                            if (!inJob)
                            {
                                CommonStatistic.WriteLog("Не верная строка в файле, количество столбцов != 9");
                                CommonStatistic.RowPosition++;
                                CommonStatistic.TotalErrorRow++;
                            }
                            continue;
                        }

                        var product = ProductService.GetProduct(csvReader.CurrentRecord[0]);
                        
                        // offer
                        var offer = new Offer();
                        if (product != null && !product.HasMultiOffer && product.Offers != null && product.Offers.Count > 0)
                        {
                            offer = product.Offers.First(); //OfferService.GetOffer(csvReader.CurrentRecord[0]);
                        }
                        else if (product != null && product.HasMultiOffer && product.Offers != null && product.Offers.Count(x => x.ArtNo == csvReader.CurrentRecord[1]) > 0)
                        {
                            offer = product.Offers.First(x => x.ArtNo == csvReader.CurrentRecord[1]);
                        }
                        else
                        {
                            log.TotalErrorRow++;
                            if (!inJob)
                            {
                                CommonStatistic.WriteLog(
                                    string.Format("Товар с артикулом {0} не найден. Требуется обновление каталога. ",
                                        csvReader.CurrentRecord[0]));
                                CommonStatistic.RowPosition++;
                                CommonStatistic.TotalErrorRow++;
                            }
                            continue;
                        }

                        var stringRetailPriceValue = string.Empty;
                        var stringDiscount = "0";
                        switch (
                            ModuleSettingsProvider.GetSettingValue<string>("RetailPriceType",
                                SupplierOfHappiness.ModuleID))
                        {
                            case "RetailPrice":
                                stringRetailPriceValue = csvReader.CurrentRecord[2];
                                break;
                            case "BaseRetailPrice":
                                stringRetailPriceValue = csvReader.CurrentRecord[3];
                                break;

                            case "BaseRetailPriceWithDiscount":
                                stringRetailPriceValue = csvReader.CurrentRecord[3];
                                stringDiscount = csvReader.CurrentRecord[6];
                                break;

                            case "WholePrice":
                                stringRetailPriceValue = csvReader.CurrentRecord[4];
                                break;

                            case "BaseWholePrice":
                                stringRetailPriceValue = csvReader.CurrentRecord[5];
                                break;

                            case "BaseWholePriceWithDiscount":
                                stringRetailPriceValue = csvReader.CurrentRecord[5];
                                stringDiscount = csvReader.CurrentRecord[6];
                                break;

                            default:
                                stringRetailPriceValue = csvReader.CurrentRecord[2];
                                break;
                        }

                        var stringWholePriceValue = string.Empty;
                        switch (
                            ModuleSettingsProvider.GetSettingValue<string>("WholePriceType",
                                SupplierOfHappiness.ModuleID))
                        {
                            case "WholePrice":
                                stringWholePriceValue = csvReader.CurrentRecord[4];
                                break;
                            case "BaseWholePrice":
                                stringWholePriceValue = csvReader.CurrentRecord[5];
                                break;
                            default:
                                stringWholePriceValue = csvReader.CurrentRecord[4];
                                break;
                        }

                        if (ModuleSettingsProvider.GetSettingValue<bool>("UpdateDiscount", SupplierOfHappiness.ModuleID))
                        {
                            var discount = 0f;
                            if (Single.TryParse(stringDiscount, NumberStyles.Float, CultureInfo.InvariantCulture, out discount) &&
                                offer.Product.Discount.Percent != discount)
                            {
                                offer.Product.Discount = new Discount(discount, 0);
                                ProductService.UpdateProduct(offer.Product, false);
                            }
                        }


                        var price = 0f;
                        if (Single.TryParse(stringRetailPriceValue, NumberStyles.Float, CultureInfo.InvariantCulture, out price))
                        {
                            offer.BasePrice = price + price * extraCharge / 100;
                            /*if (extraCharge >= 0)
                            {
                                offer.BasePrice = price + price * extraCharge / 100;
                            }
                            else if (extraCharge < 0)
                            {
                                offer.Product.Discount = extraCharge;
                                ProductService.UpdateProduct(offer.Product, false);
                            }*/
                        }



                        var supplyPrice = 0f;
                        if (Single.TryParse(stringWholePriceValue, NumberStyles.Float, CultureInfo.InvariantCulture,
                            out supplyPrice))
                        {
                            offer.SupplyPrice = supplyPrice;
                        }


                        //var price = 0f;
                        //if (Single.TryParse(csvReader.CurrentRecord[2], NumberStyles.Float, CultureInfo.InvariantCulture, out price))
                        //{
                        //    offer.Price = price;
                        //}


                        var amount = 0;
                        if (int.TryParse(csvReader.CurrentRecord[7], out amount))
                        {
                            offer.Amount = amount;
                        }

                        productIds.Add(offer.ProductId);

                        try
                        {
                            OfferService.UpdateOffer(offer);
                        }
                        catch (Exception ex)
                        {
                            log.Write(DateTime.Now.ToString("[dd.MM.yy HH:mm]") + " Не удалось обновить " + offer.ArtNo);
                            Debug.Log.Error(ex);
                        }

                        offerArtnos.Add(offer.ArtNo);

                        log.TotalUpdateRow++;
                        if (!inJob)
                        {
                            CommonStatistic.TotalUpdateRow++;
                            CommonStatistic.RowPosition++;
                        }

                    }
                    catch (Exception ex)
                    {
                        log.Write(DateTime.Now.ToString("[dd.MM.yy HH:mm]") + " Ошибка " + ex.Message + " " + ex.StackTrace);
                    }
                }

                new Task(() =>
                {
                    // don't disable in quick loading
                    if (ModuleSettingsProvider.GetSettingValue<bool>("DeactivateProducts", SupplierOfHappiness.ModuleID))
                    {
                        DisableAllProductsNotInList(productIds);
                    }

                    if (ModuleSettingsProvider.GetSettingValue<bool>("AmountNulling", SupplierOfHappiness.ModuleID))
                    {
                        ClearAmountAllProductsNotInList(offerArtnos);
                    }

                    CategoryService.SetCategoryHierarchicallyEnabled(0);
                    CategoryService.RecalculateProductsCountManual();
                    ProductService.PreCalcProductParamsMassInBackground();
                    SQLDataAccess.ExecuteNonQuery("[Settings].[sp_Reindex]", CommandType.StoredProcedure);
                }).Start();

            }

            log.WriteTotal();
            if (!inJob)
            {
                CommonStatistic.IsRun = false;
            }
        }

        private static void ProcessProperties(Product product, IEnumerable<ProductProperty> productProperties)
        {
            foreach (var productProperty in productProperties)
            {
                //TODO need optimization 
                //PropertyService.AddProductProperty(product.ProductId, productProperty.Name, productProperty.Value, 0, 0, true, true);

                if (string.IsNullOrEmpty(productProperty.Value.Trim()))
                {
                    continue;
                }

                Property property = PropertyService.GetPropertyByName(productProperty.Name);

                if (property == null)
                {
                    property = new Property()
                    {
                        Name = productProperty.Name,
                        UseInDetails = productProperty.DisplayInDetails,
                        UseInFilter = productProperty.DisplayInFilter,
                        UseInBrief = productProperty.DisplayInBrief,
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

                var propertyVales = PropertyService.GetPropertyValuesByProductId(product.ProductId);

                var oldPropertyValues = propertyVales.Where(x => x.PropertyId == property.PropertyId && x.PropertyValueId != propertyValue.PropertyValueId).ToList();
                foreach (var oldPropertyValue in oldPropertyValues)
                {
                    PropertyService.DeleteProductPropertyValue(product.ProductId, oldPropertyValue.PropertyValueId);
                }

                if (propertyVales.All(p => p.PropertyValueId != propertyValue.PropertyValueId))
                {
                    PropertyService.AddProductProperyValue(propertyValue.PropertyValueId, product.ProductId);
                }
            }
        }

        private static void ProcessPhotos(Product product, IEnumerable<string> pictureUrls)
        {
            foreach (var pictureUrl in pictureUrls)
            {
                if (string.IsNullOrEmpty(pictureUrl))
                {
                    continue;
                }

                var photoname = pictureUrl.Md5() + "." + (pictureUrl.Split('.').LastOrDefault() ?? "jpg");

                if (!PhotoService.IsProductHaveThisPhotoByName(product.ProductId, photoname))
                {
                    if (!FileHelpers.DownloadRemoteImageFile(pictureUrl,
                            FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoname)))
                    {
                        continue;
                    }

                    var fullfilename = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoname);
                    if (File.Exists(fullfilename))
                    {
                        ProductService.AddProductPhotoByProductId(product.ProductId, fullfilename, string.Empty, true, null, true);
                        File.Delete(fullfilename);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        private static int GetRowsCount(string fullPath)
        {
            //вычитаем заголовок колонок
            int count = 0;
            using (var reader = new StreamReader(fullPath, Encoding.UTF8, true))
            {
                while (reader.ReadLine() != null)
                    count++;
            }
            return count;
        }

        private static Product GetProduct(string artNo)
        {
            var productId = ProductService.GetProductId(artNo);

            return productId > 0 ? ProductService.GetProduct(productId) : new Product();
        }

        public static List<int> GetProductsIDs(bool enabledOnly = false)
        {
            return SQLDataAccess.ExecuteReadColumnIEnumerable<int>("SELECT [ProductID] FROM [Catalog].[Product]" + (enabledOnly ? " where enabled=1" : ""), CommandType.Text, "ProductID").ToList();
        }

        public static List<string> GetOffersArtno()
        {
            return SQLDataAccess.ExecuteReadColumnIEnumerable<string>("SELECT [ArtNo] FROM [Catalog].[Offer]", CommandType.Text, "ArtNo").ToList();
        }

        public static void DisableAllProductsNotInList(List<int> ids)
        {
            var oldproducts = GetProductsIDs(true);
            foreach (var id in oldproducts.Where(i => !ids.Contains(i)))
            {
                SQLDataAccess.ExecuteNonQuery(
                    "Update [Catalog].[Product] Set Enabled = 0 Where ProductId=@id ",
                    CommandType.Text, new SqlParameter("@id", id));
            }
            CategoryService.ClearCategoryCache();
        }

        public static void DisableAllProductsNotInStock()
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update [Catalog].[Product] Set Enabled = 0 Where (Select Max(Amount) From Catalog.Offer Where Offer.ProductId = Product.ProductId) <= 0",
                CommandType.Text);
        }

        public static void ClearAmountAllProductsNotInList(List<string> artnos)
        {
            var oldoffers = GetOffersArtno();
            foreach (var artNo in oldoffers.Where(i => !artnos.Contains(i)))
            {
                SQLDataAccess.ExecuteNonQuery(
                   "Update [Catalog].[Offer] Set [Amount] = 0 Where ArtNo = @ArtNo ",
                    CommandType.Text, new SqlParameter("@ArtNo", artNo));
            }
        }

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

        public static void ProcessUpdateImg(string fullPath)
        {
            try
            {
                var log = new SupplierOfHappinessLog();

                log.Write(DateTime.Now.ToString("[dd.MM.yy HH:mm]") + " Обновление каталога");

                log.TotalRow = GetRowsCount(fullPath);

                var listImg =
                    LoadImgs(fullPath, log);

                ProcessImgs(listImg, log);
                
                log.WriteTotal();
            }
            catch (Exception ex)
            {
                CommonStatistic.WriteLog(DateTime.Now.ToString("[dd.MM.yy HH:mm]") + " Ошибка " + ex.Message + " " +
                                         ex.StackTrace);
                CommonStatistic.TotalErrorRow++;
            }
            ProductService.PreCalcProductParamsMassInBackground();

            CommonStatistic.IsRun = false;
        }

        private static List<ProductImg> LoadImgs(string fullPath, SupplierOfHappinessLog log)
        {
            var listImg = new List<ProductImg>();

            if (!File.Exists(fullPath) || !CommonStatistic.IsRun)
                return listImg;

            CommonStatistic.RowPosition = 1;
            CommonStatistic.TotalRow = log.TotalRow;
            CommonStatistic.CurrentProcessName = "Поиск фотографий продукта";

            using (var csvReader = new CsvReader(new StreamReader(fullPath, Encoding.UTF8, true)))
            {
                csvReader.Configuration.Delimiter = ";";
                csvReader.Configuration.HasHeaderRecord = true;
                while (csvReader.Read() && (CommonStatistic.IsRun))
                {
                    try
                    {
                        if (csvReader.CurrentRecord.Length != 33)
                        {
                            log.TotalErrorRow++;
                            continue;
                        }

                        var product = ProductService.GetProduct(csvReader.CurrentRecord[0]);

                        if (product == null)
                        {
                            CommonStatistic.RowPosition++;
                            continue;
                        }

                        if (listImg.Any(x => x.ProductId == product.ProductId))
                        {
                            var index = listImg.FindIndex(x => x.ProductId == product.ProductId);
                            foreach (var photo in csvReader.CurrentRecord[21].Split(new[] { ' ' }))
                            {
                                if (!listImg[index].ImgUrl.Contains(photo))
                                {
                                    listImg[index].ImgUrl.Add(photo);
                                }
                            }
                        }
                        else
                        {
                            listImg.Add(new ProductImg { ProductId = product.ProductId, ImgUrl = new List<string>() });
                            var index = listImg.FindIndex(x => x.ProductId == product.ProductId);
                            foreach (var photo in csvReader.CurrentRecord[21].Split(new[] { ' ' }))
                            {

                                if (!listImg[index].ImgUrl.Contains(photo))
                                {
                                    listImg[index].ImgUrl.Add(photo);
                                }
                            }
                        }
                        CommonStatistic.RowPosition++;
                    }
                    catch (Exception ex)
                    {
                        log.Write(DateTime.Now.ToString("[dd.MM.yy HH:mm]") + " Ошибка " + ex.Message + " " +
                                  ex.StackTrace);
                        CommonStatistic.WriteLog(DateTime.Now.ToString("[dd.MM.yy HH:mm]") + " Ошибка " + ex.Message + " " + ex.StackTrace);
                        CommonStatistic.RowPosition++;
                        CommonStatistic.TotalErrorRow++;
                    }
                }
            }

            return listImg;
        }

        private static void ProcessImgs(List<ProductImg> listImg, SupplierOfHappinessLog log)
        {
            if (!CommonStatistic.IsRun)
                return;

            CommonStatistic.RowPosition = 0;
            CommonStatistic.TotalRow = listImg.Count;
            CommonStatistic.CurrentProcessName = "Обновление фотографий продукта";

            foreach (var product in listImg)
            {
                if (!CommonStatistic.IsRun)
                    return;

                try
                {
                    PhotoService.DeletePhotos(product.ProductId, PhotoType.Product, false);
                    foreach (var pictureUrl in product.ImgUrl)
                    {
                        if (string.IsNullOrEmpty(pictureUrl))
                        {
                            continue;
                        }

                        var photoname = pictureUrl.Md5() + "." + (pictureUrl.Split('.').LastOrDefault() ?? "jpg");

                        if (!FileHelpers.DownloadRemoteImageFile(pictureUrl,
                                FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoname)))
                        {
                            continue;
                        }

                        var fullfilename = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoname);
                        if (File.Exists(fullfilename))
                        {
                            ProductService.AddProductPhotoByProductId(product.ProductId, fullfilename, string.Empty, true, null, true);
                            File.Delete(fullfilename);
                        }
                    }
                    CommonStatistic.TotalUpdateRow++;
                    CommonStatistic.RowPosition++;
                }
                catch (Exception ex)
                {
                    log.Write(DateTime.Now.ToString("[dd.MM.yy HH:mm]") + " Ошибка " + ex.Message + " " +
                                ex.StackTrace);
                    CommonStatistic.WriteLog(DateTime.Now.ToString("[dd.MM.yy HH:mm]") + " Ошибка: " + ex.Message + " Изображение: " + product.ImgUrl + " Стек: " + ex.StackTrace);
                    CommonStatistic.RowPosition++;
                    CommonStatistic.TotalErrorRow++;
                }
            }
        }
    }
}