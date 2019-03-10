//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Repository;
using AdvantShop.SEO;

namespace AdvantShop.Catalog
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BriefDescription { get; set; }

        private BrandPhoto _brandLogo;
        public BrandPhoto BrandLogo
        {
            get
            {
                return _brandLogo ?? (_brandLogo = PhotoService.GetPhotoByObjId<BrandPhoto>(BrandId, PhotoType.Brand));
            }
            set
            {
                _brandLogo = value;
            }
        }

        //public string BrandLogo { get; set; }
        public bool Enabled { get; set; }
        public int SortOrder { get; set; }
        public int CountryId { get; set; }
        public string BrandSiteUrl { get; set; }

        private Country _brandCountry;

        public Country BrandCountry
        {
            get { return _brandCountry ?? (_brandCountry = CountryId == 0 ? null : CountryService.GetCountry(CountryId)); }
        }

        private string _urlPath;
        public string UrlPath
        {
            get { return _urlPath; }
            set { _urlPath = value.ToLower(); }
        }

        private MetaInfo _meta;

        public MetaInfo Meta
        {
            get
            {
                return _meta ??
                       (_meta =
                           MetaInfoService.GetMetaInfo(BrandId, MetaType) ??
                           MetaInfoService.GetDefaultMetaInfo(MetaType, Name));
            }
            set { _meta = value; }
        }

        public MetaType MetaType
        {
            get { return MetaType.Brand; }
        }

        public int ID
        {
            get { return BrandId; }
        }
    }
}