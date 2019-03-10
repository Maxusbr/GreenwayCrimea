using AdvantShop.Configuration;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.FilePath;

namespace AdvantShop.Models.Common
{
    public partial class LogoModel : BaseModel
    {
        public bool DisplayHref { get; set; }

        public string ImgAlt { get; set; }

        public bool Visible { get; set; }

        public string ImgSource { get; set; }

        public string CssClass { get; set; }

        public bool InLanding { get; set; }

        public LogoModel()
        {
            ImgAlt = SettingsMain.LogoImageAlt;
            DisplayHref = true;
            Visible = true;

            var logo = SettingsMain.LogoImageName;

            // если превью шаблона и лого стандартное
            if (SettingsDesign.PreviewTemplate != null && (SettingsMain.IsDefaultLogo || string.IsNullOrEmpty(logo)))
            {
                var defaultTemplateLogo = SettingsDesign.DefaultLogo;

                if (!string.IsNullOrEmpty(defaultTemplateLogo))
                    ImgSource = UrlService.GetUrl(defaultTemplateLogo);
            }
            
            if (ImgSource == null)
            {
                if (!string.IsNullOrEmpty(logo))
                    ImgSource = FoldersHelper.GetPath(FolderType.Pictures, logo, false);
            }
        }

        public string Html { get; set; }

        public bool LogoGeneratorEnabled { get; set; }
    }
}