using System.Web;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Customers;

namespace AdvantShop.Handlers.Common
{
    public class RecentlyViewedHandler
    {
        private readonly int _productAmount;

        public RecentlyViewedHandler(int productAmount)
        {
            _productAmount = productAmount;
        }

        public ProductViewModel Get()
        {
            if (HttpContext.Current.Request.Browser.Crawler)
                return null;

            var recentlyViewedItems = RecentlyViewService.LoadViewDataByCustomer(CustomerContext.CustomerId, _productAmount);
            return new ProductViewModel(recentlyViewedItems);
        }
    }
}