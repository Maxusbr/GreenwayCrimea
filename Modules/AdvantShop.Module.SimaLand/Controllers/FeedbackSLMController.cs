using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvantShop.Web.Infrastructure.Controllers;
using System.Web.Mvc;
using AdvantShop.Security;
using System.Web;
using AdvantShop.Module.SimaLand.ViewModel;
using AdvantShop.Mails;

namespace AdvantShop.Module.SimaLand.Controllers
{
    public class FeedbackSLMController : ModuleController
    {
        bool allow = Secure.VerifyAccess();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetForm()
        {
            if (!allow)
            {
                throw new HttpException(403, "Доступ запрещён!");
            }

            var user = Customers.CustomerContext.CurrentCustomer;

            var model = new FeedbackViewModel();

            model.Name = user.LastName +" "+ user.FirstName;
            model.Email = user.EMail;
            model.Phone = user.Phone;

            return PartialView("~/Modules/SimaLand/Views/Feedback/modal.cshtml",model);
        }

        [HttpPost]
        public ActionResult SendForm(FeedbackViewModel model)
        {
            if (!allow)
            {
                throw new HttpException(403, "Доступ запрещён!");
            }

            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Phone))
            {
                return Json(new { success = false, message = "Заполните контактные данные." }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(model.Message))
            {
                return Json(new { success = false, message = "Введите сообщение." }, JsonRequestBehavior.AllowGet);
            }

            var domen = Request.Url.Authority;

            var nl = "<br/>";
            var mail = model.Message+nl + nl + nl;
            mail += model.Name + nl + model.Email + nl + model.Phone+nl+ "<a href=\"" + domen + "\">"+ domen+"</a>";
            try
            {
                SendMail.SendMailNow(Guid.Empty, "help@promo-z.ru", "Нужна помощь", mail, true);

                return Json(new { success = true, message = "Сообщение отправлено!<br/> В скором времени с Вами свяжутся.<br/><br/>" }, JsonRequestBehavior.AllowGet);
            }
            catch ( Exception ex)
            {
                return Json(new { success = false, message = "При попытке отправки произошла ошибка."}, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
