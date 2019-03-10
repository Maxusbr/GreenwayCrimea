using AdvantShop.Module.BannerMania.Service;

namespace AdvantShop.Module.BannerMania.Models
{
    public enum PlacementType
    {
        AboveDeliveryInfo = 1,
        UnderDeliveryInfo,
        AboveFilter,
        UnderFilter,
        AboveFooter,
        UnderMenu
    }
    
    public enum EntityType
    {
        Products = 0,
        ProductsByCategories = 1,
        Categories = 2
    }

    public class BannerEntity
    {
        public int BannerId { get; set; }

        public int EntityId { get; set; }

        public string EntityName { get; set; }

        public EntityType EntityType { get; set; }

        public string ImagePath { get; set; }

        public PlacementType Placement { get; set; }
        
        public string PlacementString
        {
            get { return BMService.GetPlacementTypeName(Placement.ToString()); }
        }

        public string URL { get; set; }

        public bool NewWindow { get; set; }

        public bool Enabled { get; set; }
    }

    public class ShortCategory
    {
        public ShortCategory() { }

        public ShortCategory(int categoryId, string categoryName)
        {
            Id = categoryId;
            Name = categoryName;
        }

        public int Id { get; set; }
        
        public string Name { get; set; }
    }
}
