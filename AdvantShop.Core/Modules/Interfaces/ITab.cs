//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface ITab
    {
        int ProductId { get; set; }

        string Title { get; set; }

        string Body { get; set; }

        string TabGroup { get; set; }

        bool Active { get; set; }

        int SortOrder { get; set; }
    }
}
