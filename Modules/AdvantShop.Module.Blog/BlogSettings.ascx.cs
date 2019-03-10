using System;
using System.Drawing;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.Blog
{
    public partial class BlogSettings : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            txtPageTitle.Text = ModuleSettingsProvider.GetSettingValue<string>("PageTitle", Blog.ModuleID);
            txtMetaTitle.Text = ModuleSettingsProvider.GetSettingValue<string>("MetaTitle", Blog.ModuleID);
            txtMetaKeywords.Text = ModuleSettingsProvider.GetSettingValue<string>("MetaKeywords", Blog.ModuleID);
            txtMetaDescription.Text = ModuleSettingsProvider.GetSettingValue<string>("MetaDescription", Blog.ModuleID);

            txtCategoriesTitle.Text = ModuleSettingsProvider.GetSettingValue<string>("CategoriesListTitle", Blog.ModuleID);

            txtMaxWidth.Text = ModuleSettingsProvider.GetSettingValue<string>("MaxImageWidth", Blog.ModuleID);
            txtMaxHeight.Text = ModuleSettingsProvider.GetSettingValue<string>("MaxImageHeight", Blog.ModuleID);

            ckbShowAddDate.Checked = ModuleSettingsProvider.GetSettingValue<bool>("ShowAddDate", Blog.ModuleID);

            txtStaticBlock.Text = ModuleSettingsProvider.GetSettingValue<string>("StaticBlock", Blog.ModuleID);
            ckeSbRight.Text = ModuleSettingsProvider.GetSettingValue<string>("SbBlogRight", Blog.ModuleID);
            ckbShowRssBlog.Checked = ModuleSettingsProvider.GetSettingValue<bool>("ShowRssBlog", Blog.ModuleID);

            lnkGoToModule.NavigateUrl = "~/blog";
        }

        protected void Save()
        {

            if (!Validate())
            {
                lblMessage.Text = (string)GetLocalResourceObject("WrongData");
                lblMessage.ForeColor = Color.Red;
                lblMessage.Visible = true;
                return;
            }

            ModuleSettingsProvider.SetSettingValue("ShowRssBlog", ckbShowRssBlog.Checked, Blog.ModuleID);
            ModuleSettingsProvider.SetSettingValue("PageTitle", txtPageTitle.Text, Blog.ModuleID);
            ModuleSettingsProvider.SetSettingValue("MetaTitle", txtMetaTitle.Text, Blog.ModuleID);
            ModuleSettingsProvider.SetSettingValue("MetaKeywords", txtMetaKeywords.Text, Blog.ModuleID);
            ModuleSettingsProvider.SetSettingValue("MetaDescription", txtMetaDescription.Text, Blog.ModuleID);

            ModuleSettingsProvider.SetSettingValue("CategoriesListTitle", txtCategoriesTitle.Text, Blog.ModuleID);

            ModuleSettingsProvider.SetSettingValue("MaxImageWidth", Convert.ToInt32(txtMaxWidth.Text), Blog.ModuleID);
            ModuleSettingsProvider.SetSettingValue("MaxImageHeight", Convert.ToInt32(txtMaxHeight.Text), Blog.ModuleID);

            ModuleSettingsProvider.SetSettingValue("ShowAddDate", ckbShowAddDate.Checked, Blog.ModuleID);

            ModuleSettingsProvider.SetSettingValue("StaticBlock", txtStaticBlock.Text, Blog.ModuleID);
            ModuleSettingsProvider.SetSettingValue("SbBlogRight", ckeSbRight.Text, Blog.ModuleID);

            lblMessage.Text = (string)GetLocalResourceObject("ChangesSaved");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        protected bool Validate()
        {
            bool valid = true;

            var maxWidth = 0;
            var maxHeight = 0;
            

            if (!int.TryParse(txtMaxWidth.Text, out maxWidth) || maxWidth < 0)
            {
                txtMaxWidth.BorderColor = Color.Red;
                txtMaxWidth.BorderWidth = 1;
                valid = false;
            }
            else
            {
                txtMaxWidth.BorderColor = Color.Gray;
                txtMaxWidth.BorderWidth = 1;
            }

            if (!int.TryParse(txtMaxHeight.Text, out maxHeight) || maxHeight < 0)
            {
                txtMaxHeight.BorderColor = Color.Red;
                txtMaxHeight.BorderWidth = 1;
                valid = false;
            }
            else
            {
                txtMaxHeight.BorderColor = Color.Gray;
                txtMaxHeight.BorderWidth = 1;
            }
            return valid;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}