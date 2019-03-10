using System.Collections.Generic;

namespace AdvantShop.Web.Infrastructure.Admin
{
    public enum SelectModeCommand
    {
        None,
        All
    }

    public class BasePagingCommand
    {
        public List<int> Ids { get; set; }

        public SelectModeCommand SelectMode { get; set; }
    }
}
