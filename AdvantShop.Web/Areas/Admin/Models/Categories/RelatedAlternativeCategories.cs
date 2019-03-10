using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.Catalog;

namespace AdvantShop.Web.Admin.Models.Categories
{
    public class RelatedAlternativeCategories
    {
        public List<SelectListItem> Categories { get; set; }
        public List<Category> RelatedCategories { get; set; }
        public List<Category> AlternativeCategories { get; set; }
        public List<SelectListItem> Properties { get; set; }
        public List<Property> RelatedProperties { get; set; }
        public List<PropertyValue> RelatedPropertyValues { get; set; }
        public List<Property> AlternativeProperties { get; set; }
        public List<PropertyValue> AlternativePropertyValues { get; set; }
    }
}
