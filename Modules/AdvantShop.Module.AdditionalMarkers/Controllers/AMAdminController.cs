using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Module.AdditionalMarkers.Models;
using AdvantShop.Module.AdditionalMarkers.Service;
using AdvantShop.Security;
using AdvantShop.Web.Infrastructure.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AdvantShop.Module.AdditionalMarkers.Controllers
{
    public class AMAdminController : ModuleAdminController
    {
        [ChildActionOnly]
        public ActionResult ModuleSettings()
        {
            return PartialView("~/modules/" + AdditionalMarkers.ModuleStringId + "/Views/Admin/_ModuleSettings.cshtml");
        }

        #region feedback

        [ChildActionOnly]
        public ActionResult Feedback()
        {
            return PartialView("~/Modules/" + AdditionalMarkers.ModuleStringId + "/Views/Admin/_Feedback.cshtml");
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

                var mailSubject = "Обратная связь. Дополнительные маркеры.";
                ModulesService.SendModuleMail(Guid.Empty, mailSubject, mailBody, "help@promo-z.ru", true);
            }
            catch (Exception ex)
            {
                AdvantShop.Diagnostics.Debug.Log.Error("AdditionalMarkers Module, feedback error: " + ex.Message);
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

        #endregion

        public JsonResult GetAllMarkers()
        {
            var model = MarkerService.GetMarkers();
            return Json(model.OrderBy(x => x.SortOrder), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActionMarker(Marker marker, bool editIdentity = false, int oldMarkerId = -1)
        {
            var productIds = new List<int>();
            if (string.IsNullOrEmpty(marker.Name))
            {
                marker.MarkerId = MarkerService.DeleteMarker(marker.MarkerId) ? marker.MarkerId : -1;
            }
            else
            {
                if (marker.MarkerId != -1 && editIdentity)
                {
                    var ids = MarkerService.GetKeys();
                    if (ids.Contains(marker.MarkerId))
                    {
                        return Json(new { success = false, msg = "Маркер с таким идентификатором уже существует в базе данных", marker });
                    }
                    else
                    {
                        if (oldMarkerId != -1)
                        {
                            productIds = MarkerService.GetLinksByMarkerId(oldMarkerId);
                            MarkerService.DeleteMarker(oldMarkerId);
                        }
                    }
                }

                marker.MarkerId = MarkerService.InsertOrUpdateMarker(marker, editIdentity);

                if (editIdentity && productIds != null && marker.MarkerId != oldMarkerId)
                {
                    foreach (var productId in productIds)
                    {
                        MarkerService.Link(productId, marker.MarkerId);
                    }
                }
            }
            return Json(new { success = true, msg = "", marker }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMarkers(int productId)
        {
            return Json(MarkerService.GetLinks(productId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AdminProductTab(int productId)
        {
            return PartialView("~/modules/" + AdditionalMarkers.ModuleStringId + "/Views/Admin/_AdminProductTab.cshtml", productId);
        }

        [HttpPost]
        public JsonResult Link(int productId, int markerId)
        {
            return Json(MarkerService.Link(productId, markerId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSorting()
        {
            return Json(MarkerService.CurrentSortOrder(), JsonRequestBehavior.AllowGet);
        }
    }
}
