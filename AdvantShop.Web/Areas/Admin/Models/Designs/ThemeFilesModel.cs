using AdvantShop.Design;

namespace AdvantShop.Web.Admin.Models.Designs
{
    public class ThemeFilesModel
    {
        public string Theme { get; set; }
        public eDesign Design { get; set; }
        public string Action { get; set; }

        public string RemoveFile { get; set; }
    }
}
