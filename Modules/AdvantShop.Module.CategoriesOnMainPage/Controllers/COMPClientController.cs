using AdvantShop.Module.CategoriesOnMainPage.Models;
using AdvantShop.Module.CategoriesOnMainPage.Service;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdvantShop.Module.CategoriesOnMainPage.Controllers
{
    [Module(Type = "CategoriesOnMainPage")]
    public class COMPClientController : ModuleController
    {
        string ModuleID = CategoriesOnMainPage.ModuleStringId;

        public string LinkScriptStyle()
        {
            var scripts = "<script src=\"modules/" + ModuleID + "/content/scripts/client-script.js\"></script>";
            var styles = "<link rel=\"stylesheet\" href=\"modules/" + ModuleID + "/content/styles/client-style.css\">";
            return scripts + styles;
        }

        public ActionResult ShowCategories()
        {
            var model = new CategoriesOnMainPageView();

            var categories = COMPService.GetCategories();
            model.Categories.AddRange(categories);
            model.CountCategoriesInLine = COMPSettings.PicturesQuantityInLine;

            return PartialView("~/modules/" + ModuleID + "/Views/Client/ShowCategories.cshtml", model);
        }

        public ActionResult Location()
        {
            return Content(COMPSettings.UnderCarousel.ToString());
        }

    }
}
