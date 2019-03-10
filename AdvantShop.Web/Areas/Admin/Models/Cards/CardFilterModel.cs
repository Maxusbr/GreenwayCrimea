using System;
using System.Collections.Generic;
using AdvantShop.Web.Admin.Models.Common;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Cards
{
    public class CardFilterModel : BaseFilterModel<Guid>
    {
        public string CardNumber { get; set; }
        public int? GradeId { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public int? BonusAmountFrom { get; set; }
        public int? BonusAmountTo { get; set; }

        public string FIO { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string Location { get; set; }
        public DateTime? RegDateFrom { get; set; }
        public DateTime? RegDateTo { get; set; }

        public int? OrdersCountFrom { get; set; }
        public int? OrdersCountTo { get; set; }
        public float? OrderSumFrom { get; set; }
        public float? OrderSumTo { get; set; }

        public Dictionary<string, CustomerFiledFilterModel> CustomerFields { get; set; }
    }
}
