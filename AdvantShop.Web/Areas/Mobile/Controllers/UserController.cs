using System.Web.Mvc;
using AdvantShop.Core.Controls;
using AdvantShop.Customers;
using AdvantShop.Security;

namespace AdvantShop.Areas.Mobile.Controllers
{
    public class UserController : BaseMobileController
    {
        public ActionResult Login(string email, string password)
        {
            if (CustomerContext.CurrentCustomer.RegistredUser)
                return RedirectToRoute("Home");

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                if (AuthorizeService.SignIn(email, password, false, true))
                    return RedirectToRoute("Home");

                ShowMessage(NotifyType.Error, T("User.Login.WrongPassword"));
            }

            SetTitle(T("User.Login.Authorization"));
            SetMetaInformation(T("User.Login.Authorization"));

            return View();
        }

        public ActionResult Logout()
        {
            AuthorizeService.SignOut();
            return RedirectToRoute("Home");
        }
    }
}