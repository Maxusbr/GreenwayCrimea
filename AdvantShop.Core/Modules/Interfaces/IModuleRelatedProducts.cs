//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.Core.Services.Catalog;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface IModuleRelatedProducts : IModule
    {
        List<ProductModel> GetRelatedProducts(Product product, RelatedType relatedType);

        string GetRelatedProductsHtml(Product product, RelatedType relatedType);
    }
}
