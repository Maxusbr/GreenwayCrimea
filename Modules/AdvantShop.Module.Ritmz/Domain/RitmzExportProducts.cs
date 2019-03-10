//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.FilePath;

namespace AdvantShop.Modules
{
    public class RitmzExportProducts
    {
        public const string ExportFile = "exportRitmZProducts.xml";
        public const string ExporDir = "~/modules/ritmz/export";
        private const string _currency = "RUB";
        private static readonly string _salesNotes = string.Empty;
        private const string _moduleName = "Ritmz";

        public static void Export()
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(ExporDir)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(ExporDir));
            }
            using (var fs = new FileStream(HttpContext.Current.Server.MapPath(ExporDir + "/" + ExportFile), FileMode.Create, FileAccess.ReadWrite))
            {
                if (string.IsNullOrEmpty(ModuleSettingsProvider.GetSettingValue<string>("RitmzSiteUrl", _moduleName)) ||
                    string.IsNullOrEmpty(ModuleSettingsProvider.GetSettingValue<string>("RitmzSiteName", _moduleName)) ||
                    string.IsNullOrEmpty(ModuleSettingsProvider.GetSettingValue<string>("RitmzLogin", _moduleName)) ||
                    string.IsNullOrEmpty(ModuleSettingsProvider.GetSettingValue<string>("RitmzPassword", _moduleName)))
                {

                    using (var writer = new StreamWriter(fs))
                    {
                        writer.WriteLine("Ошибка. Проверте правильность настроек модуля Ritmz");
                    }
                    fs.Close();
                    return;
                }

                GetExportFeedString(fs);
                fs.Close();
            }
        }

        public static void WriteToResponce(HttpResponse httpResponse, string exportWay)
        {
            using (var fs = new FileStream(exportWay, FileMode.Open))
            using (var reader = new StreamReader(fs, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    httpResponse.Write(line);
                }
            }
        }

        public static long GetExportFeedString(Stream s)
        {
            long filesize;
            var settings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true };
            using (var writer = XmlWriter.Create(s, settings))
            {
                writer.WriteStartDocument();
                writer.WriteDocType("yml_catalog", null, "shops.dtd", null);
                writer.WriteStartElement("yml_catalog");
                writer.WriteAttributeString("date", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

                writer.WriteStartElement("name");
                writer.WriteString(ModuleSettingsProvider.GetSettingValue<string>("RitmzSiteName", _moduleName));
                writer.WriteEndElement();

                writer.WriteStartElement("company");
                writer.WriteString(ModuleSettingsProvider.GetSettingValue<string>("RitmzSiteName", _moduleName));
                writer.WriteEndElement();

                writer.WriteStartElement("url");
                writer.WriteString(ModuleSettingsProvider.GetSettingValue<string>("RitmzSiteUrl", _moduleName));
                writer.WriteEndElement();

                writer.WriteStartElement("currencies");
                DataTable currenciesDataTable = GetCurrencies();
                foreach (DataRow curRow in currenciesDataTable.Rows)
                {
                    ProcessCurrencyRow(curRow, writer);
                }
                writer.WriteEndElement();

                writer.WriteStartElement("categories");
                DataTable categoriesDataTable = GetCategories();
                foreach (DataRow catRow in categoriesDataTable.Rows)
                {
                    ProcessCategoryRow(catRow, writer);
                }
                writer.WriteEndElement();

                writer.WriteStartElement("offers");
                foreach (var row in GetProduts())
                {
                    //ProcessProductRow(offerRow, writer);
                    if (string.IsNullOrWhiteSpace(row.BrandName)) ProcessSimpleModel(row, writer);
                    else ProcessVendorModel(row, writer);
                }

                writer.WriteEndElement();

                //writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();

                writer.Flush();
                filesize = s.Length;
                writer.Close();
            }
            return filesize;
        }

        private static void ProcessCurrencyRow(DataRow row, XmlWriter writer)
        {
            writer.WriteStartElement("currency");
            writer.WriteAttributeString("id", ModulesRepository.ConvertTo<string>(row["CurrencyIso3"]));
            var nfi = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            nfi.NumberDecimalSeparator = ".";
            writer.WriteAttributeString("rate", ModulesRepository.ConvertTo<float>(row["CurrencyValue"]).ToString(nfi));
            writer.WriteEndElement();
        }

        private static void ProcessCategoryRow(DataRow row, XmlWriter writer)
        {
            writer.WriteStartElement("category");
            writer.WriteAttributeString("id", row["CategoryID"].ToString().Replace("ID", ""));
            if (row["ParentCategory"].ToString().Trim() != "0")
            {
                writer.WriteAttributeString("parentId", row["ParentCategory"].ToString().Replace("ID", ""));
            }
            writer.WriteString(ModulesRepository.ConvertTo<string>(row["Name"]));
            writer.WriteEndElement();
        }

        protected static DataTable GetCategories()
        {
            return ModulesRepository.ModuleExecuteTable(
                 @"SELECT [Category].[CategoryID], [Category].[ParentCategory], [Name] 
                            FROM [Catalog].[Category]      
                            WHERE [Category].CategoryID <> '0'", CommandType.Text);
        }

        protected static string ProcessProductDescription(string desc)
        {
            var sb = new StringBuilder(desc);
            sb.Replace("\n", "");
            sb.Replace('\t', ' ');

            var regEx = new Regex("<[a-zA-Z/]+>");
            var res = regEx.Replace(sb.ToString(), "");

            return res;
        }

        protected static DataTable GetCurrencies()
        {
            return ModulesRepository.ModuleExecuteTable("SELECT CurrencyValue, CurrencyIso3 FROM [Catalog].[Currency]", CommandType.Text);
        }

        private static void ProcessSimpleModel(RitmzProduct row, XmlWriter writer)
        {
            //var tempUrl = (_shopUrl.EndsWith("/") ? _shopUrl.TrimEnd('/') : _shopUrl);
            writer.WriteStartElement("offer");
            writer.WriteAttributeString("id", row.ArtNo);
            writer.WriteAttributeString("available", (row.Amount > 0 && row.Enabled).ToString().ToLower());

            writer.WriteStartElement("url");
            writer.WriteString(CreateLink(row));
            writer.WriteEndElement();


            writer.WriteStartElement("price");
            var nfi = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            nfi.NumberDecimalSeparator = ".";
            writer.WriteString(Math.Round(CatalogService.CalculatePrice(row.Price, row.Discount)).ToString(nfi));
            writer.WriteEndElement();

            writer.WriteStartElement("currencyId");
            writer.WriteString(_currency);
            writer.WriteEndElement();

            writer.WriteStartElement("categoryId");
            writer.WriteString(row.ParentCategory.ToString());
            writer.WriteEndElement();

            if (!string.IsNullOrEmpty(row.Photos))
            {
                var temp = row.Photos.Split(',');
                foreach (var item in temp.Where(item => !string.IsNullOrWhiteSpace(item)))
                {
                    writer.WriteStartElement("picture");
                    writer.WriteString(GetImageProductPath(item));
                    writer.WriteEndElement();
                }
            }

            writer.WriteStartElement("name");
            writer.WriteString(row.Name);
            writer.WriteEndElement();

            writer.WriteStartElement("description");
            var desc = ModulesRepository.ConvertTo<string>(row.Description);

            writer.WriteString(desc.XmlEncode());

            writer.WriteEndElement();

            if (!string.IsNullOrWhiteSpace(row.SalesNote))
            {
                writer.WriteStartElement("sales_notes");
                writer.WriteString(row.SalesNote);
                writer.WriteEndElement();
            }
            else if (!string.IsNullOrWhiteSpace(_salesNotes))
            {
                writer.WriteStartElement("sales_notes");
                writer.WriteString(_salesNotes);
                writer.WriteEndElement();
            }

            // 𠱸鱥 ﲠRitmz
            writer.WriteStartElement("rz_Active");
            writer.WriteString(ModulesRepository.ConvertTo<string>(row.Enabled));
            writer.WriteEndElement();

            writer.WriteStartElement("rz_Quantity");
            writer.WriteString(ModulesRepository.ConvertTo<string>(row.Amount));
            writer.WriteEndElement();

            writer.WriteStartElement("rz_Weight");
            writer.WriteString(ModulesRepository.ConvertTo<string>(row.Weight));
            writer.WriteEndElement();

            writer.WriteStartElement("rz_Length");
            writer.WriteString(string.Empty);
            writer.WriteEndElement();
            writer.WriteStartElement("rz_Width");
            writer.WriteString(string.Empty);
            writer.WriteEndElement();
            writer.WriteStartElement("rz_Height");
            writer.WriteString(string.Empty);
            writer.WriteEndElement();

            writer.WriteStartElement("rz_SupplierName");
            writer.WriteString(string.Empty);
            writer.WriteEndElement();
            writer.WriteStartElement("rz_SupplierCode");
            writer.WriteString(string.Empty);
            writer.WriteEndElement();
            writer.WriteStartElement("rz_SupplierPrice");
            writer.WriteString(ModulesRepository.ConvertTo<string>(row.SupplyPrice));
            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        private static void ProcessVendorModel(RitmzProduct row, XmlWriter writer)
        {
            writer.WriteStartElement("offer");
            writer.WriteAttributeString("id", row.ArtNo);
            writer.WriteAttributeString("available", (row.Amount > 0 && row.Enabled).ToString().ToLower());

            writer.WriteAttributeString("type", "vendor.model");

            writer.WriteStartElement("url");
            writer.WriteString(CreateLink(row));
            writer.WriteEndElement();


            writer.WriteStartElement("price");
            var nfi = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            nfi.NumberDecimalSeparator = ".";
            writer.WriteString(Math.Round(CatalogService.CalculatePrice(row.Price, row.Discount)).ToString(nfi));
            writer.WriteEndElement();

            writer.WriteStartElement("currencyId");
            writer.WriteString(_currency);
            writer.WriteEndElement();

            writer.WriteStartElement("categoryId");
            writer.WriteString(row.ParentCategory.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();


            if (!string.IsNullOrEmpty(row.Photos))
            {
                var temp = row.Photos.Split(',');
                foreach (var item in temp.Where(item => !string.IsNullOrWhiteSpace(item)))
                {
                    writer.WriteStartElement("picture");
                    writer.WriteString(GetImageProductPath(item));
                    writer.WriteEndElement();
                }
            }

            writer.WriteStartElement("delivery");
            writer.WriteEndElement();

            writer.WriteStartElement("vendor");
            writer.WriteString(row.BrandName);
            writer.WriteEndElement();

            writer.WriteStartElement("vendorCode");
            writer.WriteString(row.ArtNo);
            writer.WriteEndElement();

            writer.WriteStartElement("model");
            writer.WriteString(row.Name);
            writer.WriteEndElement();

            writer.WriteStartElement("description");
            string desc = ModulesRepository.ConvertTo<string>(row.Description);

            writer.WriteString(desc.XmlEncode());

            writer.WriteEndElement();

            if (!string.IsNullOrWhiteSpace(row.SalesNote))
            {
                writer.WriteStartElement("sales_notes");
                writer.WriteString(row.SalesNote);
                writer.WriteEndElement();
            }
            else if (!string.IsNullOrWhiteSpace(_salesNotes))
            {
                writer.WriteStartElement("sales_notes");
                writer.WriteString(_salesNotes);
                writer.WriteEndElement();
            }

            if (!string.IsNullOrWhiteSpace(row.ColorName))
            {
                writer.WriteStartElement("param");
                writer.WriteAttributeString("name", "Цвет");
                writer.WriteString(row.ColorName);
                writer.WriteEndElement();
            }

            if (!string.IsNullOrWhiteSpace(row.SizeName))
            {
                writer.WriteStartElement("param");
                writer.WriteAttributeString("name", "Размер");
                writer.WriteString(row.SizeName);
                writer.WriteEndElement();
            }


            // Additional Ritmz fields
            writer.WriteStartElement("rz_Active");
            writer.WriteString(ModulesRepository.ConvertTo<string>(row.Enabled));
            writer.WriteEndElement();

            writer.WriteStartElement("rz_Quantity");
            writer.WriteString(ModulesRepository.ConvertTo<string>(row.Amount));
            writer.WriteEndElement();

            writer.WriteStartElement("rz_Weight");
            writer.WriteString(ModulesRepository.ConvertTo<string>(row.Weight));
            writer.WriteEndElement();

            writer.WriteStartElement("rz_Length");
            writer.WriteString(string.Empty);
            writer.WriteEndElement();
            writer.WriteStartElement("rz_Width");
            writer.WriteString(string.Empty);
            writer.WriteEndElement();
            writer.WriteStartElement("rz_Height");
            writer.WriteString(string.Empty);
            writer.WriteEndElement();

            writer.WriteStartElement("rz_SupplierName");
            writer.WriteString(string.Empty);
            writer.WriteEndElement();
            writer.WriteStartElement("rz_SupplierCode");
            writer.WriteString(string.Empty);
            writer.WriteEndElement();
            writer.WriteStartElement("rz_SupplierPrice");
            writer.WriteString(ModulesRepository.ConvertTo<string>(row.SupplyPrice));
            writer.WriteEndElement();


            writer.WriteEndElement();
        }

        private static string CreateLink(RitmzProduct row)
        {
            var sufix = string.Empty;
            if (!row.Main)
            {
                if (row.ColorId != 0)
                    sufix = "color=" + row.ColorId;
                if (row.SizeId != 0)
                {
                    if (string.IsNullOrEmpty(sufix))
                        sufix = "size=" + row.SizeId;
                    else
                        sufix += "&size=" + row.SizeId;
                }
                sufix = !string.IsNullOrEmpty(sufix) ? "?" + sufix : sufix;
            }

            return ModuleSettingsProvider.GetSettingValue<string>("RitmzSiteUrl", _moduleName).TrimEnd('/') + "/" + UrlService.GetLink(ParamType.Product, row.UrlPath, row.ProductId) + sufix;
        }

        private static string GetImageProductPath(string photoPath)
        {
            if (string.IsNullOrEmpty(photoPath))
                photoPath = "";

            photoPath = photoPath.Trim();

            if (photoPath.ToLower().Contains("http://"))
                return photoPath;

            return ModuleSettingsProvider.GetSettingValue<string>("RitmzSiteUrl", _moduleName).TrimEnd('/') + "/" + FoldersHelper.GetImageProductPath(ProductImageType.Middle, photoPath, false);
        }

        private static IEnumerable<RitmzProduct> GetProduts()
        {
            return ModulesRepository.ModuleExecuteReadList<RitmzProduct>("[Module].[RitmzGetExportProducts]",
                                                                               CommandType.StoredProcedure,
                                                                               reader => new RitmzProduct
                                                                               {
                                                                                   ProductId = ModulesRepository.ConvertTo<int>(reader, "ProductID"),
                                                                                   OfferId = ModulesRepository.ConvertTo<int>(reader, "OfferId"),
                                                                                   ArtNo = ModulesRepository.ConvertTo<string>(reader, "ArtNo"),
                                                                                   Amount = ModulesRepository.ConvertTo<int>(reader, "Amount"),
                                                                                   UrlPath = ModulesRepository.ConvertTo<string>(reader, "UrlPath"),
                                                                                   Price = ModulesRepository.ConvertTo<float>(reader, "Price"),
                                                                                   Discount = ModulesRepository.ConvertTo<float>(reader, "Discount"),
                                                                                   ParentCategory = ModulesRepository.ConvertTo<int>(reader, "ParentCategory"),
                                                                                   Name = ModulesRepository.ConvertTo<string>(reader, "Name"),
                                                                                   Description = ModulesRepository.ConvertTo<string>(reader, "Description"),
                                                                                   BriefDescription = ModulesRepository.ConvertTo<string>(reader, "BriefDescription"),
                                                                                   Photos = ModulesRepository.ConvertTo<string>(reader, "Photos"),
                                                                                   SalesNote = ModulesRepository.ConvertTo<string>(reader, "SalesNote"),
                                                                                   ColorId = ModulesRepository.ConvertTo<int>(reader, "ColorId"),
                                                                                   ColorName = ModulesRepository.ConvertTo<string>(reader, "ColorName"),
                                                                                   SizeId = ModulesRepository.ConvertTo<int>(reader, "SizeId"),
                                                                                   SizeName = ModulesRepository.ConvertTo<string>(reader, "SizeName"),
                                                                                   BrandName = ModulesRepository.ConvertTo<string>(reader, "BrandName"),
                                                                                   Main = ModulesRepository.ConvertTo<bool>(reader, "Main"),
                                                                                   Enabled = ModulesRepository.ConvertTo<bool>(reader, "Enabled"),
                                                                                   SupplyPrice = ModulesRepository.ConvertTo<float>(reader, "SupplyPrice"),
                                                                                   Weight = ModulesRepository.ConvertTo<float>(reader, "Weight")
                                                                               },
                                                                               new SqlParameter("@selectedCurrency", _currency));
        }
    }
}