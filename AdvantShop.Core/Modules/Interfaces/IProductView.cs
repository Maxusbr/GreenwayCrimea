using System.Collections.Generic;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface IProductView : IModule
    {
        List<string> Views { get; }
    }
}
