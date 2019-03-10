//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//-------------------------------------------------


namespace AdvantShop.Module.Blog.Domain
{
    public class BlogCategory
    {
        public int ItemCategoryId { get; set; }

        public string Name { get; set; }

        public int SortOrder { get; set; }

        public int CountItems { get; set; }

        public string UrlPath { get; set; }


        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
    }
}
