using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.SimaLand.Models
{
    public class CategoryContainer
    {
        public int? ParentId { get; set; }
        public List<SimalandCategory> Categories { get; set; }
        public CategoryContainer(int? parentId, List<SimalandCategory> categories)
        {
            ParentId = parentId;
            Categories = categories;
        }
    }
}
