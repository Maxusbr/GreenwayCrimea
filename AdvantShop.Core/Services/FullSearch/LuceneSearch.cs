//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Configuration;
using AdvantShop.Core.Services.FullSearch;
using AdvantShop.Core.Services.FullSearch.Core;
using System;

namespace AdvantShop.FullSearch
{
    public static class LuceneSearch
    {
        public static void CreateNewIndex<T>() where T : BaseDocument
        {
            var type = typeof(T);
            if (type == typeof(ProductDocument))
                ProductWriter.CreateIndexFromDb();
            else if (type == typeof(CategoryDocument))
                CategoryWriter.CreateIndexFromDb();
            else
                throw new Exception("type unknown");
        }

        public static void CreateAllIndex()
        {
            ProductWriter.CreateIndexFromDb();
            CategoryWriter.CreateIndexFromDb();
        }

        public static void CreateAllIndexInBackground()
        {
            ProductWriter.CreateIndexFromDbInTask();
            CategoryWriter.CreateIndexFromDbInTask();
        }

        //public static SearchResult Get<TSercher,TDocument>(string searchTerm, string field = "") where TSercher: BaseSearcher<TDocument> where TDocument:BaseDocument
        //{
        //    using (var searcher = (TSercher)Activator.CreateInstance(typeof(TSercher), SettingsCatalog.SearchMaxItems, SettingsCatalog.SearchDeep))
        //    {
        //        var res = searcher.SearchItems(searchTerm, field);
        //        return res;
        //    }
        //}      
    }
}