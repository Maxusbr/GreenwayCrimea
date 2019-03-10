using AdvantShop.SEO;
using AdvantShop.News;
using AdvantShop.Web.Admin.Models.News;
using System.Linq;
using System.Web.Mvc;

namespace AdvantShop.Web.Admin.Handlers.News
{
    public class GetNewsModel
    {
        private readonly NewsItem _news;

        public GetNewsModel(NewsItem news)
        {
            _news = news;
        }

        public AddEditNewsModel Execute()
        {
            var categories = NewsService.GetNewsCategories().ToList();
            var model = new AddEditNewsModel
            {
                IsEditMode = true,
                NewsId = _news.NewsId,
                NewsCategoryId =
                    categories.Select(x => x.NewsCategoryId == _news.NewsCategoryId).ToList().Count > 0
                        ? _news.NewsCategoryId
                        : categories.Count > 0 ? categories[0].NewsCategoryId : 0,
                NewsCategory =
                    categories.Select(x => new SelectListItem() {Text = x.Name, Value = x.NewsCategoryId.ToString()}).ToList(),
                Title = _news.Title,
                TextAnnotation = _news.TextAnnotation,
                TextToPublication = _news.TextToPublication,
                ShowOnMainPage = _news.ShowOnMainPage,
                Enabled = _news.Enabled,
                UrlPath = _news.UrlPath,
                AddingDate = _news.AddingDate
            };

            if (_news.Picture != null)
            {
                model.PhotoId = _news.Picture.PhotoId;
                model.PhotoSrc = _news.Picture.ImageSrc();
            }

            if (string.IsNullOrEmpty(model.PhotoSrc))
            {
                model.PhotoSrc = "../images/nophoto_small.jpg";
            }

            var meta = MetaInfoService.GetMetaInfo(_news.NewsId, MetaType.News);
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
