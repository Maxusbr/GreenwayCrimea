using System.Collections.Generic;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.FullSearch.Core;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;

namespace AdvantShop.Core.Services.FullSearch
{
    public class ProductSeacherAdmin : BaseSearcher<ProductDocument>
    {
        public ProductSeacherAdmin(int hitsLimit, ESearchDeep deepLimit)
            : base(hitsLimit, deepLimit)
        {
        }

        public static SearchResult Search(string searchTerm, int limit = 10000, string field = "")
        {
            using (var searcher = new ProductSeacherAdmin(limit, ESearchDeep.WordsBetween))
            {
                var res = searcher.SearchItems(searchTerm, field);
                return res;
            }
        }

        protected override List<string> GetIgnoredFields()
        {
            return new List<string>() {"Tags", "Desc"};
        }
    }

    public class ProductSeacher : BaseSearcher<ProductDocument>
    {
        public ProductSeacher(int hitsLimit, ESearchDeep deepLimit) : base(hitsLimit, deepLimit)
        {
        }

        protected override BooleanQuery ProcessCondition(BooleanQuery bq)
        {
            var conditionParser = new QueryParser(CurrentVersion, Nameof<ProductDocument>.Property(e => e.Enabled), _analyzer);
            var conditionQuery = ParseQuery(true.ToString(), conditionParser);
            bq.Add(conditionQuery, Occur.MUST);
            return bq;
        }

        public static SearchResult Search(string searchTerm, string field = "")
        {
            using (var searcher = new ProductSeacher(SettingsCatalog.SearchMaxItems, SettingsCatalog.SearchDeep))
            {
                var res = searcher.SearchItems(searchTerm, field);
                return res;
            }
        }
    }
}
