using System;
using System.Web.UI;
using AdvantShop.Core.Modules;
using AdvantShop.Module.FindCheaper;

namespace Advantshop.Modules.UserControls.FindCheaper
{
    public partial class Admin_FindCheaperSettings : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            txtTitle.Text = ModuleSettingsProvider.GetSettingValue<string>("Title", FindCheaperModule.ModuleStringId);
            txtTopText.Text = ModuleSettingsProvider.GetSettingValue<string>("TopText", FindCheaperModule.ModuleStringId);
            txtFinalText.Text = ModuleSettingsProvider.GetSettingValue<string>("FinalText", FindCheaperModule.ModuleStringId);
            txtEmailTo.Text = ModuleSettingsProvider.GetSettingValue<string>("EmailTo", FindCheaperModule.ModuleStringId);
        }

        protected void Save()
        {
            //if (!Validate())
            //{
            //    lblMessage.Text = (string)GetLocalResourceObject("WrongData");
            //    lblMessage.ForeColor = Color.Red;
            //    lblMessage.Visible = true;
            //    return;
            //}

            ModuleSettingsProvider.SetSettingValue("Title", txtTitle.Text, FindCheaperModule.ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("TopText", txtTopText.Text, FindCheaperModule.ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("FinalText", txtFinalText.Text, FindCheaperModule.ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("EmailTo", txtEmailTo.Text, FindCheaperModule.ModuleStringId);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }


    }
}