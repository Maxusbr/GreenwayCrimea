//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Web.Routing;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface IModuleBase
    {
        
    }

    public interface IModule : IModuleBase
    {
        string ModuleStringId { get; }

        string ModuleName { get; }

        List<IModuleControl> ModuleControls { get; }

        bool HasSettings { get; }

        bool CheckAlive();

        bool InstallModule();

        bool UpdateModule();

        bool UninstallModule();
    }
    
    public interface IModuleControl
    {
        string NameTab { get; }
        string File { get; }
    }
}
