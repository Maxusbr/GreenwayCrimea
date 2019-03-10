using System.Collections.Generic;

namespace AdvantShop.Core.Modules.Interfaces
{

    public interface IAdminBundles : IModule
    {
        List<string> AdminCssBottom();

        List<string> AdminJsBottom();
    }
}
