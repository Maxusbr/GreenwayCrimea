using System.Collections.Generic;

namespace AdvantShop.Web.Admin.Models.Catalog
{
    public class CategoriesTree
    {
        public string Id { get; set; }

        public int? CategoryIdSelected { get; set; }

        public string ExcludeIds { get; set; }
        public string SelectedIds { get; set; }

        public bool ShowRoot { get; set; }
    }
}
