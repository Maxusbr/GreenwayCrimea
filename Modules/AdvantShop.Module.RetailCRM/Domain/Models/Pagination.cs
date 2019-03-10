
namespace AdvantShop.Modules.RetailCRM.Models
{
    public class Pagination
    {
        public int Limit { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPageCount { get; set; }
    }
}