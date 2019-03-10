using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Repository;

namespace AdvantShop.Web.Admin.Models.Settings
{
    public class AdminLocalizationsFilterModel : BaseFilterModel
    {
        public int? LanguageId { get; set; }

        public string ResourceKey { get; set; }

        public string ResourceValue { get; set; }

        public string Value { get; set; }

        public string Text { get; set; }
        
        public bool ChangeAll { get; set; }
    }
}