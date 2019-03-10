using System.Collections.Generic;
using AdvantShop.Module.BannerMania.Models;

namespace AdvantShop.Module.BannerMania.ViewModel
{
    public class BannerInTopViewModel
    {
        public string ImagePath { get; set; }

        public string URL { get; set; }

        public bool TargetBlank { get; set; }
    }

    public class EntityBanners
    {
        public List<BannerEntity> Banners { get; set; }
    }
}
