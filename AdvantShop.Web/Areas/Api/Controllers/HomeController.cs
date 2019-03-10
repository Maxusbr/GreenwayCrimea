using System.Web.Mvc;

namespace AdvantShop.Areas.Api.Controllers
{
    public class HomeController : BaseApiController
    {
        // GET: Api/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}