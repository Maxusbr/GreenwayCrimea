using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.SQL;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Helpers;
using NExcel;
using Color = System.Drawing.Color;

namespace AdvantShop.Module.MoySklad
{
    public partial class Modules_MoySklad_MoySkladImportExcel : UserControl
    {
        private static readonly string _fileName = "prodms.xls";

        private readonly string _filePath = ModuleSettingsProvider.GetAbsolutePath() + @"\App_Data\filesmysklad\" +
                                            _fileName;

        protected void Page_Load(object sender, EventArgs e)
        {
            hlDownloadImportLog.NavigateUrl = "~/" + MoySklad.ImportStatisticMoySkladExcel.VirtualFileLogPath;
            pUpload.Visible = (string.IsNullOrEmpty(Request["action"])) || !File.Exists(_filePath) ||
                              !MoySklad.ImportStatisticMoySkladExcel.IsRun;
            pnlSelectCol.Visible = (Request["action"] == "selectcol") && File.Exists(_filePath) &&
                                   !MoySklad.ImportStatisticMoySkladExcel.IsRun;
            pnlProcessing.Visible = MoySklad.ImportStatisticMoySkladExcel.IsRun;
            //OutDiv.Visible = MoySklad.ImportStatisticMoySkladExcel.IsRun;
            pnlLog.Visible = File.Exists(MoySklad.ImportStatisticMoySkladExcel.FileLog);
            if (MoySklad.ImportStatisticMoySkladExcel.IsRun)
                pnlLog.Style.Add("display", "none");

            if (IsPostBack)
                return;

            if (pnlSelectCol.Visible)
            {
                if (File.Exists(_filePath))
                {
                    Workbook book = Workbook.getWorkbook(_filePath);
                    Sheet worksheet = book.getSheet(0);
                    int i = 0;
                    foreach (Cell cell in worksheet.getRow(0))
                    {
                        cblColumnst.Items.Add(new ListItem(Convert.ToString(cell.Value), i.ToString()));
                        ddlKey.Items.Add(new ListItem(Convert.ToString(cell.Value), i.ToString()));
                        i++;
                    }
                    if (ddlKey.Items.FindByText("Внешний код") != null)
                        ddlKey.SelectedValue = ddlKey.Items.FindByText("Внешний код").Value;
                }
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (FileUpload.HasFile &&
                VirtualPathUtility.GetExtension(FileUpload.FileName)
                                  .Equals(".xls", StringComparison.CurrentCultureIgnoreCase))
            {
                if (File.Exists(_filePath))
                    File.Delete(_filePath);

                FileUpload.SaveAs(_filePath);

                bool validFile = false;
                try
                {
                    Workbook book = Workbook.getWorkbook(_filePath);
                    validFile = true;
                }
                catch (Exception)
                {
                    validFile = false;
                }

                if (validFile)
                {
                    Response.Redirect(string.Format("{0}{1}", Request.Url.AbsolutePath,
                                                    QueryHelper.ChangeQueryParam(Request.Url.Query, "action",
                                                                                 "selectcol")));
                }
                else
                {
                    lblErr.Text = (string) GetLocalResourceObject("MoySkladValid_SelectExcelFile");
                    lblErr.ForeColor = Color.Red;
                    lblErr.Visible = true;
                }
            }
            else
            {
                lblErr.Text = (string) GetLocalResourceObject("MoySkladValid_SelectFile");
                lblErr.ForeColor = Color.Red;
                lblErr.Visible = true;
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                if (!MoySklad.ImportStatisticMoySkladExcel.IsRun)
                {
                    if (!string.IsNullOrEmpty(ddlKey.SelectedValue) && !string.IsNullOrEmpty(cblColumnst.SelectedValue))
                    {
                        var processData = new ProcessData {NumbersLoadColumns = new List<int>()};

                        processData.NumberKey = int.Parse(ddlKey.SelectedValue);

                        foreach (ListItem item in cblColumnst.Items)
                            if (item.Selected)
                                processData.NumbersLoadColumns.Add(int.Parse(item.Value));

                        processData.DeletePropertyNotFile = cbDeletePropertyNotFile.Checked;

                        MoySklad.ImportStatisticMoySkladExcel.Init();
                        MoySklad.ImportStatisticMoySkladExcel.IsRun = true;
                        linkCancel.Visible = true;
                        lblRes.Text = string.Empty;

                        var tr = new Thread(ProcessExcel);
                        MoySklad.ImportStatisticMoySkladExcel.ThreadImport = tr;
                        tr.Start(processData);

                        Response.Redirect(string.Format("{0}{1}", Request.Url.AbsolutePath,
                                                        QueryHelper.ChangeQueryParam(Request.Url.Query, "action",
                                                                                     "loading")));

                        OutDiv.Visible = true;
                    }
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                MoySklad.ImportStatisticMoySkladExcel.WriteLog(ex.Message);
            }
        }

        private void ProcessExcel(object data)
        {
            try
            {
                var processData = (ProcessData) data;
                Workbook book = Workbook.getWorkbook(_filePath);
                Sheet worksheet = book.getSheet(0);
                var listPropertyNames = new Dictionary<int, string>();
                string[] listNoloadProp =
                    ModuleSettingsProvider.GetSettingValue<string>("MoySkladPropNoLoad", MoySklad.GetModuleStringId())
                                          .Split(new[] {"[;]"}, StringSplitOptions.RemoveEmptyEntries);
                var propWeight = ModuleSettingsProvider.GetSettingValue<string>("MoySkladNamePropWeight",
                                                                                MoySklad.GetModuleStringId());
                var propSize = ModuleSettingsProvider.GetSettingValue<string>("MoySkladNamePropSize",
                                                                              MoySklad.GetModuleStringId());
                var propBrand = ModuleSettingsProvider.GetSettingValue<string>("MoySkladNamePropBrand",
                                                                               MoySklad.GetModuleStringId());
                var propDiscount = ModuleSettingsProvider.GetSettingValue<string>("MoySkladNamePropDiscount",
                                                                                  MoySklad.GetModuleStringId());

                MoySklad.ImportStatisticMoySkladExcel.TotalRow = worksheet.getColumn(processData.NumberKey).Length - 1;

                // Step by rows
                foreach (int numbProp in processData.NumbersLoadColumns)
                {
                    listPropertyNames.Add(numbProp, Convert.ToString(worksheet.getCell(numbProp, 0).Value));
                }

                for (int i = 1; i <= worksheet.getColumn(processData.NumberKey).Length - 1; i++)
                {
                    if (!MoySklad.ImportStatisticMoySkladExcel.IsRun) return;

                    string moyskladId = Convert.ToString(worksheet.getCell(processData.NumberKey, i).Value);
                    if (!string.IsNullOrEmpty(moyskladId))
                    {
                        int productId = MoySklad.GetProductIdByMoyskladId(moyskladId);

                        //if (productId <= 0)
                        //    productId = ProductService.GetProductId(MoySklad.TrimArtNo(moyskladId));

                        if (productId > 0)
                        {
                            if (processData.DeletePropertyNotFile)
                                PropertyService.DeleteProductProperties(productId);

                            Product product = null;

                            foreach (var prop in listPropertyNames)
                            {
                                bool novalidVal = false;
                                string value = Convert.ToString(worksheet.getCell(prop.Key, i).Value);

                                if (
                                    !listNoloadProp.Any(
                                        p => p.Equals(prop.Value, StringComparison.InvariantCultureIgnoreCase)))
                                {
                                    if (propWeight.Equals(prop.Value, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        if (!string.IsNullOrWhiteSpace(value) && value.IsDecimal())
                                        {
                                            float weight = value.TryParseFloat();

                                            if (weight >= 0F)
                                            {
                                                if (product == null) product = ProductService.GetProduct(productId);
                                                product.Weight = weight;
                                            }
                                            else
                                                novalidVal = true;
                                        }
                                        else
                                            novalidVal = true;
                                    }
                                    else if (propSize.Equals(prop.Value, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        if (!string.IsNullOrWhiteSpace(value))
                                        {
                                            string[] vals = value.Split(new[] {"x"},
                                                                        StringSplitOptions.RemoveEmptyEntries);
                                            if (vals.Length == 3 && vals.Count(v => v.IsInt()) == 3)
                                            {
                                                if (product == null) product = ProductService.GetProduct(productId);

                                                product.Length = Convert.ToSingle(vals[0]);
                                                product.Width = Convert.ToSingle(vals[1]);
                                                product.Height = Convert.ToSingle(vals[2]);
                                            }
                                            else
                                                novalidVal = true;
                                        }
                                        else
                                            novalidVal = true;
                                    }
                                    else if (propBrand.Equals(prop.Value, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        if (!string.IsNullOrWhiteSpace(value))
                                        {
                                            if (BrandService.GetBrandIdByName(value) == 0)
                                            {
                                                var tempBrand = new Brand
                                                    {
                                                        Enabled = true,
                                                        Name = value,
                                                        Description = value,
                                                        UrlPath =
                                                            UrlService.GetAvailableValidUrl(0, ParamType.Brand, value),
                                                        Meta = null
                                                    };

                                                if (product == null) product = ProductService.GetProduct(productId);
                                                product.BrandId = BrandService.AddBrand(tempBrand);
                                            }
                                            else
                                            {
                                                if (product == null) product = ProductService.GetProduct(productId);
                                                product.BrandId = BrandService.GetBrandIdByName(value);
                                            }
                                        }
                                        else
                                        {
                                            if (product == null) product = ProductService.GetProduct(productId);
                                            product.BrandId = 0;
                                        }
                                    }
                                    else if (propDiscount.Equals(prop.Value, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        if (!string.IsNullOrWhiteSpace(value) && value.IsDecimal())
                                        {
                                            float discount = value.TryParseFloat();

                                            if (discount >= 0F && discount <= 100F)
                                            {
                                                if (product == null) product = ProductService.GetProduct(productId);
                                                product.Discount = new Discount(discount, 0);
                                            }
                                            else
                                                novalidVal = true;
                                        }
                                        else
                                            novalidVal = true;
                                    }
                                    else if (!string.IsNullOrWhiteSpace(value))
                                    {
                                        SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_ParseProductProperty]",
                                                                      CommandType.StoredProcedure,
                                                                      new SqlParameter("@nameProperty", prop.Value),
                                                                      new SqlParameter("@propertyValue", value),
                                                                      new SqlParameter("@rangeValue", value.TryParseFloat()),
                                                                      new SqlParameter("@productId", productId));
                                    }
                                    else
                                        novalidVal = true;

                                    if (novalidVal)
                                    {
                                        MoySklad.ImportStatisticMoySkladExcel.WriteLog(
                                            string.Format("Invalid value ({0}) at cell [{1}; {2}] ([column, row])",
                                                          value, prop.Key + 1, i + 1));
                                        MoySklad.ImportStatisticMoySkladExcel.TotalErrorRow++;
                                    }
                                }
                            }

                            if (product != null)
                            {
                                product.ModifiedBy = "moysklad";
                                ProductService.UpdateProduct(product, false);
                            }
                        }
                        else
                        {
                            MoySklad.ImportStatisticMoySkladExcel.WriteLog(
                                string.Format(
                                    "Not found productId by moyskladId ({0}) at cell [{1}; {2}] ([column, row])",
                                    moyskladId, processData.NumberKey + 1, i + 1));
                            MoySklad.ImportStatisticMoySkladExcel.TotalErrorRow++;
                        }
                    }
                    else
                    {
                        LogInvalidData(moyskladId, processData.NumberKey, i);
                    }
                    MoySklad.ImportStatisticMoySkladExcel.RowPosition++;
                }
                book.close();
                if (File.Exists(_filePath))
                    File.Delete(_filePath);
            }
            catch (Exception ex)
            {
                MoySklad.ImportStatisticMoySkladExcel.TotalErrorRow++;
                MoySklad.ImportStatisticMoySkladExcel.WriteLog(ex.Message);
            }
            MoySklad.ImportStatisticMoySkladExcel.IsRun = false;
        }

        private static void LogInvalidData(string value, int column, int row)
        {
            MoySklad.ImportStatisticMoySkladExcel.WriteLog(
                string.Format("Invalid value ({0}) at cell [{1}; {2}] ([column, row])", value, column + 1, row + 1));
            MoySklad.ImportStatisticMoySkladExcel.TotalErrorRow++;
        }

        protected void linkCancel_Click(object sender, EventArgs e)
        {
            if (MoySklad.ImportStatisticMoySkladExcel.ThreadImport.IsAlive)
            {
                //ImportStatistic.ThreadImport.Abort();
                MoySklad.ImportStatisticMoySkladExcel.IsRun = false;
                //MoySklad.ImportStatisticMoySkladExcel.Init();
                //hlDownloadImportLog.Attributes.CssStyle["display"] = "inline";
                pnlLog.Visible = true;
            }
        }

        protected class ProcessData
        {
            public int NumberKey { set; get; }
            public List<int> NumbersLoadColumns { set; get; }
            public bool DeletePropertyNotFile { get; set; }
        }
    }
}