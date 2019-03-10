using AdvantShop.Design;

namespace AdvantShop.Web.Admin.Models.Designs
{
    public class ThemeModel
    {
        public string CssContent { get; set; }
        public string ThemeName { get; set; }
        public string ThemeTitle { get; set; }
        public eDesign Design { get; set; }
    }
}
