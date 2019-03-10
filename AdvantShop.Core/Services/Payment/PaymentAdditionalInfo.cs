//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Payment
{
    public class PaymentAdditionalInfo
    {
        public int PaymentMethodId { get; set; }
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}