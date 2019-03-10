
using System.Collections.Generic;

namespace AdvantShop.Payment
{
    public class PaymentModel
    {
        public int SelectedPaymentId { get; set; }
        public bool ShowAddress { get; set; }
        public bool ShowCustomFields { get; set; }
        public string PaymentType { get; set; }
        public List<PaymentMethodModel> PaymentMethods { get; set; }
        public bool IsValid { get; set; }
    }


    public class PaymentMethodModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Img { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}