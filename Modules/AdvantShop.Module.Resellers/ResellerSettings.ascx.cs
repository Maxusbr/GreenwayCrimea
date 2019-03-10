//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Globalization;
using System.Xml;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Module.Resellers.Domain;
using AdvantShop.Statistic;
using AdvantShop.Saas;
using AdvantShop.Diagnostics;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;

namespace AdvantShop.Module.Resellers
{
    public partial class ResellerSettings : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            if (!IsPostBack)
            {
                OutDiv.Visible = CommonStatistic.IsRun;
                linkCancel.Visible = CommonStatistic.IsRun;
                ModulesRepository.ModuleExecuteNonQuery("DELETE from [Catalog].[ImportLog]", CommandType.Text);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {

            txtSupplierSite.Text = ModuleSettingsProvider.GetSettingValue<string>("SupplierSite", Reseller.ModuleID);
            txtResellerID.Text = ModuleSettingsProvider.GetSettingValue<string>("ResellerID", Reseller.ModuleID);
            txtMargin.Text = ModuleSettingsProvider.GetSettingValue<float>("Margin", Reseller.ModuleID).ToString();
            ckbAmountNulling.Checked = ModuleSettingsProvider.GetSettingValue<bool>("AmountNulling", Reseller.ModuleID);
            ckbDeactivateProducts.Checked = ModuleSettingsProvider.GetSettingValue<bool>("DeactivateProducts", Reseller.ModuleID);

            ckbAutoUpdate.Checked = ModuleSettingsProvider.GetSettingValue<bool>("AutoUpdate", Reseller.ModuleID);

            lblLastUpdate.Text =
                ModuleSettingsProvider.GetSettingValue<string>("LastUpdate", Reseller.ModuleID).IsNullOrEmpty()
                    ? @"Не обновлялось"
                    : ModuleSettingsProvider.GetSettingValue<string>("LastUpdate", Reseller.ModuleID);
        }

        protected void Save()
        {
            var site = txtSupplierSite.Text;
            site = site.StartsWith("http://") || site.StartsWith("https://") ? site : "http://" + site;

            ModuleSettingsProvider.SetSettingValue("SupplierSite", site, Reseller.ModuleID);
            ModuleSettingsProvider.SetSettingValue("ResellerID", txtResellerID.Text, Reseller.ModuleID);
            ModuleSettingsProvider.SetSettingValue("Margin", txtMargin.Text.TryParseFloat(), Reseller.ModuleID);
            ModuleSettingsProvider.SetSettingValue("AmountNulling", ckbAmountNulling.Checked, Reseller.ModuleID);
            ModuleSettingsProvider.SetSettingValue("DeactivateProducts", ckbDeactivateProducts.Checked, Reseller.ModuleID);

            ModuleSettingsProvider.SetSettingValue("AutoUpdate", ckbAutoUpdate.Checked, Reseller.ModuleID);

            lblMessage.Text = "Изменения сохранены";
            lblMessage.ForeColor = System.Drawing.Color.Blue;
            lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        protected void OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!CommonStatistic.IsRun)
                {
                    CommonStatistic.Init();
                    CommonStatistic.CurrentProcess = Request.Url.PathAndQuery;
                    linkCancel.Visible = true;
                    lblRes.Text = string.Empty;

                    pUploadExcel.Visible = false;

                    if (SaasDataService.IsSaasEnabled)
                    {
                        divSaasPlanProducts.Visible = true;
                        hfProductsCount.Value = AdvantShop.Catalog.ProductService.GetProductsCount().ToString();
                        lTotalSaasPlanProducts.Text = SaasDataService.CurrentSaasData.ProductsCount.ToString();
                    }

                    Task.Factory.StartNew(() =>
                    {
                        ProcessFile.Import(true);
                    }, TaskCreationOptions.LongRunning);

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
        private static void LogInvalidData(string message)
        {
            CommonStatistic.WriteLog(message);
            CommonStatistic.TotalErrorRow++;
        }

        protected void linkCancel_Click(object sender, EventArgs e)
        {
            CommonStatistic.IsRun = false;
            lblRes.Attributes.CssStyle.Value = lblRes.Attributes.CssStyle.Value.Replace("none", "inline");
            hlDownloadImportLog.Attributes.CssStyle.Value = hlDownloadImportLog.Attributes.CssStyle.Value.Replace("none", "inline");
            linkCancel.Visible = false;
            lblRes.Visible = true;
            if (CommonStatistic.TotalErrorRow > 0)
            {
                lblRes.Text = "Загрузка каталога завершена с ошибками";
                lblRes.ForeColor = Color.Red;
            }
            else
            {
                lblRes.Text = "Загрузка каталога завершена";
            }
        }

    }
}