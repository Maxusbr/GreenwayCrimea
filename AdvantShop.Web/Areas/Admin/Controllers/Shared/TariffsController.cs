using System.Web.Mvc;
using AdvantShop.Web.Infrastructure.Controllers;

namespace AdvantShop.Web.Admin.Controllers.Shared
{
    public partial class TariffsController : BaseAdminController
    {
        public ActionResult Index()
        {
            SetNgController(NgControllers.NgControllersTypes.TariffsCtrl);
            return View();
        }

        public ActionResult Change()
        {
            SetNgController(NgControllers.NgControllersTypes.TariffsCtrl);
            return View();
        }
    }
}
