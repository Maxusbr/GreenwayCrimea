using System.Web.Mvc;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Configuration.Settings;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Controllers;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Crm)]
    [SaasFeature(Saas.ESaasProperty.HaveCrm)]
    public class SettingsCrmController : BaseAdminController
    {
        public ActionResult Index()
        {
            var model = new CrmSettingsModel()
            {
                FinalDealStatusId = SettingsCrm.FinalDealStatusId,
                OrderStatusIdFromLead = SettingsCrm.OrderStatusIdFromLead
            };

            var finalDealStatus = model.DealStatuses.Find(x => x.Value == model.FinalDealStatusId.ToString());
            if (finalDealStatus != null)
                finalDealStatus.Selected = true;

            var orderStatus = model.OrderStatuses.Find(x => x.Value == model.OrderStatusIdFromLead.ToString());
            if (orderStatus != null)
                orderStatus.Selected = true;


            SetMetaInformation(T("Admin.Settings.CRM.Title"));
            SetNgController(NgControllers.NgControllersTypes.SettingsCtrl);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(CrmSettingsModel model)
        {
            if (ModelState.IsValid)
            {
                SettingsCrm.FinalDealStatusId = model.FinalDealStatusId;
                SettingsCrm.OrderStatusIdFromLead = model.OrderStatusIdFromLead;

                ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));
            }

            return Index();
        }
    }
}
