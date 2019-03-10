using System;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Models;
using AdvantShop.Core.Services.SEO;
using AdvantShop.Handlers.Brands;
using AdvantShop.Models.Brand;
using AdvantShop.SEO;
using AdvantShop.Web.Infrastructure.Extensions;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Core.Services.SEO.MetaData;

namespace AdvantShop.Controllers
{
    public partial class BrandController : BaseClientController
    {
        public ActionResult Index(string letter, string countryId, string q, int? page)
        {
            var model = new BrandHandler().GetBrandList(letter, countryId, q, page);
            if (model == null)
                return Error404();

            model.BrandLogoHeight = SettingsPictureSize.BrandLogoHeight;

            model.Pager = new Pager()
            {
                CurrentPage = page != null && page > 0 ? (int) page : 1,
                TotalPages = (int) (Math.Ceiling((double) model.Brands.Count/ SettingsCatalog.BrandsPerPage))
            };

            if (model.Brands.Count > 0)
            {
                if ((model.Pager.TotalPages < model.Pager.CurrentPage && model.Pager.CurrentPage > 1) ||
                    model.Pager.CurrentPage < 0)
                {
                    return Error404();
                }

                model.Brands =
                    model.Brands.Skip((model.Pager.CurrentPage - 1)* SettingsCatalog.BrandsPerPage).Take(SettingsCatalog.BrandsPerPage).ToList();
            }

            SetMetaInformation(
                new MetaInfo
                {
                    Type = MetaType.Default,
                    Title = SettingsSEO.BrandMetaTitle,
                    MetaKeywords = SettingsSEO.BrandMetaKeywords,
                    MetaDescription = SettingsSEO.BrandMetaDescription,
                    H1 = SettingsSEO.BrandMetaTitle
                },
                page: model.Pager.CurrentPage);

            var meta = new OpenGraphModel();
            foreach(var brand in model.Brands)
            {
                meta.Images.Add(brand.BrandLogo.ImageSrc());
            }

            MetaDataContext.CurrentObject = meta;

            SetNgController(NgControllers.NgControllersTypes.BrandCtrl);

            return View(model);
        }


        public ActionResult BrandItem(string url, BrandModel brandModel)
        {
            var brand = BrandService.GetBrand(url);
            if (brand == null || !brand.Enabled)
                return Error404();

            var model = new BrandHandler().GetBrandItem(brand, brandModel);
            if (model == null)
                return Error404();

            SetMetaInformation(brand.Meta, brand.Name);
            SetNgController(NgControllers.NgControllersTypes.BrandCtrl);

            var meta = new OpenGraphModel();
            meta.Images.Add(brand.BrandLogo.ImageSrc());

            MetaDataContext.CurrentObject = meta;

            WriteLog(brand.Name, Url.AbsoluteRouteUrl("Brand", brand.UrlPath), ePageType.brand);

            return View(model);
        }

        public ActionResult BrandCarousel(int count)
        {
            if (SettingsDesign.BrandCarouselVisibility)
            {
                var brands = BrandService.GetBrandsOnLimit(count);
                return PartialView(brands);
            }
            return null;
        }
    }
}