using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.ExportImport;
using AdvantShop.Helpers;
using AdvantShop.Taxes;
using Resources;

using Newtonsoft.Json;

namespace AdvantShop.Admin.UserControls.ExportFeedUc
{
    public partial class ExportFeedCsvFieldsSettingsTab : UserControl
    {
        #region Fields 

        protected int ExportFeedId;

        protected List<ProductFields> FieldMapping = new List<ProductFields>();
        protected List<CSVField> ModuleFieldMapping = new List<CSVField>();
        private List<CSVField> _listModuleFields = new List<CSVField>();

        protected string CategorySort = "categorySort";

        protected static ExportFeedCsvOptions CsvExportFeedSettings { get; set; }
        protected ExportFeed CurrentExportFeed { get; set; }

        private enum SelectState
        {
            None,
            Deselect,
            Select
        }

        private SelectState State
        {
            // при постбэке не учитываем state из параметров
            get { return IsPostBack ? SelectState.None : Request["state"].TryParseEnum<SelectState>(); }
        }

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["feedid"]))
                return;

            ExportFeedId = 0;
            if (string.IsNullOrEmpty(Request["feedid"]) || !int.TryParse(Request["feedid"], out ExportFeedId))
                return;

            CurrentExportFeed = ExportFeedService.GetExportFeed(ExportFeedId);
            if (CurrentExportFeed == null ||
                !(CurrentExportFeed.Type == EExportFeedType.Csv || CurrentExportFeed.Type == EExportFeedType.Reseller))
            {
                return;
            }

            if (!IsPostBack)
            {
                var commonSettings = ExportFeedSettingsProvider.GetSettings(ExportFeedId);
                CsvExportFeedSettings = ExportFeedSettingsProvider.ConvertAdvancedSettings<ExportFeedCsvOptions>(commonSettings.AdvancedSettings);
                //CsvExportFeedSettings = ExportFeedSettingsProvider.GetSettings<ExportFeedCsvOptions>(ExportFeedId);
                //CsvExportFeedSettings = a
                FieldMapping = CsvExportFeedSettings.FieldMapping ?? new List<ProductFields>();
                ModuleFieldMapping = CsvExportFeedSettings.ModuleFieldMapping ?? new List<CSVField>();
            }

            CommonHelper.DisableBrowserCache();
            InitModuleFields();
            LoadFirstProduct();

            GenerateTable();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (State == SelectState.Deselect || State == SelectState.Select)
            {
                FieldMapping = new List<ProductFields>();
                ModuleFieldMapping = new List<CSVField>();

                SaveSettings();
                return;
            }

            LoadSelectedValues();
        }

        public void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        /// <summary>
        /// Формирует таблицу для выбора полей (без выставления выбранных значений)
        /// </summary>
        private void GenerateTable()
        {
            choseDiv.Controls.Clear();

            var tbl = new Table { ID = "tblValuesNew", CssClass = "export-tb table-values" };

            // строка заголовков
            var rowFirst = new TableHeaderRow();
            var cellM = new TableHeaderCell();
            cellM.Controls.Add(new Label { Text = Resource.Admin_ExportCsv_Column });
            rowFirst.Cells.Add(cellM);
            var cellL = new TableHeaderCell();
            cellL.Controls.Add(new Label { Text = Resource.Admin_ExportCsv_SampleOfData });
            rowFirst.Cells.Add(cellL);
            tbl.Rows.Add(rowFirst);

            var rowsDict = new Dictionary<string, TableRow>();

            foreach (ProductFields item in Enum.GetValues(typeof(ProductFields)))
            {
                var enumIntStringTemp = item.ConvertIntString();
                if (item == ProductFields.None || item == ProductFields.Sorting)
                    continue;

                var newRow = new TableRow();
                newRow.Cells.Add(GetTableCellForDdlCol(enumIntStringTemp));
                newRow.Cells.Add(GetTableCellForLblCol(enumIntStringTemp));
                rowsDict.Add(enumIntStringTemp, newRow);
            }

            foreach (var moduleField in _listModuleFields)
            {
                var newRow = new TableRow();
                var moduleFieldStrName = moduleField.StrName;
                if (rowsDict.ContainsKey(moduleFieldStrName))
                    continue;

                newRow.Cells.Add(GetTableCellForDdlCol(moduleFieldStrName));
                newRow.Cells.Add(GetTableCellForLblCol(moduleFieldStrName));
                rowsDict.Add(moduleFieldStrName, newRow);
            }

            tbl.Rows.AddRange(rowsDict.Select(item => item.Value).ToArray());
            choseDiv.Controls.Add(tbl);
        }

        /// <summary>
        /// Возвращает ячейку таблицы с выпадающим списком
        /// </summary>
        private TableCell GetTableCellForDdlCol(string ddlSelectedValue)
        {
            var cell = new TableCell();
            var ddl = new DropDownList { ID = "ddlType" + ddlSelectedValue };

            foreach (ProductFields innerItem in Enum.GetValues(typeof(ProductFields)))
            {
                // за выгрузку сортировки отвечает ChbCategorySort.Checked
                if (innerItem == ProductFields.Sorting)
                    continue;
                ddl.Items.Add(new ListItem(innerItem.Localize(), innerItem.ConvertIntString()));
            }
            foreach (var innerModuleField in _listModuleFields)
            {
                ddl.Items.Add(new ListItem(innerModuleField.DisplayName, innerModuleField.StrName));
            }

            if (State != SelectState.Deselect && (State == SelectState.Select)) //  || select
                ddl.SelectedValue = ddlSelectedValue;

            ddl.Attributes.Add("onchange", string.Format("Change(this)"));
            cell.Controls.Add(ddl);

            return cell;
        }

        /// <summary>
        /// Возвращает ячейку таблицы с текстом
        /// </summary>
        private TableCell GetTableCellForLblCol(string ddlSelectedValue)
        {
            var cellLbl = new TableCell();
            var lb = new Label
            {
                ID = "lbProduct" + ddlSelectedValue,
                Text = ddlProduct.Items.Count > 0 && (State != SelectState.Deselect && (State == SelectState.Select)) //  || select
                           ? ddlProduct.Items.FindByValue(ddlSelectedValue).Text
                           : string.Empty
            };
            lb.Attributes.Add("style", "display:block");
            cellLbl.Controls.Add(lb);

            return cellLbl;
        }

        private void GetFieldMapping()
        {
            var table = (Table)choseDiv.FindControl("tblValuesNew");

            if (table == null)
                return;

            foreach (TableRow item in table.Rows)
            {
                var element = item.Cells[0].Controls.OfType<DropDownList>().FirstOrDefault();
                if (element == null) continue;

                if (element.SelectedValue == ProductFields.None.ConvertIntString())
                    continue;

                ProductFields currentField;
                if (Enum.TryParse(element.SelectedValue, out currentField))
                {
                    if (!FieldMapping.Contains(currentField))
                    {
                        FieldMapping.Add(currentField);
                    }
                }
                else if (_listModuleFields.Select(f => f.StrName).Contains(element.SelectedValue))
                {
                    if (!ModuleFieldMapping.Select(f => f.StrName).Contains(element.SelectedValue))
                    {
                        ModuleFieldMapping.Add(_listModuleFields.First(f => f.StrName == element.SelectedValue));
                    }

                }
            }

            if (CsvExportFeedSettings.CsvCategorySort)
            {
                var ind = FieldMapping.IndexOf(ProductFields.Category);
                if (ind > 0)
                    FieldMapping.Insert(ind + 1, ProductFields.Sorting);
                else
                    FieldMapping.Add(ProductFields.Sorting);
            }
        }

        /// <summary>
        /// Выставляет значения у дропдаунлистов и текст к ним
        /// </summary>
        private void LoadSelectedValues()
        {
            var table = (Table)choseDiv.FindControl("tblValuesNew");

            if (table == null)
                return;

            var i = 0;
            var j = 0;
            foreach (TableRow item in table.Rows)
            {
                var element = item.Cells[0].Controls.OfType<DropDownList>().FirstOrDefault();
                var label = item.Cells[1].Controls.OfType<Label>().FirstOrDefault();
                if (element == null) continue;

                if (i < FieldMapping.Count && element.Items.FindByValue(FieldMapping[i].ConvertIntString()) != null)
                {
                    element.SelectedValue = FieldMapping[i].ConvertIntString();
                }
                else if (j < ModuleFieldMapping.Count && element.Items.FindByValue(ModuleFieldMapping[j].StrName) != null)
                {
                    element.SelectedValue = ModuleFieldMapping[j].StrName;
                    j++;
                }

                if (label != null && element.SelectedValue != "0" && ddlProduct.Items.Count > 0)
                {
                    label.Text = ddlProduct.Items.FindByValue(element.SelectedValue).Text;
                }

                i++;
            }
        }

        private void SaveSettings()
        {
            GetFieldMapping();
            var commonSettings = ExportFeedSettingsProvider.GetSettings(ExportFeedId);
            if (CurrentExportFeed.Type == EExportFeedType.Csv)
            {
                var settings = ExportFeedSettingsProvider.ConvertAdvancedSettings<ExportFeedCsvOptions>(commonSettings.AdvancedSettings);
                settings.FieldMapping = FieldMapping;
                settings.ModuleFieldMapping = ModuleFieldMapping;
                ExportFeedSettingsProvider.SetAdvancedSettings(ExportFeedId, JsonConvert.SerializeObject(settings));
            }
            else if (CurrentExportFeed.Type == EExportFeedType.Reseller)
            {
                var settings = ExportFeedSettingsProvider.ConvertAdvancedSettings<ExportFeedResellerOptions>(commonSettings.AdvancedSettings);

                var ind = FieldMapping.IndexOf(ProductFields.Category);
                if (ind > 0)
                    if(!FieldMapping.Contains(ProductFields.Sorting))
                        FieldMapping.Insert(ind + 1, ProductFields.Sorting);
                else
                    FieldMapping.Remove(ProductFields.Sorting);

                settings.FieldMapping = FieldMapping;
                settings.ModuleFieldMapping = ModuleFieldMapping;
                ExportFeedSettingsProvider.SetAdvancedSettings(ExportFeedId, JsonConvert.SerializeObject(settings));
            }

            saveSuccess.Visible = true;
        }

        #region LoadFirstProduct, InitModuleFields

        private void InitModuleFields()
        {
            foreach (var csvExportImportModule in AttachedModules.GetModules<ICSVExportImport>())
            {
                var classInstance = (ICSVExportImport)Activator.CreateInstance(csvExportImportModule, null);
                if (ModulesRepository.IsActiveModule(classInstance.ModuleStringId) && classInstance.CheckAlive())
                {
                    _listModuleFields.AddRange(classInstance.GetCSVFields());
                }
            }
        }

        private void LoadFirstProduct()
        {
            if(CsvExportFeedSettings == null)
            {
                return;
            }

            var columnSeparator = CsvExportFeedSettings.CsvColumSeparator;
            var propertySeparator = CsvExportFeedSettings.CsvPropertySeparator;

            var product = ProductService.GetFirstProduct();
            if (product == null) return;
            foreach (ProductFields item in Enum.GetValues(typeof(ProductFields)))
            {
                var itemText = string.Empty;
                switch (item)
                {
                    case ProductFields.None:
                        itemText = @"-";
                        break;
                    case ProductFields.Sku:
                        itemText = product.ArtNo;
                        break;
                    case ProductFields.Name:
                        itemText = product.Name.HtmlEncode();
                        break;
                    case ProductFields.ParamSynonym:
                        itemText = product.UrlPath;
                        break;
                    case ProductFields.Category:
                        itemText = CategoryService.GetCategoryStringByProductId(product.ProductId, columnSeparator);
                        break;
                    case ProductFields.Sorting:
                        itemText = CategoryService.GetCategoryStringByProductId(product.ProductId, columnSeparator, true);
                        break;
                    case ProductFields.Enabled:
                        itemText = product.Enabled ? "+" : "-";
                        break;
                    case ProductFields.Currency:
                        itemText = product.Currency.Iso3;
                        break;
                    case ProductFields.Price:
                        itemText = product.Offers.Select(x => x.BasePrice).FirstOrDefault().ToString("F2");
                        break;
                    case ProductFields.PurchasePrice:
                        itemText = product.Offers.Select(x => x.SupplyPrice).FirstOrDefault().ToString("F2");
                        break;
                    case ProductFields.Amount:
                        itemText = product.Offers.Select(x => x.Amount).FirstOrDefault().ToString("F2");
                        break;
                    case ProductFields.MultiOffer:
                        itemText = OfferService.OffersToString(product.Offers, columnSeparator, propertySeparator);
                        break;
                    case ProductFields.Unit:
                        itemText = product.Unit;
                        break;
                    case ProductFields.Discount:
                        itemText = product.Discount.Percent.ToString("F2");
                        break;
                    case ProductFields.DiscountAmount:
                        itemText = product.Discount.Amount.ToString("F2");
                        break;
                    case ProductFields.ShippingPrice:
                        itemText = product.ShippingPrice == null ? "" : product.ShippingPrice.Value.ToString("F2");
                        break;
                    case ProductFields.Weight:
                        itemText = product.Weight.ToString("F2");
                        break;
                    case ProductFields.Size:
                        itemText = product.Length + "x" + product.Width + "x" + product.Height;
                        break;
                    case ProductFields.BriefDescription:
                        itemText = product.BriefDescription.Reduce(20).HtmlEncode();
                        break;
                    case ProductFields.Description:
                        itemText = product.Description.Reduce(20).HtmlEncode();
                        break;
                    case ProductFields.Title:
                        itemText = product.Meta.Title.Reduce(20);
                        break;
                    case ProductFields.H1:
                        itemText = product.Meta.H1.Reduce(20);
                        break;
                    case ProductFields.MetaKeywords:
                        itemText = product.Meta.MetaKeywords.Reduce(20);
                        break;
                    case ProductFields.MetaDescription:
                        itemText = product.Meta.MetaDescription.Reduce(20);
                        break;
                    case ProductFields.Photos:
                        itemText = PhotoService.PhotoToString(product.ProductPhotos, columnSeparator, propertySeparator);
                        break;
                    case ProductFields.Videos:
                        itemText = ProductVideoService.VideoToString(product.ProductVideos, columnSeparator).Reduce(20).HtmlEncode();
                        break;
                    case ProductFields.Markers:
                        itemText = ProductService.MarkersToString(product, columnSeparator);
                        break;
                    case ProductFields.Properties:
                        itemText = PropertyService.PropertiesToString(product.ProductPropertyValues, columnSeparator, propertySeparator).HtmlEncode();
                        break;
                    case ProductFields.Producer:
                        itemText = BrandService.BrandToString(product.BrandId);
                        break;
                    case ProductFields.OrderByRequest:
                        itemText = product.AllowPreOrder ? "+" : "-";
                        break;
                    case ProductFields.SalesNotes:
                        itemText = product.SalesNote;
                        break;
                    case ProductFields.Related:
                        itemText = ProductService.LinkedProductToString(product.ProductId, RelatedType.Related, columnSeparator);
                        break;
                    case ProductFields.Alternative:
                        itemText = ProductService.LinkedProductToString(product.ProductId, RelatedType.Alternative, columnSeparator);
                        break;
                    case ProductFields.CustomOption:
                        itemText = CustomOptionsService.CustomOptionsToString(CustomOptionsService.GetCustomOptionsByProductId(product.ProductId));
                        break;
                    case ProductFields.Gtin:
                        itemText = product.Gtin;
                        break;
                    case ProductFields.GoogleProductCategory:
                        itemText = product.GoogleProductCategory;
                        break;
                    case ProductFields.YandexProductCategory:
                        itemText = product.YandexMarketCategory;
                        break;
                    case ProductFields.YandexTypePrefix:
                        itemText = product.YandexTypePrefix;
                        break;
                    case ProductFields.YandexModel:
                        itemText = product.YandexModel;
                        break;
                    case ProductFields.Adult:
                        itemText = product.Adult ? "+" : "-";
                        break;
                    case ProductFields.MinAmount:
                        itemText = (product.MinAmount ?? 0).ToString("F2");
                        break;
                    case ProductFields.MaxAmount:
                        itemText = (product.MaxAmount ?? 0).ToString("F2");
                        break;
                    case ProductFields.Multiplicity:
                        itemText = product.Multiplicity.ToString("F2");
                        break;
                    case ProductFields.BarCode:
                        itemText = product.BarCode;
                        break;
                    case ProductFields.Tax:
                        if (product.TaxId != null)
                        {
                            var tax = TaxService.GetTax(product.TaxId.Value);
                            if (tax != null)
                                itemText = tax.Name;
                        }
                        break;
                }
                ddlProduct.Items.Add(new ListItem(itemText, item.ConvertIntString()));
            }
            foreach (var csvExportImportModule in AttachedModules.GetModules<ICSVExportImport>())
            {
                var classInstance = (ICSVExportImport)Activator.CreateInstance(csvExportImportModule, null);
                if (ModulesRepository.IsActiveModule(classInstance.ModuleStringId) && classInstance.CheckAlive())
                {
                    foreach (var moduleField in classInstance.GetCSVFields())
                    {
                        ddlProduct.Items.Add(new ListItem(classInstance.PrepareField(moduleField, product.ProductId, columnSeparator, propertySeparator), moduleField.StrName));
                    }
                }
            }
        }

        #endregion
    }
}