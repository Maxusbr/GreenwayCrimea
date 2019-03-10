//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.SEO;

namespace AdvantShop.News
{
    public class NewsCategory
    {
        public int NewsCategoryId { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public int CountNews { get; set; }

        private string _urlPath;
        public string UrlPath
        {
            get { return _urlPath; }
            set { _urlPath = value.ToLower(); }
        }

        public MetaType MetaType
        {
            get { return MetaType.NewsCategory; }
        }

        private MetaInfo _meta;
        public MetaInfo Meta
        {
            get
            {
                return _meta ??
                       (_meta =
                        MetaInfoService.GetMetaInfo(NewsCategoryId, MetaType) ??
                        MetaInfoService.GetDefaultMetaInfo(MetaType, Name));
            }
            set
            {
                _meta = value;
            }
        }
    }
}