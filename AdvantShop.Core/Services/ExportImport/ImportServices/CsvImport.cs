using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Repository.Currencies;
using AdvantShop.Saas;
using AdvantShop.Statistic;
using AdvantShop.FullSearch;
using AdvantShop.Taxes;
using CsvHelper;

namespace AdvantShop.ExportImport
{
    public class CsvImport
    {
        private readonly string _fullPath;
        private readonly bool _disablebProduct;
        private readonly bool _hasHeadrs;
        private static bool _skipOriginalPhoto;
        private Dictionary<string, int> _fieldMapping;
        private readonly string _separators;
        private readonly string _encodings;
        private readonly string _columnSeparator;
        private readonly string _propertySeparator;

        private readonly Dictionary<ICSVExportImport, List<CSVField>> _modulesAndFields;

        private CsvImport(string filePath, bool hasHeadrs, bool disablebProduct, string separators, string encodings, Dictionary<string, int> fieldMapping, string columSeparator, string propertySeparator, bool skipOriginalPhoto)
        {
            _fullPath = filePath;
            _hasHeadrs = hasHeadrs;
            _disablebProduct = disablebProduct;
            _fieldMapping = fieldMapping;
            _encodings = encodings;
            _separators = separators;
            _columnSeparator = columSeparator;
            _propertySeparator = propertySeparator;
            _skipOriginalPhoto = skipOriginalPhoto;

            _modulesAndFields = new Dictionary<ICSVExportImport, List<CSVField>>();
            foreach (var csvExportImportModule in AttachedModules.GetModules<ICSVExportImport>())
            {
                var classInstance = (ICSVExportImport)Activator.CreateInstance(csvExportImportModule, null);
                if (ModulesRepository.IsActiveModule(classInstance.ModuleStringId) && classInstance.CheckAlive())
                {
                    var moduleFields = classInstance.GetCSVFields().ToList();
                    if (moduleFields.Any() && !_modulesAndFields.ContainsKey(classInstance))
                        _modulesAndFields.Add(classInstance, moduleFields);
                }
            }
        }

        public static CsvImport Factory(string filePath, bool hasHeadrs, bool disablebProduct, string separators, string encodings, Dictionary<string, int> fieldMapping, string columSeparator, string propertySeparator, bool skipOriginalPhoto = false, bool remains = false)
        {
            return new CsvImport(filePath, hasHeadrs, disablebProduct, separators, encodings, fieldMapping, columSeparator, propertySeparator, skipOriginalPhoto);
        }

        public static CsvImport Factory(string filePath, bool hasHeadrs, bool skipOriginalPhoto = false)
        {
            return new CsvImport(filePath, hasHeadrs, false, null, null, null, null, null, skipOriginalPhoto);
        }

        private CsvReader InitReader(bool? hasHeaderRecord = null)
        {
            var reader = new CsvReader(new StreamReader(_fullPath, Encoding.GetEncoding(_encodings ?? EncodingsEnum.Utf8.StrName())));
            //var reader = new CsvReader(new StreamReader(_fullPath));
            reader.Configuration.Delimiter = _separators ?? SeparatorsEnum.SemicolonSeparated.StrName();
            if (hasHeaderRecord.HasValue)
                reader.Configuration.HasHeaderRecord = (bool)hasHeaderRecord;
            else
                reader.Configuration.HasHeaderRecord = _hasHeadrs;
            return reader;
        }

        public List<string[]> ReadFirst2()
        {
            var list = new List<string[]>();
            using (var csv = InitReader())
            {
                int count = 0;
                while (csv.Read())
                {
                    if (count == 2)
                        break;

                    if (csv.CurrentRecord != null)
                        list.Add(csv.CurrentRecord);
                    count++;
                }
            }
            return list;
        }

        public Task Process(Func<Product, Product> func = null)
        {
            return CommonStatistic.StartNew(() =>
           {
               CommonStatistic.IsRun = true;
               try
               {
                   _process(func);
               }
               catch (Exception ex)
               {
                   Debug.Log.Error(ex);
                   CommonStatistic.WriteLog(ex.Message);
               }
               CommonStatistic.IsRun = false;
           });
        }

