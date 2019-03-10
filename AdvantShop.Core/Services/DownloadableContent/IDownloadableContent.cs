//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.DownloadableContent.Interfaces
{
    public interface IDownloadableContent
    {
        int Id { get; set; }
        string StringId { get; set; }
        string Version { get; set; }
        bool Active { get; set; }

        string Name { get; set; }
        float Price { get; set; }
        string Currency { get; set; }

        string DetailsLink { get; set; }
        string BriefDescription { get; set; }

        int SortOrder { get; set; }
        string Icon { get; set; }

        string CurrentVersion { get; set; }
        bool IsLocalVersion { get; set; }
        bool IsInstall { get; set; }

        bool NeedUpdate { get; set; }
    }
}