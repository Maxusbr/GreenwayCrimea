using AdvantShop.Configuration;
using AdvantShop.Core.Services.FullSearch.Core;

namespace AdvantShop.Core.Services.FullSearch
{
    public class CategorySeacher : BaseSearcher<CategoryDocument>
    {
        public CategorySeacher(int hitsLimit, ESearchDeep deepLimit, string path)
            : base(hitsLimit, deepLimit, path)
        {
        }
        public CategorySeacher(int hitsLimit, ESearchDeep deepLimit)
            : base(hitsLimit, deepLimit)
        {
        }
        public static SearchResult Search(string searchTerm, string field = "")
        {
            using (var searcher = new CategorySeacher(SettingsCatalog.SearchMaxItems, SettingsCatalog.SearchDeep))
            {
                return searcher.SearchItems(searchTerm, field);
            }
        }
    }
}
