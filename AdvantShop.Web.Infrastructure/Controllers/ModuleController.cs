using System.Web.Mvc;
using AdvantShop.Core.Controls;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Infrastructure.Controllers
{
    public abstract class ModuleController : BaseController
    {
        protected ActionResult Error404()
        {
            System.Web.HttpContext.Current.Server.TransferRequest("/error/notfound");
            return new EmptyResult();
        }

        protected void ShowErrorMessages()
        {
            foreach (var modelState in ViewData.ModelState.Values)
                foreach (var error in modelState.Errors)
                    ShowMessage(NotifyType.Error, error.ErrorMessage);
        }
    }

    [AdminAuth]
    public abstract class ModuleAdminController : ModuleController
    {

    }
}