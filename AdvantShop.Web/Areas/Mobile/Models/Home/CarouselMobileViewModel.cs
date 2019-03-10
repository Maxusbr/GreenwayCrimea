using System.Collections.Generic;
using AdvantShop.Core.Services.CMS;

namespace AdvantShop.Areas.Mobile.Models.Home
{
    public class CarouselMobileViewModel
    {
        public List<Carousel> Sliders { get; set; }

        public int Speed { get; set; }

        public int Pause { get; set; }
    }
}