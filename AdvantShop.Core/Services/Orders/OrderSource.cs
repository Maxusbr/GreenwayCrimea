using AdvantShop.Core.Services.Orders;

namespace AdvantShop.Orders
{
    public class OrderSource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int SortOrder { get; set; }

        public bool Main { get; set; }

        public OrderType Type { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as OrderSource;
            if (other == null)
                return false;

            return Id == other.Id && Name == other.Name && SortOrder == other.SortOrder && 
                   Main == other.Main && Type == other.Type;
        }

        public override int GetHashCode()
        {
            return Id ^ Name.GetHashCode() ^ SortOrder ^ Main.GetHashCode() ^ Type.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
