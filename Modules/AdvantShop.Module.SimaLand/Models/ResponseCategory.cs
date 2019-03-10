using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.SimaLand.Models
{
    #region IMplementation of SimaLandCategories

    public class ResponseCategory
    {
        public List<SimalandCategory> items { get; set; }
        public Links _links { get; set; }
        public Meta _meta { get; set; }
    }

    public class Links
    {
        public Href self { get; set; }
        public Href next { get; set; }
        public Href prev { get; set; }
    }

    public class Href
    {
        public string href { get; set; }
    }

    public class Meta
    {
        public int totalCount { get; set; }
        public int pageCount { get; set; }
        public int currentPage { get; set; }
        public int perPage { get; set; }
    }

    #endregion
}
