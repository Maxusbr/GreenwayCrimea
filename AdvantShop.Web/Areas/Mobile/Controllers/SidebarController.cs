using System.Web.Mvc;
using AdvantShop.Areas.Mobile.Handlers.Sidebar;

namespace AdvantShop.Areas.Mobile.Controllers
{
    public class SidebarController : BaseMobileController
    {
        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            var model = new SidebarHandler().Get();
            return PartialView(model);
        }
    }
}