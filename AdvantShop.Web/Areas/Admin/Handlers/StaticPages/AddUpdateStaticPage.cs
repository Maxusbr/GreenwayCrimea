using System;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Diagnostics;
using AdvantShop.SEO;
using AdvantShop.CMS;
using AdvantShop.Web.Admin.Models.StaticPages;

namespace AdvantShop.Web.Admin.Handlers.StaticPages
{
    public class AddUpdateStaticPage
    {
        private AdminStaticPageModel _model;

        public AddUpdateStaticPage(AdminStaticPageModel model)
        {
            _model = model;
        }

        public int Execute()
        {
            var staticPage = new StaticPage()
            {
                StaticPageId = _model.StaticPageId,
                PageName = _model.PageName,
                PageText = _model.PageText,
                SortOrder = _model.SortOrder,
                AddDate = _model.AddDate,
                ModifyDate = DateTime.Now,
                IndexAtSiteMap = _model.IndexAtSiteMap,
                Enabled = _model.Enabled,
                UrlPath = _model.UrlPath,
                ParentId = _model.ParentId,
                Meta = new MetaInfo(0, _model.StaticPageId, MetaType.StaticPage, _model.SeoTitle, _model.SeoKeywords, _model.SeoDescription, _model.SeoH1)
            };

            try
            {
                if (_model.IsEditMode)
                {
                    StaticPageService.UpdateStaticPage(staticPage);
                }
                else
                {
                    _model.StaticPageId = StaticPageService.AddStaticPage(staticPage);
                }

                if(staticPage.StaticPageId == 0)
                {
                    return 0;
                }

                return _model.StaticPageId;
            }
            catch(Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return 0;
        }
    }
}
