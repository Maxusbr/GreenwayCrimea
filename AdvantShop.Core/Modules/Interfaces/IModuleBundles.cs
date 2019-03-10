using System.Collections.Generic;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface IModuleBundles : IModule
    {
        List<string> GetCssBundles();

        List<string> GetJsBundles();
    }
}
