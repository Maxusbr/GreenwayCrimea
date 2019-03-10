using AdvantShop.SEO;

namespace AdvantShop.Core.Services.Catalog
{
    public enum ETagType
    {
        None,
        Product,
        Category
    }

    public class Tag
    {
        public Tag()
        {
            VisibilityForUsers = true;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public int SortOrder { get; set; }
        public string Description { get; set; }
        public string BriefDescription { get; set; }
        
        public bool VisibilityForUsers { get; set; }

        private string _urlPath;
        public string UrlPath
        {
            get { return _urlPath; }
            set { _urlPath = value.ToLower(); }
        }

        public MetaType MetaType
        {
            get { return MetaType.Tag; }
        }

        private MetaInfo _meta;
        public MetaInfo Meta
        {
            get
            {
                return _meta ??
                       (_meta =
                        MetaInfoService.GetMetaInfo(Id, MetaType) ??
                        MetaInfoService.GetDefaultMetaInfo(MetaType, Name));
            }
            set
            {
                _meta = value;
            }
        } 
    }
}