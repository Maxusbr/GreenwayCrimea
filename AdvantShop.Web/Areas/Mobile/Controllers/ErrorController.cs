using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AdvantShop.Areas.Mobile.Controllers
{
    public class ErrorController : BaseMobileController
    {
        // 404
        public ActionResult NotFound()
        {
            SetResponse(HttpStatusCode.NotFound);

            var ext = VirtualPathUtility.GetExtension(Request.RawUrl);
            if (ext != null)
            {
                var list = new List<string> { ".css", ".js", ".jpg", ".jpeg", ".png", ".map", ".ico" };
                if (list.Contains(ext.ToLower()))
                    return new EmptyResult();
            }

            SetMetaInformation(T("Error.NotFound.Title"));

            return View();
        }

        private void SetResponse(HttpStatusCode httpStatusCode)
        {
            Response.Clear();
            Response.StatusCode = (int)httpStatusCode;
            Response.StatusDescription = HttpWorkerRequest.GetStatusDescription((int)httpStatusCode);
            Response.TrySkipIisCustomErrors = true;
        }
    }
}