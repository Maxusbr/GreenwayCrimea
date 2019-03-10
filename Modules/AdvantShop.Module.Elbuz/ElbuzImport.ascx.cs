//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using AdvantShop.Core.Modules;
using AdvantShop.Diagnostics;
using AdvantShop.Module.Elbuz.Domain;
using AdvantShop.Saas;
using AdvantShop.Statistic;

//todo переделать без завязывания на движке

namespace Advantshop.Modules.UserControls
{
    public partial class Admin_ElbuzImportModule : UserControl
    {
        private const string _moduleName = "Elbuz";

        private readonly string _filePath;
        private readonly string _fullPath;

        public Admin_ElbuzImportModule()
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~\\Modules\\" + _moduleName + "\\temp\\")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~\\Modules\\" + _moduleName + "\\temp\\"));
            }
            _filePath = HttpContext.Current.Server.MapPath("~\\Modules\\" + _moduleName + "\\temp\\");
            _fullPath = _filePath + "products.csv";
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
                if (!FileUpload1.HasFile)
                {
                    MsgErr((string) GetLocalResourceObject("ElbuzImport_ChooseFile"));
                    return;
                }

                if (!CommonStatistic.IsRun)
                {
                    CommonStatistic.Init();
                    CommonStatistic.IsRun = true;
                    CommonStatistic.CurrentProcess = Request.Url.PathAndQuery;
                    linkCancel.Visible = true;
                    MsgErr(true);

                    lblRes.Text = "";

                    bool boolSuccess = true;

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

                    FileUpload1.SaveAs(_fullPath);

                    CommonStatistic.StartNew(() => new ElbuzProcessingFiles().ProcessData(_fullPath, ckbDisableProducts.Checked, ckbDisableCategories.Checked,
                        string.Equals(ddlTypeArtNo.SelectedItem.Value, "ArtNo")));
                    //CommonStatistic.ThreadImport = new Thread(ElbuzProcessingFiles.ProcessData) { IsBackground = true };
                    //CommonStatistic.ThreadImport.Start(_fullPath);

                    pUploadExcel.Visible = false;

                    OutDiv.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
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
    }
}