//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.IO;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Design;
using Resources;

namespace Admin
{
    public partial class EditTheme : AdvantShopAdminPage
    {
        private Theme _theme;

        protected eDesign Design;
        protected string CssContent;
        protected string ThemeName;
        protected string ThemeTitle;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["theme"]) || string.IsNullOrEmpty(Request["design"]))
            {
                Response.Redirect("~/admin/DesignConstructor.aspx");
                return;
            }

            Enum.TryParse(Request["design"], true, out Design);

            var themes = DesignService.GetDesigns(Design);

            _theme = themes.Find(x => x.Name.ToLower() == Request["theme"].ToLower());
            if (_theme == null)
            {
                Error404();
                return;
            }

            var designFolderPath = SettingsDesign.Template != TemplateService.DefaultTemplateId
                                            ? Server.MapPath("~/Templates/" + SettingsDesign.Template + "/")
                                            : Server.MapPath("~/");

            var cssPath = string.Format("{0}design/{1}s/{2}/styles/styles.css", designFolderPath, Design, _theme.Name);
            if (!File.Exists(cssPath))
            {
                using (File.Create(cssPath)){}
            }

            using (TextReader reader = new StreamReader(cssPath))
            {
                CssContent = reader.ReadToEnd();
            }

            ThemeName = _theme.Name;
            ThemeTitle = _theme.Title;


            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_EditTheme_Header));
        }
    }
}