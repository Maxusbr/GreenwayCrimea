using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Hosting;
using System.Web.UI;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Helpers;
using AdvantShop.Module.MoySklad.Domain;
using CsvHelper;
using CsvHelper.Configuration;

namespace AdvantShop.Module.MoySklad
{
    public partial class MoySkladExportExcel : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnExportCatalog_Click(object sender, EventArgs e)
        {
            var onlyProducts = chkOnlyProducts.Checked;
            var fileName = "exportMoySklad.csv".FileNamePlusDate();
            var fileDirectory = HostingEnvironment.MapPath("~/modules/moysklad/");

            using (
                var csvWriter = new CsvWriter(new StreamWriter(fileDirectory + fileName, false, Encoding.UTF8),
                    new CsvConfiguration() {Delimiter = ";", SkipEmptyRecords = false}))
            {
                var columns = new List<string>
                {
                    "Группы", "Код", "Наименование", "Внешний код", "Артикул", "Единица измерения", 
                    "Цена продажи", "Валюта (Цена продажи)", "Закупочная цена", "Валюта (Закупочная цена)",
                    "Описание", 
                    "Код модификации", "Характеристика:" + SettingsCatalog.ColorsHeader, "Характеристика:" + SettingsCatalog.SizesHeader
                };

                var moySklad = new MoySklad();
                var showAmount = !moySklad.UpdateOnlyProducts;

                if (showAmount)
                {
                    columns.Add("Остаток");
                }

                foreach (var c in columns)
                    csvWriter.WriteField(c);

                csvWriter.NextRecord();
                

                foreach (var productCsv in MoySkladExportService.GetProducts())
                {
                    if (productCsv.Offers == null || productCsv.Offers.Count == 0)
                        continue;

                    WriteToCsv(csvWriter, productCsv, productCsv.ExternalCode);
                    csvWriter.WriteField("");
                    csvWriter.WriteField("");
                    csvWriter.WriteField("");

                    if (showAmount)
                    {
                        csvWriter.WriteField(productCsv.Amount);
                    }

                    csvWriter.NextRecord();

                    if (onlyProducts)
                        continue;


                    //if (productCsv.HasMultiOffer)
                    //{
                        foreach (var offer in productCsv.Offers)
                        {
                            productCsv.Price = offer.BasePrice.ToString("F2");
                            productCsv.PurchasePrice = offer.SupplyPrice.ToString("F2");
                            productCsv.Amount = offer.Amount.ToString();

                            var externalCode = MoySklad.GetMoyskladIdByOfferId(offer.OfferId);
                            if (string.IsNullOrWhiteSpace(externalCode))
                                externalCode = offer.ArtNo;

                            WriteToCsv(csvWriter, productCsv, externalCode);

                            csvWriter.WriteField(productCsv.Description);

                            //Код модификации
                            csvWriter.WriteField(offer.ArtNo);

                            csvWriter.WriteField(offer.Color != null ? offer.Color.ColorName : "");
                            csvWriter.WriteField(offer.Size != null ? offer.Size.SizeName : "");

                            if (showAmount)
                            {
                                csvWriter.WriteField(productCsv.Amount);
                            }

                            csvWriter.NextRecord();
                        }
                    //}
                }
            }

            CommonHelper.WriteResponseFile(fileDirectory + fileName, fileName);

        }

        private void WriteToCsv(CsvWriter csvWriter, MoySkladExportFeedCsvProduct product, string externalCode)
        {
            csvWriter.WriteField(product.Category);
            csvWriter.WriteField(product.ProductId);
            csvWriter.WriteField(product.Name);
            csvWriter.WriteField(externalCode);
            csvWriter.WriteField(product.ArtNo);
            csvWriter.WriteField(product.Unit);
            csvWriter.WriteField(product.Price);
            csvWriter.WriteField(product.Currency);
            csvWriter.WriteField(product.PurchasePrice);
            csvWriter.WriteField(product.Currency);

            //csvWriter.WriteField(product.Description);
        }
    }
}