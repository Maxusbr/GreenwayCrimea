using System;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Diagnostics;
using AdvantShop.SEO;
using AdvantShop.Web.Admin.Models.Brands;

namespace AdvantShop.Web.Admin.Handlers.Brands
{
    public class AddUpdateBrand
    {
        private AdminBrandModel _model;

        public AddUpdateBrand(AdminBrandModel model)
        {
            _model = model;
        }

        public int Execute()
        {
            var brand = new Brand
            {
                BrandId = _model.BrandId,
                Name = _model.BrandName.Trim(),
                UrlPath = _model.UrlPath.Trim(),
                Description =
                    _model.Description == null || _model.Description == "<br />" || _model.Description == "&nbsp;" || _model.Description == "\r\n"
                        ? string.Empty
                        : _model.Description,
                BriefDescription =
                    _model.BriefDescription == null || _model.BriefDescription == "<br />" || _model.BriefDescription == "&nbsp;" || _model.BriefDescription == "\r\n"
                        ? string.Empty
                        : _model.BriefDescription,
                SortOrder = _model.SortOrder,
                Enabled = _model.Enabled,
                BrandSiteUrl = _model.BrandSiteUrl,
                CountryId = _model.CountryId,
                Meta =
                    new MetaInfo(0, _model.BrandId, MetaType.Brand, _model.SeoTitle.DefaultOrEmpty(),
                        _model.SeoKeywords.DefaultOrEmpty(), _model.SeoDescription.DefaultOrEmpty(),
                        _model.SeoH1.DefaultOrEmpty()),
            };

            try
            {
                if (_model.IsEditMode)
                {
                    BrandService.UpdateBrand(brand);
                }
                else
                {
                    brand.BrandId = BrandService.AddBrand(brand);
                }

                if (brand.BrandId == 0)
                    return 0;

                if (!_model.IsEditMode)
                {
                    AddPictureLink(_model.PhotoId, brand.BrandId);
                }
                
                return brand.BrandId;
            }
            catch (Exception ex)
            {
                Debug.Log.Error("AddUpdate brand handler", ex);
            }

            return 0;
        }

        private void AddPictureLink(int pictureId, int brandId)
        {
            if (pictureId == 0) return;
            
            var photo = PhotoService.GetPhoto(pictureId);
            if (photo != null)
                PhotoService.UpdateObjId(photo.PhotoId, brandId);
        }
    }
}
