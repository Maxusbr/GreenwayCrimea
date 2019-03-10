using System.Web.Mvc;
using AdvantShop.Handlers.Common;

namespace AdvantShop.Controllers
{
    public partial class RecentlyViewsController : BaseClientController
    {
        [ChildActionOnly]
        public ActionResult RecentlyViewed(int productAmount = 3)
        {
            var model = new RecentlyViewedHandler(productAmount).Get();
            if (model == null)
                return new EmptyResult();

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult RecentlyViewedBlock(int productAmount = 10)
        {
            var model = new RecentlyViewedHandler(productAmount).Get();

            if (model == null)
                return new EmptyResult();

            return PartialView(model);
        }
    }
}