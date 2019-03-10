using System.Linq;
using System.Web.Mvc;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Models;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings)]
    public class ManagersController : BaseAdminController
    {
        public JsonResult GetManagersSelectOptions(bool onlyActive = true)
        {
            var managers = ManagerService.GetManagersList(onlyActive).OrderBy(x => x.FullName);
            return Json(managers.Select(x => new SelectItemModel(x.FullName, x.ManagerId.ToString())));
        }
    }
}
