using AdvantShop.Configuration;
using AdvantShop.Core.Models;
using AdvantShop.Core.SQL2;
using AdvantShop.Module.Blog.Domain;
using AdvantShop.Module.Blog.Models;

namespace AdvantShop.Module.Blog.Handlers.Blog
{
    public class BlogPagingHandler
    {
        #region Fields

        private SqlPaging _paging;
        private BlogPagingModel _model;

        private readonly int _categoryId;
        private readonly int _currentPageIndex;
        private readonly int? _itemPerPage;
        #endregion

        #region Constructor

        public BlogPagingHandler(int categoryId, int currentPageIndex, int? itemPerPage = null)
        {
            _categoryId = categoryId;
            _currentPageIndex = currentPageIndex;
            _itemPerPage = itemPerPage;
        }

        #endregion

        public BlogPagingModel Get()
        {
            _model = new BlogPagingModel();

            _paging = new SqlPaging();
            _paging.Select(
                "BlogItem.ItemId",
                "BlogItem.ItemCategoryId",
                "BlogItem.Title",
                "BlogItem.Picture",
                "BlogItem.TextAnnotation",
                "BlogItem.UrlPath",
                "BlogItem.AddingDate",
                "BlogCategory.Name as ItemCategoryName",
                "BlogCategory.UrlPath as ItemCategoryUrl"
                );

            _paging.From("Module.BlogItem");
            _paging.Left_Join("Module.BlogCategory On BlogCategory.ItemCategoryId = BlogItem.ItemCategoryId");

            _paging.Where("Enabled = 1");

            if (_categoryId != 0)
                _paging.Where("And BlogItem.ItemCategoryId={0}", _categoryId);

            _paging.OrderByDesc("BlogItem.AddingDate".AsSqlField("AddingDateSort"));

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

            _model.BlogItems = _paging.PageItemsList<BlogItem>();

            return _model;
        }
    }
}
