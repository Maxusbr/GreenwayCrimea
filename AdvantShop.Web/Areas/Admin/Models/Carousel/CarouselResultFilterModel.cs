using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Catalog;

namespace AdvantShop.Web.Admin.Models.Carousel
{
    public class CarouselResultFilterModel : BaseFilterModel
    {
        public int CarouselId { get; set; }
        public int SortOrder { get; set; }
        public string CaruselUrl { get; set; }
        public bool Enabled { set; get; }

        public bool DisplayInOneColumn { get; set; }
        public bool DisplayInTwoColumns { get; set; }
        public bool DisplayInMobile { get; set; }
        public bool Blank { get; set; }
        public string ImageSrc { get; set; }
        public string PhotoName { get; set; }
        public string Description { get; set; }

        private CarouselPhoto _picture;

        public CarouselPhoto Picture
        {
            get
            {
                return _picture ??
                       (_picture = PhotoService.GetPhotoByObjId<CarouselPhoto>(CarouselId, PhotoType.Carousel));
            }
            set { _picture = value; }
        }
    }
}