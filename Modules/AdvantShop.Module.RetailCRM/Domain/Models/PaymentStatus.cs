
namespace AdvantShop.Modules.RetailCRM.Models
{

    public class EditPaymentStatus
    {
        public PaymentStatus paymentStatus { get; set; }
    }

    public class PaymentStatus
    {
        public string name { get; set; }
        public string code { get; set; }
        public bool paymentComplete { get; set; }
        public int ordering { get { return 0; } }
    }
}