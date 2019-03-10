//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Web.Mvc;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Module.FindCheaper.Controllers
{
    [Module(Type = "FindCheaperModule")]
    public partial class HomeController : ModuleController
    {
        [HttpPost]
        public JsonResult AddRequest(FindCheaperRequest model)
        {
            FindCheaperService.AddRequest(model);

            return Json(new EmptyResult());
        }
    }
}