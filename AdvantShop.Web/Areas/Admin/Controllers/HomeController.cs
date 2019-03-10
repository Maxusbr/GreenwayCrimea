using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.DownloadableContent;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Home;
using AdvantShop.Web.Admin.Models.Home;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers
{
    public partial class HomeController : BaseAdminController
    {
        // GET: Admin/Home        
        public ActionResult Index()
        {
            SetMetaInformation(null, string.Empty);
            SetNgController(NgControllers.NgControllersTypes.HomeCtrl);

            var model = new AdminHomeViewModel()
            {
                ActionText = ShopActionsService.GetLast()
            };

            return View(model);
        }

        public ActionResult CongratulationsDashboard()
        {
            SetMetaInformation(null, string.Empty);
            return View();
        }

        [ChildActionOnly]
        [Auth(RoleAction.Orders)]
        public ActionResult OrdersDashboard()
        {
            return PartialView(new GetOrdersDasboard().Execute());
        }


        [ChildActionOnly]
        [Auth(RoleAction.Orders)]
        public ActionResult OrderSourcesDashboard()
        {
            return PartialView(new GetOrderSourcesDasboard().Execute());
        }

        [ChildActionOnly]        
        public ActionResult LearningDashboard()
        {
            var model = new GetLearningDasboard().Execute();
            if (model.News.Count == 0)
                return new EmptyResult();

            return PartialView(model);
        }

        [ChildActionOnly]        
        public ActionResult RecommendationsDashboard()
        {
            var model = new GetRecommendationsDasboard().Execute();
            if (model.News.Count == 0)
                return new EmptyResult();

            return PartialView(model);
        }
        
        [ChildActionOnly]        
        public ActionResult PartnersDashboard()
        {
            var model = new GetPartnersDasboard().Execute();
            if (model.News.Count == 0)
                return new EmptyResult();

            return PartialView(model);
        }

        [ChildActionOnly]
        [Auth(RoleAction.Orders)]
        public ActionResult SalesPlanDashboard()
        {
            return PartialView(new GetSalesPlanDasboard().Execute());
        }
        
        [ChildActionOnly]
        [Auth(RoleAction.Orders)]
        public ActionResult StatisticsDashboard()
        {
            return PartialView(new GetStatisticsDasboard().Execute());
        }

        [HttpPost]
        [Auth(RoleAction.Orders)]
        public ActionResult SaveStatisticsSettings(StatisticsDashboardModelSetting model)
        {
            var handler = new SaveStatisticsDasboard(model);
            handler.Execute();

            return RedirectToAction("Index");
        }


        [ChildActionOnly]
        [Auth(RoleAction.Orders)]
        public ActionResult OrderGraphDashboard()
        {
            return PartialView(new GetOrderGraphDasboard().Execute());
        }

        [ChildActionOnly]
        [Auth(RoleAction.Orders)]
        public ActionResult LastOrdersDashboard(string status)
        {
            return PartialView();
        }

        [Auth(RoleAction.Orders)]
        public JsonResult GetLastOrders(string status)
        {
            var model = new GetLastOrdersDashboard(status).Execute();
            return Json(new { DataItems = model });
        }

        [ChildActionOnly]
        [Auth(RoleAction.Customers)]
        public ActionResult BirthdayDashboard(string status)
        {
            return PartialView(new GetBirthdayDasboard().Execute());
        }
        
        [ChildActionOnly]
        public ActionResult UserInformation()
        {
            if (!Trial.TrialService.IsTrialEnabled || CustomerContext.CurrentCustomer.IsVirtual)
                return new EmptyResult();
            
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetUserInformation()
        {
            return Json(new GetUserInformationModel().Execute());
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SaveUserInformation(AdditionClientInfo data)
        {
            return Json(new
            {
                result = new GetUserInformationModel().Save(data),
                fio = CustomerContext.CurrentCustomer.FirstName + " " + CustomerContext.CurrentCustomer.LastName
            });
        }

        [HttpGet]
        public JsonResult GetAdminShopName()
        {
            return Json(new {name = SettingsMain.AdminShopName});
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SaveAdminShopName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                SettingsMain.AdminShopName = name;
            return Json(new {name = SettingsMain.AdminShopName});
        }
    }
}