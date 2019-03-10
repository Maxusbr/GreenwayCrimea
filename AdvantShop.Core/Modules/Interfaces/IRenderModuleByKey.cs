//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface IRenderModuleByKey : IModuleBase
    {
        List<ModuleRoute> GetModuleRoutes();
    }
}
