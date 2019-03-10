using log4net;
using Lucene.Net.Index;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;

namespace AdvantShop.Core.Services.FullSearch.Core
{
    public class BaseWriter<T> : BaseSearch<T> where T : BaseDocument
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BaseWriter<T>));

        private IndexWriter _writer;
        private readonly string _indexField;

        /// <summary>
        /// Constructor
        /// </summary>
        public BaseWriter(string path)
            : base(path)
        {
            _writer = new IndexWriter(_luceneDirectory, _analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
            _indexField = Nameof<BaseDocument>.Property(e => e.Id);
        }

        /// <summary>
        /// Private helper to add an item to the Index
        /// </summary>
        /// <param name="doc">A ADocument type, representing the values that have to be added to the index</param>
        /// <param name="writer">The Lucene writer</param>
        private void AddItemToIndex(BaseDocument doc, IndexWriter writer)
        {
            var query = new TermQuery(new Term(_indexField, doc.Id.ToString()));
            writer.DeleteDocuments(query);
            writer.AddDocument(doc.Document);
        }

        /// <summary>
        /// Adds or update items in the Lucene index
        /// </summary>
        /// <param name="docs">The documents that have to be updated or added in the database</param>
        public void AddUpdateItemsToIndex(IEnumerable<BaseDocument> docs)
        {
            foreach (var doc in docs)
            {
                AddItemToIndex(doc, _writer);
            }
        }

        public void AddUpdateToIndex(BaseDocument doc)
        {
            AddUpdateItemsToIndex(new List<BaseDocument> { doc });
        }

        /// <summary>
        /// Private helper to delete an item from the index
        /// </summary>
        /// <param name="doc">The document representing the item that has to be deleted</param>
        /// <param name="writer">The Lucene writer</param>
        private void DeleteItemFromIndex(BaseDocument doc, IndexWriter writer)
        {
            var query = new TermQuery(new Term(_indexField, doc.Id.ToString()));
            writer.DeleteDocuments(query);
        }

        /// <summary>
        /// Deletes ites from the Lucene index
        /// </summary>
        /// <param name="docs"></param>
        public void DeleteItemsFromIndex(IEnumerable<BaseDocument> docs)
        {
            foreach (var doc in docs)
            {
                DeleteItemFromIndex(doc, _writer);
            }
        }

        /// <summary>
        /// optimizes the Lucene Index
        /// </summary>
        public void Optimize()
        {
            _writer.Optimize();
        }

        #region  IDisposable Support

        private bool _disposed; // To detect redundant calls

        // IDisposable

        ~BaseWriter()// the finalizer
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
                if (_writer != null)
                {
                    _writer.Commit();
                    _writer.Dispose();
                    _writer = null;
                }
            }
            _disposed = true;
            base.Dispose(disposing);
        }

        #endregion
    }
}