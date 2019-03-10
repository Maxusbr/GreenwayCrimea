using System.Web;
using System.Web.Mvc;
using AdvantShop.Core.Services.Security;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Handlers.Search;
using AdvantShop.Web.Admin.Models.Catalog;
using AdvantShop.Web.Admin.ViewModels.Common;

namespace AdvantShop.Web.Admin.Controllers.Shared
{
    public partial class SearchController : BaseAdminController
    {
        [ChildActionOnly]
        public ActionResult SearchBlock()
        {
            var model = new SearchViewModel();

            if (!string.IsNullOrWhiteSpace(Request["search"]))
                model.Search = HttpUtility.HtmlEncode(Request["search"]);

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Index(string search)
        {
            search = HttpUtility.HtmlEncode(search);

            return RedirectToAction("Index", "Catalog", new { showMethod = ECatalogShowMethod.AllProducts, search = search});
        }

        [HttpGet]
        [AuthorizeRole(RoleAction.Catalog, RoleAction.Orders)]
        public JsonResult Autocomplete(string q, string type)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Json(null);

            var model = new GetSearchAutocomplete(q, type).Execute();

            return Json(model);
        }
    }
}
