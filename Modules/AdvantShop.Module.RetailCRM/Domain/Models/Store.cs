
namespace AdvantShop.Modules.RetailCRM.Models
{

    public class EditStore
    {
        public Store status { get; set; }
    }

    public class Store
    {
        public string name { get; set; }
        public string code { get; set; }
        public int ordering { get; set; }
        public string group { get { return "advantshop"; } }
    }
}