        private void _process(Func<Product, Product> func = null)
        {
            if (_fieldMapping == null)
                MapFileds();

            if (_fieldMapping == null)
            {
                throw new Exception("can mapping colums");
            }

            var startAt = DateTime.Now;

            CommonStatistic.TotalRow = GetRowCount();

            var somePostProcessing = _fieldMapping.ContainsKey(ProductFields.Related.StrName()) ||
                                     _fieldMapping.ContainsKey(ProductFields.Alternative.StrName()) ||
                                     _fieldMapping.ContainsKey(ProductFields.Gifts.StrName());

            foreach (var moduleFields in _modulesAndFields.Values)
            {
                somePostProcessing |= moduleFields.Any(moduleField => _fieldMapping.ContainsKey(moduleField.StrName));
            }

            if (somePostProcessing)
            {
                CommonStatistic.TotalRow *= 2;
            }

            ProcessRow(false, _columnSeparator, _propertySeparator, func);
            if (somePostProcessing && CommonStatistic.IsRun)
                ProcessRow(true, _columnSeparator, _propertySeparator);

            CommonStatistic.IsRun = false;

            if (_disablebProduct)
            {
                ProductService.DisableAllProducts(startAt);
            }

            CategoryService.RecalculateProductsCountManual();
            CategoryService.SetCategoryHierarchicallyEnabled(0);
            LuceneSearch.CreateAllIndexInBackground();
            ProductService.PreCalcProductParamsMassInBackground();

            CacheManager.Clean();
            FileHelpers.DeleteFilesFromImageTempInBackground();
            FileHelpers.DeleteFile(_fullPath);
        }

        private void MapFileds()
        {
            _fieldMapping = new Dictionary<string, int>();
            using (var csv = InitReader(false))
            {
                csv.Read();
                for (var i = 0; i < csv.CurrentRecord.Length; i++)
                {
                    if (csv.CurrentRecord[i] == ProductFields.None.StrName()) continue;
                    if (!_fieldMapping.ContainsKey(csv.CurrentRecord[i]))
                        _fieldMapping.Add(csv.CurrentRecord[i], i);
                }
            }
        }

        private long GetRowCount()
        {
            long count = 0;
            using (var csv = InitReader())
            {
                while (csv.Read())
                    count++;
            }
            return count;
        }

        private void ProcessRow(bool onlyPostProcess, string columSeparator, string propertySeparator, Func<Product, Product> func = null)
        {
            if (!File.Exists(_fullPath)) return;
            using (var csv = InitReader())
            {
                while (csv.Read())
                {
                    if (!CommonStatistic.IsRun)
                    {
                        csv.Dispose();
                        FileHelpers.DeleteFile(_fullPath);
                        return;
                    }
                    try
                    {
                        var productInStrings = PrepareRow(csv);
                        if (productInStrings == null) continue;

                        if (!onlyPostProcess)
                            UpdateInsertProduct(productInStrings, columSeparator, propertySeparator, func);
                        else
                            PostProcess(productInStrings, PrepareModuleRow(csv), _modulesAndFields, _columnSeparator, _propertySeparator);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log.Error(ex);
                    }
                }
            }
        }

        private Dictionary<ProductFields, object> PrepareRow(ICsvReader csv)
        {
            // Step by rows
            var productInStrings = new Dictionary<ProductFields, object>();

            foreach (ProductFields productField in Enum.GetValues(typeof(ProductFields)))
            {
                switch (productField.Status())
                {
                    case CsvFieldStatus.String:
                        GetString(productField, csv, productInStrings);
                        break;
                    case CsvFieldStatus.StringRequired:
                        GetStringRequired(productField, csv, productInStrings);
                        break;
                    case CsvFieldStatus.NotEmptyString:
                        GetStringNotNull(productField, csv, productInStrings);
                        break;
                    case CsvFieldStatus.Float:
                        if (!GetDecimal(productField, csv, productInStrings))
                            return null;
                        break;
                    case CsvFieldStatus.NullableFloat:
                        if (!GetNullableDecimal(productField, csv, productInStrings))
                            return null;
                        break;
                    case CsvFieldStatus.Int:
                        if (!GetInt(productField, csv, productInStrings))
                            return null;
                        break;
                }
            }
            return productInStrings;
        }

        private Dictionary<CSVField, object> PrepareModuleRow(ICsvReader csv)
        {
            var productInStrings = new Dictionary<CSVField, object>();
            foreach (var moduleFields in _modulesAndFields.Values)
            {
                foreach (var moduleField in moduleFields)
                {
                    var nameField = moduleField.StrName;
                    if (_fieldMapping.ContainsKey(nameField))
                        productInStrings.Add(moduleField, TrimAnyWay(csv[_fieldMapping[nameField]]));
                }
            }
            return productInStrings;
        }

