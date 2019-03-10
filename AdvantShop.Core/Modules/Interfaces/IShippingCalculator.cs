using System.Collections.Generic;
using AdvantShop.Orders;
using AdvantShop.Shipping;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface IShippingCalculator : IModule
    {
         void ProcessOptions(List<BaseShippingOption> options, List<PreOrderItem> preOrder);
    }
}
