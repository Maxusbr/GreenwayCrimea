using AdvantShop.Design;
using AdvantShop.DownloadableContent;
using System.Collections.Generic;

namespace AdvantShop.Web.Admin.ViewModels.Design
{
    public class DesignModel
    {
        public List<DownloadableContentObject> AvaliableTemplates { get; set; }
        public List<DownloadableContentObject> MarketTemplates { get; set; }
        public DownloadableContentObject CurrentTemplate { get; set; }

        public DownloadableContentObject CurrentPreviewTemplate { get; set; }


        public List<Theme> Themes { get; set; }
        public List<Theme> BackGrounds { get; set; }
        public List<Theme> ColorSchemes { get; set; }

        public string CurrentTheme { get; set; }
        public string CurrentBackGround { get; set; }
        public string CurrentColorScheme { get; set; }
    }
}