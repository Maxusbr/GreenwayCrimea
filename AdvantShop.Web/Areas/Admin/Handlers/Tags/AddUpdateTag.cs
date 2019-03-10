using System;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Diagnostics;
using AdvantShop.SEO;
using AdvantShop.Web.Admin.Models.Tags;

namespace AdvantShop.Web.Admin.Handlers.Tags
{
    public class AddUpdateTag
    {
        private readonly TagModel _model;

        public AddUpdateTag(TagModel model)
        {
            _model = model;
        }

        public int Execute()
        {
            var tag = new Tag()
            {
                Id = _model.IsEditMode ? _model.Id : 0,
                Name = _model.Name.DefaultOrEmpty().Trim(),
                SortOrder = _model.SortOrder,
                Enabled = _model.Enabled,
                VisibilityForUsers = _model.VisibilityForUsers,
                Description =
                    _model.Description == null || _model.Description == "<br />" || _model.Description == "&nbsp;" || _model.Description == "\r\n"
                        ? string.Empty
                        : _model.Description,
                BriefDescription =
                    _model.BriefDescription == null || _model.BriefDescription == "<br />" || _model.BriefDescription == "&nbsp;" || _model.BriefDescription == "\r\n"
                        ? string.Empty
                        : _model.BriefDescription,

                UrlPath = _model.UrlPath.Trim(),
                Meta =
                    new MetaInfo(0, _model.Id, MetaType.Tag, _model.SeoTitle.DefaultOrEmpty(),
                        _model.SeoKeywords.DefaultOrEmpty(), _model.SeoDescription.DefaultOrEmpty(),
                        _model.SeoH1.DefaultOrEmpty()),
            };


            try
            {
                if (_model.IsEditMode)
                {
                    TagService.Update(tag);
                }
                else
                {
                    tag.Id = TagService.Add(tag);
                }

                if (tag.Id == 0)
                    return 0;

                return tag.Id;
            }
            catch (Exception ex)
            {
                Debug.Log.Error("AddUpdate tag", ex);
            }

            return 0;
        }
    }
}
