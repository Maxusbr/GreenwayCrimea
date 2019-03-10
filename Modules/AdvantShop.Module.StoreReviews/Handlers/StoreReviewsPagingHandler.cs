using AdvantShop.Core.Models;
using AdvantShop.Core.Modules;
using AdvantShop.Core.SQL2;
using AdvantShop.Module.StoreReviews.Domain;
using AdvantShop.Module.StoreReviews.Models;

namespace AdvantShop.Module.StoreReviews.Handlers
{
    public class StoreReviewsPagingHandler
    {
        private SqlPaging _paging;
        private ReviewsModelViewModel _model;

        private readonly int _currentPageIndex;

        public StoreReviewsPagingHandler(int currentPageIndex)
        {
            _currentPageIndex = currentPageIndex;
        }

        public ReviewsModelViewModel Get()
        {
            _model = new ReviewsModelViewModel();

            _paging = new SqlPaging();
            _paging.Select(
                "ID",
                "ParentID",
                "ReviewerEmail",
                "ReviewerName",
                "Review",
                "DateAdded",
                "Moderated",
                "Rate",
                "ReviewerImage",
                "(SELECT Count(temp.ID) FROM [Module].[StoreReview] temp  WHERE temp.[ParentID] = StoreReview.ID)".AsSqlField("ChildsCount")
                );

            _paging.From("Module.StoreReview");

            _paging.Where("ParentID IS NULL");

            if (ModuleSettingsProvider.GetSettingValue<bool>("ActiveModerateStoreReviews", StoreReviews.ModuleID))
                _paging.Where("AND Moderated = 1");
            
            _paging.OrderByDesc("DateAdded");

            _paging.ItemsPerPage = _currentPageIndex != 0 ? ModuleSettingsProvider.GetSettingValue<int>("PageSize", StoreReviews.ModuleID) : int.MaxValue;
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

            _model.Items = _paging.PageItemsList<StoreReview>();

            return _model;
        }
    }
}
