//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.SEO;

namespace AdvantShop.CMS
{
    public class StaticPage// : IMetaContainer
    {
        public int StaticPageId { get; set; }

        public string PageName { get; set; }

        public string PageText { get; set; }

        public int SortOrder { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime ModifyDate { get; set; }

        public bool IndexAtSiteMap { get; set; }

        public bool Enabled { get; set; }

        private bool? _hasChildren;

        public bool HasChildren
        {
            get { return _hasChildren ?? (bool)(_hasChildren = (bool?)StaticPageService.CheckChilds(StaticPageId)); }
            set { _hasChildren = value; }
        }

        private string _urlPath;
        public string UrlPath
        {
            get { return _urlPath; }
            set { _urlPath = value.ToLower(); }
        }
        
        public int ParentId { get; set; }
        private StaticPage _parent;

        public StaticPage Parent
        {
            get { return _parent ?? (_parent = StaticPageService.GetStaticPage(ParentId)); }
        }

        public MetaType MetaType
        {
            get { return MetaType.StaticPage; }
        }

        private bool _isMetaLoadded;
        private MetaInfo _meta;
        public MetaInfo Meta
        {
            get
            {
                if (_isMetaLoadded)
                    return _meta;

                _isMetaLoadded = true;

                return _meta =
                        MetaInfoService.GetMetaInfo(StaticPageId, MetaType) ??
                        MetaInfoService.GetDefaultMetaInfo(MetaType, PageName);
            }
            set
            {
                _meta = value;
                _isMetaLoadded = true;
            }
        }

        public int ID
        {
            get { return StaticPageId; }
        }
    }
}