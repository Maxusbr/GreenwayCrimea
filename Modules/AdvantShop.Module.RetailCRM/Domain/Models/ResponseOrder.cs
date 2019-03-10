namespace AdvantShop.Modules.RetailCRM.Models
{
    public class ResponseOrder : ResponseSimple
    {
        public RetailOrder order { get; set; }
    }

    public class RetailOrder
    {
        public int id { get; set; }
        //public string number { get; set; }
        public string externalId { get; set; }
    }

}