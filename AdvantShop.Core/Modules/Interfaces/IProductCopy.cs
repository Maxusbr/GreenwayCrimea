using AdvantShop.Catalog;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface IProductCopy : IModule
    {
        void AfterCopyProduct(Product sourceProduct, Product newProduct);
    }
}
