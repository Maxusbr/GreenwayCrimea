//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface IClientPageModule : IModuleBase
    {
        string ClientPageControlFileName { get; }
        string UrlPath { get; }
        string PageTitle { get; }
        string MetaKeyWords { get; }
        string MetaDescription { get; }
    }
}
