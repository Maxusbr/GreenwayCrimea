using System;
using System.Linq;

using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.Tags;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Handlers;

namespace AdvantShop.Web.Admin.Handlers.Tags
{
    public class GetTagsHandler : AbstractHandler<TagsFilterModel, int, TagsModel>
    {
        public GetTagsHandler(TagsFilterModel filterModel) : base(filterModel)
        {
            
        }
        
        protected override SqlPaging Select(SqlPaging paging)
        {
            paging.Select("Id", "Name", "UrlPath", "Enabled", "SortOrder", "VisibilityForUsers");
            paging.From("[Catalog].[Tag]");
            return paging;
        }

        protected override SqlPaging Filter(SqlPaging paging)
        {
            if (!string.IsNullOrEmpty(FilterModel.Search))
            {
                paging.Where("Name LIKE '%'+{0}+'%' OR UrlPath LIKE '%'+{0}+'%'", FilterModel.Search);
            }

            if (!string.IsNullOrEmpty(FilterModel.Name))
            {
                paging.Where("Name LIKE '%'+{0}+'%'", FilterModel.Name);
            }

            if (!string.IsNullOrEmpty(FilterModel.Url))
            {
                paging.Where("UrlPath LIKE '%'+{0}+'%'", FilterModel.Url);
            }

            if (FilterModel.Enabled != null)
            {
                paging.Where("Enabled = {0}", Convert.ToInt32(FilterModel.Enabled.Value));
            }
            return paging;
        }

        protected override SqlPaging Sorting(SqlPaging paging)
        {
            if (string.IsNullOrEmpty(FilterModel.Sorting) || FilterModel.SortingType == FilterSortingType.None)
            {
                paging.OrderBy("SortOrder");
                return paging;
            }

            var sorting = FilterModel.Sorting.ToLower().Replace("formatted", "");

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