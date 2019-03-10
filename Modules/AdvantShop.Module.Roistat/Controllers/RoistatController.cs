using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.Module.Roistat.Domain;
using AdvantShop.Module.Roistat.Handlers;
using AdvantShop.Module.Roistat.Models.Roistat;
using AdvantShop.Web.Infrastructure.Controllers;

namespace AdvantShop.Module.Roistat.Controllers
{
    public class RoistatController : ModuleController
    {
        [ChildActionOnly]
        public ActionResult RoistatScript()
        {
            return Content(RoistatSettings.RoistatScript);
        }


        [HttpGet]
        public JsonResult GetOrders(RoistatOrdersExportModel model)
        {
            Debug.Log.Info(Request.Url.ToString());

            var error = "";
            if (!CheckAccess(model.User, model.Token, out error))
                return Json(error);

            var action = (string)Request["action"];
            if (action != null && action == "export_clients")
                return Json(new GetClients(model).Execute());

            return Json(new GetOrders(model).Execute());
        }

        [HttpGet]
        public JsonResult GetClients(RoistatOrdersExportModel model)
        {
            var error = "";
            if (!CheckAccess(model.User, model.Token, out error))
                return Json(error);

            return Json(new GetClients(model).Execute());
        }

        public ActionResult OrdersRedirect(string id)
        {
            if (id != null && id.StartsWith("lead"))
                return Redirect(UrlService.GetUrl("adminv2/leads/edit/" + id.Replace("lead_", "")));

            return Redirect(UrlService.GetUrl("adminv2/orders/edit/" + (id != null ? id.Replace("order_", "") : "")));
        }

        [HttpPost]
        public JsonResult SaveRoistatCookie(string rositatId, int entityId, RoistatEntityType type)
        {
            RoistatService.AddUpdateRoistatOrder(entityId, type, rositatId);

            if (type == RoistatEntityType.Order)
                SQLDataAccess.ExecuteNonQuery(
                    "UPDATE [Order].[Order] Set [ModifiedDate] = Getdate() Where OrderId = @OrderId", CommandType.Text,
                    new SqlParameter("@OrderId", entityId));

            return JsonOk();
        }

        private bool CheckAccess(string user, string token, out string error)
        {
            error = "";

            if (string.IsNullOrWhiteSpace(RoistatSettings.RoistatLogin) ||
                string.IsNullOrWhiteSpace(RoistatSettings.RoistatPassword))
                return true;

            if (string.IsNullOrWhiteSpace(user) || user.ToLower() != RoistatSettings.RoistatLogin.ToLower())
            {
                error = "Ошибка авторизации: проверьте имя пользователя в настройках модуля и Roistat";
                return false;
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                error = "Ошибка авторизации: проверьте token";
                return false;
            }

            var result = (user + RoistatSettings.RoistatPassword).Md5(false) == token;
            if (!result)
                error = "Ошибка авторизации: неверный token. Проверьте пароль в настройках модуля";

            return result;
        }
    }
}
