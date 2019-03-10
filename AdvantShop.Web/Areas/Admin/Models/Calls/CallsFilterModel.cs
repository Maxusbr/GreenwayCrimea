using System;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Calls
{
    public class CallsFilterModel : BaseFilterModel
    {
        public ECallType? Type { get; set; }
        public string SrcNum { get; set; }
        public string DstNum { get; set; }
        public string Extension { get; set; }
        public int? DurationFrom { get; set; }
        public int? DurationTo { get; set; }
        public DateTime? CallDateFrom { get; set; }
        public DateTime? CallDateTo { get; set; }
    }
}
