using System;
using System.Runtime.CompilerServices;
using Lucene.Net.Documents;
using Lucene.Net.Search;

namespace AdvantShop.Core.Services.FullSearch
{
    public enum EFieldBoost
    {
        None,
        Low,
        Medium,
        High
    }

    public abstract class BaseDocument
    {
        public const float LowBoost = 0.1F;
        public const float MediumBoost = 1F;
        public const float HighBoost = 2F;

        private readonly Document _document;
        public Document Document { get { return _document; } }

        private int _id;
        //[SearchField]
        public int Id
        {
            set
            {
                _id = value;
                AddParameterToDocumentStoreNotAnalyzed(_id);
            }
            get { return _id; }
        }

        protected BaseDocument()
        {
            _document = new Document();
        }

        private void AddParameterToDocument<T>(string name, T value, float boost, Field.Store store, Field.Index index)
        {
            if (value != null)
            {
                _document.Add(new Field(name, value.ToString(), store, index) { Boost = boost });
            }
        }

        protected void AddParameterToDocumentStoreAnalyzed<T>(T value, [CallerMemberName] string name = "", float boost = MediumBoost)
        {
            AddParameterToDocument(name, value, boost, Field.Store.YES, Field.Index.ANALYZED);
        }

        protected void AddParameterToDocumentNoStoreAnalyzed<T>(T value, [CallerMemberName] string name = "", float boost = MediumBoost)
        {
            AddParameterToDocument(name, value, boost, Field.Store.NO, Field.Index.ANALYZED);
        }

        protected void AddParameterToDocumentStoreNotAnalyzed<T>(T value, [CallerMemberName] string name = "", float boost = MediumBoost)
        {
            AddParameterToDocument(name, value, boost, Field.Store.YES, Field.Index.NOT_ANALYZED);
        }

        protected virtual BooleanQuery Condition(BooleanQuery bq)
        {
            return bq;
        }

        protected void Boost(float boost)
        {
            _document.Boost = boost;
        }
    }
}