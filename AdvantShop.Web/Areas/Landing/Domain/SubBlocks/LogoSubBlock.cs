using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.FilePath;

namespace AdvantShop.App.Landing.Domain.SubBlocks
{
    public class LogoSubBlock : BaseLpSubBlock
    {
        //public override string GetContent(Product product)
        //{
        //    var imgSource =
        //        string.IsNullOrEmpty(SettingsMain.LogoImageName)
        //            ? UrlService.GetUrl("images/nophoto-logo.png")
        //            : FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.LogoImageName, false);

        //    var alt =
        //        !string.IsNullOrEmpty(SettingsMain.LogoImageAlt)
        //            ? string.Format(" alt=\"{0}\"", SettingsMain.LogoImageAlt)
        //            : string.Empty;

        //    return string.Format("<img id=\"logo\" src=\"{0}\"{1} class=\"lp-logo\"/>", imgSource, alt);
        //}

        public override dynamic GetSettings(LpBlock currentBlock, Product product, dynamic settings)
        {

            if (settings != null)
            {
                settings.alt = SettingsMain.LogoImageAlt;
                settings.src = string.IsNullOrEmpty(SettingsMain.LogoImageName)
                    ? UrlService.GetUrl("images/nophoto-logo.png")
                    : FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.LogoImageName, false);
            }

            return settings;
        }
    }
}
