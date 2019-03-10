using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Models;
using AdvantShop.Core.SQL2;
using AdvantShop.Models.News;
using AdvantShop.News;

namespace AdvantShop.Handlers.News
{
    public class NewsPagingHandler
    {
        #region Fields

        private SqlPaging _paging;
        private NewsPagingModel _model;

        private readonly int _newsCategoryId;
        private readonly int _currentPageIndex;
        private readonly int? _itemPerPage;

        #endregion

        #region Constructor

        public NewsPagingHandler(int newsCategoryId, int currentPageIndex, int? itemPerPage = null)
        {
            _newsCategoryId = newsCategoryId;
            _currentPageIndex = currentPageIndex;
            _itemPerPage = itemPerPage;
        }

        #endregion

        public NewsPagingModel Get()
        {
            _model = new NewsPagingModel();

            _paging = new SqlPaging(CacheNames.News);
            _paging.Select(
                "NewsID",
                "Title",
                "PhotoName",
                "TextAnnotation",
                "UrlPath",
                "AddingDate"
                );

            _paging.From("Settings.News");
            _paging.Left_Join("Catalog.Photo on Photo.objId=News.NewsID and Type={0}", PhotoType.News.ToString());
            _paging.OrderByDesc("AddingDate".AsSqlField("AddingDateSort"));
            _paging.Where("AddingDate <= GetDate()");
            _paging.Where("Enabled = 1");

            if (_newsCategoryId != 0)
                _paging.Where("and NewsCategoryId={0}", _newsCategoryId);

            if (_itemPerPage.HasValue)
                _paging.ItemsPerPage = _itemPerPage.Value;
            else
                _paging.ItemsPerPage = _currentPageIndex != 0 ? SettingsNews.NewsPerPage : int.MaxValue;
            _paging.CurrentPageIndex = _currentPageIndex != 0 ? _currentPageIndex : 1;

            var totalCount = _paging.TotalRowsCount;
            var totalPages = _paging.PageCount(totalCount);

            _model.Pager = new Pager()
            {
                TotalItemsCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = _currentPageIndex,
                DisplayShowAll = true,
            };

            if ((totalPages < _currentPageIndex && _currentPageIndex > 1) || _currentPageIndex < 0)
            {
                return _model;
            }

            _model.News = _paging.PageItemsList<NewsItem>();

            return _model;
        }
    }
}