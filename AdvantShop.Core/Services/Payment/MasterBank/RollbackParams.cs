//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Payment
{
    public class RollbackParams
    {
        public int orderId { get; set; }
        public string amount { get; set; }
        public string order { get; set; }
        public string rrn { get; set; }
        public string int_ref { get; set; }
    }
}