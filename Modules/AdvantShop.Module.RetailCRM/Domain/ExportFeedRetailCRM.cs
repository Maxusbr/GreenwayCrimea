//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.SQL;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.ExportImport;
using AdvantShop.Helpers;
using AdvantShop.Core.Modules;
using AdvantShop.Statistic;
using AdvantShop.Module.RetailCRMs.Domain.Models;
//using Yogesh.Extensions;

namespace AdvantShop.Modules.RetailCRM
{
    public class ExportFeedRetailCRM : BaseExportFeed
    {
        protected string ModuleName
        {
            get { return "RetailCRM"; }
        }

        private IEnumerable<ExportFeedCategories> _categories;
        private IEnumerable<ExportFeedProductModel> _products;
        private ExportFeedYandexOptions _options;
        private int _categoriesCount;
        private int _productsCount;
        private string _retailArtNoType;



        //private string _currency;
        //private string _description;
        //private string _salesNotes;
        //private bool _delivery;
        //private bool _removeHTML;

        private List<ProductDiscount> _productDiscountModels = null;

        public ExportFeedRetailCRM(IEnumerable<ExportFeedCategories> categories, IEnumerable<ExportFeedProductModel> products, ExportFeedSettings options, int categoriesCount, int productsCount) :
            base(categories, products, options, categoriesCount, productsCount)
        {
            _categories = categories;
            _products = products;
            _options = (ExportFeedYandexOptions)options;
            _categoriesCount = categoriesCount;
            _productsCount = productsCount;
            _retailArtNoType = ModuleSettingsProvider.GetSettingValue<string>("RetailArtNoType", RetailCRMModule.ModuleStringId);
        }

