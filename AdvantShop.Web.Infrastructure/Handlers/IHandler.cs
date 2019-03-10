using System.Collections.Generic;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Infrastructure.Handlers
{
    public interface IHandler<TResult, TIds>
    {
        FilterResult<TResult> Execute();

        List<TIds> GetItemsIds(string field);
    }
}