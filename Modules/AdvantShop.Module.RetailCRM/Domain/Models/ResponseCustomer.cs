namespace AdvantShop.Modules.RetailCRM.Models
{
    public class ResponseCustomer : ResponseSimple
    {
        public RetailCustomer customer { get; set; }
    }

    public class RetailCustomer
    {
        public int id { get; set; }
        public string number { get; set; }
        public string externalId { get; set; }
    }

}