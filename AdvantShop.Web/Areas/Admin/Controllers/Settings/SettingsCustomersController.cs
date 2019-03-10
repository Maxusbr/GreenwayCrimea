using System;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Scheduler;
using AdvantShop.Core.Services.Vk;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Settings.Customers;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings)]
    public partial class SettingsCustomersController : BaseAdminController
    {
        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.Settings.Customers.Title"));
            SetNgController(NgControllers.NgControllersTypes.SettingsCustomersCtrl);

            var model = new GetCustomersSettingsHandler().Execute();
            return View("Index", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(CustomersSettingsModel model)
        {
            if (ModelState.IsValid)
            {
                new SaveCustomersSettingsHandler(model).Execute();
                ShowMessage(NotifyType.Success, T("Admin.Settings.SaveSuccess"));
            }
            else
            {
                ShowErrorMessages();
            }

            return Index();
        }
    }
}
