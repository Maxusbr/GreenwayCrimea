using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvantShop.Catalog;

namespace AdvantShop.Module.SimaLand.ViewModel
{
    public class AdvCategoryViewModel
    {
        public List<Category> Categories { get; set; }
        public int Back { get; set; }
        public bool DeleteLink { get; set; }

        public AdvCategoryViewModel(List<Category> categories)
        {
            Categories = categories;
        }
    }
}
