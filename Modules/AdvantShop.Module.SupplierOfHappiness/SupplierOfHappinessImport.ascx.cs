//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Data;
using System.IO;
using System.Net;
using System.Web;
using System.Xml;
using AdvantShop.Core.Modules;
using AdvantShop.Module.SupplierOfHappiness.Domain;
//todo переделать без завязывания на движке
using AdvantShop.Saas;
using AdvantShop.Statistic;

namespace Advantshop.Module.SupplierOfHappiness
{
    public partial class SupplierOfHappinessImport : System.Web.UI.UserControl
    {
        private readonly string _directory;
        private readonly string _fullImportPath;
        private readonly string _quickImportPath;

        public SupplierOfHappinessImport()
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~\\Modules\\" + AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID + "\\temp\\")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~\\Modules\\" + AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID + "\\temp\\"));
            }

            _directory = HttpContext.Current.Server.MapPath("~\\Modules\\" + AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID + "\\temp\\");
            _fullImportPath = _directory + "products.csv";
            _quickImportPath = _directory + "remains.csv";
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

        protected void btnUpdateImg_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CommonStatistic.IsRun)
                {
                    CommonStatistic.Init();
                    CommonStatistic.CurrentProcess = Request.Url.PathAndQuery;
                    linkCancel.Visible = true;
                    MsgErr(true);

                    lblRes.Text = string.Empty;

                    pUploadExcel.Visible = false;

                    if (File.Exists(_fullImportPath))
                    {
                        File.Delete(_fullImportPath);
                    }

                    (new WebClient()).DownloadFile(AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.UrlPathFull + "?param=" + Guid.NewGuid(), _fullImportPath);

                    CommonStatistic.IsRun = true;
                    SupplierOfHappinessProcessFile.UpdateIsProcessing = true;
                    CommonStatistic.StartNew(() =>
                    {
                        SupplierOfHappinessProcessFile.ProcessUpdateImg(_fullImportPath);
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
                var log = new SupplierOfHappinessLog();
                log.Write(DateTime.Now.ToString("[dd.MM.yy HH:mm]") + " Ошибка " + xmlEx.Message);
            }
            catch (Exception ex)
            {
                LogInvalidData(ex.Message);
                CommonStatistic.IsRun = false;
                hlDownloadImportLog.Attributes.CssStyle["display"] = "inline";

                var log = new SupplierOfHappinessLog();
                log.Write(DateTime.Now.ToString("[dd.MM.yy HH:mm]") + " Ошибка " + ex.Message);
            }
        }

        protected void btnStartFullImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CommonStatistic.IsRun)
                {
                    CommonStatistic.Init();
                    CommonStatistic.CurrentProcess = Request.Url.PathAndQuery;
                    linkCancel.Visible = true;
                    MsgErr(true);

                    lblRes.Text = string.Empty;

                    pUploadExcel.Visible = false;

                    if (File.Exists(_fullImportPath))
                    {
                        File.Delete(_fullImportPath);
                    }

                    (new WebClient()).DownloadFile(AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.UrlPathFull + "?param=" + Guid.NewGuid(), _fullImportPath);

                    CommonStatistic.IsRun = true;
                    SupplierOfHappinessProcessFile.UpdateIsProcessing = true;
                    CommonStatistic.StartNew(() =>
                    {
                        SupplierOfHappinessProcessFile.ProcessFile(_fullImportPath);
                        SupplierOfHappinessProcessFile.UpdateIsProcessing = false;
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
                var log = new SupplierOfHappinessLog();
                log.Write(DateTime.Now.ToString("[dd.MM.yy HH:mm]") + " Ошибка " + xmlEx.Message);
            }
            catch (Exception ex)
            {
                LogInvalidData(ex.Message);
                CommonStatistic.IsRun = false;
                hlDownloadImportLog.Attributes.CssStyle["display"] = "inline";

                var log = new SupplierOfHappinessLog();
                log.Write(DateTime.Now.ToString("[dd.MM.yy HH:mm]") + " Ошибка " + ex.Message);
            }
        }

        protected void btnStartQuickImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CommonStatistic.IsRun)
                {
                    CommonStatistic.Init();
                    CommonStatistic.CurrentProcess = Request.Url.PathAndQuery;
                    linkCancel.Visible = true;
                    MsgErr(true);

                    lblRes.Text = string.Empty;

                    pUploadExcel.Visible = false;

                    if (File.Exists(_quickImportPath))
                    {
                        File.Delete(_quickImportPath);
                    }

                    (new WebClient()).DownloadFile(AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.UrlPathQuick + "?param=" + Guid.NewGuid(), _quickImportPath);

                    //using (var client = new WebClient())
                    //using (var stream = client.OpenRead("http://stripmag.ru/datafeed/insales_quick.csv?"))
                    //using (var file = File.Create(_quickImportPath))
                    //{
                    //    var buffer = new byte[4096];
                    //    int bytesReceived;
                    //    while ((bytesReceived = stream.Read(buffer, 0, buffer.Length)) != 0)
                    //    {
                    //        file.Write(buffer, 0, bytesReceived);
                    //    }
                    //}

                    CommonStatistic.IsRun = true;

                    SupplierOfHappinessProcessFile.UpdateIsProcessing = true;
                    CommonStatistic.StartNew(() =>
                    {
                        SupplierOfHappinessProcessFile.ProcessFileQuick(_quickImportPath);
                        SupplierOfHappinessProcessFile.UpdateIsProcessing = false;
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
                lvLogs.DataSource = SupplierOfHappinessLog.GetLogFiles();
                lvLogs.DataBind();
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