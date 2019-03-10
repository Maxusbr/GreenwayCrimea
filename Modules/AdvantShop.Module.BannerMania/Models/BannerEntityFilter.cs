using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Module.BannerMania.Models
{
    public class BannerEntityFilterModel : BaseFilterModel
    {
        public int EntityId { get; set; }

        public string EntityName { get; set; }

        public string EntityType { get; set; }

        public string ImagePath { get; set; }

        public string Placement { get; set; }
        
        public string URL { get; set; }

        public bool? NewWindow { get; set; }

        public bool? Enabled { get; set; }
    }
}