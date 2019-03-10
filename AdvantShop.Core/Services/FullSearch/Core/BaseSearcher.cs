using AdvantShop.Core.Common.Extensions;
using AdvantShop.Diagnostics;
using AdvantShop.FullSearch;
using AdvantShop.Helpers;
using log4net;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.Tokenattributes;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdvantShop.Core.Services.FullSearch.Core
{
    public class BaseSearcher<T> : BaseSearch<T> where T : BaseDocument
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BaseSearcher<T>));
        private int _hitsLimit;
        private ESearchDeep _deepLimit;
        private IndexSearcher _searcher;

        /// <summary> 
        /// Constructor 
        /// </summary>
        public BaseSearcher(int hitsLimit, ESearchDeep deepLimit) : this(hitsLimit, deepLimit, string.Empty)
        {
        }

        public BaseSearcher(int hitsLimit, ESearchDeep deepLimit, string path)
            : base(path)
        {
            _hitsLimit = hitsLimit;
            _deepLimit = deepLimit;
            try
            {
                _searcher = new IndexSearcher(_luceneDirectory, true);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                LuceneSearch.CreateNewIndex<T>();
                _searcher = new IndexSearcher(_luceneDirectory, true);
            }
        }

        public SearchResult SearchItems(string searchQuery, string field = "", bool ifError = false) //where TDoc : BaseDocument
        {
            try
            {
                return _search(searchQuery, field);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                if (ifError) return new SearchResult(searchQuery);
                LuceneSearch.CreateNewIndex<T>();
                return SearchItems(searchQuery, field, true);
            }
        }

        /// <summary> 
        /// Base Search method 
        /// </summary> 
        /// <typeparam name="T">The type of document that has tobe searched for</typeparam> 
        /// <param name="field">The field that have to be searched for</param>
        /// <param name="searchQuery">The query as string with the search term</param> 
        /// <returns>A SearchResult object</returns> 
        private SearchResult _search(string searchQuery, string field = "") //where TDoc : BaseDocument
        {
            searchQuery = StringHelper.ReplaceCirilikSymbol(searchQuery);
            searchQuery = RemoveStopWords(searchQuery);            
            var currentType = typeof(T);
            //Fetch the possible fields to search on 
            var fieldsToSearchOn = GetFields(currentType);

            fieldsToSearchOn.ForEach(f => Log.DebugFormat("{0}", f));

            var hits = !string.IsNullOrEmpty(field)
                       ? ProcessSingle(field, fieldsToSearchOn, searchQuery)
                       : ProcessMulti(fieldsToSearchOn, searchQuery);
            var searchResults = new SearchResult(searchQuery);
            if (hits != null)
            {
                var nameField = Nameof<T>.Property(e => e.Id);
                searchResults.Hits = hits.Count();
                searchResults.SearchResultItems = hits.Select(x =>
                {
                    var doc = _searcher.Doc(x.Doc);
                    return new SearchResultItem
                    {
                        Id = doc.Get(nameField).TryParseInt(),
                        Score = x.Score,
                    };
                }).ToList();
            }
            return searchResults;
        }

        protected virtual ScoreDoc[] ProcessSingle(string searchField, string[] fieldsToSearchOn, string searchQuery)
        {
            searchQuery = searchQuery.RemoveSymvolsExt(" ").Trim();
            var mergedQuery = new BooleanQuery();
            if (!fieldsToSearchOn.Contains(searchField))
            {
                throw new SearchException(string.Format("Field {0} is not a search field", searchField));
            }
            var parser = new QueryParser(CurrentVersion, searchField, _analyzer);
            var query = ParseQuery(searchQuery, parser);
            mergedQuery.Add(query, Occur.MUST);

            var searchLike = string.Join(" ", searchQuery.Trim().Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*"));
            var parserLike = new QueryParser(CurrentVersion, searchField, _analyzer);
            var queryLike = ParseQuery(searchLike, parserLike);
            mergedQuery.Add(queryLike, Occur.MUST);

            return _searcher.Search(query, _hitsLimit).ScoreDocs;
        }

        private BooleanQuery StrongPhaseProcess(string searchQuery, string[] fieldsToSearchOn, BooleanQuery mergedQuery)
        {
            var searchQueryStrongPhrase = "\"" + searchQuery + "\"^1000";
            var parserStrongPhrase = new MultiFieldQueryParser(CurrentVersion, fieldsToSearchOn, _analyzer);
            var queryStrongPhrase = ParseQuery(searchQueryStrongPhrase, parserStrongPhrase);
            mergedQuery.Add(queryStrongPhrase, Occur.SHOULD);
            return mergedQuery;
        }

        private BooleanQuery SepareteWordsProcess(string searchQuery, string[] fieldsToSearchOn, BooleanQuery mergedQuery)
        {
            var searchQueryPhrase = string.Join(" ", searchQuery.Trim()
                                                                    .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                                                    .Where(x => !string.IsNullOrEmpty(x))
                                                                    .Select(x => x.Trim()));
            if (!string.IsNullOrWhiteSpace(searchQueryPhrase))
            {
                searchQueryPhrase = "(" + searchQueryPhrase + ")^100";
                var parserPhrase = new MultiFieldQueryParser(CurrentVersion, fieldsToSearchOn, _analyzer);
                var queryPhrase = ParseQuery(searchQueryPhrase, parserPhrase);
                mergedQuery.Add(queryPhrase, Occur.SHOULD);
            }
            return mergedQuery;
        }

        private BooleanQuery WordsStartFromProcess(string searchQuery, string[] fieldsToSearchOn, BooleanQuery mergedQuery)
        {
            var searchLike = string.Join(" ", searchQuery.Trim()
                                                         .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                                         .Where(x => !string.IsNullOrEmpty(x))
                                                         .Select(x => x.Trim() + "*"));
            if (!string.IsNullOrWhiteSpace(searchLike))
            {
                var parserLike = new MultiFieldQueryParser(CurrentVersion, fieldsToSearchOn, _analyzer);
                //parserLike.AllowLeadingWildcard = true;
                var queryLike = ParseQuery(searchLike, parserLike);
                mergedQuery.Add(queryLike, Occur.SHOULD);
            }
            return mergedQuery;
        }

        private BooleanQuery WordsBetweenProcess(string searchQuery, string[] fieldsToSearchOn, BooleanQuery mergedQuery)
        {
            var search2Like = string.Join(" ", searchQuery.Trim()
                                                             .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                                             .Where(x => !string.IsNullOrEmpty(x))
                                                             .Select(x => "*" + x.Trim() + "*"));
            if (!string.IsNullOrWhiteSpace(search2Like))
            {
                var parser2Like = new MultiFieldQueryParser(CurrentVersion, fieldsToSearchOn, _analyzer);
                parser2Like.AllowLeadingWildcard = true;
                var query2Like = ParseQuery(search2Like, parser2Like);
                mergedQuery.Add(query2Like, Occur.SHOULD);
            }
            return mergedQuery;
        }

        protected virtual BooleanQuery ProcessCondition(BooleanQuery bq)
        {
            return bq;
        }

        protected virtual ScoreDoc[] ProcessMulti(string[] fieldsToSearchOn, string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery)) return null;
            var mergedQuery = new BooleanQuery();

            //if (useCondition)
            mergedQuery = ProcessCondition(mergedQuery);
            var mergedQueryF = new BooleanQuery();
            var searchQueryAfterClean = searchQuery.RemoveSymvolsExt(" ").Trim();

            //strong phase 
            if (_deepLimit == ESearchDeep.StrongPhase)
            {
                mergedQueryF = StrongPhaseProcess(searchQueryAfterClean, fieldsToSearchOn, mergedQueryF);
            }

            //phase 
            if (_deepLimit == ESearchDeep.SepareteWords)
            {
                mergedQueryF = SepareteWordsProcess(searchQueryAfterClean, fieldsToSearchOn, mergedQueryF);
            }

            //separate words with *
            if (_deepLimit == ESearchDeep.WordsStartFrom)
            {
                searchQueryAfterClean = prepareString(searchQueryAfterClean);
                mergedQueryF = WordsStartFromProcess(searchQueryAfterClean, fieldsToSearchOn, mergedQueryF);
            }

            if (_deepLimit == ESearchDeep.WordsBetween)
            {
                searchQueryAfterClean = prepareString(searchQueryAfterClean);
                mergedQueryF = WordsBetweenProcess(searchQueryAfterClean, fieldsToSearchOn, mergedQueryF);
            }
            mergedQuery.Add(mergedQueryF, Occur.MUST);
            var result = _searcher.Search(mergedQuery, null, _hitsLimit, Sort.RELEVANCE).ScoreDocs;
            return result;
        }

        protected virtual List<string> GetIgnoredFields()
        {
            return null;
        }

        private string[] GetFields(Type type)
        {
            var fieldsToSearchOn = new List<string>();
            var ignoredFields = GetIgnoredFields();

            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (ignoredFields != null && ignoredFields.Contains(property.Name))
                    continue;

                var attributes = property.GetCustomAttributes(true).OfType<SearchField>();
                foreach (var attr in attributes)
                {
                    if (attr.CombinedSearchFields.Length > 0)
                    {
                        for (var i = 0; i < attr.CombinedSearchFields.Length; i++)
                            fieldsToSearchOn.Add(attr.CombinedSearchFields[i]);
                    }
                    else
                        fieldsToSearchOn.Add(property.Name);
                }
            }
            return fieldsToSearchOn.ToArray();
        }

        /// <summary> 
        /// Parse the givven query string to a Lucene Query object 
        /// </summary> 
        /// <param name="searchQuery">The query string</param> 
        /// <param name="parser">The Lucense QueryParser</param> 
        /// <returns>A Lucene Query object</returns> 
        protected Query ParseQuery(string searchQuery, QueryParser parser)
        {
            Query q;
            try
            {
                q = parser.Parse(searchQuery);
            }
            catch (ParseException e)
            {
                Log.Error("Query parser exception", e);
                q = null;
            }

            if (q == null || string.IsNullOrEmpty(q.ToString()))
            {
                var cooked = QueryParser.Escape(searchQuery);
                q = parser.Parse(cooked);
            }
            return q;
        }

        protected string prepareString(string str)
        {
            var t = ParseQuery(str, new QueryParser(CurrentVersion, "", _analyzer));
            return t.ToString();
        }

        private string RemoveStopWords(string str) {
            TokenStream tokenStream = new StandardTokenizer(CurrentVersion, new StringReader(str.Trim()));
            tokenStream = new StopFilter(StopFilter.GetEnablePositionIncrementsVersionDefault(CurrentVersion), tokenStream, new HashSet<string>(RUSSIAN_STOP_WORDS_30));
            StringBuilder sb = new StringBuilder();
            var charTermAttribute = tokenStream.GetAttribute<ITermAttribute>();
            tokenStream.Reset();
            while (tokenStream.IncrementToken()) {
                String term = charTermAttribute.Term.ToString();

                sb.Append(term + " ");
            }
            return sb.ToString().Trim();            
        }


        #region  IDisposable Support

        private bool _disposed; // To detect redundant calls

        // IDisposable

        ~BaseSearcher()// the finalizer
        {
            Dispose(false);
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public override void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_searcher != null)
                {
                    _searcher.Dispose();
                    _searcher = null;
                }
            }
            _disposed = true;
            base.Dispose(disposing);
        }

        #endregion
    }
}