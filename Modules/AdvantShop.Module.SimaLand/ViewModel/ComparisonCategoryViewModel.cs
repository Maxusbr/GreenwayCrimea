using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvantShop.Module.SimaLand.Models;
using AdvantShop.Catalog;

namespace AdvantShop.Module.SimaLand.ViewModel
{
    public class ComparisonCategoryViewModel
    {
        public List<SimalandCategoryView> List { get; set; }
        public string Domain { get; set; }
        public int? PrevCategoryId { get; set; }
        public int? PrevLevel { get; set; }
        public int TotalCountActive { get; set; }
        public int TotalCountLink { get; set; }
    }

    public class SimalandCategoryView : SimalandCategory
    {
       public string AdvCategoryName {
            get
            {
                return CategoryId != null ? CategoryService.GetCategory((int)CategoryId).Name : "";
            }
        }
    }
}
