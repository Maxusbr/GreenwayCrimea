//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------
using System;
using System.Collections.Generic;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Helpers;
using AdvantShop.SEO;
using Newtonsoft.Json;

namespace AdvantShop.Catalog
{
    [Serializable]
    public class Category 
    {
        public Category()
        {
            CategoryId = CategoryService.DefaultNonCategoryId;
        }

        public Category(int id, string name)
        {
            CategoryId = id;
            Name = name;
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BriefDescription { get; set; }
        public bool Enabled { get; set; }
        public bool Hidden { get; set; }
        public bool HasChild { get; set; }
        public int SortOrder { get; set; }
        public int ProductsCount { get; set; }
        public int TotalProductsCount { get; set; }
        public int ParentCategoryId { get; set; }

        public ECategoryDisplayStyle DisplayStyle { get; set; }
        public bool DisplayChildProducts { get; set; }
        public bool DisplayBrandsInMenu { get; set; }
        public bool DisplaySubCategoriesInMenu { get; set; }
        public bool ParentsEnabled { get; set; }
        public ESortOrder Sorting { get; set; }

        private CategoryPhoto _picture;
        public CategoryPhoto Picture
        {
            get { return _picture ?? (_picture = PhotoService.GetPhotoByObjId<CategoryPhoto>(CategoryId, PhotoType.CategoryBig)); }
            set { _picture = value; }
        }

        private CategoryPhoto _minipicture;
        public CategoryPhoto MiniPicture
        {
            get
            {
                return _minipicture ?? (_minipicture = PhotoService.GetPhotoByObjId<CategoryPhoto>(CategoryId, PhotoType.CategorySmall));
            }
            set { _minipicture = value; }
        }

        private CategoryPhoto _icon;
        public CategoryPhoto Icon
        {
            get { return _icon ?? (_icon = PhotoService.GetPhotoByObjId<CategoryPhoto>(CategoryId, PhotoType.CategoryIcon)); }
            set { _icon = value; }
        }

        private Category _parentcategory;

        [JsonIgnore]
        public Category ParentCategory
        {
            get { return _parentcategory ?? (_parentcategory = CategoryService.GetCategory(ParentCategoryId)); }
        }
        
        private string _urlPath;
        public string UrlPath
        {
            get { return _urlPath; }
            set { _urlPath = value.ToLower(); }
        }

        public MetaType MetaType
        {
            get { return MetaType.Category; }
        }

        private MetaInfo _meta;
        public MetaInfo Meta
        {
            get
            {
                return _meta ??
                       (_meta =
                           MetaInfoService.GetMetaInfo(CategoryId, MetaType) ??
                           MetaInfoService.GetDefaultMetaInfo(MetaType, Name));
            }
            set { _meta = value; }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Name, SQLDataHelper.GetString(CategoryId));
        }

        public int ID
        {
            get { return CategoryId; }
        }

        private bool _tagsLoaded;
        private List<Tag> _tags;

        public List<Tag> Tags
        {
            get
            {
                if (_tagsLoaded)
                    return _tags;
                
                _tagsLoaded = true;
                return _tags = TagService.Gets(CategoryId, ETagType.Category, true);
            }
            set
            {
                _tags = value;
                _tagsLoaded = true;
            }
        }
    }
}
