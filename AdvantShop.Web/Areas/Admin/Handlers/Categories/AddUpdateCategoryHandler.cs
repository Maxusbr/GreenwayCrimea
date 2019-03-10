using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.SEO;
using AdvantShop.Trial;
using AdvantShop.Web.Admin.Models.Categories;

namespace AdvantShop.Web.Admin.Handlers.Categories
{
    public class AddUpdateCategoryHandler
    {
        private AdminCategoryModel _model;

        public AddUpdateCategoryHandler(AdminCategoryModel model)
        {
            _model = model;
        }

        public int Execute()
        {
            var category = new Category
            {
                CategoryId = _model.CategoryId,
                Name = _model.Name.Trim(),
                UrlPath = _model.UrlPath.Trim(),
                ParentCategoryId = _model.ParentCategoryId,
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
                Hidden = _model.Hidden,
                DisplayChildProducts = false,
                DisplayStyle = _model.DisplayStyle,
                DisplayBrandsInMenu = _model.DisplayBrandsInMenu,
                DisplaySubCategoriesInMenu = _model.DisplaySubCategoriesInMenu,
                Sorting = _model.Sorting,
                Meta =
                    new MetaInfo(0, _model.CategoryId, MetaType.Category, _model.SeoTitle.DefaultOrEmpty(),
                        _model.SeoKeywords.DefaultOrEmpty(), _model.SeoDescription.DefaultOrEmpty(),
                        _model.SeoH1.DefaultOrEmpty()),
                Tags = new List<Tag>()
            };

            if (_model.Tags != null && _model.Tags.Count > 0)
            {
                category.Tags = _model.Tags.Select(x => new Tag
                {
                    Name = x,
                    UrlPath = StringHelper.TransformUrl(StringHelper.Translit(x)),
                    Enabled = true,
                    VisibilityForUsers = true
                }).ToList();
            }

            try
            {
                if (_model.IsEditMode)
                {
                    CategoryService.UpdateCategory(category, true);
                }
                else
                {
                    category.CategoryId = CategoryService.AddCategory(category, true, true);
                }

                if (category.CategoryId == 0)
                    return 0;

                if (!_model.IsEditMode)
                {
                    AddPictureLink(_model.PictureId, category.CategoryId);
                    AddPictureLink(_model.MiniPictureId, category.CategoryId);
                    AddPictureLink(_model.IconId, category.CategoryId);
                }

                if (!_model.IsEditMode)
                {
                    TrialService.TrackEvent(TrialEvents.AddCategory, "");
                    TrialService.TrackEvent(ETrackEvent.Trial_AddCategory);
                }

                return category.CategoryId;
            }
            catch (Exception ex)
            {
                Debug.Log.Error("AddUpdate category handler", ex);
            }

            return 0;
        }

        private void AddPictureLink(int pictureId, int categoryId)
        {
            if (pictureId != 0)
            {
                var photo = PhotoService.GetPhoto(pictureId);
                if (photo != null)
                    PhotoService.UpdateObjId(photo.PhotoId, categoryId);
            }
        }
    }
}
