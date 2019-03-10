namespace AdvantShop.Web.Admin.Models.PaymentMethods
{
    public class AdminPaymentMethodItemModel
    {
        public int PaymentMethodId { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public bool Enabled { get; set; }
        public string Icon { get; set; }
        public string PaymentType { get; set; }
    }
}
