

namespace AdvantShop.Catalog
{
    public class Size
    {
        public int SizeId { get; set; }
        public string SizeName { get; set; }
        public int SortOrder { get; set; }

        public override string ToString()
        {
            return SizeName;
        }
    }
}