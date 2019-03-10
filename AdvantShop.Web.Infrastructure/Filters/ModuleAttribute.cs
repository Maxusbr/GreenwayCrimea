using System.Web;
using System.Web.Mvc;
using AdvantShop.Core.Modules;

namespace AdvantShop.Web.Infrastructure.Filters
{
    /// <summary>
    /// Check module activity
    /// </summary>
    public class ModuleAttribute : ActionFilterAttribute
    {
        public string Type { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;

            if (!ModulesRepository.IsActiveModule(Type))
            {
                HttpContext.Current.Server.TransferRequest("error/notfound");
            }
        }
    }
}
