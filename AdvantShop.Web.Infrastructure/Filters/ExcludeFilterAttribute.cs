using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AdvantShop.Web.Infrastructure.Filters
{
    /// <summary>
    /// using  [ExcludeFilter(typeof(SomeAtr))]
    /// </summary>
    public class ExcludeFilterAttribute : FilterAttribute
    {
        private readonly List<Type> _filterTypes;

        public ExcludeFilterAttribute(Type filterType)
        {
            _filterTypes = new List<Type>() { filterType };
        }

        public ExcludeFilterAttribute(Type filterType1, Type filterType2)
        {
            _filterTypes = new List<Type>() { filterType1, filterType2 };
        }

        public ExcludeFilterAttribute(Type filterType1, Type filterType2, Type filterType3)
        {
            _filterTypes = new List<Type>() { filterType1, filterType2, filterType3 };
        }

        public ExcludeFilterAttribute(Type filterType1, Type filterType2, Type filterType3, Type filterType4)
        {
            _filterTypes = new List<Type>() { filterType1, filterType2, filterType3, filterType4 };
        }

        public ExcludeFilterAttribute(Type filterType1, Type filterType2, Type filterType3, Type filterType4, Type filterType5)
        {
            _filterTypes = new List<Type>() { filterType1, filterType2, filterType3, filterType4, filterType5 };
        }

        public List<Type> FilterTypes
        {
            get { return _filterTypes; }
        }
    }
}
