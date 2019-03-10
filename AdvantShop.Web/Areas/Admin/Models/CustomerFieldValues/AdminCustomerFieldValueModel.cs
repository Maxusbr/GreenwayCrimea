namespace AdvantShop.Web.Admin.Models.CustomerFieldValues
{
    public class AdminCustomerFieldValueModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int CustomerFieldId { get; set; }
        public int SortOrder { get; set; }
    }
}
