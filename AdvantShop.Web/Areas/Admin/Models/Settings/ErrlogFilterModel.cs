using AdvantShop.Diagnostics;
using AdvantShop.Web.Infrastructure.Admin;


namespace AdvantShop.Web.Admin.Models.Settings
{
    public class ErrlogFilterModel : BaseFilterModel
    {

        public ErrType Type { get; set; }

        public string DateTimeFormatted { get; set; }

        public bool? Level { get; set; }

        public string Message { get; set; }

        public string ErrorMessage { get; set; }     
    }

}