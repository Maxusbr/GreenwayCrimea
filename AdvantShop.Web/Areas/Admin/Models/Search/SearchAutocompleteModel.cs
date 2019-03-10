using System.Collections.Generic;

namespace AdvantShop.Web.Admin.Models.Search
{
    public class SearchAutocompleteModel
    {
        public string Category { get; set; }
        public long Time { get; set; }
        public List<SearchAutocompleteItem> Items {get; set;}
        public SearchAutocompleteItem More { get; set; }
    }

    public class SearchAutocompleteItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }

}
