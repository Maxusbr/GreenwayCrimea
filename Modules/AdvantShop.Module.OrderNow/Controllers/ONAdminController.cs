using AdvantShop.Security;
using AdvantShop.Web.Infrastructure.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Module.OrderNow.ViewModel;
using AdvantShop.Module.OrderNow.Models;
using AdvantShop.Module.OrderNow.Service;
using Newtonsoft.Json;
using System.Drawing;
using System.Web.Hosting;
using System.IO;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Module.OrderNow.Controllers
{
    public class ONAdminController : ModuleAdminController
    {
        string ModuleID = OrderNow.ModuleStringId;

        public ActionResult Index()
        {
            var model = new SettingViewmodel();
            return PartialView("~/modules/" + OrderNow.ModuleStringId + "/Views/Admin/Index.cshtml",model);
        }

        public ActionResult OrderNowTab()
        {
            var model = new AdminTabViewmodel();
            var id = Request.Url.AbsoluteUri.Substring(Request.Url.AbsoluteUri.LastIndexOf('/')+1);
            model = ModuleService.GetTabsContent(id);
            if (model == null)
            {
                model = new AdminTabViewmodel();
                model.Message = string.Empty;
                model.TimeoutMessage = string.Empty;
                model.productid = id.TryParseInt();
            }
            return PartialView("~/modules/" + OrderNow.ModuleStringId + "/Views/Admin/OrderNowTab.cshtml", model);
        }

        [HttpPost]
        public ActionResult SaveSettings(SaveSettings send)
        {
            ModuleSettings.Message = send.text;
            ModuleSettings.TimeoutTime = send.timeout;
            ModuleSettings.IncludeWeekend = send.weekends;
            ModuleSettings.LookForAvailability = send.checkAvailability;
            ModuleSettings.ShowInCart = send.showInCart;
            ModuleSettings.ShowInOrderConfirm = send.showInOrderConfirm;
            ModuleSettings.ShowAt = send.ShowAt;
            ModuleSettings.IconHeight = send.IconHeight.TryParseInt();
            ModuleSettings.ShowStart = send.ShowStart;
            ModuleSettings.ShowFinish = send.ShowFinish;
            ModuleSettings.Ndays = send.Ndays.TryParseInt();
            ModuleSettings.ShowInMobile = send.ShowMobile;
            ModuleSettings.TimeoutMessage = send.TimeoutMessage;

            var model = new SettingViewmodel();
            return PartialView("~/modules/" + OrderNow.ModuleStringId + "/Views/Admin/Index.cshtml", model);
        }

        [HttpPost]
        public void AddUpdateProductTimeoutMessage(int productid, string message)
        {
            ModuleService.AddUpdateProductTimeoutMessage(productid.ToString(), message);
        }

        [HttpPost]
        public void AddUpdateProductMessage(int productid, string message)
        {
            ModuleService.AddUpdateProductMessage(productid.ToString(), message);
        }

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase image)
        {

            try
            {
                var accept = Image.FromStream(image.InputStream);
            }
            catch (Exception ex)
            {
                return Json(new { error = true, message = "На сервер можно загрузить только картинку" });
            }

            var path = HostingEnvironment.MapPath("~/modules/" + OrderNow.ModuleStringId + "/");
            path += "content/images/";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var fileName = "icon.png";
            path += fileName;

            image.SaveAs(path);
            ModuleSettings.IconUsed = true;
            var model = new SettingViewmodel();
            return PartialView("~/modules/" + OrderNow.ModuleStringId + "/Views/Admin/Index.cshtml", model);
        }

        [HttpPost]
        public ActionResult DeleteImage()
        {
            ModuleService.DeleteImage();
            var model = new SettingViewmodel();
            return PartialView("~/modules/" + OrderNow.ModuleStringId + "/Views/Admin/Index.cshtml", model);
        }

        public string LinkScriptStyle()
        {
            var scripts = "<script src=\"../modules/" + ModuleID + "/content/scripts/admin-script.js\"></script>";
            var styles = "<link rel=\"stylesheet\" href=\"../modules/" + ModuleID + "/content/styles/admin-style.css\">";
            return scripts + styles;
        }

    }
}
