
namespace AdvantShop.Module.ReturnCustomer.Models
{
    public class ReturnCustomerSettings
    {
        public string MessageSubject { get; set; }

        public string MessageText { get; set; }

        public string AlternativeMessageSubject { get; set; }

        public string AlternativeMessageText { get; set; }

        public string DisabledMailsList { get; set; }

        public int DaysInterval { get; set; }

        public bool AutoSending { get; set; }
    }
}
