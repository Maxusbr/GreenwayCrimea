//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Design;
using AdvantShop.Trial;
using Resources;
using AdvantShop.Core.Common.Extensions;

namespace Admin.UserControls.Design
{
    public partial class TemplatesSettings : System.Web.UI.UserControl
    {
        protected const string ThemePicturePath = "http://modules.advantshop.net/template/getthemepicture/?id={0}";
        protected const string _default = TemplateService.DefaultTemplateId;

        #region Errors
        private void MsgErr(bool clear)
        {
            lblError.Visible = false;
            lblError.Text = string.Empty;
        }

        private void MsgErr(string messageText)
        {
            lblError.Visible = true;
            lblError.Text += messageText;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            MsgErr(true);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            LoadData();
            lTrialMode.Visible = TrialService.IsTrialEnabled;
        }

        private void LoadData()
        {
            var templates = TemplateService.GetTemplates();

            DataListTemplates.DataSource = templates.Items;
            DataListTemplates.DataBind();
        }

        protected void dlItems_ItemCommand(object source, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                var id = e.CommandArgument.ToString();

                TemplateService.InstallTemplate(id.TryParseInt(), id, false);
            }

            if (e.CommandName == "Delete")
            {
                var stringId = e.CommandArgument.ToString();

                if (TemplateService.UninstallTemplate(stringId))
                    MsgErr(Resource.Admin_Templates_UninstallSuccess);
                else
                    MsgErr(Resource.Admin_Templates_UninstallFail);
            }

            if (e.CommandName == "ApplyTheme")
            {
                var stringId = e.CommandArgument.ToString();

                SettingsDesign.ChangeTemplate(stringId);
                CacheManager.Clean();
            }

            if (e.CommandName == "InstallLastVersion")
            {
                var id = e.CommandArgument.ToString();

                TemplateService.InstallLastTemplate(id.TryParseInt());
            }
            LoadData();
        }

        protected string RenderTemplatePicture(string previewImage, string templateId)
        {
            if (!string.IsNullOrEmpty(previewImage))
                return string.Format("<img src=\"{0}\" />", previewImage);

            return string.Format("<img src=\"{0}\" />", string.Format(ThemePicturePath, templateId));
        }
    }
}