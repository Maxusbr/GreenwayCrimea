using AdvantShop.Catalog;
using System.Collections.Generic;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface ILabel : IModule
    {
        ProductLabel GetLabel();
        List<ProductLabel> GetLabels();
    }
}