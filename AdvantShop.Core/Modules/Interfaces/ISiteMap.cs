
using AdvantShop.SEO;
using System.Collections.Generic;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface ISiteMap : IModule
    {
        List<SiteMapData> GetData();        
    }
}