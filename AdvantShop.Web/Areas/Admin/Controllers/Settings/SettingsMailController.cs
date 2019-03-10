using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using AdvantShop.Core;
using AdvantShop.Core.Controls;
using AdvantShop.Customers;
using AdvantShop.Mails;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Settings.Mails;
using AdvantShop.Web.Admin.Models;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Admin.Models.SettingsMail;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings)]
    public partial class SettingsMailController : BaseAdminController
    {
        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.Settings.NotifyEMails.Title"));
            SetNgController(NgControllers.NgControllersTypes.MailSettingsCtrl);
            
            var model = new GetNotifyEmailsSettings().Execute();

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(MailSettingsModel model)
        {
            if (ModelState.IsValid)
            {
                new SaveNotifyEmailsSettings(model).Execute();
                ShowMessage(NotifyType.Success, "Изменения сохранены");
            }

            ShowErrorMessages();

            return RedirectToAction("Index");
        }
        

        public JsonResult GetMailFormats(MailFormatsFilterModel model)
        {
            var result = new GetMailFormatsHandler(model).Execute();
            return Json(result);
        }

        public JsonResult GetMailFormatTypes()
        {
            return Json(MailFormatService.GetMailFormatTypes());
        }

        public JsonResult GetMailFormatTypesSelectOptions()
        {
            return Json(MailFormatService.GetMailFormatTypes().Select(x => new SelectItemModel(x.TypeName, x.MailFormatTypeId.ToString())));
        }

        public JsonResult GetMailFormat(int id)
        {
            return Json(MailFormatService.Get(id));
        }

        [HttpPost]
        public JsonResult Add(MailFormatModel model)
        {
            if (ModelState.IsValid)
            {
                var mailFormatId = MailFormatService.Add(new MailFormat()
                {
                    MailFormatTypeId = model.MailFormatTypeId,
                    FormatName = model.FormatName,
                    FormatSubject = model.FormatSubject,
                    FormatText = model.FormatText,
                    Enable = model.Enable,
                    SortOrder = model.SortOrder,
                });

                if (mailFormatId > 0)
                    return Json(new {result = true});
            }

            var errors = new List<string>();

            foreach (var modelState in ViewData.ModelState.Values)
                foreach (var error in modelState.Errors)
                    errors.Add(error.ErrorMessage);

            return Json(new {result = false, errors = String.Join(", ", errors) });
        }

        [HttpPost]
        public JsonResult Edit(MailFormat model)
        {
            var format = MailFormatService.Get(model.MailFormatId);

            format.FormatName = model.FormatName;
            format.FormatSubject = model.FormatSubject;
            format.FormatText = model.FormatText;
            format.Enable = model.Enable;
            format.SortOrder = model.SortOrder;

            MailFormatService.Update(format);

            return Json(new {result = true});
        }

        public JsonResult GetTypeDescription(int mailFormatTypeId)
        {
            var mailType = MailFormatService.GetMailFormatType(mailFormatTypeId);
            if (mailType != null)
                return Json(new {result = true, message = mailType.Comment});

            return Json(new {result = false, error = "Не найден тип с id =" + mailFormatTypeId});
        }

        public JsonResult DeleteMailFormat(int mailFormatId)
        {
            MailFormatService.Delete(mailFormatId);
            return Json(new {result = true, message = T("Admin.Settings.MailFormat.Deleted")});
        }

        #region Command

        private void Command(MailFormatsFilterModel model, Func<int, MailFormatsFilterModel, bool> func)
        {
            if (model.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in model.Ids)
                {
                    func(id, model);
                }
            }
            else
            {
                var handler = new GetMailFormatsHandler(model);
                var mailFormatIds = handler.GetItemsIds();

                foreach (int id in mailFormatIds)
                {
                    if (model.Ids == null || !model.Ids.Contains(id))
                        func(id, model);
                }
            }
        }

        [HttpPost]
        public JsonResult DeleteMailFormats(MailFormatsFilterModel command)
        {
            Command(command, (id, c) =>
            {
                MailFormatService.Delete(id);
                return true;
            });
            return Json(true);
        }

        #endregion


        #region Inplace

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult InplaceMailFormat(MailFormat model)
        {
            var dbModel = MailFormatService.Get(model.MailFormatId);
            if (dbModel == null)
                return Json(new { result = false });

            dbModel.Enable = model.Enable;
            dbModel.SortOrder = model.SortOrder;
            MailFormatService.Update(dbModel);

            return Json(new { result = true });
        }

        #endregion

    }


    [Auth(RoleAction.Settings)]
    public partial class SettingsMailTestController : BaseAdminController
    {
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SendTestMessage(SendTestMessageModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = new SendTestMessageHandler().Execute(model);
                    if (result)
                        return JsonOk();
                }
                catch (BlException ex)
                {
                    ModelState.AddModelError(ex.Property, ex.Message);
                }
            }
            return JsonError();
        }
    }
}
