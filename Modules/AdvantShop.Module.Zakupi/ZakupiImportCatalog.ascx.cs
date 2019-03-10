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
using AdvantShop.Module.ZakupiImport.Domain;
using AdvantShop.Saas;
using AdvantShop.Statistic;

namespace AdvantShop.Module.ZakupiImport
{
    public partial class ZakupiImportCatalog : System.Web.UI.UserControl
    {

        private readonly string _filePath;
        private readonly string _fullImportPath;
        private readonly string _partialImportPath;

        public ZakupiImportCatalog()
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~\\Modules\\" + ZakupiImport.ModuleID + "\\temp\\")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~\\Modules\\" + ZakupiImport.ModuleID + "\\temp\\"));
            }

            _filePath = HttpContext.Current.Server.MapPath("~\\Modules\\" + ZakupiImport.ModuleID + "\\temp\\");
            _fullImportPath = _filePath + "zakupiImport.xml";
            _partialImportPath = _filePath + "zakupiPartialImport.xml";
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

                        if (File.Exists(_fullImportPath))
                        {
                            File.Delete(_fullImportPath);
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
                        FileUpload1.SaveAs(_fullImportPath);
                    }
                    else if (!string.IsNullOrEmpty(txtFileUrlPath.Text))
                    {
                        new WebClient().DownloadFile(ZakupiImport.FileUrlPath, _fullImportPath);
                    }

                    lblCategoryRows.Text = ZakupiImportProcessFile.GetCategoryRowsCount(_fullImportPath).ToString();
                    lblOfferRows.Text = ZakupiImportProcessFile.GetOfferRowsCount(_fullImportPath).ToString();

                    CommonStatistic.StartNew(() => ZakupiImportProcessFile.ProcessYml(_fullImportPath));

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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ZakupiImport.FileUrlPath = txtFileUrlPath.Text;
            ZakupiImport.UpdateName = ckbUpdateName.Checked;
            ZakupiImport.UpdateDescription = ckbUpdateDescription.Checked;
            ZakupiImport.UpdateParams = ckbUpdateParams.Checked;
            ZakupiImport.UpdatePhotos = ckbUpdatePhotos.Checked;

            lblMessage.Text = @"Изменения сохранены";
            lblMessage.ForeColor = System.Drawing.Color.Blue;
            lblMessage.Visible = true;
        }

        protected void btnPartialImport_Click(object sender, EventArgs e)
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

                        if (File.Exists(_partialImportPath))
                        {
                            File.Delete(_partialImportPath);
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
                        FileUpload1.SaveAs(_partialImportPath);
                    }
                    else if (!string.IsNullOrEmpty(txtFileUrlPath.Text))
                    {
                        new WebClient().DownloadFile(ZakupiImport.FileUrlPath + (ZakupiImport.FileUrlPath.Contains("?") ? "&" : "?") + "content=off", _partialImportPath);
                    }

                    //lblCategoryRows.Text = ZakupiImportProcessFile.GetCategoryRowsCount(_partialImportPath).ToString();
                    lblOfferRows.Text = ZakupiImportProcessFile.GetOfferRowsCount(_partialImportPath).ToString();

                    CommonStatistic.StartNew(() => ZakupiImportProcessFile.ProcessPartialYml(_partialImportPath));

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

            txtFileUrlPath.Text = ModuleSettingsProvider.GetSettingValue<string>("FileUrlPath",
                ZakupiImport.ModuleID);

            txtFileUrlPath.Text = ZakupiImport.FileUrlPath;
            ckbUpdateName.Checked = ZakupiImport.UpdateName;
            ckbUpdateDescription.Checked = ZakupiImport.UpdateDescription;
            ckbUpdateParams.Checked = ZakupiImport.UpdateParams;
            ckbUpdatePhotos.Checked = ZakupiImport.UpdatePhotos;

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