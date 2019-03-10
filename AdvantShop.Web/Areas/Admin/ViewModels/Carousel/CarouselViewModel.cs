using AdvantShop.Web.Infrastructure.Localization;
using AdvantShop.Core.Services.Localization;

namespace AdvantShop.Web.Admin.ViewModels.Carousel
{
    public class CarouselViewModel
    {
        public CarouselViewModel()
        {
            Header = LocalizationService.GetResource("Admin.Carousel.Index.Title");
            ButtonTextAdd = new LocalizedString(LocalizationService.GetResource("Admin.Carousel.Index.AddCarousel"));
        }
        public string Header { get; set; }

        public LocalizedString ButtonTextAdd { get; set; }
    }
}
