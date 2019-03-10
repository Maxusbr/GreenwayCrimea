using AdvantShop.Module.LastOrder.Service;
using AdvantShop.Security;
using AdvantShop.Web.Infrastructure.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AdvantShop.Module.LastOrder.Controllers
{
    public class FNPAdminController : ModuleAdminController
    {
        public ActionResult Index()
        {
            return PartialView("~/modules/" + LastOrder.ModuleStringId + "/Views/Admin/Index.cshtml");
        }

        [HttpPost]
        public ActionResult Save(ModuleSettings settings)
        {
            return Json(settings.SaveSettings(), JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        public ActionResult Load()
        {
            return Json(new ModuleSettings());
        }
    }
}
