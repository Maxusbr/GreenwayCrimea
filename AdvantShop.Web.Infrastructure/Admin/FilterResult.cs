using System.Collections.Generic;

namespace AdvantShop.Web.Infrastructure.Admin
{
    public class FilterResult<TModel> 
    {
        public List<TModel> DataItems { get; set; }

        public int TotalPageCount { get; set; }
        public int TotalItemsCount { get; set; }
        public int PageIndex { get; set; }
        public string Error { get; set; }
        public string TotalString { get; set; }
    }
}