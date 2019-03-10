using AdvantShop.Catalog;
using AdvantShop.SEO;
using AdvantShop.CMS;
using AdvantShop.Web.Admin.Models.StaticPages;
using System.Linq;
using System;

namespace AdvantShop.Web.Admin.Handlers.StaticPages
{
    public class GetStaticPageModel
    {
        private readonly StaticPage _page;

        public GetStaticPageModel(StaticPage model)
        {
            _page = model;
        }

        public AdminStaticPageModel Execute()
        {
            var model = new AdminStaticPageModel()
            {
                IsEditMode = true,
                AddDate = _page.AddDate,
                Enabled = _page.Enabled,
                IndexAtSiteMap = _page.IndexAtSiteMap,
                ModifyDate = _page.ModifyDate,
                PageName = _page.PageName,
                PageText = _page.PageText,
                ParentId = _page.ParentId,
                StaticPageId = _page.StaticPageId,
                UrlPath = _page.UrlPath,
                HasChildren = _page.HasChildren,
                SortOrder = _page.SortOrder,
                ParentPageName = _page.Parent != null ? _page.Parent.PageName : "Корень"
            };

            var meta = MetaInfoService.GetMetaInfo(_page.StaticPageId, MetaType.StaticPage);
            if(meta == null)
            {
                model.DefaultMeta = true;
            }
            else
            {
                model.DefaultMeta = false;
                model.SeoDescription = meta.MetaDescription;
                model.SeoH1 = meta.H1;
                model.SeoKeywords = meta.MetaKeywords;
                model.SeoTitle = meta.Title;
            }

            return model;
        }
    }
}
