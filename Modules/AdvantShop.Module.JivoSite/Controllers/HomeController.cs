using System.Web.Mvc;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using AdvantShop.Module.JivoSite.Domain;
using Newtonsoft.Json;
using System.IO;
using System;
using AdvantShop.Diagnostics;

namespace AdvantShop.Module.JivoSite.Controllers
{
    [Module(Type = "JivoSite")]
    public partial class HomeController : ModuleController
    {
        public ActionResult Webhook()
        {
            var str = new StreamReader(System.Web.HttpContext.Current.Request.InputStream).ReadToEnd().Replace("event = ", "");

            if (string.IsNullOrWhiteSpace(str))
            {
                return Json(new JivoBaseResponse() { result = string.Format("Error. Empty params") });
            }

            JivoRequest request;
            try
            {
                request = JsonConvert.DeserializeObject<JivoRequest>(str);
            }
            catch
            {
                return Json(new JivoBaseResponse() { result = string.Format("Error. Invalid json:" + str) });
            }

            if (request == null)
            {
                return Json(new JivoBaseResponse() { result = string.Format("Error. Null object") });
            }

            switch (request.event_name)
            {
                case "chat_accepted":
                    return Content(JsonConvert.SerializeObject(JivoService.ChatAccepted(request), new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
                case "chat_finished":
                case "chat_updated":
                    return Json(JivoService.ChatFinished(request));
                case "offline_message":
                    return Json(JivoService.OfflineMessage(request));
                default:
                    return Json(new JivoBaseResponse());
            }

            return Json(new JivoBaseResponse() { result = string.Format("Error. event_name '{0}' is not supported", request.event_name) });
        }
    }
}