        private bool GetString(ProductFields rEnum, ICsvReaderRow csv, IDictionary<ProductFields, object> productInStrings)
        {
            var nameField = rEnum.StrName();
            if (_fieldMapping.ContainsKey(nameField))
                productInStrings.Add(rEnum, TrimAnyWay(csv[_fieldMapping[nameField]]));
            return true;
        }

        private bool GetStringNotNull(ProductFields rEnum, ICsvReaderRow csv, IDictionary<ProductFields, object> productInStrings)
        {
            var nameField = rEnum.StrName();
            if (!_fieldMapping.ContainsKey(nameField)) return true;
            var tempValue = TrimAnyWay(csv[_fieldMapping[nameField]]);
            if (!string.IsNullOrEmpty(tempValue))
                productInStrings.Add(rEnum, tempValue);
            return true;
        }

        private bool GetStringRequired(ProductFields rEnum, ICsvReaderRow csv, IDictionary<ProductFields, object> productInStrings)
        {
            var nameField = rEnum.StrName();
            if (!_fieldMapping.ContainsKey(nameField)) return true;
            var tempValue = TrimAnyWay(csv[_fieldMapping[nameField]]);
            if (!string.IsNullOrEmpty(tempValue))
                productInStrings.Add(rEnum, tempValue);
            else
            {
                LogInvalidData(string.Format(LocalizationService.GetResource("Core.ExportImport.ImportCsv.CanNotEmpty"), ProductFields.Name.Localize(), CommonStatistic.RowPosition + 2));
                return false;
            }
            return true;
        }

