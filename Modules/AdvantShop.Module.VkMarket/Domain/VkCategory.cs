namespace AdvantShop.Module.VkMarket.Domain
{
    /// <summary>
    /// Категория (альбом/подборка товаров в терминологии Вконтакте)
    /// </summary>
    public class VkCategory
    {
        /// <summary>
        /// Внутренний id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id альбома/подборки в Вконтакте
        /// </summary>
        public long VkId { get; set; }

        /// <summary>
        /// Id раздела товаров  (Гардероб, Десткиие товар и тд, чтобы не назначать каждому товару, назаначаем всей категории)
        /// </summary>
        public long VkCategoryId { get; set; }

        public string Name { get; set; }

        public int SortOrder { get; set; }
    }
}
