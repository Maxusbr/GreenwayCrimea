using System.Collections.Generic;

namespace AdvantShop.Web.Infrastructure.Admin
{
    public enum FilterSortingType
    {
        None,
        Asc,
        Desc
    }

    public enum FilterOutputDataType
    {
        List,
        Csv,
        Xml
    }

    public class BaseFilterModel<T>
    {
        public BaseFilterModel()
        {
            Page = 1;
            ItemsPerPage = 15;
            OutputDataType = FilterOutputDataType.List;
        }


        private int _page = 1;
        public int Page
        {
            get { return _page; }
            set { _page = value > 0 && value < int.MaxValue ? value : 1; }
        }

        public int ItemsPerPage { get; set; }

        public string Sorting { get; set; }

        public string Search { get; set; }

        public FilterSortingType SortingType { get; set; }

        public FilterOutputDataType OutputDataType { get; set; }


        public List<T> Ids { get; set; }
        public SelectModeCommand SelectMode { get; set; }
    }

    public class BaseFilterModel : BaseFilterModel<int>
    {

    }
}