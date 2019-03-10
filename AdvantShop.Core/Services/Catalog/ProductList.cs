namespace AdvantShop.Catalog
{
    public class ProductList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public bool Enabled { get; set; }
        public string Description { get; set; }
    }
}