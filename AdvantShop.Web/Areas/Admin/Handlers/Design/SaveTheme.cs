using System.IO;
using System.Text;
using System.Web.Hosting;
using AdvantShop.Configuration;
using AdvantShop.Design;

namespace AdvantShop.Web.Admin.Handlers.Design
{
    public class SaveTheme
    {
        private readonly Theme _theme;
        private readonly eDesign _design;
        private readonly string _themeCss;

        public SaveTheme(Theme theme, eDesign design, string themeCss)
        {
            _theme = theme;
            _design = design;
            _themeCss = themeCss;
        }

        public void Execute()
        {
            var designFolderPath = SettingsDesign.Template != TemplateService.DefaultTemplateId
                                            ? HostingEnvironment.MapPath("~/Templates/" + SettingsDesign.Template + "/")
                                            : HostingEnvironment.MapPath("~/");

            var cssPath = string.Format("{0}design/{1}s/{2}/styles/styles.css", designFolderPath, _design, _theme.Name);
            File.WriteAllText(cssPath, _themeCss, Encoding.UTF8);
        }
    }
}
