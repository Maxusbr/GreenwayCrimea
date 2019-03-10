using VkNet.Model;

namespace AdvantShop.Module.VkMarket.Domain
{
    /// <summary>
    /// Категория в API Вконтакте (у товаров есть внутренние категории) != категория в магазине
    /// </summary>
    public class VkMarketCategory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public VkMarketSection Section { get; set; }

        public VkMarketCategory(MarketCategory category)
        {
            Id = category.Id ?? 0;
            Name = category.Name;

            Section = new VkMarketSection()
            {
                Id = category.Section.Id ?? 0,
                Name = category.Section.Name
            };
        }
    }

    public class VkMarketSection
    {
        public long? Id { get; set; }
        public string Name { get; set; }
    }
}
