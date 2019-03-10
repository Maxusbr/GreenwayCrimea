using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AdvantShop.Web.Infrastructure.Filters
{
    public class ExcludeFilterProvider : IFilterProvider
    {
        private static readonly Type ExcludeFilterAttributeType = typeof(ExcludeFilterAttribute);
        private readonly FilterProviderCollection _filterProviders;

        public ExcludeFilterProvider(IFilterProvider[] filters)
        {
            _filterProviders = new FilterProviderCollection(filters);
        }
        
        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = _filterProviders.GetFilters(controllerContext, actionDescriptor);

            if (controllerContext.IsChildAction)
                return filters;

            var filtersArr = filters.ToArray();
            
            var excudeFilterTypes = new List<Type>();

            foreach (var filter in filtersArr)
            {
                if (filter.Instance.GetType() == ExcludeFilterAttributeType)
                {
                    var excludeFilter = (ExcludeFilterAttribute) filter.Instance;
                    if (excludeFilter.FilterTypes != null)
                    {
                        for (int i = 0; i < excludeFilter.FilterTypes.Count; i++)
                        {
                            excudeFilterTypes.Add(excludeFilter.FilterTypes[i]);
                        }
                    }
                }
            }

            var result = new List<Filter>();

            for (int i = 0; i < filtersArr.Length; i++)
            {
                if (!excudeFilterTypes.Contains(filtersArr[i].Instance.GetType()))
                    result.Add(filtersArr[i]);
            }
            return result;
        }
    }
}
