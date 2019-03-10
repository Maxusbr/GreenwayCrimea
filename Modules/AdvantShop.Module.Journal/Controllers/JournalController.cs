using System.Linq;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Module.Journal.Domain;
using AdvantShop.Module.Journal.Models;
using AdvantShop.Module.Journal.Services;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using Newtonsoft.Json;

namespace AdvantShop.Module.Journal.Controllers
{
    [Module(Type = "Journal")]
    public class JournalController : ModuleController
    {
        public ActionResult Cover()
        {
            var model = new JournalCoverViewModel()
            {
                ColorScheme = ((JournalCoverType) JournalModuleSetting.CoverType).ToString().ToLower(),
            };

            return View("~/Modules/Journal/Views/Journal/Cover.cshtml", model);
        }

        public ActionResult Pdf(int page, int categoryId, bool isLeft, int pageIndex = 1, string categoryName = null, bool indepth = false)
        {
            var colorScheme = (JournalCoverType) JournalModuleSetting.CoverType;
            var productViewMode = (JournalViewMode) JournalModuleSetting.ViewMode;

            var products = JournalService.GetProducts(categoryId, page, indepth);
            var productModel = new JournalProductViewModel(products);

            var model = new JournalViewModel()
            {
                DisplayBottom = true,
                DisplayHead = true,
                Page = pageIndex,
                IsLeft = isLeft,
                CategoryName = categoryName,
                ColorScheme = colorScheme.ToString().ToLower(),

                HeadBlock = JournalModuleSetting.CatalogPageHead,

                Products = productModel,
                ViewMode = productViewMode.ToString().ToLower(),
            };
            
            return View("~/Modules/Journal/Views/Journal/Pdf.cshtml", model);
        }

        [ChildActionOnly]
        public ActionResult SizeColorBlock(int productId, bool allowPreOrder)
        {
            var offers = OfferService.GetProductOffers(productId);

            var colors = offers.Where(o => o.Color != null && (o.Amount > 0 || allowPreOrder))
                        .OrderBy(o => o.Color.SortOrder)
                        .Select(x => new
                        {
                            x.Color.ColorId,
                            x.Color.ColorName,
                            x.Color.ColorCode,
                            x.Color.IconFileName.PhotoName
                        })
                        .Distinct();

            var sizes = offers.Where(o => o.Size != null && (o.Amount > 0 || allowPreOrder))
                        .OrderBy(o => o.Size.SortOrder)
                        .Select(x => new
                        {
                            x.Size.SizeId,
                            x.Size.SizeName
                        })
                        .Distinct();

            var model = new SizeColorPickerViewModel
            {
                Colors = colors.Any() ? JsonConvert.SerializeObject(colors) : string.Empty,
                Sizes = sizes.Any() ? JsonConvert.SerializeObject(sizes) : string.Empty,
                ColorIconWidthDetails = SettingsPictureSize.ColorIconWidthDetails,
                ColorIconHeightDetails = SettingsPictureSize.ColorIconHeightDetails,
                SizesHeader = SettingsCatalog.SizesHeader,
                ColorsHeader = SettingsCatalog.ColorsHeader
            };

            return PartialView("~/Modules/Journal/Views/Journal/SizeColorBlock.cshtml", model);
        }
    }
}
