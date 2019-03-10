using AdvantShop.Security;
using AdvantShop.Web.Infrastructure.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AdvantShop.Module.CategoriesOnMainPage.Controllers
{
    public class COMPAdminController : ModuleController
    {
        bool allow = Secure.VerifyAccess();

        public ActionResult Index()
        {
            VerifyAccess(); //Должен присутствовать в каждом Action

            return PartialView("~/modules/" + CategoriesOnMainPage.ModuleStringId + "/Views/Admin/Index.cshtml");
        }

        private void VerifyAccess()
        {
            if (!allow)
            {
                throw new HttpException(403, "Доступ запрещён!");
            }
        }

    }
}
