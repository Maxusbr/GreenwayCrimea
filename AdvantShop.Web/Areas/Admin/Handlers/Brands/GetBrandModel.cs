using AdvantShop.Catalog;
using AdvantShop.SEO;
using AdvantShop.Web.Admin.Models.Brands;
using System.Linq;

namespace AdvantShop.Web.Admin.Handlers.Brands
{
    public class GetBrandModel
    {
        private readonly Brand _brand;

        public GetBrandModel(Brand brand)
        {
            _brand = brand;
        }

        public AdminBrandModel Execute()
        {
            var model = new AdminBrandModel
            {
                IsEditMode = true,
                BrandId = _brand.BrandId,
                BrandName = _brand.Name,
                BrandSiteUrl = _brand.BrandSiteUrl,
                BriefDescription = _brand.BriefDescription,
                Description = _brand.Description,
                CountryId = _brand.CountryId,
                Enabled = _brand.Enabled,
                SortOrder = _brand.SortOrder,
                UrlPath = _brand.UrlPath,
                Picture = _brand.BrandLogo
            };

            if (_brand.BrandLogo != null)
            {
                model.PhotoId = _brand.BrandLogo.PhotoId;
                model.PhotoName = _brand.BrandLogo.PhotoName;
            }            

            var meta = MetaInfoService.GetMetaInfo(_brand.BrandId, MetaType.Brand);
            if (meta == null)
            {
                model.DefaultMeta = true;
            }
            else
            {
                model.DefaultMeta = false;
                model.SeoTitle = meta.Title;
                model.SeoH1 = meta.H1;
                model.SeoKeywords = meta.MetaKeywords;
                model.SeoDescription = meta.MetaDescription;
            }

            return model;
        }
    }
}
