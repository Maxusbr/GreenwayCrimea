//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.ExportImport;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Saas;
using AdvantShop.Statistic;
using AdvantShop.Trial;
using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace Admin
{
    public partial class ImportCSV : AdvantShopAdminPage
    {
        private readonly string _filePath;
        private readonly string _fullPath;
        private bool _hasHeadrs;
        private readonly Dictionary<string, int> _fieldMapping = new Dictionary<string, int>();
        private readonly List<ProductFields> _mustRequiredField;
        private const string StrFileName = "importCSV";
        private const string StrFileExt = ".csv";

        private readonly List<CSVField> _listModuleFields = new List<CSVField>();

        protected ImportCSV()
        {
            _filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
            FileHelpers.CreateDirectory(_filePath);
            _mustRequiredField = new List<ProductFields>();

            _fullPath = Directory.GetFiles(_filePath).Where(f => f.Contains(StrFileName)).OrderByDescending(x => x).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(_fullPath)) return;
            _fullPath = _filePath + (StrFileName + StrFileExt).FileNamePlusDate();
        }

        private void MsgErr(bool clean)
        {
            if (clean)
            {
                lblError.Visible = false;
                lblError.Text = string.Empty;
            }
            else
            {
                lblError.Visible = false;
            }
        }

        private void MsgErr(string messageText)
        {
            lblError.Visible = true;
            lblError.Text = @"<br/>" + messageText;
        }

        protected void btnAction_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(lblError.Text))
                return;

            if (!_fieldMapping.ContainsKey(ProductFields.Sku.StrName()) && !_fieldMapping.ContainsKey(ProductFields.Name.StrName()))
            {
                MsgErr(Resource.Admin_ImportCsv_SelectNameOrSKU);
                return;
            }


            if (SaasDataService.IsSaasEnabled)
            {
                divSaasPlanProducts.Visible = true;
                hfProductsCount.Value = AdvantShop.Catalog.ProductService.GetProductsCount().ToString();
                lTotalSaasPlanProducts.Text = SaasDataService.CurrentSaasData.ProductsCount.ToString();
            }

            divAction.Visible = false;
            choseDiv.Visible = false;
            if (!File.Exists(_fullPath))
                return;

            if (CommonStatistic.IsRun)
                return;
            _hasHeadrs = Request["hasheadrs"] == "true";
            CommonStatistic.Init();
            CommonStatistic.CurrentProcess = Request.Url.PathAndQuery;
            CommonStatistic.CurrentProcessName = Resource.Admin_ImportXLS_CatalogUpload;
            linkCancel.Visible = true;
            MsgErr(true);
            lblRes.Text = string.Empty;
            CommonStatistic.IsRun = true;
            CsvImport.Factory(_fullPath, _hasHeadrs, chboxDisableProducts.Checked, SettingsGeneral.CsvSeparator, SettingsGeneral.CsvEnconing, _fieldMapping, SettingsGeneral.CsvColumSeparator, SettingsGeneral.CsvPropertySeparator).Process();
            OutDiv.Visible = true;
            TrialService.TrackEvent(TrialEvents.MakeCSVImport, "");


            CategoryService.SetCategoryHierarchicallyEnabled(0);
            CategoryService.RecalculateProductsCountManual();
            ProductService.PreCalcProductParamsMass();
        }

        protected TableCell FirstColumnCell(string text, string className)
        {
            var cell = new TableCell { Width = 200, BackColor = System.Drawing.Color.White };
            cell.Controls.Add(new Label { Text = text, CssClass = "firstColumn" });

            var pnl = new Panel { CssClass = className };
            pnl.Controls.Add(new Panel { CssClass = "arrow_right_bg" });
            cell.Controls.Add(pnl);

            var div = new Panel { Width = 200 };
            cell.Controls.Add(div);
            return cell;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            MsgErr(true);
            InitModuleFields();
            divStart.Visible = !CommonStatistic.IsRun && (string.IsNullOrEmpty(Request["action"]));
            divAction.Visible = !CommonStatistic.IsRun && (Request["action"] == "start");
            choseDiv.Visible = !CommonStatistic.IsRun;
            OutDiv.Visible = CommonStatistic.IsRun;
            linkCancel.Visible = CommonStatistic.IsRun;

            if (CommonStatistic.IsRun || (Request["action"] != "start"))
                return;
            if (!File.Exists(_fullPath))
                return;

            var tblValues = new Table { ID = "tblValues" };
            var namesRow = new TableRow { ID = "namesRow", BackColor = System.Drawing.ColorTranslator.FromHtml("#0D76B8"), Height = 28 };
            var firstValRow = new TableRow { ID = "firstValsRow" };
            var ddlRow = new TableRow { ID = "ddlRow" };

            namesRow.Cells.Add(FirstColumnCell(Resource.Admin_ImportCsv_Column, "arrow_left_bg"));
            firstValRow.Cells.Add(FirstColumnCell(Resource.Admin_ImportCsv_FistLineInTheFile, "arrow_left_bg_two"));
            ddlRow.Cells.Add(FirstColumnCell(Resource.Admin_ImportCsv_DataType, "arrow_left_bg_free"));

            _hasHeadrs = Request["hasheadrs"].TryParseBool();
            var csvrows = CsvImport.Factory(_fullPath, false, false, SettingsGeneral.CsvSeparator, SettingsGeneral.CsvEnconing, null, SettingsGeneral.CsvColumSeparator, SettingsGeneral.CsvPropertySeparator).ReadFirst2();
            if (csvrows.Count == 0)
            {
                MsgErr(Resource.Admin_ImportCsv_ErrorReadFille);
                return;
            }

            if (_hasHeadrs && csvrows[0].HasDuplicates())
            {
                MsgErr(Resource.Admin_ImportCsv_DuplicateHeader + csvrows[0].Duplicates().AggregateString(','));
                btnAction.Visible = false;
            }

            for (int i = 0; i < csvrows[0].Length; i++)
            {
                var cell = new TableCell();
                var lb = new Label() { ForeColor = System.Drawing.Color.White };
                bool flagMustReqField = false;
                if (_hasHeadrs)
                {
                    var tempCsv = (csvrows[0][i].Length > 50 ? csvrows[0][i].Substring(0, 49) : csvrows[0][i]);
                    if (_mustRequiredField.Any(item => item.StrName() == tempCsv.ToLower()))
                    {
                        flagMustReqField = true;
                    }
                    lb.Text = tempCsv;
                }
                else
                {
                    lb.Text = Resource.Admin_ImportCsv_Empty;
                }
                cell.Controls.Add(lb);

                if (flagMustReqField)
                {
                    cell.Controls.Add(new Label { Text = @"*", ForeColor = System.Drawing.Color.Red });
                }

                namesRow.Cells.Add(cell);

                cell = new TableCell { Width = 150 };
                var ddl = new DropDownList { ID = "ddlType" + i, Width = 150 };

                foreach (ProductFields item in Enum.GetValues(typeof(ProductFields)))
                {
                    ddl.Items.Add(new ListItem(item.Localize(), item.StrName().ToLower()));
                }

                foreach (var moduleField in _listModuleFields)
                {
                    ddl.Items.Add(new ListItem(moduleField.DisplayName, moduleField.StrName.ToLower()));
                }

                ddl.SelectedValue = lb.Text.Trim().ToLower();
                cell.Controls.Add(ddl);
                ddlRow.Cells.Add(cell);
            }

            var dataRow = csvrows.Count > 1 ? csvrows[1] : csvrows[0];

            if (dataRow != null)
            {
                foreach (var data in dataRow)
                {
                    var cell = new TableCell();
                    if (data == null)
                        cell.Controls.Add(new Label { Text = string.Empty });
                    else
                        cell.Controls.Add(new Label
                        {
                            Text = data.Length > 50 ? data.Substring(0, 49).HtmlEncode() : data.HtmlEncode()
                        });
                    firstValRow.Cells.Add(cell);
                }
            }

            tblValues.Rows.Add(namesRow);
            tblValues.Rows.Add(firstValRow);
            tblValues.Rows.Add(ddlRow);
            choseDiv.Controls.Add(tblValues);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ImportXLS_Title));

            //if ((SaasDataService.IsSaasEnabled) && (!SaasDataService.CurrentSaasData.HaveExcel))
            //{
            //    mainDiv.Visible = false;
            //    notInTariff.Visible = true;
            //}

            if (!IsPostBack)
            {
                if (divStart.Visible)
                {
                    ddlEncoding.Items.Clear();
                    foreach (EncodingsEnum enumItem in Enum.GetValues(typeof(EncodingsEnum)))
                    {
                        ddlEncoding.Items.Add(new ListItem(enumItem.ToString(), enumItem.StrName()));
                    }
                    ddlEncoding.SelectedValue = SettingsGeneral.CsvEnconing;

                    ddlSeparators.Items.Clear();
                    foreach (SeparatorsEnum enumItem in Enum.GetValues(typeof(SeparatorsEnum)))
                    {
                        ddlSeparators.Items.Add(new ListItem(enumItem.Localize(), enumItem.StrName()));
                    }

                    if (ddlSeparators.Items.FindByValue(SettingsGeneral.CsvSeparator) != null)
                    {
                        ddlSeparators.SelectedValue = SettingsGeneral.CsvSeparator;
                    }
                    else
                    {
                        ddlSeparators.SelectedValue = SeparatorsEnum.Custom.StrName();
                        txtCustomSeparator.Text = SettingsGeneral.CsvSeparator;
                    }

                    txtColumSeparator.Text = SettingsGeneral.CsvColumSeparator;
                    txtPropertySeparator.Text = SettingsGeneral.CsvPropertySeparator;
                }
            }

            if (IsPostBack && choseDiv.FindControl("tblValues") != null)
            {
                var cells = ((TableRow)choseDiv.FindControl("ddlRow")).Cells;
                short index = 0;
                foreach (TableCell item in cells)
                {
                    var element = item.Controls.OfType<DropDownList>().FirstOrDefault();
                    // в €чейке нет выпадающего списка
                    if (element == null)
                        continue;

                    // пропуск колонки
                    if (element.SelectedValue == ProductFields.None.StrName())
                    {
                        index++;
                        continue;
                    }

                    if (!_fieldMapping.ContainsKey(element.SelectedValue))
                        _fieldMapping.Add(element.SelectedValue, index);
                    else
                    {
                        MsgErr(string.Format(Resource.Admin_ImportCsv_DuplicateMessage, element.SelectedItem.Text));
                        return;
                    }
                    index++;
                }
            }

            if (!btnAction.Visible || !IsPostBack) return;

            foreach (var item in _mustRequiredField.Where(item => !_fieldMapping.ContainsKey(item.StrName())))
            {
                MsgErr(string.Format(Resource.Admin_ImportCsv_NotChoice, item.Localize()));
                return;
            }
        }


        protected void linkCancel_Click(object sender, EventArgs e)
        {
            CommonStatistic.IsRun = false;
            hlDownloadImportLog.Attributes.CssStyle["display"] = "inline";
        }

        protected void btnSaveSettings_Click(object sender, EventArgs e)
        {
            if (!fuCsvFile.HasFile) return;
            //delete old
            foreach (var item in Directory.GetFiles(_filePath).Where(f => f.Contains(StrFileName)))
            {
                FileHelpers.DeleteFile(item);
            }

            fuCsvFile.SaveAs(_fullPath);
            if (!File.Exists(_fullPath)) return;

            SettingsGeneral.CsvEnconing = ddlEncoding.SelectedValue;

            SettingsGeneral.CsvSeparator = ddlSeparators.SelectedValue == SeparatorsEnum.Custom.StrName()
                ? txtCustomSeparator.Text
                : ddlSeparators.SelectedValue;

            SettingsGeneral.CsvColumSeparator = txtColumSeparator.Text;
            SettingsGeneral.CsvPropertySeparator = txtPropertySeparator.Text;

            Response.Redirect("ImportCSV.aspx?action=start&hasheadrs=" + chbHasHeadrs.Checked.ToString().ToLower(), true);
        }

        #region Modules

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

        #endregion
    }
}