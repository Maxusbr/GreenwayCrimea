using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Leads
{
    public class LeadsFilterModel : BaseFilterModel
    {
        public string Name { get; set; }
        public string LeadStatus { get; set; }
        public int? DealStatusId { get; set; }
        public string Manager { get; set; }
        public string CreatedDateFrom { get; set; }
        public string CreatedDateTo { get; set; }
        public float? SumFrom { get; set; }
        public float? SumTo { get; set; }
        public string ManagerCustomerId { get; set; }
        public string CustomerId { get; set; }
        public int? OrderSourceId { get; set; }
    }
}
