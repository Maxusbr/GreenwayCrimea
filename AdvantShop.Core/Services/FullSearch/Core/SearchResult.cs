using System.Collections.Generic;
using System.Linq;

namespace AdvantShop.Core.Services.FullSearch
{
    /// <summary> 
    /// Class to represent the Searh results 
    /// </summary> 
    public class SearchResult 
    { 
        public string SearchTerm { get; set; } 
        public List<SearchResultItem> SearchResultItems { get; set; } 
        public int Hits { get; set; }

        public SearchResult()
        {
        }

        public SearchResult(string searchQuery)
        {
            SearchTerm = searchQuery;
            SearchResultItems = new List<SearchResultItem>();
        }
    }

    public static class SearchResultExt
    {
        public static List<SearchResultItem> ToPageList(this List<SearchResultItem> val, int? page, int pageSize)
        {
            var p = page ?? 1;
            return val.Skip((p - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}