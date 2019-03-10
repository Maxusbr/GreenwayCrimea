//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Orders
{
    public class OrderPickPoint
    {
        public int OrderId { get; set; }
        public string PickPointId { get; set; }
        public string PickPointAddress { get; set; }
        public string AdditionalData { get; set; }
    }
}