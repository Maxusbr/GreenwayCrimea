using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.SmsTemplates
{
    public class SmsTemplateFilterModel : BaseFilterModel
    {
        public string Name { get; set; }

        public decimal BonusPercent { get; set; }

        public int SortOrder { get; set; }

        public decimal PurchaseBarrier { get; set; }
    }
}
