using AdvantShop.Catalog;
using AdvantShop.Core.Services.InplaceEditor;

namespace AdvantShop.Handlers.Inplace
{
    public class InplaceBrandHandler
    {
        public bool Execute(int id, string content, BrandInplaceField field)
        {
            var brandItem = BrandService.GetBrandById(id);
            if (brandItem == null)
                return false;

            switch (field)
            {
                case BrandInplaceField.Description:
                    brandItem.Description = content;
                    break;
                case BrandInplaceField.BriefDescription:
                    brandItem.BriefDescription = content;
                    break;
                default:
                    return false;
            }

            BrandService.UpdateBrand(brandItem);
            return true;
        }
    }
}