        private bool GetDecimal(ProductFields rEnum, ICsvReaderRow csv, IDictionary<ProductFields, object> productInStrings)
        {
            var nameField = rEnum.StrName();
            if (!_fieldMapping.ContainsKey(nameField)) return true;
            var value = TrimAnyWay(csv[_fieldMapping[nameField]]);
            if (string.IsNullOrEmpty(value))
                value = "0";
            float decValue;
            if (float.TryParse(value, out decValue))
            {
                productInStrings.Add(rEnum, decValue);
            }
            else if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decValue))
            {
                productInStrings.Add(rEnum, decValue);
            }
            else
            {
                LogInvalidData(string.Format(LocalizationService.GetResource("Core.ExportImport.ImportCsv.MustBeNumber"), rEnum.Localize(), CommonStatistic.RowPosition + 2));
                return false;
            }
            return true;
        }

        private bool GetNullableDecimal(ProductFields rEnum, ICsvReaderRow csv, IDictionary<ProductFields, object> productInStrings)
        {
            var nameField = rEnum.StrName();
            if (!_fieldMapping.ContainsKey(nameField)) return true;
            var value = TrimAnyWay(csv[_fieldMapping[nameField]]);

            if (string.IsNullOrEmpty(value))
            {
                productInStrings.Add(rEnum, default(float?));
                return true;
            }

            float decValue;
            if (float.TryParse(value, out decValue))
            {
                productInStrings.Add(rEnum, decValue);
            }
            else if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decValue))
            {
                productInStrings.Add(rEnum, decValue);
            }
            else
            {
                LogInvalidData(string.Format(LocalizationService.GetResource("Core.ExportImport.ImportCsv.MustBeNumber"), rEnum.Localize(), CommonStatistic.RowPosition + 2));
                return false;
            }
            return true;
        }

        private bool GetInt(ProductFields rEnum, ICsvReaderRow csv, IDictionary<ProductFields, object> productInStrings)
        {
            var nameField = rEnum.StrName();
            if (!_fieldMapping.ContainsKey(nameField)) return true;
            var value = TrimAnyWay(csv[_fieldMapping[nameField]]);
            if (string.IsNullOrEmpty(value))
                value = "0";
            int intValue;
            if (int.TryParse(value, out intValue))
            {
                productInStrings.Add(rEnum, intValue);
            }
            else
            {
                LogInvalidData(string.Format(LocalizationService.GetResource("Core.ExportImport.ImportCsv.MustBeNumber"), rEnum.Localize(), CommonStatistic.RowPosition + 2));
                return false;
            }
            return true;
        }

        private static string TrimAnyWay(string str)
        {
            return string.IsNullOrEmpty(str) ? str : str.Trim();
        }

        private static void LogInvalidData(string message)
        {
            CommonStatistic.WriteLog(message);
            CommonStatistic.TotalErrorRow++;
            CommonStatistic.RowPosition++;
        }

        private static bool useMultiThreadImport = false;

        public static void UpdateInsertProduct(Dictionary<ProductFields, object> productInStrings, string columSeparator, string propertySeparator, Func<Product, Product> func = null)
        {
            if (useMultiThreadImport)
            {
                var added = false;
                while (!added)
                {
                    int workerThreads;
                    int asyncIoThreads;
                    ThreadPool.GetAvailableThreads(out workerThreads, out asyncIoThreads);
                    if (workerThreads != 0)
                    {
                        //ThreadPool.QueueUserWorkItem(UpdateInsertProductWorker, productInStrings);
                        Task.Factory.StartNew(() => UpdateInsertProductWorker(columSeparator, propertySeparator, productInStrings, func), TaskCreationOptions.LongRunning);
                        added = true;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            else
            {
                UpdateInsertProductWorker(columSeparator, propertySeparator, productInStrings, func);
            }
        }

        private static void UpdateInsertProductWorker(string columSeparator, string propertySeparator, Dictionary<ProductFields, object> productInStrings, Func<Product, Product> func = null)
        {
            //return;

            try
            {
                bool addingNew;
                Product product = null;
                if (productInStrings.ContainsKey(ProductFields.Sku) && productInStrings[ProductFields.Sku].AsString().IsNullOrEmpty())
                    throw new Exception("SKU can not be empty");

                var artNo = productInStrings.ContainsKey(ProductFields.Sku) ? productInStrings[ProductFields.Sku].AsString().Trim() : string.Empty;
                if (string.IsNullOrEmpty(artNo))
                {
                    addingNew = true;
                }
                else
                {
                    product = ProductService.GetProduct(artNo);
                    addingNew = product == null;
                }

                if (addingNew)
                {
                    product = new Product { ArtNo = string.IsNullOrEmpty(artNo) ? null : artNo, Multiplicity = 1, CurrencyID = SettingsCatalog.DefaultCurrency.CurrencyId, Enabled = true };
                }

                if (productInStrings.ContainsKey(ProductFields.Name))
                    product.Name = productInStrings[ProductFields.Name].AsString();
                else
                    product.Name = product.Name ?? string.Empty;

                if (productInStrings.ContainsKey(ProductFields.Enabled))
                {
                    product.Enabled = productInStrings[ProductFields.Enabled].AsString().Trim().Equals("+");
                }

                if (productInStrings.ContainsKey(ProductFields.Currency))
                {
                    var currency = CurrencyService.GetCurrencyByIso3(productInStrings[ProductFields.Currency].AsString().Trim());
                    if (currency != null)
                        product.CurrencyID = currency.CurrencyId;
                    else
                        throw new Exception("Currency not found");
                }


                if (productInStrings.ContainsKey(ProductFields.OrderByRequest))
                    product.AllowPreOrder = productInStrings[ProductFields.OrderByRequest].AsString().Trim().Equals("+");

                if (productInStrings.ContainsKey(ProductFields.Discount) ||
                    productInStrings.ContainsKey(ProductFields.DiscountAmount))
                {
                    var percent = !productInStrings.ContainsKey(ProductFields.Discount) || productInStrings[ProductFields.Discount] == null
                        ? product.Discount.Percent
                        : productInStrings[ProductFields.Discount].AsFloat();

                    var amount = !productInStrings.ContainsKey(ProductFields.DiscountAmount) || productInStrings[ProductFields.DiscountAmount] == null
                        ? product.Discount.Amount
                        : productInStrings[ProductFields.DiscountAmount].AsFloat();

                    product.Discount = new Discount(percent, amount);
                }

                if (productInStrings.ContainsKey(ProductFields.Weight))
                    product.Weight = productInStrings[ProductFields.Weight].AsFloat();

                if (productInStrings.ContainsKey(ProductFields.Size))
                {
                    var dimensions = productInStrings[ProductFields.Size].AsString().Split(new[] { '|', 'x' }).Select(x => x.TryParseFloat()).ToList();

                    product.Length = dimensions.Count > 0 ? dimensions[0] : 0;
                    product.Width = dimensions.Count > 1 ? dimensions[1] : 0;
                    product.Height = dimensions.Count > 2 ? dimensions[2] : 0;
                }

                if (productInStrings.ContainsKey(ProductFields.BriefDescription))
                    product.BriefDescription = productInStrings[ProductFields.BriefDescription].AsString();

                if (productInStrings.ContainsKey(ProductFields.Description))
                    product.Description = productInStrings[ProductFields.Description].AsString();

                if (productInStrings.ContainsKey(ProductFields.SalesNotes))
                    product.SalesNote = productInStrings[ProductFields.SalesNotes].AsString();

                if (productInStrings.ContainsKey(ProductFields.Gtin))
                    product.Gtin = productInStrings[ProductFields.Gtin].AsString();

                if (productInStrings.ContainsKey(ProductFields.GoogleProductCategory))
                    product.GoogleProductCategory = productInStrings[ProductFields.GoogleProductCategory].AsString();

                if (productInStrings.ContainsKey(ProductFields.YandexProductCategory))
                    product.YandexMarketCategory = productInStrings[ProductFields.YandexProductCategory].AsString();

                if (productInStrings.ContainsKey(ProductFields.YandexTypePrefix))
                    product.YandexTypePrefix = productInStrings[ProductFields.YandexTypePrefix].AsString();

                if (productInStrings.ContainsKey(ProductFields.YandexModel))
                    product.YandexModel = productInStrings[ProductFields.YandexModel].AsString();

                if(productInStrings.ContainsKey(ProductFields.YandexName))
                    product.YandexName = productInStrings[ProductFields.YandexName].AsString();

                if (productInStrings.ContainsKey(ProductFields.Adult))
                    product.Adult = productInStrings[ProductFields.Adult].AsString().Trim().Equals("+");

                if (productInStrings.ContainsKey(ProductFields.ManufacturerWarranty))
                    product.ManufacturerWarranty = productInStrings[ProductFields.ManufacturerWarranty].AsString().Trim().Equals("+");

                if (productInStrings.ContainsKey(ProductFields.ShippingPrice))
                    product.ShippingPrice = productInStrings[ProductFields.ShippingPrice].AsNullableFloat();

                if (productInStrings.ContainsKey(ProductFields.Unit))
                    product.Unit = productInStrings[ProductFields.Unit].AsString();


                if (productInStrings.ContainsKey(ProductFields.MultiOffer))
                {
                    OfferService.OffersFromString(product, productInStrings[ProductFields.MultiOffer].AsString(), columSeparator, propertySeparator);
                }
                else
                {
                    OfferService.OfferFromFields(product, productInStrings.ContainsKey(ProductFields.Price) ? productInStrings[ProductFields.Price].AsFloat() : (float?)null,
                                                        productInStrings.ContainsKey(ProductFields.PurchasePrice) ? productInStrings[ProductFields.PurchasePrice].AsFloat() : (float?)null,
                                                  productInStrings.ContainsKey(ProductFields.Amount) ? productInStrings[ProductFields.Amount].AsFloat() : (float?)null);
                }

                if (productInStrings.ContainsKey(ProductFields.ParamSynonym))
                {
                    var prodUrl = productInStrings[ProductFields.ParamSynonym].AsString().IsNotEmpty()
                                      ? productInStrings[ProductFields.ParamSynonym].AsString()
                                      : product.ArtNo;
                    product.UrlPath = UrlService.GetAvailableValidUrl(product.ProductId, ParamType.Product, prodUrl);
                }
                else
                {
                    product.UrlPath = product.UrlPath ??
                                      UrlService.GetAvailableValidUrl(product.ProductId, ParamType.Product,
                                      product.ArtNo ?? product.Name.Substring(0, product.Name.Length - 1 < 50 ? product.Name.Length - 1 : 50));

                }

                product.Meta.ObjId = product.ProductId;

                if (productInStrings.ContainsKey(ProductFields.Title))
                    product.Meta.Title = productInStrings[ProductFields.Title].AsString();
                else
                    product.Meta.Title = product.Meta.Title ?? SettingsSEO.ProductMetaTitle;

                if (productInStrings.ContainsKey(ProductFields.H1))
                    product.Meta.H1 = productInStrings[ProductFields.H1].AsString();
                else
                    product.Meta.H1 = product.Meta.H1 ?? SettingsSEO.ProductMetaH1;

                if (productInStrings.ContainsKey(ProductFields.MetaKeywords))
                    product.Meta.MetaKeywords = productInStrings[ProductFields.MetaKeywords].AsString();
                else
                    product.Meta.MetaKeywords = product.Meta.MetaKeywords ?? SettingsSEO.ProductMetaKeywords;

                if (productInStrings.ContainsKey(ProductFields.MetaDescription))
                    product.Meta.MetaDescription = productInStrings[ProductFields.MetaDescription].AsString();
                else
                    product.Meta.MetaDescription = product.Meta.MetaDescription ?? SettingsSEO.ProductMetaDescription;

                if (productInStrings.ContainsKey(ProductFields.Markers))
                    ProductService.MarkersFromString(product, productInStrings[ProductFields.Markers].AsString(), columSeparator);

                if (productInStrings.ContainsKey(ProductFields.Producer))
                    product.BrandId = BrandService.BrandFromString(productInStrings[ProductFields.Producer].AsString());

                if (productInStrings.ContainsKey(ProductFields.MinAmount))
                    product.MinAmount = productInStrings[ProductFields.MinAmount].AsFloat() != 0 ? productInStrings[ProductFields.MinAmount].AsFloat() : (float?)null;

                if (productInStrings.ContainsKey(ProductFields.MaxAmount))
                    product.MaxAmount = productInStrings[ProductFields.MaxAmount].AsFloat() != 0 ? productInStrings[ProductFields.MaxAmount].AsFloat() : (float?)null;

                if (productInStrings.ContainsKey(ProductFields.Multiplicity))
                    product.Multiplicity = productInStrings[ProductFields.Multiplicity].AsFloat() != 0 ? productInStrings[ProductFields.Multiplicity].AsFloat() : 1;

                if (productInStrings.ContainsKey(ProductFields.Fee))
                    product.Fee = productInStrings[ProductFields.Fee].AsFloat() != 0 ? productInStrings[ProductFields.Fee].AsFloat() : 0;

                if (productInStrings.ContainsKey(ProductFields.Cbid))
                    product.Cbid = productInStrings[ProductFields.Cbid].AsFloat() != 0 ? productInStrings[ProductFields.Cbid].AsFloat() : 0;

                if (productInStrings.ContainsKey(ProductFields.BarCode))
                {
                    product.BarCode = productInStrings[ProductFields.BarCode].AsString();
                }

                if (productInStrings.ContainsKey(ProductFields.Tax))
                {
                    var taxName = productInStrings[ProductFields.Tax].AsString();
                    if (!string.IsNullOrEmpty(taxName))
                    {
                        var tax = TaxService.GetTaxes().FirstOrDefault(x => x.Name.ToLower() == taxName.ToLower());
                        if (tax != null)
                            product.TaxId = tax.TaxId;
                        else
                        {
                            CommonStatistic.WriteLog(string.Format("Налог '{0}' не найден в {1} строчке", taxName, CommonStatistic.RowPosition + 2));
                            CommonStatistic.TotalErrorRow++;
                        }
                    }
                    else
                    {
                        product.TaxId = null;
                    }
                }

                product.ModifiedBy = "csv";


                if (func != null)
                    product = func(product);

                if (!addingNew)
                {
                    ProductService.UpdateProduct(product, false);
                    CommonStatistic.TotalUpdateRow++;
                }
                else
                {
                    if (!(SaasDataService.IsSaasEnabled && ProductService.GetProductsCount() >= SaasDataService.CurrentSaasData.ProductsCount))
                    {
                        ProductService.AddProduct(product, false);
                        CommonStatistic.TotalAddRow++;
                    }
                }

                if (product.ProductId > 0)
                    OtherFields(productInStrings, product.ProductId, columSeparator, propertySeparator);
            }
            catch (Exception e)
            {
                CommonStatistic.TotalErrorRow++;
                Log(CommonStatistic.RowPosition + ": " + e.Message);
            }

            productInStrings.Clear();
            CommonStatistic.RowPosition++;
        }


        private static void OtherFields(IDictionary<ProductFields, object> fields, int productId, string columSeparator, string propertySeparator)
        {
            //Category
            if (fields.ContainsKey(ProductFields.Category))
            {
                string sorting = string.Empty;
                if (fields.ContainsKey(ProductFields.Sorting))
                {
                    sorting = fields[ProductFields.Sorting].AsString();
                }
                var parentCategory = fields[ProductFields.Category].AsString();
                CategoryService.SubParseAndCreateCategory(parentCategory, productId, columSeparator, sorting);
            }

            //photo
            if (fields.ContainsKey(ProductFields.Photos))
            {
                string photos = fields[ProductFields.Photos].AsString();
                if (!string.IsNullOrEmpty(photos))
                    PhotoService.PhotoFromString(productId, photos, columSeparator, propertySeparator, _skipOriginalPhoto);
            }

            //video
            if (fields.ContainsKey(ProductFields.Videos))
            {
                string videos = fields[ProductFields.Videos].AsString();
                ProductVideoService.VideoFromString(productId, videos, columSeparator);
            }

            //Properties
            if (fields.ContainsKey(ProductFields.Properties))
            {
                string properties = fields[ProductFields.Properties].AsString();
                PropertyService.PropertiesFromString(productId, properties, columSeparator, propertySeparator);
            }

            if (fields.ContainsKey(ProductFields.CustomOption))
            {
                string customOption = fields[ProductFields.CustomOption].AsString();
                CustomOptionsService.CustomOptionsFromString(productId, customOption);
            }

            if (fields.ContainsKey(ProductFields.Tags) &&
                   (!SaasDataService.IsSaasEnabled || SaasDataService.CurrentSaasData.HaveTags))
            {
                TagService.DeleteMap(productId, ETagType.Product);

                var i = 0;

                foreach (var tagName in fields[ProductFields.Tags].AsString().Split(new[] { columSeparator }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var trimTagName = tagName.Trim();
                    var tag = TagService.Get(trimTagName);
                    if (tag == null)
                    {
                        var tagId = TagService.Add(new Tag
                        {
                            Name = trimTagName,
                            Enabled = true,
                            UrlPath = UrlService.GetAvailableValidUrl(0, ParamType.Tag, trimTagName)
                        });
                        TagService.AddMap(productId, tagId, ETagType.Product, i * 10);
                    }
                    else
                    {
                        TagService.AddMap(productId, tag.Id, ETagType.Product, i * 10);
                    }
                    i++;
                }
            }
        }

        private static void Log(string message)
        {
            CommonStatistic.WriteLog(message);
        }

        public static void PostProcess(Dictionary<ProductFields, object> productInStrings, Dictionary<CSVField, object> moduleFieldValues, Dictionary<ICSVExportImport, List<CSVField>> modulesAndFields, string columnSeparator, string propertySeparator)
        {
            if (productInStrings.ContainsKey(ProductFields.Sku))
            {
                var artNo = productInStrings[ProductFields.Sku].AsString();
                int productId = ProductService.GetProductId(artNo);

                //relations
                if (productInStrings.ContainsKey(ProductFields.Related))
                {
                    var linkproducts = productInStrings[ProductFields.Related].AsString();
                    ProductService.LinkedProductFromString(productId, linkproducts, RelatedType.Related, columnSeparator);
                }

                //relations
                if (productInStrings.ContainsKey(ProductFields.Alternative))
                {
                    var linkproducts = productInStrings[ProductFields.Alternative].AsString();
                    ProductService.LinkedProductFromString(productId, linkproducts, RelatedType.Alternative, columnSeparator);
                }

                //gifts
                if (productInStrings.ContainsKey(ProductFields.Gifts))
                {
                    var linkproducts = productInStrings[ProductFields.Gifts].AsString();
                    OfferService.ProductGiftsFromString(productId, linkproducts, columnSeparator);
                }

                // modules
                foreach (var moduleInstance in modulesAndFields.Keys)
                {
                    foreach (var moduleField in modulesAndFields[moduleInstance].Where(moduleFieldValues.ContainsKey))
                    {
                        moduleInstance.ProcessField(moduleField, productId, moduleFieldValues[moduleField].AsString(), columnSeparator, propertySeparator);
                    }
                }
            }
            CommonStatistic.RowPosition++;
        }
    }

    public static class CsvExt
    {
        public static string AsString(this object val)
        {
            var t = val as string;
            return t ?? "";
        }

        public static float AsFloat(this object val)
        {
            if (val is float)
                return (float)val;
            return 0F;
        }

        public static float? AsNullableFloat(this object val)
        {
            if (val == null)
                return null;

            if (val is float)
                return (float)val;

            return 0F;
        }
    }
}