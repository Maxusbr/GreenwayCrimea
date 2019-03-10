using System.Collections.Generic;
using AdvantShop.CMS;
using AdvantShop.Core.Services.CMS;
using AdvantShop.Models;

namespace AdvantShop.ViewModel.Home
{
    public partial class CarouselViewModel : BaseModel
    {
        public string CssSlider { get; set; }

        public int Speed { get; set; }

        public int Pause { get; set; }

        public List<Carousel> Sliders { get; set; }
    }
}