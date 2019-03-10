using System.Web.Mvc;
using AdvantShop.Areas.AdminMobile.Models.User;
using AdvantShop.Customers;
using AdvantShop.Security;

namespace AdvantShop.Areas.AdminMobile.Controllers
{
    public class UserController : BaseAdminMobileController
    {
        public ActionResult Login()
        {
            var currentCustomer = CustomerContext.CurrentCustomer;
            if (currentCustomer.CustomerRole == Role.Administrator || currentCustomer.CustomerRole == Role.Moderator)
                return RedirectToRoute("AdminMobile_Home");

            SetMetaInformation(T("AdminMobile.Login.AuthorizationTitle"));
            return View(new LoginModel());
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Login) && !string.IsNullOrWhiteSpace(model.Password))
            {
                if (AuthorizeService.SignIn(model.Login, model.Password, false, true))
                    return RedirectToAction("Login");
            }
            
            model.Password = null;
            model.Error = T("User.Login.WrongPassword");

            SetMetaInformation(T("AdminMobile.Login.AuthorizationTitle"));
            return View("Login", model);
        }

        public ActionResult Logout()
        {
            AuthorizeService.SignOut();
            return RedirectToRoute("AdminMobile_Home");
        }
    }
}