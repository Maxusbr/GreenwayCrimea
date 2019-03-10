using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.SmsTemplates;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Handlers;
using System.Linq;

namespace AdvantShop.Web.Admin.Handlers.SmsTemplates
{
    public class SmsLogView : SmsLog
    {
        public string Created_Str
        {
            get { return Created.ToString("dd.MM.yyyy hh:mm:ss"); }
        }
    }

    public class GetSmsLogHandler : AbstractHandler<SmsLogFilterModel, int, SmsLogView>
    {
        public GetSmsLogHandler(SmsLogFilterModel filterModel) : base(filterModel)
        {
        }

        protected override SqlPaging Select(SqlPaging paging)
        {
            paging.Select("Id", "Body", "State", "Phone", "Created");
            paging.From("[Bonus].[SmsLog]");
            return paging;
        }

        protected override SqlPaging Filter(SqlPaging paging)
        {
            if (FilterModel.Search.IsNotEmpty())
            {
                paging.Where("Body like '%'+{0}+'%'", FilterModel.Search);
            }

            if (!string.IsNullOrWhiteSpace(FilterModel.Phone))
            {
                paging.Where("Phone = {0}", FilterModel.Phone);
            }
            if (!string.IsNullOrWhiteSpace(FilterModel.Body))
            {
                paging.Where("Body like '%'+{0}+'%'", FilterModel.Body);
            }
            return paging;
        }

        protected override SqlPaging Sorting(SqlPaging paging)
        {
            if (string.IsNullOrEmpty(FilterModel.Sorting) || FilterModel.SortingType == FilterSortingType.None)
            {
                paging.OrderByDesc("Created");
                return paging;
            }

            var sorting = FilterModel.Sorting.ToLower().Replace("formatted", "").Replace("_str","");

            var field = paging.SelectFields().FirstOrDefault(x => x.FieldName == sorting);
            if (field != null)
            {
                if (FilterModel.SortingType == FilterSortingType.Asc)
                {
                    paging.OrderBy(sorting);
                }
                else
                {
                    paging.OrderByDesc(sorting);
                }
            }
            return paging;
        }
    }
}
