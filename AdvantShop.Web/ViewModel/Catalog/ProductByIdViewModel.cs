using AdvantShop.Core.Services.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdvantShop.ViewModel.Catalog
{
    public class ProductByIdViewModel
    {

        public string Title { get; set; }

        public ProductViewModel Products { get; set; }

        public string RelatedType { get; set; }

        public int VisibleItems { get; set; }

        public bool EnabledCarousel { get; set; }
    }
}