using System.Collections.Generic;

namespace AdvantShop.App.Landing.Models
{
    public class BlockListViewModel
    {
        public BlockListViewModel()
        {
            Blocks = new List<BlockListItemViewModel>();
        }

        public string Category { get; set; }
        public List<BlockListItemViewModel> Blocks { get; set; } 
    }

    public class BlockListItemViewModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }

    }
}
