
namespace AdvantShop.Modules.RetailCRM.Models
{

    public class EditOrderMethod
    {
        public OrderMethod orderMethod { get; set; }
    }

    public class OrderMethod
    {
        public string name { get; set; }
        public string code { get; set; }
        public bool defaultForApi { get; set; }
    }
}