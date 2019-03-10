//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Catalog;

namespace AdvantShop.Core.Services.CMS
{
    public enum ECarouselPageMode
    {
        OneColumn = 0,
        TwoColumns = 1,
        Mobile = 2
    }


    public class Carousel
    {
        public int CarouselId { get; set; }
        public int SortOrder { get; set; }
        public string Url { get; set; }
        public bool Enabled { set; get; }

        public bool DisplayInOneColumn { get; set; }
        public bool DisplayInTwoColumns { get; set; }
        public bool DisplayInMobile { get; set; }
        public bool Blank { get; set; }

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