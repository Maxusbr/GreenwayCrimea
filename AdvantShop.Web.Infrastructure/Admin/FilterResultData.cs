using System.Collections.Generic;

namespace AdvantShop.Web.Infrastructure.Admin
{
    public class FilterResultData<TModel>
    {
        public List<TModel> DataItems { get; set; }
    }
}