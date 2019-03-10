using System.Collections.Generic;
using AdvantShop.Catalog;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface IDiscount : IModule
    {
        float GetDiscount(int productId);

        List<ProductDiscount> GetProductDiscountsList();

        Discount GetCartItemDiscount(int cartItemId);
    }
}