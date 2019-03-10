using AdvantShop.Security;
using AdvantShop.Web.Infrastructure.Controllers;
using System;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Module.RussianPostPrintBlank.Service;
using AdvantShop.Module.RussianPostPrintBlank.Models;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AdvantShop.Core.Modules;
using AdvantShop.Orders;

namespace AdvantShop.Module.RussianPostPrintBlank.Controllers
{
    public class RPPBAdminController : ModuleAdminController
    {
        [ChildActionOnly]
        public ActionResult OrderSearch()
        {
            return PartialView("~/Modules/" + RussianPostPrintBlank.ModuleStringId + "/Views/Admin/OrderSearch.cshtml");
        }

        [ChildActionOnly]
        public ActionResult Templates()
        {
            return PartialView("~/Modules/" + RussianPostPrintBlank.ModuleStringId + "/Views/Admin/Templates.cshtml");
        }

        [ChildActionOnly]
        public ActionResult Feedback()
        {
            return PartialView("~/Modules/" + RussianPostPrintBlank.ModuleStringId + "/Views/Admin/Feedback.cshtml");
        }

        [HttpGet]
        public int getOrdersCount()
        {
            return RPPBService.GetOrdersCount() / 20 + 1;
        }

        [HttpGet]
        public JsonResult GetOrders(int page = 1, string status = "Любой", bool? payed = null, string shipping = "Любой", string orderNumber = "")
        {
            var model = new OrderModelView();
            model.Orders = RPPBService.GetOrdersWithStatus(page, 20, status, shipping, payed, orderNumber);
            
            model.NextPage = page + 1;
            model.BackPage = page - 1;
            model.StatusList = new List<string>();
            model.StatusList.Add("Любой");
            model.StatusList.AddRange(Orders.OrderStatusService.GetOrderStatuses().Select(x => x.StatusName).ToList());
            model.SelectStatus = status;
            model.ShippingList = new List<string>();
            model.ShippingList.Add("Любой");
            model.ShippingList.AddRange(Shipping.ShippingMethodService.GetAllShippingMethods().Select(x => x.Name).ToList());
            model.SelectShipping = shipping;
            model.OrderNumber = orderNumber;
            

            model.PayedList = new List<PaySelect> {
                new PaySelect {
                    Key = "Любой",
                    Value = "null"
                },
                new PaySelect
                {
                    Key = "Оплачено",
                    Value = "true"
                },
                new PaySelect
                {
                    Key = "Не оплачено",
                    Value = "false"
                }
            };

            model.SelectPayed = payed != null ? ((bool)payed ? "true" : "false") : "null";

            model.FormTypes = new List<SelectListItem>();
            model.FormTypes.Add(new SelectListItem { Text = "Выберите форму для печати", Value = "-1", Selected = true });

            var templates = TemplatesService.GetTemplates();
            foreach (var template in templates)
            {
                model.FormTypes.Add(new SelectListItem { Text = template.Name, Value = template.TemplateID.ToString() });
            }

            foreach (var order in model.Orders)
            {
                order.FormType = new SelectListItem { Text = "Выберите форму для печати", Value = "-1", Selected = true };
            }

            return Json(
                new
                {
                    Orders = model.Orders,
                    StatusList = model.StatusList,
                    SelectStatus = model.SelectStatus,
                    PayedList = model.PayedList,
                    SelectPayed = model.SelectPayed,
                    ShippingList = model.ShippingList,
                    SelectShipping = model.SelectShipping,
                    OrderNumber = model.OrderNumber,
                    FormTypes = model.FormTypes,
                    BackPage = model.BackPage,
                    NextPage = model.NextPage,
                    Next = model.Next,
                },

                JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult OrderInfoTemplatesList(int orderId)
        {
            var model = new OrderInfoTemplatesList();
            model.OrderId = orderId;

            model.FormTypes = new List<SelectListItem>();
            model.FormTypes.Add(new SelectListItem { Text = "Выберите форму для печати", Value = "-1", Selected = true });

            var templates = TemplatesService.GetTemplates();
            foreach (var template in templates)
            {
                model.FormTypes.Add(new SelectListItem { Text = template.Name, Value = template.TemplateID.ToString() });
            }

            return PartialView("~/Modules/" + RussianPostPrintBlank.ModuleStringId + "/Views/Admin/OrderPrintBlank.cshtml", model);
        }
        
        [HttpGet]
        public JsonResult FeedbackSettings()
        {
            var settings = new FeedbackSettingsModel();
            var customer = AdvantShop.Customers.CustomerContext.CurrentCustomer;

            if (customer != null)
            {
                settings.Name = string.Format("{0} {1} {2}", customer.FirstName, customer.Patronymic, customer.LastName);
                settings.Email = customer.EMail;
                settings.Phone = customer.Phone;
                settings.Message = string.Empty;
            }

            return Json(settings, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string CheckActive()
        {
            if (ModulesRepository.IsActiveModule("RussianPostPrintBlank"))
                return "true";
            else return "false";
        }

        [HttpPost]
        public JsonResult FeedbackSend(FeedbackSettingsModel settings)
        {
            if(!FeedbackValidate(settings))
            {
                return Json(new { success = false, msg = "Заполните поле 'Сообщение'" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var mailBody = settings.Message + "<br /><br />";
                mailBody += string.Format("URL магазина: {0}; ФИО: {1}; Почта администратора: {2}; Телефон: {3}", 
                    AdvantShop.Configuration.SettingsMain.SiteUrl,
                    settings.Name,
                    settings.Email,
                    settings.Phone);

                var mailSubject = "Обратная связь. Модуль Печать бланков почты РФ.";
                ModulesService.SendModuleMail(Guid.Empty, mailSubject, mailBody, "help@promo-z.ru", true);
            }
            catch (Exception ex)
            {
                AdvantShop.Diagnostics.Debug.Log.Error("RPPB Module, feedback error: " + ex.Message);
                return Json(new { success = false, msg = "Не удалось отправить сообщение" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, msg = "Сообщение успешно отправлено" }, JsonRequestBehavior.AllowGet);
        }

        public bool FeedbackValidate(FeedbackSettingsModel settings)
        {
            if(string.IsNullOrEmpty(settings.Message))
            {
                return false;
            }

            return true;
        }
    }
}
