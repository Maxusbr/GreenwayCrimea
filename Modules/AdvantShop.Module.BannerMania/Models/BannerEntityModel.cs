
namespace AdvantShop.Module.BannerMania.Models
{
    public partial class BannerEntityModel
    {
        public int BannerId { get; set; }

        public int EntityId { get; set; }

        public string EntityType { get; set; }

        public string EntityName { get; set; }
        
        public string ImagePath { get; set; }

        public string Placement { get; set; }
        
        public string URL { get; set; }

        public bool NewWindow { get; set; }

        public bool Enabled { get; set; }

        public string Link
        {
            get
            {
                return EntityType == "Products" || EntityType == "Для отдельных товаров" ? "product" : "category";
            }
        }
    }
}
