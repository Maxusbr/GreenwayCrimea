//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------


//todo переделать без завязывания на движке
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Web;
using System.Xml;
using AdvantShop.Core.Modules;
using AdvantShop.Diagnostics;
using AdvantShop.Module.YandexMarketImport.Domain;
using AdvantShop.Saas;
using AdvantShop.Statistic;

namespace AdvantShop.Module.YandexMarketImport
{
    public partial class YandexMarketImportCatalog : System.Web.UI.UserControl
    {
        private readonly string _filePath;
        private readonly string _fullPath;

        public YandexMarketImportCatalog()
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~\\Modules\\" + YandexMarketImport.ModuleID + "\\temp\\")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~\\Modules\\" + YandexMarketImport.ModuleID + "\\temp\\"));
            }

            _filePath = HttpContext.Current.Server.MapPath("~\\Modules\\" + YandexMarketImport.ModuleID + "\\temp\\");
            _fullPath = _filePath + "products.yml";
        }

        private void MsgErr(bool clean)
        {
            if (clean)
            {
                lblError.Visible = false;
                lblError.Text = "";
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

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (!FileUpload1.HasFile && string.IsNullOrEmpty(txtFileUrlPath.Text))
                {
                    MsgErr((string)GetLocalResourceObject("YandexMarketImport_ChooseFile"));
                }

                if (!CommonStatistic.IsRun)
                {
                    CommonStatistic.Init();
                    CommonStatistic.CurrentProcess = Request.Url.PathAndQuery;
                    linkCancel.Visible = true;
                    MsgErr(true);

                    lblRes.Text = string.Empty;

                    var boolSuccess = true;

                    try
                    {
                        if (Directory.Exists(_filePath) == false)
                        {
                            Directory.CreateDirectory(_filePath);
                        }

                        if (File.Exists(_fullPath))
                        {
                            File.Delete(_fullPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgErr(ex.Message + " at Files");
                        boolSuccess = false;
                    }

                    if (boolSuccess == false)
                    {
                        return;
                    }
                    pUploadExcel.Visible = false;
                    // Save file
                    if (FileUpload1.HasFile)
                    {
                        FileUpload1.SaveAs(_fullPath);
                    }
                    else if (!string.IsNullOrEmpty(txtFileUrlPath.Text))
                    {
                        new WebClient().DownloadFile(txtFileUrlPath.Text, _fullPath);
                    }

                    if (SaasDataService.IsSaasEnabled)
                    {
                        divSaasPlanProducts.Visible = true;
                        hfProductsCount.Value = AdvantShop.Catalog.ProductService.GetProductsCount().ToString();
                        lTotalSaasPlanProducts.Text = SaasDataService.CurrentSaasData.ProductsCount.ToString();
                    }

                    //lblCategoryRows.Text = YandexMarketImportProcessFile.GetCategoryRowsCount(_fullPath).ToString();
                    //lblOfferRows.Text = (YandexMarketImportProcessFile.GetOfferRowsCount(_fullPath) + YandexMarketImportProcessFile.GetCategoryRowsCount(_fullPath)).ToString();
                    lblOfferRows.Text = YandexMarketImportProcessFile.GetOfferRowsCount(_fullPath).ToString();
                    CommonStatistic.StartNew(() =>
                    {
                        try
                        {
                            YandexMarketImportProcessFile.ProcessYml(_fullPath);
                        }
                        catch (Exception ex)
                        {
                            Debug.Log.Error(ex);
                        }
                    });
                    pUploadExcel.Visible = false;

                    OutDiv.Visible = true;
                }
            }
            catch (XmlException xmlEx)
            {
                LogInvalidData(xmlEx.Message);
                CommonStatistic.IsRun = false;
                hlDownloadImportLog.Attributes.CssStyle["display"] = "inline";
            }
            catch (Exception ex)
            {
                LogInvalidData(ex.Message);
                CommonStatistic.IsRun = false;
                hlDownloadImportLog.Attributes.CssStyle["display"] = "inline";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OutDiv.Visible = CommonStatistic.IsRun;
                linkCancel.Visible = CommonStatistic.IsRun;
                ModulesRepository.ModuleExecuteNonQuery("DELETE from [Catalog].[ImportLog]", CommandType.Text);
            }
        }

        protected void linkCancel_Click(object sender, EventArgs e)
        {
            CommonStatistic.IsRun = false;
            hlDownloadImportLog.Attributes.CssStyle["display"] = "inline";
        }

        private static void LogInvalidData(string message)
        {
            CommonStatistic.WriteLog(message);
            CommonStatistic.TotalErrorRow++;
        }
    }
}