//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Catalog;
using AdvantShop.SEO;

namespace AdvantShop.News
{
    public class NewsItem
    {
        public int NewsId { get; set; }

        public int NewsCategoryId { get; set; }

        public string Title { get; set; }
        
        public string TextToPublication { get; set; }

        public string TextToEmail { get; set; }

        public string TextAnnotation { get; set; }

        public bool ShowOnMainPage { get; set; }

        public DateTime AddingDate { get; set; }

        public string UrlPath { get; set; }

        public bool Enabled { get; set; }

        private string PhotoName { get; set; }

        private NewsPhoto _picture;
        public NewsPhoto Picture
        {
            get
            {
                if (_picture != null)
                    return _picture;

                if (!string.IsNullOrEmpty(PhotoName))
                {
                    _picture = new NewsPhoto() {PhotoName = PhotoName};
                    return _picture;
                }

                return (_picture = PhotoService.GetPhotoByObjId<NewsPhoto>(NewsId, PhotoType.News));
            }
            set
            {
                _picture = value;
            }
        }

        public MetaType MetaType
        {
            get { return MetaType.News; }
        }

        private MetaInfo _meta;
        public MetaInfo Meta
        {
            get
            {
                return _meta ??
                       (_meta =
                        MetaInfoService.GetMetaInfo(NewsId, MetaType) ??
                        MetaInfoService.GetDefaultMetaInfo(MetaType, Title));
            }
            set
            {
                _meta = value;
            }
        }
    }
}