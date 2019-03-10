using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace AdvantShop.Web.Infrastructure.Extensions
{
    public static class RazorExtensions
    {
        public static string RenderForce(this ControllerBase controller, string viewName, object model)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller", "Extension method called on a null controller");
            }

            if (controller.ControllerContext == null)
            {
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(viewName)) return string.Empty;
            
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        
        public static bool HasFile(this HttpPostedFileBase file)
        {
            return file != null && file.ContentLength > 0;
        }   
    }
}