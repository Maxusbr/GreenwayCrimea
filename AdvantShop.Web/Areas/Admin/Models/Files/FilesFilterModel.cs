using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Files
{
    public class FilesFilterModel : BaseFilterModel<string>
    {
        public string FileName { get; set; }
    }

}
