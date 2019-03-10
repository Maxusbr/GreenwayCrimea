using System.Collections.Generic;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Repository;

namespace AdvantShop.Web.Admin.Models.Brands
{
    public class AdminBrandsFilterResult : FilterResult<AdminBrandModel>
    {
        public List<Country> Countries { get; set; }
    }
}