        [Obsolete("This method is obsolete. Use Export method", false)]
        public override void Build()
        {
            var fileName = _options.FileName + "." + _options.FileExtention;
            var filePath = SettingsGeneral.AbsolutePath + "/" + fileName;
            var directory = filePath.Substring(0, filePath.LastIndexOf('/'));

            var tempFilePath = SettingsGeneral.AbsolutePath + "/" + _options.FileName + "_temp." + _options.FileExtention;

            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            FileHelpers.DeleteFile(tempFilePath);

            try
            {
                //_currency = "RUB";
                //_removeHTML = true;
                var shopName = SettingsMain.ShopName.XmlEncode().RemoveInvalidXmlChars();
                var companyName = SettingsMain.ShopName.XmlEncode().RemoveInvalidXmlChars();


                using (var outputFile = new FileStream(tempFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
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
                        writer.WriteRaw(shopName.XmlEncode().RemoveInvalidXmlChars());
                        writer.WriteEndElement();

                        writer.WriteStartElement("company");
                        writer.WriteRaw(companyName.XmlEncode().RemoveInvalidXmlChars());
                        writer.WriteEndElement();

                        writer.WriteStartElement("url");
                        writer.WriteRaw(SettingsMain.SiteUrl);
                        writer.WriteEndElement();

                        writer.WriteStartElement("categories");
                        foreach (var categoryRow in _categories)
                        {
                            ProcessCategoryRow(categoryRow, writer);
                        }
                        writer.WriteEndElement();

                        writer.WriteStartElement("offers");

                        foreach (RetailCrmExportFeedProduct offerRow in _products)
                        {
                            ProcessProductRow(offerRow, writer);
                        }
                        writer.WriteEndElement();

                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        writer.WriteEndDocument();

                        writer.Flush();
                        writer.Close();
                    }
                }

                FileHelpers.DeleteFile(filePath);
                File.Move(tempFilePath, filePath);

            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
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

        private void ProcessProductRow(RetailCrmExportFeedProduct row, XmlWriter writer)
        {
            ProcessVendorModel(row, writer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private string CreateLink(ExportFeedYandexProduct row)
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
                        sufix += "&amp;size=" + row.SizeId;
                }
                sufix = !string.IsNullOrEmpty(sufix) ? "?" + sufix : sufix;
            }
            return SettingsMain.SiteUrl.TrimEnd('/') + "/" + UrlService.GetLink(ParamType.Product, row.UrlPath, row.ProductId) + sufix;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="writer"></param>
        private void ProcessVendorModel(RetailCrmExportFeedProduct row, XmlWriter writer)
        {
            writer.WriteStartElement("offer");
            writer.WriteAttributeString("id", _retailArtNoType == "ID" ? row.OfferId.ToString() : row.ArtNo);
            writer.WriteAttributeString("productId", row.ProductId.ToString());
            writer.WriteAttributeString("quantity", row.Amount.ToString());
            writer.WriteAttributeString("available", (row.Amount > 0).ToString().ToLower());

            writer.WriteAttributeString("type", "vendor.model");

            writer.WriteStartElement("url");
            writer.WriteRaw(CreateLink(row));
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

            writer.WriteStartElement("price");
            var nfi = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            nfi.NumberDecimalSeparator = ".";

            var price = PriceService.GetFinalPrice(row.Price, new Discount(discount != 0 ? discount : row.Discount, discount != 0 ? 0 : row.DiscountAmount), row.CurrencyValue);
            //PriceService.GetFinalPrice(row.Price, discount != 0 ? discount : row.Discount, row.CurrencyValue);

            writer.WriteRaw(Math.Round(price).ToString(nfi));
            writer.WriteEndElement();

            writer.WriteStartElement("purchasePrice");
            nfi.NumberDecimalSeparator = ".";
            writer.WriteRaw(Math.Round(row.SupplyPrice).ToString(nfi));
            writer.WriteEndElement();

            writer.WriteStartElement("productActivity");
            writer.WriteRaw(row.Enabled ? "Y" : "N");
            writer.WriteEndElement();



            writer.WriteStartElement("article");
            writer.WriteRaw(row.ArtNo.XmlEncode().RemoveInvalidXmlChars());
            writer.WriteEndElement();

            writer.WriteStartElement("xmlId");
            writer.WriteRaw(GetExternalCode(row.ArtNo).XmlEncode().RemoveInvalidXmlChars());
            writer.WriteEndElement();

            writer.WriteStartElement("currencyId");
            writer.WriteRaw(_options.Currency);
            writer.WriteEndElement();

            writer.WriteStartElement("categoryId");
            writer.WriteRaw(row.ParentCategory.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();


            if (!string.IsNullOrEmpty(row.Photos))
            {
                var temp = row.Photos.Split(',').Take(9);
                foreach (var item in temp.Where(item => !string.IsNullOrWhiteSpace(item)))
                {
                    writer.WriteStartElement("picture");
                    writer.WriteRaw(GetImageProductPath(item));
                    writer.WriteEndElement();
                }
            }

            if (_options.Delivery)
            {
                writer.WriteStartElement("delivery");
                writer.WriteRaw("true");
                writer.WriteEndElement();
            }

            writer.WriteStartElement("vendor");
            writer.WriteRaw(row.BrandName.XmlEncode().RemoveInvalidXmlChars());
            writer.WriteEndElement();

            writer.WriteStartElement("vendorCode");
            writer.WriteRaw(row.ArtNo.XmlEncode().RemoveInvalidXmlChars());
            writer.WriteEndElement();

            writer.WriteStartElement("name");
            writer.WriteRaw(row.Name.XmlEncode().RemoveInvalidXmlChars());
            writer.WriteEndElement();

            writer.WriteStartElement("description");
            if (!string.IsNullOrEmpty(_options.ProductDescriptionType) && !string.Equals(_options.ProductDescriptionType, "none"))
            {
                string desc = SQLDataHelper.GetString(_options.ProductDescriptionType == "full" ? row.Description : row.BriefDescription);

                if (_options.RemoveHtml)
                {
                    desc = StringHelper.RemoveHTML(desc);
                }

                writer.WriteRaw(desc.XmlEncode().RemoveInvalidXmlChars());
            }
            writer.WriteEndElement();

            if (!string.IsNullOrWhiteSpace(row.SalesNote))
            {
                writer.WriteStartElement("sales_notes");
                writer.WriteRaw(row.SalesNote.XmlEncode().RemoveInvalidXmlChars());
                writer.WriteEndElement();
            }
            else if (!string.IsNullOrWhiteSpace(row.SalesNote))
            {
                writer.WriteStartElement("sales_notes");
                writer.WriteRaw(row.SalesNote.XmlEncode().RemoveInvalidXmlChars());
                writer.WriteEndElement();
            }

            if (row.Adult)
            {
                writer.WriteStartElement("adult");
                writer.WriteRaw(row.Adult.ToString().ToLower());
                writer.WriteEndElement();
            }

            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "Артикул");
            writer.WriteAttributeString("code", "article");
            writer.WriteRaw(row.ArtNo.XmlEncode().RemoveInvalidXmlChars());
            writer.WriteEndElement();

            if (row.Weight != 0)
            {
                writer.WriteStartElement("param");
                writer.WriteAttributeString("name", "Вес");
                writer.WriteAttributeString("code", "weight");
                writer.WriteRaw((row.Weight * 1000).ToString());
                writer.WriteEndElement();
            }

            if (!string.IsNullOrWhiteSpace(row.ColorName))
            {
                writer.WriteStartElement("param");
                writer.WriteAttributeString("name", "Цвет");
                writer.WriteAttributeString("code", "color");
                writer.WriteRaw(row.ColorName.XmlEncode().RemoveInvalidXmlChars());
                writer.WriteEndElement();
            }

            if (!string.IsNullOrWhiteSpace(row.SizeName))
            {
                writer.WriteStartElement("param");
                writer.WriteAttributeString("name", "Размер");
                writer.WriteAttributeString("code", "size");
                writer.WriteRaw(row.SizeName.XmlEncode().RemoveInvalidXmlChars());
                writer.WriteEndElement();
            }

            if (row.BarCode.IsNotEmpty())
            {
                writer.WriteStartElement("barcode");
                writer.WriteRaw(row.BarCode);
                writer.WriteEndElement();
            }

            writer.WriteStartElement("dimensions");
            writer.WriteRaw(string.Format("{0}/{1}/{2}", Math.Round(row.Length / 10, 3).ToInvariantString(),
                                                         Math.Round(row.Width / 10, 3).ToInvariantString(),
                                                         Math.Round(row.Height / 10, 3).ToInvariantString()));
            
            writer.WriteEndElement();



            writer.WriteEndElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artNo"></param>
        /// <returns></returns>
        private string GetExternalCode(string artNo)
        {
            if (!SQLDataAccess.ExecuteScalar<bool>(
                "IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Catalog' AND  TABLE_NAME = 'ProductFromMoysklad'))" +
                "BEGIN select 1 END else begin select 0 end", CommandType.Text))
            {
                return artNo;
            }
            else
            {
                string productCode =
                    SQLDataAccess.ExecuteScalar<string>(
                        "select top 1 [ProductExternalId] from Catalog.ProductFromMoysklad " +
                        "inner join catalog.product on ProductFromMoysklad.ProductID = Product.ProductID " +
                        "where product.artno=@artno", CommandType.Text, new SqlParameter("@artno", artNo));

                string offerCode =
                    SQLDataAccess.ExecuteScalar<string>(
                        "select top 1 [OfferExternalId] from Catalog.OfferFromMoysklad " +
                        "inner join catalog.Offer on OfferFromMoysklad.Offerid = Offer.OfferID " +
                        "where Offer.artno=@artno", CommandType.Text, new SqlParameter("@artno", artNo));

                if (productCode.IsNullOrEmpty() && offerCode.IsNullOrEmpty())
                {
                    return artNo;
                }
                else
                {
                    if (productCode == offerCode)
                    {
                        return offerCode;
                    }
                    else
                    {
                        return (productCode + "#" + offerCode).Trim('#');
                    }
                }
            }
        }

        public override string Export(int exportFeedId)
        {
            throw new NotImplementedException();
        }

        public override string Export(IEnumerable<ExportFeedCategories> categories, IEnumerable<ExportFeedProductModel> products, ExportFeedSettings options, int categoriesCount, int productsCount)
        {
            throw new NotImplementedException();
        }

        public override void SetDefaultSettings(int exportFeedId)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override int GetCategoriesCount(int exportFeedId)
        {
            throw new NotImplementedException();
        }

        public override List<string> GetAvailableFileExtentions()
        {
            throw new NotImplementedException();
        }
    }
}