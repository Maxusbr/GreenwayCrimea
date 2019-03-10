using System;
using System.Web.Mvc;
using System.Linq;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using AdvantShop.Catalog;
using AdvantShop.Module.BannerMania.Service;
using AdvantShop.Module.BannerMania.ViewModel;
using AdvantShop.Module.BannerMania.Models;

namespace AdvantShop.Module.BannerMania.Controllers
{
    [Module(Type = "BannerMania")]
    public class BMClientController : ModuleController
    {
        string ModuleID = BannerMania.ModuleStringId;

        public ActionResult ShowBannerInTop()
        {
            var controllerName = Request.RequestContext.RouteData.Values["controller"].ToString();
            if (BMSettings.BannerInTopOnlyOnMainPage && controllerName != "Home")
            {
                return new EmptyResult();
            }

            if(string.IsNullOrEmpty(BMSettings.BannerInTopImagePath))
            {
                return new EmptyResult();
            }

            var model = new BannerInTopViewModel();
            model.URL = BMSettings.BannerInTopURL;
            model.ImagePath = "userfiles/Modules/BannerMania/Content/Pictures/BannerInTop/" + BMSettings.BannerInTopImagePath;
            model.TargetBlank = BMSettings.BannerInTopTargetBlank;
            
            return PartialView("~/modules/" + ModuleID + "/Views/Client/BannerInTop.cshtml", model);
        }

        public ActionResult ShowProductEntityBanners(Product product, Offer offer)
        {
            if (product == null)
            {
                return new EmptyResult();
            }

            var bannerEntities = BMService.GetBannerEntitiesByEntityId(product.ProductId).Where(banner => banner.Enabled).ToList();
            if (bannerEntities == null || bannerEntities.Count < 4)
            {
                var mainCategory = product.MainCategory;
                if (mainCategory == null) { return new EmptyResult(); }

                if(bannerEntities.Count < 1)
                {
                    bannerEntities = BMService.GetBannerEntitiesByEntityIdAndType(mainCategory.CategoryId, EntityType.ProductsByCategories).Where(banner => banner.Enabled).ToList();

                    if(bannerEntities.Count < 1)
                    {
                        return new EmptyResult();
                    }
                }
                else
                {
                    foreach (var placement in Enum.GetValues(typeof(PlacementType)))
                    {
                        var bannerEntity = BMService.GetBannerEntity(product.ProductId, EntityType.Products, (PlacementType)placement);
                        if(bannerEntity == null || !bannerEntity.Enabled)
                        {
                            bannerEntity = BMService.GetBannerEntity(mainCategory.CategoryId, EntityType.ProductsByCategories, (PlacementType)placement);
                            if(bannerEntity != null && bannerEntity.Enabled)
                            {
                                bannerEntities.Add(bannerEntity);
                            }
                        }
                    }
                }
            }
            
            return PartialView("~/modules/" + ModuleID + "/Views/Client/EntityBanners.cshtml", bannerEntities);
        }

        public ActionResult ShowCategoryEntityBanners(Category category)
        {
            if (category == null) { return new EmptyResult(); }

            var bannerEntities = BMService.GetBannerEntitiesByEntityIdAndType(category.CategoryId, EntityType.Categories).Where(banner => banner.Enabled).ToList();
            if (bannerEntities == null || bannerEntities.Count < 1) { return new EmptyResult(); }

            return PartialView("~/modules/" + ModuleID + "/Views/Client/EntityBanners.cshtml", bannerEntities);
        }
    }
}
