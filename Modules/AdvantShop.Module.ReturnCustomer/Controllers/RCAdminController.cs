using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using System.Web.Mvc;
using AdvantShop.Module.ReturnCustomer.Service;
using AdvantShop.Module.ReturnCustomer.Models;
using AdvantShop.Core.Scheduler;
using AdvantShop.Module.ReturnCustomer.Handlers;
using AdvantShop.Web.Infrastructure.Admin;
using System;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.ReturnCustomer.Controllers
{
    public class RCAdminController : ModuleAdminController
    {
        [ChildActionOnly]
        public ActionResult Settings()
        {
            return PartialView("~/Modules/" + ReturnCustomer.ModuleStringId + "/Views/Admin/Settings.cshtml");
        }

        [ChildActionOnly]
        public ActionResult RecordsList()
        {
            return PartialView("~/Modules/" + ReturnCustomer.ModuleStringId + "/Views/Admin/RecordsList.cshtml");
        }

        [ChildActionOnly]
        public ActionResult Feedback()
        {
            return PartialView("~/Modules/" + ReturnCustomer.ModuleStringId + "/Views/Admin/Feedback.cshtml");
        }

        [HttpGet]
        public ActionResult GetSettings()
        {
            var settings = new ReturnCustomerSettings
            {
                MessageSubject = RCSettings.MessageSubject,
                MessageText = RCSettings.MessageText,
                AlternativeMessageSubject = RCSettings.AlternativeMessageSubject,
                AlternativeMessageText = RCSettings.AlternativeMessageText,
                DisabledMailsList = RCSettings.DisabledMailsList,
                DaysInterval = RCSettings.DaysInterval,
                AutoSending = RCSettings.AutoSending
            };
            
            var logFile = AdvantShop.Configuration.SettingsGeneral.AbsolutePath + "userfiles\\Modules\\" + ReturnCustomer.ModuleStringId + "\\log\\SendingLog.txt";
            var isLogExists = System.IO.File.Exists(logFile);

            return Json(new { isLogExists, settings }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveChanges(ReturnCustomerSettings settings)
        {
            RCSettings.MessageSubject = settings.MessageSubject ?? string.Empty;
            RCSettings.MessageText = settings.MessageText ?? string.Empty;
            RCSettings.AlternativeMessageSubject = settings.AlternativeMessageSubject ?? string.Empty;
            RCSettings.AlternativeMessageText = settings.AlternativeMessageText ?? string.Empty;
            RCSettings.DisabledMailsList = settings.DisabledMailsList ?? string.Empty;
            RCSettings.DaysInterval = settings.DaysInterval;
            RCSettings.AutoSending = settings.AutoSending;

            TaskManager.TaskManagerInstance().ManagedTask(TaskSettings.Settings);

            return Json(new { success = true, msg = "Сохранено" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteLog()
        {
            try
            {
                var logFile = AdvantShop.Configuration.SettingsGeneral.AbsolutePath + "userfiles\\Modules\\" + ReturnCustomer.ModuleStringId + "\\log\\SendingLog.txt";
                if(System.IO.File.Exists(logFile))
                {
                    System.IO.File.Delete(logFile);
                }
            }
            catch(System.Exception ex)
            {
                AdvantShop.Diagnostics.Debug.Log.Error("Delete log of ReturnCustomer module: " + ex.Message);
                return Json(new { success = true, msg = "Не удалось удалить лог отправленных писем" }, JsonRequestBehavior.AllowGet);
            }
            
            return Json(new { success = true, msg = "Лог отправленных писем удалён" }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult Inplace(ReturnCustomerRecord model)
        {
            var dbModel = RCService.GetReturnCustomerRecord(model.CustomerID);
            if (dbModel == null)
                return Json(new { result = false });

            return Json(new { result = true });
        }

        public JsonResult GetReturnCustomerRecords(ReturnCustomerRecordFilterModel model)
        {
            return Json(new GetReturnCustomerRecords(model).Execute());
        }

        private void Command(ReturnCustomerRecordFilterModel command, Func<Guid, ReturnCustomerRecordFilterModel, bool> func)
        {
            if (command.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in command.Ids)
                {
                    func(id, command);
                }
            }
            else
            {
                var handler = new GetReturnCustomerRecords(command);
                var ids = handler.GetItemsIds();

                foreach (var id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SendMails(ReturnCustomerRecordFilterModel command)
        {
            Command(command, (id, c) =>
            {
                RCService.SendMailToCustomer(id);
                return true;
            });
            return JsonOk();
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
        public JsonResult FeedbackSend(FeedbackSettingsModel settings)
        {
            if (!FeedbackValidate(settings))
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

                var mailSubject = "Обратная связь. Модуль 'Верните покупателя'.";
                ModulesService.SendModuleMail(Guid.Empty, mailSubject, mailBody, "help@promo-z.ru", true);

            }
            catch (Exception ex)
            {
                AdvantShop.Diagnostics.Debug.Log.Error("ReturnCustomer Module, feedback error: " + ex.Message);
                return Json(new { success = false, msg = "Не удалось отправить сообщение" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, msg = "Сообщение успешно отправлено" }, JsonRequestBehavior.AllowGet);
        }

        public bool FeedbackValidate(FeedbackSettingsModel settings)
        {
            if (string.IsNullOrEmpty(settings.Message))
            {
                return false;
            }

            return true;
        }
    }
}
