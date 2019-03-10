using System.IO;
using System.Web.Hosting;
using AdvantShop.Configuration;
using AdvantShop.Design;
using AdvantShop.Web.Admin.Models.Designs;

namespace AdvantShop.Web.Admin.Handlers.Design
{
    public class GetTheme
    {
        private readonly Theme _theme;
        private readonly eDesign _design;

        public GetTheme(Theme theme, eDesign design)
        {
            _theme = theme;
            _design = design;
        }

        public ThemeModel Execute()
        {
            var designFolderPath = SettingsDesign.Template != TemplateService.DefaultTemplateId
                                            ? HostingEnvironment.MapPath("~/Templates/" + SettingsDesign.Template + "/")
                                            : HostingEnvironment.MapPath("~/");

            var cssPath = string.Format("{0}design/{1}s/{2}/styles/styles.css", designFolderPath, _design, _theme.Name);
            if (!File.Exists(cssPath))
            {
                using (File.Create(cssPath)) { }
            }

            var model = new ThemeModel()
            {
                Design = _design,
                ThemeName = _theme.Name,
                ThemeTitle = _theme.Title
            };

            using (TextReader reader = new StreamReader(cssPath))
            {
                model.CssContent = reader.ReadToEnd();
            }

            return model;
        }
    }
}
