using System.Collections.Generic;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.TaskGroups
{
    public class TaskGroupsFilterModel : BaseFilterModel
    {
        public int? SortingFrom { get; set; }
        public int? SortingTo { get; set; }
    }
}
