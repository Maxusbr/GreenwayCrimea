using System;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Localization;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Customers
{
    public class CustomerLeadsFilterModel : BaseFilterModel<Guid>
    {
        public Guid CustomerId { get; set; }
    }

    public class CustomerLeadModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public LeadStatus LeadStatus { get; set; }
        public string Status { get { return LeadStatus.Localize(); } }

        public DateTime CreatedDate { get; set; }
        public string CreatedDateFormatted { get { return Culture.ConvertDateWithoutSeconds(CreatedDate); } }

        public float ItemsSum { get; set; }
        public float CurrencyValue { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public bool IsCodeBefore { get; set; }

        public string Sum
        {
            get
            {
                return PriceFormatService.FormatPrice(ItemsSum, CurrencyValue, CurrencySymbol, CurrencyCode, IsCodeBefore);
            }
        }
    }
}
