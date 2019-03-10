//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Taxes
{
    public class OrderTax
    {
        public string Name { get; set; }
        public float Sum { get; set; }
        public int TaxId { get; set; }
        public bool ShowInPrice { get; set; }
        public float Rate { get; set; }
    }
}