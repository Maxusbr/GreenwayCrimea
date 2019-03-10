using System;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.Watermark
{
    public partial class Admin_WatermarkModule : UserControl
    {
        private const string _moduleName = "Watermark";

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            {
                hfWatermarkPositionX.Value = ModuleSettingsProvider.GetSettingValue<decimal>("WatermarkPositionX",
                                                                                             _moduleName).ToString();
                hfWatermarkPositionY.Value = ModuleSettingsProvider.GetSettingValue<decimal>("WatermarkPositionY",
                                                                                             _moduleName).ToString();
            }
            if (
                !string.IsNullOrWhiteSpace(ModuleSettingsProvider.GetSettingValue<string>("WatermarkImage", _moduleName)))
            {
                pnlWatermarkImage.Visible = true;
                imgWatermark.ImageUrl = "../" + _moduleName + "/" +
                                        ModuleSettingsProvider.GetSettingValue<string>("WatermarkImage", _moduleName);
            }
            else
            {
                pnlWatermarkImage.Visible = false;
            }
        }

        protected void Save()
        {
            ModuleSettingsProvider.SetSettingValue("WatermarkPositionX", Convert.ToDecimal(hfWatermarkPositionX.Value),
                                                   _moduleName);
            ModuleSettingsProvider.SetSettingValue("WatermarkPositionY", Convert.ToDecimal(hfWatermarkPositionY.Value),
                                                   _moduleName);

            if (fuWatermarkImage.HasFile)
            {
                if (
                    File.Exists(
                        HttpContext.Current.Server.MapPath("~\\Modules\\" + _moduleName + "\\" +
                                                           ModuleSettingsProvider.GetSettingValue<string>(
                                                               "WatermarkImage", _moduleName))))
                {
                    File.Delete(
                        HttpContext.Current.Server.MapPath("~\\Modules\\" + _moduleName + "\\" +
                                                           ModuleSettingsProvider.GetSettingValue<string>(
                                                               "WatermarkImage", _moduleName)));
                }

                ModuleSettingsProvider.SetSettingValue("WatermarkImage", fuWatermarkImage.FileName, _moduleName);
                fuWatermarkImage.SaveAs(
                    HttpContext.Current.Server.MapPath("~\\Modules\\" + _moduleName + "\\" + fuWatermarkImage.FileName));
            }

            lblMessage.Text = (string) GetLocalResourceObject("Watermark_ChangesSaved");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        protected void btnDeleteWatermark_Click(object sender, EventArgs e)
        {
            if (
                File.Exists(
                    HttpContext.Current.Server.MapPath("~\\Modules\\" + _moduleName + "\\" +
                                                       ModuleSettingsProvider.GetSettingValue<string>("WatermarkImage",
                                                                                                      _moduleName))))
            {
                File.Delete(
                    HttpContext.Current.Server.MapPath("~\\Modules\\" + _moduleName + "\\" +
                                                       ModuleSettingsProvider.GetSettingValue<string>("WatermarkImage",
                                                                                                      _moduleName)));
            }
            ModuleSettingsProvider.RemoveSqlSetting("WatermarkImage", _moduleName);
            pnlWatermarkImage.Visible = false;
        }
    }
}