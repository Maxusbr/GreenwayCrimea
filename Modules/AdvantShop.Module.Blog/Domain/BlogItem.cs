//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//-------------------------------------------------

using System;

namespace AdvantShop.Module.Blog.Domain
{
    public class BlogItem
    {
        public int ItemId { get; set; }
        
        public int? ItemCategoryId { get; set; }
        public string ItemCategoryName { get; set; }
        public string ItemCategoryUrl { get; set; }

        public string Title { get; set; }

        public string Picture { get; set; }

        public string TextToPublication { get; set; }

        public string TextToEmail { get; set; }

        public string TextAnnotation { get; set; }

        public bool ShowOnMainPage { get; set; }

        public bool Enabled { get; set; }

        public DateTime AddingDate { get; set; }

        public string UrlPath { get; set; }


        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
    }
}