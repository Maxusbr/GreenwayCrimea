using AdvantShop.Web.Infrastructure.Admin;
using System.ComponentModel.DataAnnotations;

namespace AdvantShop.Web.Admin.Models.SmsTemplates
{
    public class SmsLogFilterModel : BaseFilterModel
    {
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Not a valid Phone number")]
        public string Phone { get; set; }
        public string Status { get; set; }
        public string Body { get; set; }
    }
}