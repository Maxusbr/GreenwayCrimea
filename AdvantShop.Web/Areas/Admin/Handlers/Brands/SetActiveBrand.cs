using AdvantShop.Catalog;

namespace AdvantShop.Web.Admin.Handlers.Brands
{
    public class SetActiveBrand
    {
        private readonly int _brandId;
        private readonly bool _active;


        public SetActiveBrand(int brandId, bool active)
        {
            _brandId = brandId;
            _active = active;
        }

        public bool Execute()
        {
            var brand = BrandService.GetBrandById(_brandId);

            if (brand == null)
            {
                return false;
            }

            BrandService.SetActive(_brandId, _active);

            return true;
        }
    }
}
