using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.InplaceEditor;
using AdvantShop.Customers;

namespace AdvantShop.Handlers.Inplace
{
    public class InplaceProductHandler
    {
        public bool Execute(int id, string content, ProductInplaceField field)
        {
            var product = ProductService.GetProduct(id);
            if (product == null)
                return false;

            switch (field)
            {
                case ProductInplaceField.ArtNo:
                    if (ProductService.IsUniqueArtNo(content))
                    {
                        product.ArtNo = content;
                        return false;
                    }
                    break;
                case ProductInplaceField.Description:
                    product.Description = content;
                    break;
                case ProductInplaceField.BriefDescription:
                    product.BriefDescription = content;
                    break;
                case ProductInplaceField.Unit:
                    product.Unit = content;
                    break;
                case ProductInplaceField.Weight:
                    product.Weight = content.TryParseFloat();
                    break;
                default:
                    return false;
            }
            product.ModifiedBy = CustomerContext.CustomerId.ToString();
            ProductService.UpdateProduct(product, true);
            return true;
        }
    }
}